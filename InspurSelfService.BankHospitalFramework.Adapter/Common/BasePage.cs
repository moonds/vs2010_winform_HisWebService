/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：BasePage.cs
 * 描    述：基类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Collections;

namespace InspurSelfService.BankHospitalFramework.Adapter
{
    /// <summary>
    /// 适配器基类
    /// </summary>
    public class BasePage
    {
        /// <summary>
        /// 获取应用程序配置
        /// </summary>
        public static void GetAppConfiguration()
        {
            string applicationName =
           Environment.GetCommandLineArgs()[0] + ".exe";
            string exePath = System.IO.Path.Combine(
                Environment.CurrentDirectory, applicationName);
            System.Configuration.Configuration config =
              ConfigurationManager.OpenExeConfiguration(exePath);
        }

        /// <summary>
        /// GetSystemParamsDictionary
        /// </summary>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetSystemParamsDictionary(string systemParamsXml)
        {
            Dictionary<string, string> systemParamsDictionary = null;
            if (!String.IsNullOrEmpty(systemParamsXml))
            {
                systemParamsDictionary = new Dictionary<string, string>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(systemParamsXml);
                XmlNodeList nodes = xmlDoc.SelectSingleNode("params").ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    systemParamsDictionary.Add(node.Name, node.InnerText);
                }
            }

            return systemParamsDictionary;
        }

        /// <summary>
        /// AdapterTrace--适配流水号
        /// </summary>
        public string AdapterTrace
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }
        }

        /// <summary>
        /// 日志操作类
        /// </summary>
        public LogUtility Log = new LogUtility();

        /// <summary>
        /// 根据XML值生成返回值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xmlValue"></param>
        /// <returns></returns>
        private static object GetReturnValueByXmlValue(Type type, string xmlValue)
        {
            object returnObj = null;

            try
            {
                if (type.Name == "DataSet")
                {
                    returnObj = (DataSet)ConvertXMLToDataSet(xmlValue);
                }
                else if (type.Name == "DataTable")
                {
                    DataSet ds = (DataSet)ConvertXMLToDataSet(xmlValue);
                    returnObj = (DataTable)ds.Tables[0];
                }
                else
                {
                    returnObj = TypeFormat(xmlValue, type);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnObj;
        }

        /// <summary>
        /// 读取凭条样式文件
        /// </summary>
        /// <param name="strPath">文件路径</param>
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
        /// 格式化IP和方法
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="terminalIp"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string FormatIpMethod(string adapterTrace, string terminalIp, System.Reflection.MethodBase method)
        {
            return String.Format("【{0}】 【{1}】 【{2}】", adapterTrace, terminalIp, method.Name);
        }

        /// <summary>
        /// 将字符串格式化成指定的数据类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object TypeFormat(string str, Type type)
        {
            if (String.IsNullOrEmpty(str))
                return null;
            if (type == null)
                return str;
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                String[] strs = str.Split(new char[] { ';' });
                Array array = Array.CreateInstance(elementType, strs.Length);
                for (int i = 0, c = strs.Length; i < c; ++i)
                {
                    array.SetValue(ConvertSimpleType(strs[i], elementType), i);
                }
                return array;
            }
            return ConvertSimpleType(str, type);
        }

        /// <summary>
        /// 根据字符串数据类型获取数据类型
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static Type GetTypeByTypeStr(string typeStr)
        {
            Type type = typeof(string);

            switch (typeStr)
            {
                case "string":
                    type = typeof(string);
                    break;
                case "bool":
                    type = typeof(bool);
                    break;
                case "int":
                    type = typeof(int);
                    break;
                case "float":
                    type = typeof(float);
                    break;
                case "double":
                    type = typeof(double);
                    break;
                case "decimal":
                    type = typeof(decimal);
                    break;
                default:
                    break;
            }

            return type;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static object ConvertSimpleType(object value, Type destinationType)
        {
            object returnValue;
            if ((value == null) && destinationType.IsInstanceOfType(value))
            {
                return value;
            }
            string str = value as string;
            if ((str != null) && (str.Length == 0))
            {
                return null;
            }
            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(destinationType))
            {
                throw new InvalidOperationException("无法转换成类型：" + value.ToString() + "==>;" + destinationType);
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, destinationType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("类型转换出错：" + value.ToString() + "==>;" + destinationType, e);
            }
            return returnValue;
        }

        /// <summary>
        /// 将xml对象内容字符串转换为DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        //public static DataSet ConvertXMLToDataSet(string xmlData)
        //{
        //    //changed by lixg 添加一个根元素
        //    xmlData = ReplaceSpecialCharactersOfXml(xmlData);
        //    if (xmlData.StartsWith("<Item>") || xmlData.StartsWith("<item>"))
        //    {
        //        xmlData = "<List>" + xmlData + "</List>";
        //    }
        //    StringReader stream = null;
        //    XmlTextReader reader = null;
        //    try
        //    {
        //        DataSet xmlDS = new DataSet();
        //        stream = new StringReader(xmlData);
        //        //从stream装载到XmlTextReader
        //        reader = new XmlTextReader(stream);
        //        xmlDS.ReadXml(reader);
        //        return xmlDS;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //            reader.Close();
        //    }
        //}
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
                strReturn = strReturn.Replace("<alertflag xml:space=&quot;preserve&quot;></alertflag>", "");

                return strReturn;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 处理方法：在产生xml文件的时候，过滤低位非打印字符
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static string ReplaceLowOrderASCIICharacters(string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat(" ", ss);
                else info.Append(cc);
            }
            return info.ToString();
        }

        /// <summary>
        /// 二维字典转数据表
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static DataTable ConvertDictionaryToDataTable(Dictionary<string, object> dict)
        {
            DataTable dt = new DataTable();
            ArrayList args = new ArrayList();

            foreach (string key in dict.Keys)
            {
                dt.Columns.Add(key);
                args.Add(dict[key].ToString());
            }
            dt.Rows.Add((object[])args.ToArray());

            return dt;
        }


        /// <summary>
        /// 列名称转换
        /// </summary>
        /// <returns></returns>
        public DataTable ColumnConvert(Dictionary<string, string> returnsDic, DataTable dt)
        {
            for (int column = 0; column < dt.Columns.Count; column++)
            {
                if (returnsDic.ContainsKey(dt.Columns[column].ColumnName))
                {
                    dt.Columns[column].ColumnName = returnsDic[dt.Columns[column].ColumnName];
                }
            }
            return dt;
        }


        /// <summary>
        /// 错误处理机制
        /// </summary>
        /// <param name="send"></param>
        /// <param name="e"></param>
        public void Adapter_Error(Exception e)
        {
            Exception currentError = e;
            string sErrorMsg = "【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + "】\r\n";
            //sErrorMsg += Request.Url.ToString() + "发生错误。";
            sErrorMsg += "错误信息：\r\n" + @currentError.Message + "\r\n";
            sErrorMsg += "调试跟踪：" + @currentError.ToString();

            //把错误信息写入日志
            Log.WritePageErrorLog(sErrorMsg + "\r\n-------------------------------------------------------\r\n");
        }
    }
}
