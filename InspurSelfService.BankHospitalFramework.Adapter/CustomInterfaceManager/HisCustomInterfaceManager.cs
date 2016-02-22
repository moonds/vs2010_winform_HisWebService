/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：HisCustomInterfaceManager.cs
 * 描    述：His 接口调用管理类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using InspurSelfService.BankHospitalFramework.Common;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;

namespace InspurSelfService.BankHospitalFramework.Adapter
{
    /// <summary>
    /// 自定义接口管理器
    /// </summary>
    public class HisCustomInterfaceManager : BasePage
    {
        public static LogUtility HisLog = new LogUtility();
        /// <summary>
        /// HisTrade
        /// </summary>
        /// <param name="adapterTrace">适配流水号</param>
        /// <param name="resultCodeMsg">返回消息</param>
        /// <param name="log">日志对象</param>
        /// <param name="terminalIp">终端IP</param>
        /// <param name="method">方法名</param>
        /// <param name="systemParams">HSS参数键值对</param>
        /// <param name="customParams">his参数键值对</param>
        /// <returns></returns>
        public static object HisTrade(string adapterTrace, out string resultCodeMsg, log4net.ILog log, string terminalIp, System.Reflection.MethodBase method, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, ref Dictionary<string, string> returnParamsDictionary)
        {
            resultCodeMsg = null;
            object ret = null;
#if debug
#else       
            log.Info(FormatIpMethod(adapterTrace, terminalIp, method));
             HisLog.HisLogInfo(log, FormatIpMethod(adapterTrace, terminalIp, method), terminalIp);
#endif
            string className = method.DeclaringType.Name;
            string methodName = method.Name;
            string xmlPath = String.Format(@"{0}Bin\{1}", AppDomain.CurrentDomain.BaseDirectory, "Adapter.His.xml");
#if debug
            string path = AppDomain.CurrentDomain.BaseDirectory;
            xmlPath = path.Replace(@"HisWebServiceTest\bin\Debug\", @"InspurSelfService.BankHospitalFramework.Adapter\adapter.his.xml");
#endif

            //返回值类型
            ParameterInfo returnType = ((MethodInfo)method).ReturnParameter;
            Type type = returnType.ParameterType;
            object inParams = null;
            //被调用方法名称：在Adapter.His.xml文件中一一对应
            string callMethodName = null;
            //是否记录返回报文
            bool logFlag = true;

            try
            {
                //加载XML配置文档
                string xmlContent = ReadFile(xmlPath).Replace(" xmlns=\"http://inspur.com/ihss/Validation\"", "");
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlContent);

                //选择接口调用方式
                object result = ASMX(adapterTrace, log, xmldoc, methodName, systemParams, customParams, terminalIp, out callMethodName, out logFlag, out inParams, ref returnParamsDictionary);
                xmldoc.RemoveAll();
                xmldoc = null;
                //GC.Collect(); 
                //选择返回值处理方式
                ret = XMLHisResult(adapterTrace, inParams, result, out resultCodeMsg, log, resultCodeMsg);
            }
            catch (Exception ex)
            {
                //E#-1#错误信息
                string inParamsList = null;
                if (inParams != null)
                    inParamsList = String.Join(",", (object)inParams);

                string error = String.Format("【{0}】 【{1}->{2}】 AdapterHis错误：{3}【ret】={4}【InParams】={5}", adapterTrace, methodName, callMethodName, ex.ToString(), ret, inParamsList);
                resultCodeMsg = String.Format("E#{0}#{1}", "-1", error);
                HisLog.HisLogError(log, resultCodeMsg, terminalIp);
            }
            return ret;
        }

