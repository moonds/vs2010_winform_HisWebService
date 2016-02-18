using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections;
using DataFormat;
using System.IO;
using System.Drawing;
using AppUI;

namespace HisWebServiceTest
{
    public partial class Form1 : Form
    {
        protected HisWebService.ChisWebServiceSoapClient hisClient;
        XmlFormat xmlformat = null;
        FunctionFormat functionformat = null;
        DynamicWebService.WebServiceAgent agent = null;
        public Form1()
        {
            InitializeComponent();
            xmlformat = new XmlFormat();
            functionformat = new FunctionFormat();
            bindCombBox();
            txtWSAddress.Text = ConfigurationManager.AppSettings["WebServiceAddress"];
        }
        #region combobox数据源
        private void bindCombBox()
        {
            ArrayList lst = new ArrayList();   　　　　//列表 
            lst.Add(new Vendor("Xml", "1"));  //在列表中增加对象
            lst.Add(new Vendor("Funtion", "2"));
            comboBox1.Items.Clear();                 //清空combobox
            comboBox1.DataSource = lst;           //将lst列表绑定到combobx
            comboBox1.DisplayMember = "Strtemname";// 指定显示的数据项
            comboBox1.ValueMember = "Strindex";  //指定comboBox1.SelectedValue返回的数据项
        }
        class Vendor
        {
            private string strtemname;
            private string strindex;
            public Vendor(string itemname, string index)
            {
                this.strtemname = itemname;
                this.strindex = index;
            }
            public string Strtemname
            {
                get { return strtemname; }
                set { strtemname = value; }
            }
            public string Strindex
            {
                get { return strindex; }
                set { strindex = value; }
            }
        }
        #endregion
        private void btnTest_Click(object sender, EventArgs e)
        {
            lblNotice.Text = "";
            if (string.IsNullOrEmpty(txtInput.Text.Trim()))
            {
                lblNotice.Text = "请输入发送字符串";
                txtInput.Focus();
                return;
            }
            switch (comboBox1.SelectedValue.ToString())
            {
                case "1":
                    string outPut = "";// hisClient.CallService(txtInput.Text.Trim());
                    bool isSuccess;
                    string outFinal = xmlformat.xmlFormatByStock(outPut, out isSuccess);
                    if (isSuccess)
                    {
                        txtOutPut.Text = outFinal;
                        //todo:将收发记录记录下来。
                        //SaveXMLRequestAndResponse(txtInput.Text.Trim(), outFinal);
                    }
                    else
                    {
                        lblNotice.Text = outFinal;
                        txtOutPut.Text = outPut;
                    }
                    break;
                case "2":
                    if (txtWSAddress.Text.Trim().Length == 0)
                    { lblNotice.Text = "WebService地址不能为空"; return; }
                    agent = new DynamicWebService.WebServiceAgent(txtWSAddress.Text.Trim());
                    if (!agent.EstablishSuccess) { lblNotice.Text = "链接未建立成功。详细错误信息：" + agent.ErrorMessage; return; }
                    //成功建立链接，且与AppSetting中的地址不同的话，重新记录新地址在appConfig中。
                    //if (txtWSAddress.Text.Trim() != ConfigurationManager.AppSettings["WebServiceAddress"])
                    //{
                    //    //todo:读取appsettings并更改内容。
                    //    //string path= System.IO.Path.GetFileName(Application.ExecutablePath);  
                    //    Configuration config = ConfigurationManager.OpenMappedExeConfiguration();
                    //    AppSettingsSection appsection = config.AppSettings;
                    //    appsection.Settings["WebServiceAddress"].Value = txtWSAddress.Text.Trim();
                    //    config.Save(ConfigurationSaveMode.Modified);
                    //}
                    if (txtInput.Text.Trim().Length == 0)
                    { lblNotice.Text = "请输入调用方法名，以及参数，均用逗号分隔"; return; }
                    string[] functionAndArgs = txtInput.Text.Trim().Replace("\r\n", ",").Split(',');
                    object[] args = new object[functionAndArgs.Length - 1];
                    for (int i = 1; i < functionAndArgs.Length; i++)
                    {
                        args[i - 1] = functionAndArgs[i].ToString().Trim();
                    }
                    object outPut1 = agent.Invoke(functionAndArgs[0].ToString().Trim(), args);
                    if (outPut1.ToString() == "failure")
                    {
                        lblNotice.Text = agent.ErrorMessage;
                        return;
                    }
                    //CreateCardAddPosit , 00045101 ,  测试账户 ,  男 ,  2015-12-12 ,  370103201512124510 ,  汉族 ,  市立医院信息科 ,  13966668888 ,  2015112200000001 ,  666666 ,  100 ,  CA ,  801001  
                    //AddDeposit , 00045101 ,  100 ,  CA ,   ,  2012080100000021 ,  801001  
                    //QueryBalanceRecord , 00045101  
                    //QueryFeeDetailRecord , 00045101 ,  2014-12-12 ,  2015-12-12  ;
                    //QueryPatientID , 00045101  ;
                    //QueryPatientInfo , 00045101  ;
                    //ZYAddDeposit   ,  100 ,  CA ,   ,  2015112200000001 ,  801001  ;
                    //ZYQueryAddBalanceRecord ,   ;
                    //ZYQueryFeeDetailRecord ,  ,  2014-12-12 ,  2015-12-12  ;
                    //ZYQueryPatientInfo ,   ;

                    bool isSuccess1;
                    txtOutPut.Text = xmlformat.xmlFormatByStock(outPut1.ToString(), out isSuccess1);
                    break;
                default:
                    break;
            }
            if (txtInput.Text.Trim().Length != 0 || txtOutPut.Text.Trim().Length != 0)
                SaveXMLRequestAndResponse(txtInput.Text.Trim(), txtOutPut.Text.Trim());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inxml"></param>
        /// <param name="outxml"></param>
        private bool SaveXMLRequestAndResponse(string inxml, string outxml)
        {
            string filepath = Directory.GetCurrentDirectory();
            try
            {
                string writeStr = "";
                if (File.Exists(filepath + "//" + DateTime.Now.ToShortDateString() + ".txt"))
                    writeStr = "\r\n【传入】\r\n" + inxml + "\r\n【传出】\r\n" + outxml;
                else
                    writeStr = "【传入】\r\n" + inxml + "\r\n【传出】\r\n" + outxml;
                File.AppendAllText(filepath + "//" + DateTime.Now.ToShortDateString() + ".txt", writeStr);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void txtClearInput_Click(object sender, EventArgs e)
        {
            txtInput.Text = "";
        }

        private void txtClearOutput_Click(object sender, EventArgs e)
        {
            txtOutPut.Text = "";
        }


        private void txtInput_MouseLeave(object sender, EventArgs e)
        {
            bool isSuccess;
            string finalStr = "";
            switch (comboBox1.SelectedValue.ToString())
            {
                case "1":
                    if (string.IsNullOrEmpty(txtInput.Text.Trim()))
                        return;
                    lblNotice.Text = "";
                    finalStr = xmlformat.xmlFormatByStock(txtInput.Text.Trim(), out isSuccess);
                    if (isSuccess)
                        txtInput.Text = finalStr;
                    else
                        lblNotice.Text = finalStr;
                    break;
                case "2":
                    if (string.IsNullOrEmpty(txtInput.Text.Trim()))
                        return;
                    lblNotice.Text = "";
                    finalStr = functionformat.FormatBySymbol(txtInput.Text.Trim(), ',', out isSuccess);
                    if (isSuccess)
                        txtInput.Text = finalStr;
                    else
                        lblNotice.Text = finalStr;
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// 转换tabcontrol面板时加载历史数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



        #region 测试记录选项卡
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tab = (TabControl)sender;
            if (tab.SelectedIndex == 1)
            {
                lstHistoryList.Items.Clear();
                string filepath = Directory.GetCurrentDirectory();
                string[] filelist = Directory.GetFiles(filepath, "*.txt");
                foreach (string f in filelist)
                {
                    lstHistoryList.Items.Add(Path.GetFileName(f));
                }
            }
        }

        private void lstHistoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filepath = Directory.GetCurrentDirectory();
            if (lstHistoryList.SelectedItem == null) return;
            lstHistoryList.SelectedItem.ToString();
            StreamReader sr = new StreamReader(filepath + "\\" + lstHistoryList.SelectedItem.ToString());
            txtHistory.Text = sr.ReadToEnd();
        }

        int index = -1;
        private void lstHistoryList_MouseUp(object sender, MouseEventArgs e)
        {
            index = lstHistoryList.IndexFromPoint(new Point(e.X, e.Y));
            if (index != -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    lstHistoryList.SelectedIndex = index;
                    contextMenuStrip1.Show(lstHistoryList, new Point(e.X, e.Y));
                }
            }
        }
        /// <summary>
        /// 打开文档所在目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDialog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }
        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelFile_Click(object sender, EventArgs e)
        {
            string filepath = Directory.GetCurrentDirectory();
            for (int i = 0; i < lstHistoryList.SelectedItems.Count; i++)
            {
                try
                {
                    string fullpath = filepath + "\\" + lstHistoryList.SelectedItems[i].ToString();
                    if (File.Exists(fullpath))
                        File.Delete(fullpath);
                    else
                        MessageBox.Show("不存在文件路径");
                    lstHistoryList.Items.Remove(lstHistoryList.Items[i]);
                    txtHistory.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除错误:" + ex.Message);
                }
            }
        }

        int flag = 0;
        private void txtFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                flag = txtHistory.Text.IndexOf(txtFind.Text.Trim(), flag);
                if (flag < 0)
                {
                    flag = 0;
                    txtHistory.SelectionStart = 0;
                    txtHistory.SelectionLength = 0;
                    MessageBox.Show("已到结尾");
                    return;
                }
                txtHistory.SelectionStart = flag;
                txtHistory.SelectionLength = txtFind.Text.Length;
                flag = flag + txtFind.Text.Length;
                txtHistory.Focus();
            }
        }
        private void txtHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtHistory.SelectionLength == 0)
                    return;
                flag = txtHistory.Text.IndexOf(txtFind.Text.Trim(), txtHistory.SelectionStart + txtHistory.SelectionLength);
                if (flag < 0)
                {
                    flag = 0;
                    txtHistory.SelectionStart = 0;
                    txtHistory.SelectionLength = 0;
                    MessageBox.Show("已到结尾");
                    return;
                }
                txtHistory.SelectionStart = flag;
                txtHistory.SelectionLength = txtFind.Text.Length;
                flag = flag + txtFind.Text.Length;
                txtHistory.Focus();
            }
        }
        #endregion
        #region 窗体放大
        AutoSizeFormClass asc = new AutoSizeFormClass();
        private void Form1_Load(object sender, EventArgs e)
        {
            //asc.controllInitializeSize(this);
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //asc.controlAutoSize(this);
        }
        #endregion

       


        public static DataSet ConvertXMLToDataSet(string xmlData)
        {
            xmlData = ReplaceSpecialCharactersOfXml(xmlData);
            StringReader stream = null;
            XmlTextReader reader = null;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlData);
            if (xmldoc == null) return null;
            try
            {
                if (xmldoc.ChildNodes[0].ChildNodes[0].Name == "RecordList")
                {
                    XmlNodeList nodelist = xmldoc.ChildNodes[0].ChildNodes[0].ChildNodes;
                    if (nodelist == null) return null;
                    DataSet xmlDS = new DataSet();
                    DataSet ReturnXmlDS = null;
                    for (int i = 0; i < nodelist.Count; i++)
                    {

                        stream = new StringReader(nodelist[i].OuterXml);
                        //从stream装载到XmlTextReader
                        reader = new XmlTextReader(stream);
                        xmlDS.ReadXml(reader);
                        if (i == 0)
                        {
                            ReturnXmlDS = xmlDS;
                        }
                        else
                        {
                            ReturnXmlDS.Merge(xmlDS);
                        }
                    }
                    return ReturnXmlDS;
                }
                else
                {

                    DataSet xmlDS = new DataSet();
                    stream = new StringReader(xmlData);
                    //从stream装载到XmlTextReader
                    reader = new XmlTextReader(stream);
                    xmlDS.ReadXml(reader);
                    return xmlDS;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        /// <summary>
        /// XML中特殊字符处理，防止在DataSet.ReadXML时报错     -lulh 20150915
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static string ReplaceSpecialCharactersOfXml(string xmlData)
        {
            string strReturn = "";
            try
            {
                strReturn = xmlData.Replace("&", "&amp;");
                strReturn = strReturn.Replace("'", "&apos;");
                strReturn = strReturn.Replace("\"", "&quot;");
                return strReturn;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
