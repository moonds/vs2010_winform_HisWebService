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

namespace HisWebServiceTest
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        XmlDocument xmldoc = null;

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            var filename = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox1.Text = filename[0];
            //todo:加载方法列表，并在groupbox1和groupbox2中显示方法列表中第一条入参和出参情况
            //对xml文件进行操作。
            xmlload(filename[0]);
            loadXmlNodeAddControl(xmldoc, checkedListBox1.Items[0].ToString());
            lblMethodName.Text = checkedListBox1.Items[0].ToString();
        }
        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }
        /// <summary>
        /// 辅助读取xml方法
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 从xml文件中加载方法列表
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        bool xmlload(string filepath)
        {
            try
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(filepath);
                DataSet ds= XmlHelper.ConvertXmlFileToDS(null);
                XmlNodeList nodeListFunction = xmldoc.SelectNodes("root/method");
                foreach (XmlNode node in nodeListFunction)
                {
                    checkedListBox1.Items.Add(node.Attributes["name"].Value);
                }
                loadXmlNodeAddControl(xmldoc, checkedListBox1.Items[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常信息：" + ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 根据具体方法名称加载入参与出参
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="methodname"></param>
        /// <returns></returns>
        bool loadXmlNodeAddControl(XmlDocument xmldoc, string methodname)
        {
            try
            {
                //加载方法列表内的参数对
                XmlNode nodeListParams = xmldoc.SelectSingleNode(string.Format("root/method[@name='{0}']", methodname));
                XmlNodeList list = null;
                XmlNodeList returnList = null;
                list = nodeListParams.SelectSingleNode("params").ChildNodes;
                returnList = nodeListParams.SelectSingleNode("returns").ChildNodes;
                addControltoPanel(panel1, list, methodname);
                addControltoPanel(panel2, returnList, methodname);
                //加载方法列表
                XmlNodeList nodeList = xmldoc.SelectNodes(string.Format("root/method[@name='{0}']", methodname));
                addControltoGroupBox(groupBox3, nodeList);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载控件异常" + ex.Message);
                return false;
            }

        }
        /// <summary>
        /// 加载参数至GroupBox：grp
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        bool addControltoGroupBox(GroupBox grp, XmlNodeList list)
        {

            try
            {
                grp.Controls.Clear();
                int x = 10;
                int y = 15;
                foreach (XmlNode node in list)
                {
                    Label lbl = null;
                    TextBox txt = null;
                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        lbl = new Label();
                        lbl.Left = x;
                        lbl.Top = y;
                        lbl.Text = node.Attributes[i].Name;
                        lbl.Name = "lbl" + lbl.Text;
                        lbl.Width = 35;
                        if (lbl.Text == "description")
                            lbl.Width = 60;
                        lbl.Height = 50;
                        x += lbl.Width + 5;
                        if (lbl.Width > grp.Width - x)
                        {
                            x = 10;
                            y += 50 + 5;
                        }
                        grp.Controls.Add(lbl);
                        txt = new TextBox();
                        txt.Left = x;
                        txt.Top = y;
                        txt.Name = "txt" + node.Attributes[i].Name;
                        txt.Text = node.Attributes[i].Value;
                        txt.Width = 60;
                        if (lbl.Text == "description")
                            txt.Width = 120;
                        txt.Height = 50;
                        x += txt.Width + 10;
                        if (txt.Width > grp.Width - x)
                        {
                            x = 10;
                            y += 50 + 5;
                        }
                        grp.Controls.Add(txt);
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
        /// <summary>
        /// 加载参数至GroupBox：grp
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        bool addControltoPanel(Panel grp, XmlNodeList list, string methodname)
        {
            try
            {
                grp.Controls.Clear();
                int x = 10;
                int y = 15;
                foreach (XmlNode node in list)
                {
                    Label lbl = null;
                    TextBox txt = null;
                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        lbl = new Label();
                        lbl.Left = x;
                        lbl.Top = y;
                        lbl.Text = node.Attributes[i].Name;
                        lbl.Name = "lbl" + methodname + node.Attributes[i].Name + i;
                        lbl.Width = 35;
                        if (lbl.Text == "description")
                            lbl.Width = 60;
                        lbl.Height = 50;
                        x += lbl.Width + 5;
                        if (lbl.Width > grp.Width - x)
                        {
                            x = 10;
                            y += 50 + 5;
                        }
                        grp.Controls.Add(lbl);
                        txt = new TextBox();
                        txt.Left = x;
                        txt.Top = y;
                        txt.Name = "txt" + methodname + node.Attributes[i].Name + i;
                        txt.Text = node.Attributes[i].Value;
                        txt.Width = 60;
                        if (lbl.Text == "description")
                            txt.Width = 120;
                        txt.Height = 50;
                        x += txt.Width + 10;
                        if (txt.Width > grp.Width - x)
                        {
                            x = 10;
                            y += 50 + 5;
                        }
                        grp.Controls.Add(txt);
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
        /// <summary>
        /// 加载参数至GroupBox：grp
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        //bool addControltoPanel(TableLayoutPanel grp, XmlNodeList list)
        //{
        //    try
        //    {
        //        grp.Controls.Clear();
        //        int x = 10;
        //        int y = 15;
        //        int j = 0;
        //        foreach (XmlNode node in list)
        //        {
        //            Button btn = new Button();
        //            btn.Name = j;
        //            btn.Width = 30;
        //            btn.Height = 50;
        //            btn.Text = "Del";
        //            btn.Click += new System.EventHandler(btn_Click);
        //            Label lbl = null;
        //            TextBox txt = null;
        //            for (int i = 0; i < node.Attributes.Count; i++)
        //            {
        //                lbl = new Label();
        //                lbl.Left = x;
        //                lbl.Top = y;
        //                lbl.Text = node.Attributes[i].Name;
        //                lbl.Name = "lbl" + lbl.Text;
        //                lbl.Width = 35;
        //                if (lbl.Text == "description")
        //                    lbl.Width = 60;
        //                lbl.Height = 50;
        //                x += lbl.Width + 5;
        //                if (lbl.Width > grp.Width - x)
        //                {
        //                    x = 10;
        //                    y += 50 + 5;
        //                }
        //                grp.Controls.Add(lbl);
        //                txt = new TextBox();
        //                txt.Left = x;
        //                txt.Top = y;
        //                txt.Name = "txt" + node.Attributes[i].Name;
        //                txt.Text = node.Attributes[i].Value;
        //                txt.Width = 60;
        //                if (lbl.Text == "description")
        //                    txt.Width = 120;
        //                txt.Height = 50;
        //                x += txt.Width + 10;
        //                if (txt.Width > grp.Width - x)
        //                {
        //                    x = 10;
        //                    y += 50 + 5;
        //                }
        //                grp.Controls.Add(txt);
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }
        //}

        /// <summary>
        /// 保存当前结点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCurrent_Click(object sender, EventArgs e)
        {
            //todo:1通过当前方法名称找到对应节点，删除子节点内容，添加新内容
            //todo:2通过当前方法名称找到对应节点，判断节点内容是否有，有修改，无添加。
            XmlNode nodeListParams = xmldoc.SelectSingleNode(string.Format("root/method[@name='{0}']", lblMethodName.Text));
            XmlNodeList list = nodeListParams.SelectSingleNode("params").ChildNodes;
            XmlNodeList returnList = nodeListParams.SelectSingleNode("returns").ChildNodes;
            int i = 0;
            foreach (XmlNode node in list)
            {
                XmlElement xe = (XmlElement)node;
                string frontStr = "txt" + lblMethodName.Text;

                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is TextBox)
                    {
                        if (ctl.Name == frontStr + "to" + i)
                        {
                            xe.SetAttribute("to", ctl.Text);
                        }
                        if (ctl.Name == frontStr + "from" + i)
                        {
                            xe.SetAttribute("from", ctl.Text);
                        }
                        if (ctl.Name == frontStr + "type" + i)
                        {
                            xe.SetAttribute("type", ctl.Text);
                        }
                        if (ctl.Name == frontStr + "description" + i)
                        {
                            xe.SetAttribute("description", ctl.Text);
                        }
                        i++;
                    }

                }
            }
            i = 0;
            foreach (XmlNode node in returnList)
            {
                XmlElement xe = (XmlElement)node;
                string frontStr = "txt" + lblMethodName.Text;
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is TextBox)
                    {
                        if (ctl.Name == frontStr + "to" + i)
                        {
                            xe.SetAttribute("to", ctl.Text);
                        }
                        if (ctl.Name == frontStr + "from" + i)
                        {
                            xe.SetAttribute("from", ctl.Text);
                        }
                        if (ctl.Name == frontStr + "type" + i)
                        {
                            xe.SetAttribute("type", ctl.Text);
                        }
                        if (ctl.Name == frontStr + "description" + i)
                        {
                            xe.SetAttribute("description", ctl.Text);
                        }
                        i++;
                    }
                }
            }
            xmldoc.Save(textBox1.Text);
            MessageBox.Show(lblMethodName.Text + "方法 保存成功");
        }
        /// <summary>
        /// 重新加载xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReLoad_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_Click(object sender, EventArgs e)
        {
            CheckedListBox list = sender as CheckedListBox;
            lblMethodName.Text = list.SelectedItem.ToString();
            loadXmlNodeAddControl(xmldoc, lblMethodName.Text);
        }

        #region 删除选择，全选，反选
        private void btnDelSelect_Click(object sender, EventArgs e)
        {

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
        /// <summary>
        /// 导出xml文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportXml_Click(object sender, EventArgs e)
        {
            //导出对应的adapter.his.xml和calladapterhis.xml文件
        }


        private void btn_Click(object sender, System.EventArgs e)
        {
            Button btn = (Button)sender;
            // tableLayoutPanel1.Controls.RemoveAt(int.Parse(btn.Name));
        }

        private void 添加入参方法ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       


    }
}