        #region 接口调用方式【WSDL|ASMX|Oracle|SQLServer】
        /// <summary>
        /// //如下方式服务接口调用：http://221.199.145.82:8081/self/service/his?wsdl
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="log"></param>
        /// <param name="xmldoc"></param>
        /// <param name="methodName"></param>
        /// <param name="systemParams"></param>
        /// <param name="customParams"></param>
        /// <param name="callMethodName"></param>
        /// <param name="logFlag"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        private static object WSDL(string adapterTrace, log4net.ILog log, XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, string terminalIp, out string callMethodName, out bool logFlag, out  object inParams, ref Dictionary<string, string> returnParamsDictionary)
        {
            //服务地址：在Adapter.config中配置
            string HisWebService = CustomConfigurationManager.AppSettings["HisWebService"];
            //服务类名称：咨询接口厂家或者使用添加webService引用方式可以查看
            string wsdlName = CustomConfigurationManager.AppSettings["wsdlName"];
            inParams = CreateInXml(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag, ref returnParamsDictionary);
            //log.InfoFormat("【{0}】 【{1}->{2}】 InParams={3}", adapterTrace, methodName, callMethodName, inParams);

            HisLog.HisLogInfoFormat(log, "【{0}】 【{1}->{2}】 InParams={3}", adapterTrace, methodName, callMethodName, inParams, terminalIp);
            WebServiceProxy wsd = new WebServiceProxy(HisWebService, wsdlName);
            object result = wsd.ExecuteQuery(callMethodName, inParams);
            HisLog.HisLogInfoFormat(log, "【{0}】 【{1}->{2}】 OutParams={3}", adapterTrace, methodName, callMethodName, result.ToString(), terminalIp);
            return result;
        }

        /// <summary>
        /// //如下方式服务接口调用：http://localhost:1200/Service1.asmx
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="log"></param>
        /// <param name="xmldoc"></param>
        /// <param name="methodName"></param>
        /// <param name="systemParams"></param>
        /// <param name="customParams"></param>
        /// <param name="callMethodName"></param>
        /// <param name="logFlag"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        private static object  ASMX(string adapterTrace, log4net.ILog log, XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, string terminalIp, out string callMethodName, out bool logFlag, out object inParams, ref Dictionary<string, string> returnParamsDictionary)
        {
            //服务地址：在Adapter.config中配置
            string HisWebService = CustomConfigurationManager.AppSettings["HisWebService"];
            //服务类名称：咨询接口厂家或者使用添加webService引用方式可以查看
            string wsdlName = CustomConfigurationManager.AppSettings["wsdlName"];

            #region CreateInXml
            inParams = CreateInXml(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag, ref  returnParamsDictionary);
            //log.InfoFormat("【{0}】 【{1}->{2}】 InParams={3}", adapterTrace, methodName, callMethodName, inParams);
            HisLog.HisLogInfoFormat(log, "【{0}】 【{1}->{2}】 InParams={3}", adapterTrace, methodName, callMethodName, inParams, terminalIp);
            //以下为市立一院接口对接修改代码
            XmlDocument xmldocArgs = new XmlDocument();
            xmldocArgs.LoadXml(inParams.ToString());
            XmlNodeList nodesArgs = xmldocArgs.SelectSingleNode("Request").ChildNodes;
            object[] args = new object[nodesArgs.Count];
            args[0] = inParams;
            for (int i = 0; i < nodesArgs.Count; i++)
            {
                args[i] = nodesArgs[i].InnerText;
            }
            object result = WebServiceHelper.InvokeWebService(HisWebService, wsdlName, callMethodName, args);

            result = Regex.Replace(result.ToString(), @"\r\n", "");//去掉换行;                
            if (logFlag)
            {
                //log.InfoFormat("【{0}】 【{1}->{2}】 Result={3}", adapterTrace, methodName, callMethodName, result);
                HisLog.HisLogInfoFormat(log, "【{0}】 【{1}->{2}】 InParams={3}", adapterTrace, methodName, callMethodName, inParams, terminalIp);
            }
            else
            {
                HisLog.HisLogInfoFormat(log, "【{0}】 【{1}->{2}】 InParams={3}", adapterTrace, methodName, callMethodName, "[当前方法返回报文配置：不记录日志！]", terminalIp);
                //log.InfoFormat("【{0}】 【{1}->{2}】 Result={2}", adapterTrace, methodName, callMethodName, "[当前方法返回报文配置：不记录日志！]");
            }
            #endregion
            #region CreateParamsArray
            /*
            string paramsXml = null;
            Dictionary<string, object> inParamsDictionary = CreateParamsDictionary(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag, out paramsXml);
            log.InfoFormat("【{0}】 【{1}->{2}】 InParams={3}", adapterTrace, methodName, callMethodName, paramsXml);

            //遍历构造参数列表
            object[] args = new object[inParamsDictionary.Count];
            int index = 0;
            foreach (KeyValuePair<string, object> kvp in inParamsDictionary)
            {
                args[index] = kvp.Value;
                index++;
            }
            inParams = args;

            object result = WebServiceHelper.InvokeWebService(HisWebService, wsdlName, callMethodName, args);
            if (logFlag)
            {
                log.InfoFormat("【{0}】 【{1}->{2}】 Result={3}", adapterTrace, methodName, callMethodName, result);
                log.InfoFormat("【{0}】 【{1}->{2}】 InParams={3}", adapterTrace, methodName, callMethodName, String.Join(",", (object[])inParams));
            }
            else
            {
                log.InfoFormat("【{0}】 【{1}->{2}】 Result={2}", adapterTrace, methodName, callMethodName, "[当前方法返回报文配置：不记录日志！]");
                log.InfoFormat("【{0}】 【{1}->{2}】 InParams={3}", adapterTrace, methodName, callMethodName, String.Join(",", (object[])inParams));
            }
             */
            #endregion

            return result;
        }

