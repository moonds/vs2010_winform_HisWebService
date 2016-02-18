using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections;

namespace DataFormat
{
    public class XmlFormat
    {
        #region 利用堆栈进行格式化
        Stack sk = null;
        /// <summary>
        /// 验证xml串格式是否正确，将xml串进行格式化
        /// </summary>
        /// <param name="handleStr">需要处理的xml字符串</param>
        /// <param name="isSuccess">处理结果</param>
        /// <returns>处理后的xml串</returns>
        public string xmlFormatByStock(string handleStr, out bool isSuccess)
        {
            if (string.IsNullOrEmpty(handleStr))//验证是否是空，是否是xml串
            {
                isSuccess = false;
                return "";
            }
            try//验证是否是xml串
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(handleStr);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                return "xml格式有误，错误信息：" + ex.Message;
            }
            try
            {

                handleStr = Regex.Replace(handleStr, "\r\n", "");//将xml串进行排版前处理
                handleStr = Regex.Replace(handleStr, " ", "");//将xml串进行排版前处理
                //todo:当返回单个<bank/>类似情况时，将<bank/>转换为<bank></bank>
                //MatchCollection mcspecial = Regex.Matches(handleStr, "/>");
                //if (mcspecial != null)
                //{ 
                //    for(int j=0;j<mcspecial.Count;j++)
                //    {
                //        int frontindex= handleStr.LastIndexOf("<",mcspecial[0].Index);
                //        string dd=handleStr.Substring(frontindex,mcspecial[0].Index+1-frontindex);
                //        string ddf=dd.Replace("/>",">");
                //        handleStr.Replace(
                //    }
                   
                //}
                MatchCollection mcl = Regex.Matches(handleStr, "<");
                MatchCollection mcr = Regex.Matches(handleStr, ">");
                sk = new Stack();//循环前的初始化工作//利用堆栈后进先出
                StringBuilder sb = new StringBuilder();
                sb.Append(handleStr.Substring(mcl[0].Index, mcr[0].Index + 1));//第一个"<XX>"内容
                sk.Push("/" + handleStr.Substring(mcl[0].Index + 1, mcr[0].Index - 1));//第一个<"XX">内容前面加/构成结束串，进入堆栈
                bool flag = false;//控制深层次弹出的字符串与比较字符串相同时需要添加空格，不添加时连续pop相等的内容不会增加换行符和空格。
                for (int i = 1; i < mcl.Count; i++)//从第二个<>开始循环
                {
                    int l = mcl[i].Index;
                    int r = mcr[i].Index;
                    string tempStr = handleStr.Substring(mcl[i].Index + 1, mcr[i].Index - mcl[i].Index - 1);//第i个<"xxxx">中的内容
                    string pop = sk.Peek().ToString();//提取堆栈顶部的内容弹出与第i个<"xxx">中的内容进行比较
                    if (pop == tempStr)
                    {
                        sk.Pop();//相同弹出
                        if (flag == true)
                            sb.Append("\r\n" + new string(' ', sk.Count));
                        sb.Append(handleStr.Substring(mcr[i - 1].Index + 1, mcr[i].Index - mcr[i - 1].Index));//>与下一个>之间的内容加入最终字符串
                        flag = true;
                    }
                    else //不同
                    {
                        sb.Append("\r\n" + new string(' ', sk.Count) + handleStr.Substring(mcl[i].Index, mcr[i].Index - mcl[i].Index + 1));//不同时需要换行，并将<>内容加入最终字符串
                        sk.Push("/" + tempStr);
                        flag = false;
                    }
                }
                isSuccess = true;
                return sb.ToString();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                return ex.Message;
            }
        }
        #endregion


        #region 利用xmldocument对象+递归进行格式化
        Dictionary<string, string> kv = null;
        /// <summary>
        /// 利用xml节点递归循环实现排列
        /// </summary>
        /// <param name="handleStr">需要格式化的xml字符穿</param>
        /// <param name="isSuccess">是否格式化成功</param>
        /// <returns>格式化后的xml</returns>
        public string xmlFormatByXmlDoc(string handleStr, out bool isSuccess)
        {
            handleStr = Regex.Replace(handleStr, "\r\n", "");//将xml串进行排版前处理
            handleStr = Regex.Replace(handleStr, " ", "");//将xml串进行排版前处理
            kv = new Dictionary<string, string>();
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml(handleStr);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                return "xml格式有误，错误信息：" + ex.Message;
            }
            if (xmldoc != null)
            {
                string finalStr = "";
                int i = 0;
                DisplayXmlNode(xmldoc.ChildNodes, ref i, ref finalStr);
                isSuccess = true;
                return finalStr;
            }
            else
            {
                isSuccess = true;
                return "";
            }
        }
        /// <summary>
        /// 递归调用
        /// </summary>
        /// <param name="nodelist">循环的xmllist</param>
        /// <param name="dep">层次</param>
        /// <param name="finalStr">格式化后的字符串</param>
        private void DisplayXmlNode(XmlNodeList nodelist, ref int dep, ref string finalStr)
        {

            foreach (XmlNode node in nodelist)
            {
                //todo:判断是否有子节点，有子节点时只能添加<node.name>然后进行下一个循环，
                //当有一个子节点时判断是否为text类型，为text类型时直接输出，否则进入下一个节点
                //同时将此节点的名称和子节点最后一个节点名称记录在kv变量中。当子节点全部遍历完毕后 插入</node.name>
                //todo:如何判断子节点全部循环完毕？ 记录此子节点最后一个节点名称，利用kv数据进行插入。
                if (node.ChildNodes.Count > 1 || (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Element))
                {
                    finalStr += new string(' ', dep) + "<" + node.Name + ">\r\n";
                    kv.Add(node.ChildNodes[node.ChildNodes.Count - 1].Name, node.Name);
                    ++dep;
                    DisplayXmlNode(node.ChildNodes, ref dep, ref  finalStr);
                }
                if (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text)
                {
                    finalStr += new string(' ', dep) + "<" + node.Name + ">" + node.InnerText + "</" + node.Name + ">\r\n";
                    string nodename = node.Name;
                    while (kv.ContainsKey(nodename))
                    {
                        finalStr += new string(' ', --dep) + "</" + kv[nodename] + ">\r\n";
                        nodename = kv[nodename];
                    }
                }
            }
        }
        #endregion
    }
}
