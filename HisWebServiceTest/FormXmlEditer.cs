using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DataFormat;
using ControlUI;
using AppUI;
using System.Collections;
using System.Drawing;
using InspurSelfService.BankHospitalFramework.Adapter;



/** author by moon  
 * 2016-01-05 修复新增多行时 点击新增第一行内容type 和mode值时不自动填充问题
 *               将新增行同时填充type和mode字段。
 *               删除时增加提示,
 *    2016-01-07 增加方法查找功能，
 *               对工具进行测试。
 * 0216 修正修改节点后节点转移至最后一个节点，
 *      修正修改节点后重新加载xml文档时默认加载第一个文档
 * 0217 添加方法分类
 * 0218 添加datagridview拖拽排序
 *      修复拖拽后单元格不能编辑问题（拖拽改为只能行头拖拽 ）
 * 0219 增加分类节点新增参数
 *      修复新增参数无法新增行问题
 *      ？加载后dsInArgs为空问题，分类新增问题
 *      新增时判断该名称是否存在
 * **/



namespace HisWebServiceTest
{

    public partial class FormXmlEditer : Form
    {

        //全局变量：tab编辑xml
        int h2 = 200;
        XmlDocument xmldoc = null;
        DataSet dsInArgs = null;
        DataSet dsOutArgs = null;
        ////辅助新增行自动添加内容
        int dsInArgsCount = 0;
        int dsOutArgsCount = 0;
        //全局变量：tab测试,tab测试结果
        protected HisWebService.ChisWebServiceSoapClient hisClient;
        XmlFormat xmlformat = null;
        FunctionFormat functionformat = null;
        DynamicWebService.WebServiceAgent agent = null;
        //全局变量：tab 测试结果
        int flag = 0;
        int index = -1;
        //datagridview拖拽
        int IndexDel = 999;
        int IndexDrop = 999;
        //treeView右键菜单新增标志,新增分类标志
        bool isAdd = false;
        string nodetext = "";
        string txtnameText = "";

        public FormXmlEditer()
        {
            //AdapterHis hiscall = new AdapterHis();
            //string str = "";
            //hiscall.NetTest(out str, "", "", "");
            InitializeComponent();
            btnUp.Visible = btnDown.Visible = true;
            txth2.Text = h2.ToString();
            this.Height = 850;
            // this.Width = 1600;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            menuStrip2.Enabled = false;
            //测试tab,测试结果tab加载
            xmlformat = new XmlFormat();
            functionformat = new FunctionFormat();
            txtWSAddress.Text = System.Configuration.ConfigurationManager.AppSettings["WebServiceAddress"];
            bindCombBox();
            string path1 = System.AppDomain.CurrentDomain.BaseDirectory;
            //path1 = path1.Replace(@"HisWebServiceTest\bin\Debug\", @"InspurSelfService.BankHospitalFramework.Adapter\adapter.his.xml");
            //xmlload(path1);
        }

        void ClearAll()
        {
            checkedListBox1.Items.Clear();
            dsInArgs = null;
            dsOutArgs = null;
        }

        #region TabControl选项卡
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tab = (TabControl)sender;
            if (tab.SelectedTab.Text == "测试记录")
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
        #endregion