        /// <summary>
        /// Oracle方式(存储过程方式)
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="log"></param>
        /// <param name="xmldoc"></param>
        /// <param name="methodName"></param>
        /// <param name="systemParams"></param>
        /// <param name="customParams"></param>
        /// <param name="callMethodName"></param>
        /// <param name="logFlag"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        private static object Oracle(string adapterTrace, log4net.ILog log, XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, out string callMethodName, out bool logFlag, out  string paramsXml, ref Dictionary<string, string> returnParamsDictionary)
        {
            object result = null;
            Dictionary<string, object> paramsDictionary = CreateParamsDictionary(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag, out paramsXml, ref returnParamsDictionary);
            string conn = CustomConfigurationManager.ConnectionStrings["HisConnectionString"].ConnectionString;
            string spName = callMethodName;

            OracleParameter[] parameterValues = new OracleParameter[paramsDictionary.Count];

            int index = 0;
            foreach (string key in paramsDictionary.Keys)
            {
                parameterValues[index] = new OracleParameter(String.Format("@{0}", key), paramsDictionary[key]);
                index++;
            }

            try
            {
                result = OracleHelper.ExecuteScalar(conn, CommandType.StoredProcedure, spName, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Oracle方式(SQL方式返回DataSet)
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="Field"></param>
        /// <param name="Where"></param>
        /// <param name="xmldoc"></param>
        /// <param name="methodName"></param>
        /// <param name="systemParams"></param>
        /// <param name="customParams"></param>
        /// <param name="callMethodName"></param>
        /// <param name="logFlag"></param>
        /// <param name="paramsXml"></param>
        /// <returns></returns>
        private static DataSet Oracle_SQL(string TabName, string Field, string Where, XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, out string callMethodName, out bool logFlag, out  string paramsXml, ref Dictionary<string, string> returnParamsDictionary)
        {
            DataSet result = null;
            Dictionary<string, object> paramsDictionary = CreateParamsDictionary(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag, out paramsXml, ref returnParamsDictionary);
            string conn = CustomConfigurationManager.ConnectionStrings["HisConnectionString"].ConnectionString;

            if (Field.Substring(Field.Length - 1, 1) == ",")
            {
                Field = Field.Substring(0, Field.Length - 1);
            }
            string sql = Field + TabName + Where;
            try
            {
                result = OracleHelper.ExecuteDataset(conn, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// SQLServer方式(存储过程方式)
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="log"></param>
        /// <param name="xmldoc"></param>
        /// <param name="methodName"></param>
        /// <param name="systemParams"></param>
        /// <param name="customParams"></param>
        /// <param name="callMethodName"></param>
        /// <param name="logFlag"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        private static object SQLServer(string adapterTrace, log4net.ILog log, XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, out string callMethodName, out bool logFlag, out string paramsXml, ref Dictionary<string, string> returnParamsDictionary)
        {
            object result = null;
            Dictionary<string, object> paramsDictionary = CreateParamsDictionary(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag, out paramsXml, ref returnParamsDictionary);
            string conn = CustomConfigurationManager.ConnectionStrings["HisConnectionString"].ConnectionString;
            string spName = callMethodName;

            SqlParameter[] parameterValues = new SqlParameter[paramsDictionary.Count];

            int index = 0;
            foreach (string key in paramsDictionary.Keys)
            {
                parameterValues[index] = new SqlParameter(String.Format("@{0}", key), paramsDictionary[key]);
                index++;
            }

            try
            {
                result = SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, spName, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// SQLServer方式(查询方式)
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="log"></param>
        /// <param name="xmldoc"></param>
        /// <param name="methodName"></param>
        /// <param name="systemParams"></param>
        /// <param name="customParams"></param>
        /// <param name="callMethodName"></param>
        /// <param name="logFlag"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        private static DataSet SQLServer_SQL(string TabName, string Field, string Where, string adapterTrace, log4net.ILog log, XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, out string callMethodName, out bool logFlag, out string paramsXml, ref Dictionary<string, string> returnParamsDictionary)
        {
            DataSet result = null;
            Dictionary<string, object> paramsDictionary = CreateParamsDictionary(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag, out paramsXml, ref returnParamsDictionary);
            string conn = CustomConfigurationManager.ConnectionStrings["HisConnectionString"].ConnectionString;

            if (Field.Substring(Field.Length - 1, 1) == ",")
            {
                Field = Field.Substring(0, Field.Length - 1);
            }
            string sql = Field + TabName + Where;
            try
            {
                result = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Json方式
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="log"></param>
        /// <param name="xmldoc"></param>
        /// <param name="methodName"></param>
        /// <param name="systemParams"></param>
        /// <param name="customParams"></param>
        /// <param name="callMethodName"></param>
        /// <param name="logFlag"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        private static object Json(string adapterTrace, log4net.ILog log, XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, out string callMethodName, out bool logFlag, out string paramsXml, ref Dictionary<string, string> returnParamsDictionary)
        {
            object result = null;
            Dictionary<string, object> paramsDictionary = CreateParamsDictionary(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag, out paramsXml, ref returnParamsDictionary);
            string conn = CustomConfigurationManager.ConnectionStrings["HisConnectionString"].ConnectionString;
            string spName = callMethodName;

            //string address = "http://127.0.0.1:8888/MZ_GH_BR.HOJS?args=1;'" + patientCardId + "'&argsName=CRC32;s";
            string address = null;
            log.InfoFormat("【{0}】 【{1}】 【{2}】", adapterTrace, callMethodName, address);
            string jsonText = HisJsonHelper.GetJsonData(address);
            log.InfoFormat("【{0}】 【{1}】 【{2}】", adapterTrace, callMethodName, jsonText);
            string JH_Result;
            string JH_ErrorMsg;
            string JH_CRC32;
            string JH_DateTime;
            DataTable dt = HisJsonHelper.GetTableByHisJsonText(jsonText, out JH_Result, out JH_ErrorMsg, out JH_CRC32, out JH_DateTime);
            result = dt;
            return result;
        }

        #endregion

        #region 组织输入数据方式
        #region 组织参数成XML格式报文
        /// <summary>
        /// 组织输入数据方式
        /// </summary>
        /// <param name="xmldoc">Xml文档</param>
        /// <param name="methodName">适配器方法名称</param>
        /// <param name="systemParams">WebUI传入参数集合</param>
        /// <param name="customParams">Adapter中自定义参数集合</param>
        /// <param name="callMethodName">【输出参数】：返回在Adapter.XXX.xml文件中对应的被调用方法名称</param>
        /// <returns></returns>
        private static string CreateInXml(XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, out string callMethodName, out bool logFlag, ref Dictionary<string, string> returnParamsDictionary)
        {
            string inXml = @"<Request>{0}</Request>";
            callMethodName = null;
            logFlag = true;//默认记录日志
            string paramsXml = null;

            string to = null;
            string from = null;
            string mode = null;
            string type = null;

            try
            {
                XmlNodeList list = null;
                XmlNodeList returnList = null;
                XmlNode methodNode = xmldoc.SelectSingleNode(String.Format("root/method[@name='{0}']", methodName));
                if (methodName != null)
                {
                    callMethodName = methodNode.Attributes["call"].Value;
                    logFlag = Convert.ToBoolean(methodNode.Attributes["log"].Value);
                    list = methodNode.SelectSingleNode("params").ChildNodes;
                    returnList = methodNode.SelectSingleNode("returns").ChildNodes;
                }
                if (returnList != null && returnList.Count > 0)
                {
                    foreach (XmlNode node in returnList)
                    {
                        to = node.Attributes["to"].Value;
                        from = node.Attributes["from"].Value;
                        //mode = node.Attributes["mode"].Value;
                        //type = node.Attributes["type"].Value;
                        returnParamsDictionary.Add(from, to);//获取返回web参数类型
                    }
                }
                if (list != null && list.Count > 0)
                {
                    foreach (XmlNode node in list)
                    {
                        to = node.Attributes["to"].Value;
                        from = node.Attributes["from"].Value;
                        mode = node.Attributes["mode"].Value;
                        type = node.Attributes["type"].Value;

                        if (!String.IsNullOrEmpty(to))
                        {
                            switch (mode)
                            {
                                case "system":
                                    paramsXml += String.Format("<{0}>{1}</{0}>", to, systemParams[from]);
                                    break;
                                case "custom":
                                    paramsXml += String.Format("<{0}>{1}</{0}>", to, customParams[from]);
                                    break;
                                case "null":
                                    paramsXml += String.Format("<{0}>{1}</{0}>", to, null);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("{0}-【to】={1},【from】={2},【mode】={3},【type】={4}", ex.Message, to, from, mode, type));
            }

            return String.Format(inXml, paramsXml);
        }

        #endregion

        #region 组织参数成参数字典
        /// <summary>
        /// 组织参数成参数字典
        /// </summary>
        /// <param name="xmldoc">Xml文档</param>
        /// <param name="methodName">适配器方法名称</param>
        /// <param name="systemParams">WebUI传入参数集合</param>
        /// <param name="customParams">Adapter中自定义参数集合</param>
        /// <param name="callMethodName">【输出参数】：返回在Adapter.XXX.xml文件中对应的被调用方法名称</param>
        /// <returns></returns>
        private static Dictionary<string, object> CreateParamsDictionary(XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, out string callMethodName, out bool logFlag, out string paramsXml, ref Dictionary<string, string> returnParamsDictionary)
        {
            Dictionary<string, object> paramsDictionary = new Dictionary<string, object>();
            callMethodName = null;
            logFlag = true;//默认记录日志
            paramsXml = null;

            string to = null;
            string from = null;
            string mode = null;
            string type = null;

            try
            {
                paramsXml = CreateInXml(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag, ref returnParamsDictionary);
                XmlNodeList list = null;
                XmlNode methodNode = xmldoc.SelectSingleNode(String.Format("root/method[@name='{0}']", methodName));
                if (methodName != null)
                {
                    callMethodName = methodNode.Attributes["call"].Value;
                    logFlag = Convert.ToBoolean(methodNode.Attributes["log"].Value);
                    list = methodNode.SelectSingleNode("params").ChildNodes;
                }

                if (list != null && list.Count > 0)
                {
                    foreach (XmlNode node in list)
                    {
                        to = node.Attributes["to"].Value;
                        from = node.Attributes["from"].Value;
                        mode = node.Attributes["mode"].Value;
                        type = node.Attributes["type"].Value;

                        if (!String.IsNullOrEmpty(to))
                        {
                            switch (mode)
                            {
                                case "system":
                                    paramsDictionary.Add(to, TypeFormat(systemParams[from], GetTypeByTypeStr(type)));
                                    break;
                                case "custom":
                                    paramsDictionary.Add(to, TypeFormat(customParams[from], GetTypeByTypeStr(type)));
                                    break;
                                case "null":
                                    paramsDictionary.Add(to, null);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                paramsDictionary = null;
                throw new Exception(String.Format("{0}-【to】={1},【from】={2},【mode】={3},【type】={4}", ex.Message, to, from, mode, type));
            }

            return paramsDictionary;
        }
        #endregion
        #endregion

        #region 处理输出数据方式
        #region 处理XML输出数据
        /// <summary>
        /// 解析输出的报文【字典方式输出】
        /// </summary>
        /// <param name="outXml">需要解析的报文</param>
        /// <param name="root">根节点值</param>
        /// <returns>返回的各节点值</returns>
        private static Dictionary<string, object> AnalyzeOutXmlToDictionary(string outXml, string root)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(outXml))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(outXml);

                XmlNodeList nodes = xmldoc.SelectSingleNode(root).ChildNodes;

                for (int i = 0; i < nodes.Count; i++)
                {
                    values.Add(nodes.Item(i).Name, nodes.Item(i).InnerXml);
                }
            }

            return values;
        }

        /// <summary>
        /// XMLHisResult
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="retVal"></param>
        /// <param name="resultCodeMsg"></param>
        /// <param name="log"></param>
        /// <param name="tradeCodeMsg"></param>
        /// <returns></returns>
        private static Dictionary<string, object> XMLHisResult(string adapterTrace, object inParams, object retVal, out string resultCodeMsg, log4net.ILog log, string tradeCodeMsg)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            resultCodeMsg = null;
            if (retVal == null || retVal.ToString() == "")
            {
                //E#-2#错误信息
                string error = String.Format("【{0}】 HisResult错误：{1}", adapterTrace, "返回值为空！");
                if (!String.IsNullOrEmpty(tradeCodeMsg))
                {
                    resultCodeMsg = tradeCodeMsg;
                }
                else
                {
                    resultCodeMsg = String.Format("E#{0}#{1}", "-2", error);
                }
                log.Error(resultCodeMsg);
                return ret;
            }
            try
            {
                Dictionary<string, object> values = AnalyzeOutXmlToDictionary(retVal.ToString(), "Response");
                if (values.ContainsKey("ResultCode") && values.ContainsKey("ErrorMsg"))
                {
                    if (values["ResultCode"].ToString() == "0")
                    {
                        values["ResultCode"] = "0";
                        resultCodeMsg = String.Format("S#{0}#【{1}】 {2}", values["ResultCode"].ToString(), adapterTrace, values["ErrorMsg"].ToString());
                    }
                    else
                    {
                        resultCodeMsg = String.Format("F#{0}#【{1}】 {2}", values["ResultCode"].ToString(), adapterTrace, values["ErrorMsg"].ToString());
                    }
                    log.Info(resultCodeMsg);
                    ret = values;
                }
                else
                {
                    //E#-3#错误信息
                    string error = String.Format("【{0}】 HisResult错误：{1}", adapterTrace, "返回值不合法！");
                    resultCodeMsg = String.Format("E#{0}#{1}", "-3", error);
                    log.Error(resultCodeMsg);
                    throw new Exception(error);
                }

            }
            catch (Exception ex)
            {
                //E#-4#错误信息
                string error = String.Format("【{0}】 HisResult错误：{1}", adapterTrace, ex);
                resultCodeMsg = String.Format("E#{0}#{1}", "-4", error);
                log.Error(resultCodeMsg);
            }

            return ret;
        }
        #endregion

        #region 处理输出参数形式数据
        /// <summary>
        /// OutParamsHisResult
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="retVal"></param>
        /// <param name="resultCodeMsg"></param>
        /// <param name="log"></param>
        /// <param name="tradeCodeMsg"></param>
        /// <returns></returns>
        private static Dictionary<string, object> OutParamsHisResult(string adapterTrace, object inParams, object retVal, out string resultCodeMsg, log4net.ILog log, string tradeCodeMsg)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            resultCodeMsg = null;
            if (retVal == null || retVal.ToString() == "")
            {
                //E#-2#错误信息
                string error = String.Format("【{0}】 HisResult错误：{1}", adapterTrace, "返回值为空！");
                if (!String.IsNullOrEmpty(tradeCodeMsg))
                {
                    resultCodeMsg = tradeCodeMsg;
                }
                else
                {
                    resultCodeMsg = String.Format("E#{0}#{1}", "-2", error);
                }
                log.Info(resultCodeMsg);
                return ret;
            }

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                if (inParams != null)
                {
                    object[] objs = (object[])inParams;
                    for (int i = 0; i < objs.Length; i++)
                    {
                        values.Add(i.ToString(), objs[i]);
                    }
                    ret = values;
                    resultCodeMsg = "S#00#成功";
                }

            }
            catch (Exception ex)
            {
                //E#-4#错误信息
                string error = String.Format("【{0}】 HisResult错误：{1}", adapterTrace, ex);
                resultCodeMsg = String.Format("E#{0}#{1}", "-4", error);
                log.Info(resultCodeMsg);
            }

            return ret;
        }


        #endregion
        #endregion
    }
}
