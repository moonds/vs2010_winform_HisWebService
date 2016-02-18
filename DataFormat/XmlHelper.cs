using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;

namespace DataFormat
{
    public class XmlHelper
    {
        public static DataSet ConvertXmlFileToDS(XmlNode node)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            string xmlData = node.OuterXml;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
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
        /// XML中特殊字符处理，防止在DataSet.ReadXML时报错   
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
                strReturn = strReturn.Replace("<alertflag xml:space=&quot;preserve&quot;></alertflag>", "");

                return strReturn;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="methodname"></param>
        /// <param name="type"></param>
        /// <param name="insertXmlString"></param>
        /// <param name="insertnode"></param>
        /// <param name="placenode"></param>
        /// <param name="place"></param>
        /// <param name="errormessage"></param>
        /// <returns></returns>
        public static bool AddXmlNode(ref XmlDocument xmldoc, string methodname, string type, string insertXmlString,XmlNode insertnode,XmlNode placenode,string place,out string errormessage)
        {
            try
            {
                switch (type)
                {
                    case "method":
                        XmlNode xmlRoot = xmldoc.SelectSingleNode("root");
                        //将节点插入指定位置。
                        if(place=="insertbefore")
                        {
                            xmldoc.InsertBefore(insertnode,placenode);
                        }
                        else if(place=="insertafter")
                        {
                            //XmlNode node=xmldoc.SelectSingleNode(string.Format("root/metho"
                            xmlRoot.InsertAfter(insertnode,placenode);
                            
                        }
                        break;
                    case "params"://String.Format("root/method[@name='{0}']", methodName)
                        XmlNode nodeMethodParams = xmldoc.SelectSingleNode(String.Format("root/method[@name='{0}']/params", methodname));
                        nodeMethodParams.InnerXml = insertXmlString;
                        break;
                    case "returns":
                        XmlNode nodeMethodReturns = xmldoc.SelectSingleNode(String.Format("root/method[@name='{0}']/params", methodname));
                        nodeMethodReturns.InnerXml = insertXmlString;
                        break;
                    default: break;
                }
                errormessage = "";
                return true;
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return false;
            }

        }
        public static bool DelXmlNode(ref XmlDocument xmldoc, string methodname, string type, out string errormessage)
        {
            try
            {
                switch (type)
                {
                    case "method":
                        XmlNodeList nodeListMethod = xmldoc.SelectNodes("root/method");
                        foreach (XmlNode node in nodeListMethod)
                        {
                            XmlElement xe = (XmlElement)node;
                            if (xe.OuterXml.IndexOf(methodname, 0) > 0)
                            {
                                node.ParentNode.RemoveChild(node);
                                break;
                            }
                        }
                        break;
                    default: break;
                }
                errormessage = "";
                return true;
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return false;
            }

        }

    }
}