        #region 菜单操作
        private void 重新加载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //对xml文件进行操作。
            treeView1.Nodes.Clear();
            xmlload(textBox1.Text);
            string methodname = "";
            try
            {
                methodname = checkedListBox1.Items[0].ToString();
            }
            catch (Exception)
            {
            }
            lblMethodName.Text = methodname;
        }
        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 0;
            Export(ref i);
        }
        private void Export(ref int i)
        {
            sfd.Filter = "xml文件|*.xml";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (i == 0)
                sfd.FileName = "Adapter.His.xml";
            else
                sfd.FileName = "CallAdapterHis.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (i == 0)
                {
                    string localFilePath = sfd.FileName.ToString();
                    xmldoc.Save(localFilePath);
                    tsrslblNotice.Text = "Adapter.his.xml文件导出成功," + DateTime.Now.ToLongTimeString();
                    i++;
                    Export(ref i);
                }
                else
                {
                    string localFilePath = sfd.FileName.ToString();
                    AdapterXml2CallAdapterXml(xmldoc).Save(localFilePath);
                    tsrslblNotice.Text += ",CallAdapterHis.xml文件导出成功," + DateTime.Now.ToLongTimeString();
                }
            }
        }
        XmlDocument AdapterXml2CallAdapterXml(XmlDocument xmldoc)
        {
            XmlDocument returnXmldoc = new XmlDocument();
            returnXmldoc.LoadXml(@"<?xml version='1.0' encoding='utf-8' ?><root></root>");
            XmlNodeList nodelist = xmldoc.SelectNodes("root/method");
            foreach (XmlNode node in nodelist)
            {
                //读出method节点子节点returns，处理后添加内容至
                string paramsStr = "<params>";
                XmlNodeList paramslist = node.SelectSingleNode("params").ChildNodes;
                foreach (XmlNode param in paramslist)
                {
                    paramsStr += String.Format(@"<param name='{0}' description='{1}'></param>", param.Attributes["from"] != null ? param.Attributes["from"].Value : "", param.Attributes["description"] != null ? param.Attributes["description"].Value : "");
                }
                paramsStr += "</params>";
                //添加节点至returnXmldoc对象。
                string returnStr = "";
                XmlNodeList returnslist = node.SelectSingleNode("returns").ChildNodes;
                returnStr = String.Format(@"<return type='{0}' description='' ></return>", (returnslist.Count > 1 ? "datatable" : "string"));
                //读出method节点子节点params，添加内容至
                string finalStr = String.Format(@"<method name='{0}' description='{1}'>{2}{3}</method>", node.Attributes["name"] != null ? node.Attributes["name"].Value : "", node.Attributes["description"] != null ? node.Attributes["description"].Value : "", paramsStr, returnStr);
                returnXmldoc.SelectSingleNode("root").InnerXml += finalStr;
            }
            return returnXmldoc;
        }
        private void 添加新参数方法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblMethodName.Text = "";
            if (dsInArgs == null)
            {
                tsrslblNotice.Text = "请将adapter.his.xml文件拖拽至“adpate.his.xml目的地”";
                return;
            }
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is TextBox)
                {
                    TextBox txt = (TextBox)ctl;
                    txt.Text = "";
                }
            }
            dsInArgs.Clear();
            dsOutArgs.Clear();
            DataRow dr = dsInArgs.Tables[0].NewRow();
            dr["to"] = "";
            dr["from"] = "";
            dr["type"] = "";
            dr["mode"] = "";
            dr["description"] = "";
            dsInArgs.Tables[0].Rows.Add(dr);
            DataRow dr1 = dsOutArgs.Tables[0].NewRow();
            dsOutArgs.Tables[0].Rows.Add(dr1);
            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;

            dataGridView1.DataSource = dsInArgs;
            dataGridView2.DataSource = dsOutArgs;
        }
        private void 保存xmldoc对象ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xmldoc.Save(textBox1.Text);
        }
        //分类右键新增参数方法
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            isAdd = true;
            lblMethodName.Text = "";
            if (dsInArgs == null)
            {
                tsrslblNotice.Text = "请将adapter.his.xml文件拖拽至“adpate.his.xml目的地”";
                return;
            }
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is TextBox)
                {
                    TextBox txt = (TextBox)ctl;
                    txt.Text = "";
                }
            }
            dsInArgs.Clear();
            dsOutArgs.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;

            dataGridView1.DataSource = dsInArgs.Tables[0];
            dataGridView2.DataSource = dsOutArgs.Tables[0];
        }
        #endregion

        #region 拖拽读取文件
        private void checkedListBox1_DragDrop(object sender, DragEventArgs e)
        {
            treeView1.Nodes.Clear();
            ClearAll();
            var filename = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox1.Text = filename[0];
            //加载方法列表，并在groupbox1和groupbox2中显示方法列表中第一条入参和出参情况
            //对xml文件进行操作。
            if (xmlload(filename[0]))
            {
                menuStrip2.Enabled = true;
                string methodname = "";
                try
                {
                    methodname = checkedListBox1.Items[0].ToString();
                }
                catch (Exception)
                {
                }
                lblMethodName.Text = methodname;
            }
            else
            {
                tsrslblNotice.Text = "加载xml文件失败" + DateTime.Now.ToLongTimeString();
            }
        }
        private void checkedListBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }
        #endregion

        #region 左侧菜单列表
        //treeView选择事件
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                isAdd = false;
                ClearAll();
                string type = treeView1.SelectedNode.Text;
                //读取xml节点加载至checkedlistbox
                XmlNodeList nodeListFunction = xmldoc.SelectSingleNode("root").ChildNodes;
                int i = 0;
                foreach (XmlNode node in nodeListFunction)
                {
                    if (node is XmlComment)
                    {
                        XmlComment commnet = node as XmlComment;
                        if (type == commnet.InnerText)
                        {
                            i = 1;
                            continue;
                        }
                    }
                    if (node is XmlComment)
                    {
                        if (i == 1)
                            break;
                        else
                            continue;
                    }
                    if (i == 1)
                    {
                        checkedListBox1.Items.Add(node.Attributes["name"].Value);
                    }
                }
                if (checkedListBox1.Items.Count != 0)
                    loadXmlNodeAddControl(xmldoc, checkedListBox1.Items[0].ToString());
            }
        }
        //treeView
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo info = this.treeView1.HitTest(e.X, e.Y);
            if (info.Node == null) return;
            TreeNode tnode = info.Node;
            treeView1.SelectedNode = tnode;
            nodetext = info.Node.Text;
            if (nodetext != "")
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip2.Show(treeView1, new Point(e.X, e.Y));
                }
            }
            dsInArgsCount = 0;
            dsOutArgsCount = 0;
        }
        //辅助读取xml方法
        public static string ReadFile(string strPath)
        {
            FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Read,
    FileShare.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string contents = sr.ReadToEnd();
            fs.Close();
            sr.Close();
            return contents;
        }
        //【左侧方法列表加载（checkListBox1）】
        bool xmlload(string filepath)
        {
            try
            {
                string xmlContent = ReadFile(filepath).Replace(" xmlns=\"http://inspur.com/ihss/Validation\"", "");
                xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlContent);
                //读取xml节点至treeView
                XmlNodeList nodeListall = xmldoc.SelectSingleNode("root").ChildNodes;
                foreach (XmlNode node in nodeListall)
                {
                    if (node is XmlComment)
                    {
                        XmlComment comment = node as XmlComment;
                        treeView1.Nodes.Add(comment.InnerText);
                    }
                }
                //读取xml节点加载至checkedlistbox
                XmlNodeList nodeListFunction = xmldoc.SelectSingleNode("root").ChildNodes;
                int i = 0;
                foreach (XmlNode node in nodeListFunction)
                {
                    if (node is XmlComment && (((XmlComment)node).InnerText == treeView1.Nodes[0].Text))
                    {
                        i = 1;
                        continue;
                    }
                    if (node is XmlComment && (((XmlComment)node).InnerText != treeView1.Nodes[0].Text))
                    {
                        break;
                    }
                    if (i == 1)
                    {
                        checkedListBox1.Items.Add(node.Attributes["name"].Value);
                    }
                }
                if (checkedListBox1.Items.Count != 0)
                    loadXmlNodeAddControl(xmldoc, checkedListBox1.Items[0].ToString());
                tsrslblNotice.Text = "文件-" + textBox1.Text + "-加载成功," + DateTime.Now.ToLongTimeString();
            }
            catch (Exception ex)
            {
                tsrslblNotice.Text = "文件-" + textBox1.Text + "-加载失败," + DateTime.Now.ToLongTimeString() + "失败原因:" + (ex.Message.Length > 30 ? ex.Message.Substring(0, 29) : ex.Message);
                return false;
            }
            return true;
        }
        bool xmlload(string filepath, string methodname)
        {
            try
            {

                string xmlContent = ReadFile(filepath).Replace(" xmlns=\"http://inspur.com/ihss/Validation\"", "");
                xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlContent);
                //xmldoc = new XmlDocument();
                //xmldoc.Load(filepath);
                XmlNodeList nodeListFunction = xmldoc.SelectNodes("root/method");
                foreach (XmlNode node in nodeListFunction)
                {
                    checkedListBox1.Items.Add(node.Attributes["name"].Value);
                }
                if (checkedListBox1.Items.Count != 0)
                    loadXmlNodeAddControl(xmldoc, methodname);
                tsrslblNotice.Text = "文件-" + textBox1.Text + "-加载成功," + DateTime.Now.ToLongTimeString();
            }
            catch (Exception ex)
            {
                tsrslblNotice.Text = "文件-" + textBox1.Text + "-加载失败," + DateTime.Now.ToLongTimeString() + "失败原因:" + (ex.Message.Length > 30 ? ex.Message.Substring(0, 29) : ex.Message);
                return false;
            }
            return true;
        }
        //【左侧方法列表点击事件】
        private void checkedListBox1_Click(object sender, EventArgs e)
        {
            CheckedListBox list = sender as CheckedListBox;
            if (list.SelectedItem == null) return;
            lblMethodName.Text = list.SelectedItem.ToString();
            switch (tabControl1.SelectedTab.Text)
            {
                case "Xml编辑":
                    loadXmlNodeAddControl(xmldoc, lblMethodName.Text);
                    break;
                case "测试":
                    //todo:组织xml串
                    bool iSuccess;
                    string inxml = SetParamsDictionaryXml(textBox1.Text, lblMethodName.Text);
                    txtInput.Text = xmlformat.xmlFormatByStock(inxml, out iSuccess);
                    break;
                default: break;

            }

        }
        int CheckListBoxFind(string content)
        {
            checkedListBox1.Items.Contains(lblMethodName.Text);
            return checkedListBox1.Items.IndexOf(lblMethodName.Text);
        }
        bool updateChecklistbox(int index, string methodname)
        {
            checkedListBox1.Items[index] = methodname;
            return true;
        }
        #region 删除选择，全选，反选
        private void btnDelSelect_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除?", "提示", MessageBoxButtons.YesNo) == DialogResult.No) return;
            int checkedcount = checkedListBox1.CheckedItems.Count;
            if (checkedcount != 0)
            {
                string error = "";
                string finalerror = "";
                for (int i = 0; i < checkedcount; i++)
                {
                    string methodname = checkedListBox1.CheckedItems[i].ToString();
                    XmlHelper.DelXmlNode(ref xmldoc, methodname, "method", out error);
                    xmldoc.Save(textBox1.Text);
                    //更新左侧列表
                    //并重新加载上一项或者下一项，如果本类无内容，则不予加载。
                    int intindex= checkedListBox1.Items.IndexOf(checkedListBox1.CheckedItems[i]);
                    checkedListBox1.Items.RemoveAt(intindex);
                    if (checkedListBox1.Items.Count == 0)
                    {
                       //分类下的所有方法为空了如何处理

                    }
                    else
                    {
                        //分类下方法不为空的处理，加载上一项，或者下一项（下一项index与删除项index相同）。
                        if (intindex - 1 >= 0)
                            intindex = intindex - 1;
                         loadXmlNodeAddControl(xmldoc, checkedListBox1.Items[intindex].ToString());
                    }
                    if (error != "")
                        finalerror += error + ",";
                }
                if (finalerror == "")
                    tsrslblNotice.Text = "选中的" + checkedcount.ToString() + "个方法已删除成功" + DateTime.Now.ToLongTimeString();
                else
                    tsrslblNotice.Text = "选中的" + checkedcount.ToString() + "个方法删除失败" + DateTime.Now.ToLongTimeString() + "错误信息:" + (finalerror.Length > 30 ? finalerror.Substring(0, 29) : finalerror);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < checkedListBox1.Items.Count; j++)
                checkedListBox1.SetItemChecked(j, true);
        }

        private void btnSelectOther_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < checkedListBox1.Items.Count; j++)
            {
                if (checkedListBox1.GetItemChecked(j))
                {
                    checkedListBox1.SetItemChecked(j, false);
                }
                else
                {
                    checkedListBox1.SetItemChecked(j, true);
                }
            }

        }
        #endregion
        #endregion

        #region Tab:xml编辑
        #region 右侧主参数及出入参datagridview
        //【DataGridView出入参加载 Panel1方法属性加载】
        bool loadXmlNodeAddControl(XmlDocument xmldoc, string methodname)
        {
            try
            {
                //加载方法列表内的参数
                XmlNode nodeListParams = xmldoc.SelectSingleNode(string.Format("root/method[@name='{0}']", methodname));
                XmlNode list = null;
                XmlNode returnList = null;
                list = nodeListParams.SelectSingleNode("params");
                returnList = nodeListParams.SelectSingleNode("returns");
                dsInArgs = XmlHelper.ConvertXmlFileToDS(list);
                dsOutArgs = XmlHelper.ConvertXmlFileToDS(returnList);
                if (BindDataGridView(dsInArgs, dataGridView1))
                    dsInArgsCount = dsInArgs.Tables[0].Rows.Count;
                if (BindDataGridView(dsOutArgs, dataGridView2))
                    dsOutArgsCount = dsOutArgs.Tables[0].Rows.Count;
                //加载方法列表
                XmlNodeList nodeList = xmldoc.SelectNodes(string.Format("root/method[@name='{0}']", methodname));
                loadFunctionArgs(nodeList);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载控件异常" + ex.Message);
                return false;
            }

        }
        //datagridview绑定
        bool BindDataGridView(DataSet ds, DataGridView dgv)
        {
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                dgv.DataSource = ds.Tables[0];
                UI_DataGridView.AutoSizeColumn(dgv);
                return true;
            }
            return false;
        }
        //【Panel1加载方法参数】
        bool loadFunctionArgs(XmlNodeList list)
        {
            try
            {
                foreach (XmlNode node in list)
                {
                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        string value = node.Attributes[i].Value;
                        switch (node.Attributes[i].Name)
                        {
                            case "name":
                                txtname.Text = value;
                                txtnameText = value;
                                break;
                            case "call":
                                txtcall.Text = value;
                                break;
                            case "log":
                                txtlog.Text = value;
                                break;
                            case "description":
                                txtdescription.Text = value;
                                break;
                            default: break;
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }
        //【自动填充出入参固定内容】
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {//dataGridView1.CurrentCell.ColumnIndex == 0 &&
            if ((dataGridView1.CurrentRow.Index >= (dsInArgsCount - 1)))
            {
                dataGridView1.CurrentRow.Cells["mode"].Value = "system";
                dataGridView1.CurrentRow.Cells["type"].Value = "string";
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == 4 && (dataGridView1.CurrentRow.Index >= (dsInArgsCount - 1)))
                dataGridView1.CurrentCell.Value = "system";
            else if (dataGridView1.CurrentCell.ColumnIndex == 5 && (dataGridView1.CurrentRow.Index >= (dsInArgsCount - 1)))
                dataGridView1.CurrentCell.Value = "string";
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {//dataGridView2.CurrentCell.ColumnIndex == 0 &&
            if ((dataGridView2.CurrentRow.Index > dsInArgsCount - 1))
            {
                dataGridView2.CurrentRow.Cells["mode1"].Value = "system";
                dataGridView2.CurrentRow.Cells["type1"].Value = "string";
            }
            else if (dataGridView2.CurrentCell.ColumnIndex == 4 && (dataGridView2.CurrentRow.Index > dsInArgsCount - 1))
                dataGridView2.CurrentCell.Value = "system";
            else if (dataGridView2.CurrentCell.ColumnIndex == 5 && (dataGridView2.CurrentRow.Index > dsInArgsCount - 1))
                dataGridView2.CurrentCell.Value = "string";
        }
        #endregion

        #region 保存操作
        //入参
        private void btnSaveInArgs_Click(object sender, EventArgs e)
        {
            if (isAdd == true) return;
            if (dsInArgs == null)
            {
                tsrslblNotice.Text = "请将adapter.his.xml文件拖拽至“adpate.his.xml目的地”";
                return;
            }
            dsInArgs = dsInArgs.GetChanges();
            string insertString = "";
            foreach (DataRow dr in dsInArgs.Tables[0].Rows)
            {
                insertString += String.Format(@"<param to='{0}' from='{1}' mode='{2}' type='{3}' description='{4}'></param>", dr["to"].ToString(), dr["from"].ToString(), dr["mode"].ToString(), dr["type"].ToString(), dr["description"].ToString());
            }
            string error;
            try
            {
                if (XmlHelper.AddXmlNode(ref xmldoc, lblMethodName.Text, "params", insertString, null, null, "", out error)) tsrslblNotice.Text = lblMethodName.Text + "方法，入参保存成功。" + DateTime.Now.ToLongTimeString();
                xmldoc.Save(textBox1.Text);
                xmlload(textBox1.Text);
            }
            catch (Exception ex)
            {
                tsrslblNotice.Text = lblMethodName.Text + "方法，入参保存失败。" + DateTime.Now.ToLongTimeString() + "错误信息：" + (ex.Message.Length > 30 ? ex.Message.Substring(0, 29) : ex.Message);
                throw;
            }
        }
        //出参
        private void btnSaveOutArgs_Click(object sender, EventArgs e)
        {
            if (isAdd == true) return;
            if (dsInArgs == null)
            {
                tsrslblNotice.Text = "请将adapter.his.xml文件拖拽至“adpate.his.xml目的地”";
                return;
            }
            dsOutArgs = dsOutArgs.GetChanges();
            string insertString = "";
            foreach (DataRow dr in dsOutArgs.Tables[0].Rows)
            {
                insertString += String.Format(@"<return to='{0}' from='{1}' mode='{2}' type='{3}' description='{4}'></return>", dr["to"].ToString(), dr["from"].ToString(), dr["mode"].ToString(), dr["type"].ToString(), dr["description"].ToString());
            }
            string error;
            XmlHelper.AddXmlNode(ref xmldoc, lblMethodName.Text, "params", insertString, null, null, "", out error);
            xmldoc.Save(textBox1.Text);
            xmlload(textBox1.Text);
        }
        //全部
        private void btnSaveNewFunc_Click(object sender, EventArgs e)
        {
            if (dsInArgs == null)
            {
                tsrslblNotice.Text = "请将adapter.his.xml文件拖拽至“adpate.his.xml目的地”" + DateTime.Now.ToLongTimeString();
                return;
            }
            CheckIsExsit();
            string error;

            string insertStr = "";//String.Format(@"<method name='{0}' call='{1}' log='{2}' description='{3}' >", txtname.Text, txtcall.Text, txtlog.Text, txtdescription.Text);
            //添加入参内容
            dsInArgs = dsInArgs.GetChanges();
            insertStr += "<params>";
            foreach (DataRow dr in dsInArgs.Tables[0].Rows)
            {
                insertStr += String.Format(@"<param to='{0}' from='{1}' mode='{2}' type='{3}' description='{4}'></param>", dr["to"].ToString(), dr["from"].ToString(), dr["mode"].ToString(), dr["type"].ToString(), dr["description"].ToString());
            }
            insertStr += "</params>";

            //添加出参内容
            insertStr += "<returns>";
            dsOutArgs = dsOutArgs.GetChanges();
            foreach (DataRow dr in dsOutArgs.Tables[0].Rows)
            {
                insertStr += String.Format(@"<return to='{0}' from='{1}' mode='{2}' type='{3}' description='{4}'></return>", dr["to"].ToString(), dr["from"].ToString(), dr["mode"].ToString(), dr["type"].ToString(), dr["description"].ToString());
            }
            insertStr += "</returns>";
            //method节点尾
            //insertStr += "</method>";
            try
            {
                //将xml string 转换成xmlnode
                XmlNode nodeinsert = xmldoc.CreateElement("method");
                XmlAttribute attname = xmldoc.CreateAttribute("name");
                attname.Value = txtname.Text;
                nodeinsert.Attributes.Append(attname);
                //新增节点最外层属性 赋值后无法使用
                ////String.Format(@"<method name='{0}' call='{1}' log='{2}' description='{3}' >", txtname.Text, txtcall.Text, txtlog.Text, txtdescription.Text);
                addattribute(nodeinsert, "name", txtname.Text);
                addattribute(nodeinsert, "call", txtcall.Text);
                addattribute(nodeinsert, "log", txtlog.Text);
                addattribute(nodeinsert, "description", txtdescription.Text);
                //XmlAttribute attcall = xmldoc.CreateAttribute("name");
                //attname.Value = txtname.Text;
                //nodeinsert.Attributes.Append(attname);
                nodeinsert.InnerXml = insertStr;
                //0217改为先插入后删除
                XmlNode node = null;
                node = xmldoc.SelectSingleNode(string.Format("root/method[@name='{0}']", lblMethodName.Text));
                //新增时找到分类最后一个方法
                if (isAdd == true)
                {
                    XmlNodeList nodelist = xmldoc.SelectSingleNode("root").ChildNodes;
                    int w = 0;
                    for (int i = 0; i < nodelist.Count; i++)
                    {
                        if (nodelist[i] is XmlComment)
                        {
                            if (nodelist[i].InnerText == nodetext)
                            {
                                if (w == 0)
                                {
                                    w = 1;
                                    continue;
                                }
                            }
                            if (w == 1)
                            {
                                node = nodelist[i - 1];
                                break;
                            }
                        }
                    }
                }
                if (node != null)
                {
                    XmlHelper.AddXmlNode(ref xmldoc, lblMethodName.Text, "method", insertStr, nodeinsert, node, "insertafter", out error);
                    if (isAdd == false)
                    {
                        XmlHelper.DelXmlNode(ref xmldoc, lblMethodName.Text, "method", out error);
                    }
                    xmldoc.Save(textBox1.Text);
                }
                if (isAdd == false)
                {
                    int index1 = CheckListBoxFind(lblMethodName.Text);
                    updateChecklistbox(index1, txtname.Text);
                }
                else
                {
                    checkedListBox1.Items.Add(txtname.Text);
                }

                //xmlload(textBox1.Text, txtname.Text);
                tsrslblNotice.Text = "参数名称:" + txtname.Text + "及相关出入参保存成功," + DateTime.Now.ToLongDateString();
            }
            catch (Exception ex)
            {
                tsrslblNotice.Text = "新增参数" + txtname.Text + "及相关出入参保存失败," + DateTime.Now.ToLongDateString() + "失败原因:" + (ex.Message.Length > 30 ? ex.Message.Substring(0, 29) : ex.Message);
            }
        }
        void addattribute(XmlNode node, string name, string value)
        {
            XmlAttribute attname = xmldoc.CreateAttribute(name);
            attname.Value = value;
            node.Attributes.Append(attname);
        }
        #endregion

        #region 【出入参显示框长短变】
        private void btnUp_Click(object sender, EventArgs e)
        {
            int f = int.Parse(txth2.Text);
            if (btnUp.Visible == true && btnDown.Visible == true)
            {
                btnUp.Visible = false;
                dataGridView1.Height -= f;

                dataGridView2.Top -= f;
                dataGridView2.Height += f;
            }

            if (btnUp.Visible == true && btnDown.Visible == false)
            {
                btnDown.Visible = true;
                dataGridView1.Height -= f;
                dataGridView2.Top -= f;
                dataGridView2.Height += f;
            }
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            int f = int.Parse(txth2.Text);
            if (btnUp.Visible == true && btnDown.Visible == true)
            {
                btnDown.Visible = false;
                dataGridView1.Height += f;
                dataGridView2.Top += f;
                dataGridView2.Height -= f;

            }
            if (btnUp.Visible == false && btnDown.Visible == true)
            {
                btnUp.Visible = true;
                dataGridView1.Height += f;
                dataGridView2.Top += f;
                dataGridView2.Height -= f;
            }
        }
        //变化范围
        private void txth2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #endregion

        #region Tab:测试
        //绑定Combox
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
        private void btnTest_Click(object sender, EventArgs e)
        {
            lblNotice.Text = "";
            if (string.IsNullOrEmpty(txtInput.Text.Trim()))
            {
                tsrslblNotice.Text = "请输入发送字符串" + DateTime.Now.ToLocalTime();
                txtInput.Focus();
                return;
            }
            switch (comboBox1.SelectedValue.ToString())
            {
                case "1":
                    string outPut = "";
                    //todo:调用HisTrade方法
                    //流水号生成，resultCodeMsg声明，log，terminalip为10.10.10.10，
                    // HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
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
                    { tsrslblNotice.Text = "WebService地址不能为空"; return; }
                    agent = new DynamicWebService.WebServiceAgent(txtWSAddress.Text.Trim());
                    if (!agent.EstablishSuccess) { tsrslblNotice.Text = "链接未建立成功。详细错误信息：" + agent.ErrorMessage; return; }
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
                    { tsrslblNotice.Text = "请输入调用方法名，以及参数，均用逗号分隔"; return; }
                    string[] functionAndArgs = txtInput.Text.Trim().Replace("\r\n", ",").Split(',');
                    object[] args = new object[functionAndArgs.Length - 1];
                    for (int i = 1; i < functionAndArgs.Length; i++)
                    {
                        args[i - 1] = functionAndArgs[i].ToString().Trim();
                    }
                    object outPut1 = agent.Invoke(functionAndArgs[0].ToString().Trim(), args);
                    if (outPut1.ToString() == "failure")
                    {
                        tsrslblNotice.Text = agent.ErrorMessage;
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
        //保存输入，输出xml至文本文档
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
        //清空输入，输出
        private void txtClearInput_Click(object sender, EventArgs e)
        {
            txtInput.Text = "";
        }
        private void txtClearOutput_Click(object sender, EventArgs e)
        {
            txtOutPut.Text = "";
        }
        //自动整理入参内容
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
        #endregion

        #region Tab:测试记录
        //列表选中变化
        private void lstHistoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filepath = Directory.GetCurrentDirectory();
            if (lstHistoryList.SelectedItem == null) return;
            lstHistoryList.SelectedItem.ToString();
            StreamReader sr = new StreamReader(filepath + "\\" + lstHistoryList.SelectedItem.ToString());
            txtHistory.Text = sr.ReadToEnd();
        }
        //右键显示文件操作：打开文件位置，删除文件
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

        //打开文件位置
        private void OpenDialog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }
        //删除文件
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
                        tsrslblNotice.Text = "不存在文件路径" + DateTime.Now.ToLongTimeString();
                    lstHistoryList.Items.Remove(lstHistoryList.Items[i]);
                    txtHistory.Text = "";
                }
                catch (Exception ex)
                {
                    tsrslblNotice.Text = "删除错误:" + DateTime.Now.ToLongTimeString();
                }
            }

        }
        //查找功能
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

        private void checkedListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyc = e.KeyCode;
            Keys keyd = e.KeyData;
            int keyv = e.KeyValue;
        }

        //实现拖拽变换行顺序
        //点击后将row单独 读取并保存至DataRow中

        //DrugDrop事件判断为DGV哪一行
        //在后台进行数据重排
        //将数据插入，将原数据行删除，重新绑定table
        //todo:drugdrop在最后释放的位置选中行
        #region datagridview拖拽行头排序
        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.dataGridView1.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = this.dataGridView1.HitTest(p.X, p.Y);
            DataRow dr = (DataRow)e.Data.GetData(typeof(DataRow));
            //获取行名称
            IndexDrop = hit.RowIndex;
            if (IndexDrop == IndexDel) return;
            //操作gridview行
            //dataGridView1.Rows.RemoveAt(IndexDel);
            //dataGridView1.Rows.Insert(index, dgvr);
            //重新绑定
            DataTable dt = dsInArgs.Tables[0];
            if (IndexDrop == -1) return;
            if (IndexDel > IndexDrop)
            {
                dt.Rows.Remove(dt.Rows[IndexDel]);
                dt.Rows.InsertAt(dr, IndexDrop);
            }
            else if ((IndexDel + 1) < IndexDrop)
            {
                dt.Rows.Remove(dt.Rows[IndexDel]);
                dt.Rows.InsertAt(dr, IndexDrop);
            }
            dsInArgs.Tables.Clear();
            dsInArgs.Tables.Add(dt);
            dataGridView1.DataSource = dsInArgs.Tables[0];
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (dsInArgs == null) return;
            DataRow dr = dsInArgs.Tables[0].NewRow();
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo info = this.dataGridView1.HitTest(e.X, e.Y);
                if ((info.RowIndex) >= 0 && info.Type == DataGridViewHitTestType.RowHeader)
                {
                    IndexDel = info.RowIndex;
                    dr["to"] = dataGridView1.Rows[IndexDel].Cells["to"].Value;
                    dr["from"] = dataGridView1.Rows[IndexDel].Cells["from"].Value;
                    dr["mode"] = dataGridView1.Rows[IndexDel].Cells["mode"].Value;
                    dr["type"] = dataGridView1.Rows[IndexDel].Cells["type"].Value;
                    dr["description"] = dataGridView1.Rows[IndexDel].Cells["description"].Value;
                    // dr["defaultvalue"] = dataGridView1.Rows[IndexDel].Cells["defaultvalue"] != null ? dataGridView1.Rows[IndexDel].Cells["defaultvalue"].ToString() : "";
                    this.dataGridView1.DoDragDrop(dr, DragDropEffects.Move);
                }
            }
        }

        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (dsOutArgs == null) return;
            DataRow dr = dsOutArgs.Tables[0].NewRow();
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo info = this.dataGridView2.HitTest(e.X, e.Y);
                if ((info.RowIndex) >= 0 && info.Type == DataGridViewHitTestType.RowHeader)
                {
                    IndexDel = info.RowIndex;
                    dr["to"] = dataGridView2.Rows[IndexDel].Cells["to1"].Value;
                    dr["from"] = dataGridView2.Rows[IndexDel].Cells["from1"].Value;
                    dr["mode"] = dataGridView2.Rows[IndexDel].Cells["mode1"].Value;
                    dr["type"] = dataGridView2.Rows[IndexDel].Cells["type1"].Value;
                    dr["description"] = dataGridView2.Rows[IndexDel].Cells["description1"].Value;
                    //// dr["defaultvalue"] = dataGridView2.Rows[IndexDel].Cells["defaultvalue"] != null ? dataGridView2.Rows[IndexDel].Cells["defaultvalue"].ToString() : "";
                    this.dataGridView2.DoDragDrop(dr, DragDropEffects.Move);
                }
            }
        }

        private void dataGridView2_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.dataGridView2.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = this.dataGridView2.HitTest(p.X, p.Y);
            DataRow dr = (DataRow)e.Data.GetData(typeof(DataRow));
            //获取行名称
            IndexDrop = hit.RowIndex;
            if (IndexDrop == IndexDel) return;
            //操作gridview行
            //dataGridView2.Rows.RemoveAt(IndexDel);
            //dataGridView2.Rows.Insert(index, dgvr);
            //重新绑定
            DataTable dt = dsOutArgs.Tables[0];

            if (IndexDel > IndexDrop)
            {
                dt.Rows.Remove(dt.Rows[IndexDel]);
                dt.Rows.InsertAt(dr, IndexDrop);
            }
            else if ((IndexDel + 1) < IndexDrop)
            {
                dt.Rows.Remove(dt.Rows[IndexDel]);
                dt.Rows.InsertAt(dr, IndexDrop);
            }
            dsOutArgs.Tables.Clear();
            dsOutArgs.Tables.Add(dt);
            dataGridView2.DataSource = dsOutArgs.Tables[0];
        }

        private void dataGridView2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        #endregion
        //todo:


        public static string SetParamsDictionaryXml(string xmlFile, string methodName)
        {
            string paramsXml = "<params></params>";
            XmlDocument paramsXmlDoc = new XmlDocument();
            paramsXmlDoc.LoadXml(paramsXml);

            try
            {
                // string xmlDoc = String.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, xmlFile);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlFile);
                XmlNodeList list = null;
                XmlNode methodNode = xmldoc.SelectSingleNode(String.Format("root/method[@name='{0}']/params", methodName));
                if (methodName != null) { list = methodNode.ChildNodes; }

                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        string name = list[i].Attributes["from"] != null ? list[i].Attributes["from"].Value : null;

                        XmlNode paramNode = paramsXmlDoc.CreateElement(name);
                        paramNode.InnerText = list[i].Attributes["defaultvalue"] != null ? list[i].Attributes["defaultvalue"].Value : null;
                        paramsXmlDoc.LastChild.AppendChild(paramNode);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (paramsXmlDoc.ChildNodes.Count > 0)
                paramsXml = paramsXmlDoc.OuterXml;
            else
                paramsXml = null;

            return paramsXml;
        }

        private void txtname_MouseLeave(object sender, EventArgs e)
        {
            CheckIsExsit();
        }
        void CheckIsExsit()
        {
            if (xmldoc == null) return;
            if (isAdd == false && txtnameText == txtname.Text) return;
            XmlNode hasnode = xmldoc.SelectSingleNode(string.Format("root/method[@name='{0}']", txtname.Text));
            if (hasnode != null)
            {
                do
                {
                    hasnode = hasnode.PreviousSibling;
                }
                while (!(hasnode.PreviousSibling is XmlComment));
                XmlComment comment = hasnode.PreviousSibling as XmlComment;
                MessageBox.Show("“" + comment.InnerText + "”分类下，已存在名称为“" + txtname.Text + "”的方法。");
                return;
            }
        }













    }
}
