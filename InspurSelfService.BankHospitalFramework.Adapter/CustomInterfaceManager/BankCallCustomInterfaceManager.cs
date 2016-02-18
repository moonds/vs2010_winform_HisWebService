/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：BankCallCustomInterfaceManager.cs
 * 描    述：银行接口调用接口管理类
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

namespace InspurSelfService.BankHospitalFramework.Adapter
{
    /// <summary>
    /// 自定义接口管理器
    /// </summary>
    public class BankCallCustomInterfaceManager : BasePage
    {
        /// <summary>
        /// BankCallTrade
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="resultCodeMsg"></param>
        /// <param name="log"></param>
        /// <param name="terminalIp"></param>
        /// <param name="method"></param>
        /// <param name="systemParams"></param>
        /// <param name="customParams"></param>
        /// <returns></returns>
        public static object BankCallTrade(string adapterTrace, out string resultCodeMsg, log4net.ILog log, string terminalIp, System.Reflection.MethodBase method, Dictionary<string, string> systemParams, Dictionary<string, string> customParams)
        {
            resultCodeMsg = null;
            object ret = null;
            log.Info(FormatIpMethod(adapterTrace, terminalIp, method));
            string className = method.DeclaringType.Name;
            string methodName = method.Name;
            string xmlPath = String.Format(@"{0}Bin\{1}", AppDomain.CurrentDomain.BaseDirectory, "Adapter.His.xml");

            //返回值类型
            ParameterInfo returnType = ((MethodInfo)method).ReturnParameter;
            Type type = returnType.ParameterType;
            string inXml = null;
            string outXml = null;
            try
            {
                //加载XML配置文档
                string xmlContent = ReadFile(xmlPath).Replace(" xmlns=\"http://inspur.com/ihss/Validation\"", "");
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlContent);

                //如下方式服务接口调用：http://221.199.145.82:8081/self/service/his?wsdl
                //服务地址：在Adapter.config中配置
                string HisWebService = CustomConfigurationManager.AppSettings["HisWebService"];
                //服务类名称：咨询接口厂家或者使用添加webService引用方式可以查看
                string wsdlName = "SelfServiceImplService";
                //被调用方法名称：在Adapter.His.xml文件中一一对应
                string callMethodName = null;
                //是否记录返回报文
                bool logFlag = true;
                inXml = CreateInXml(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag);
                log.InfoFormat("【{0}】 【{1}】InXML={2}", adapterTrace, callMethodName, inXml);
                WebServiceProxy wsd = new WebServiceProxy(HisWebService, wsdlName);
                object result = wsd.ExecuteQuery(callMethodName, inXml);

                outXml = Regex.Replace(result.ToString(), @"\r\n", "");//去掉换行;
                ret = outXml;

                if (logFlag)
                {
                    log.InfoFormat("【{0}】 【{1}】OutXML={2}", adapterTrace, callMethodName, outXml);
                }
                else
                {
                    log.InfoFormat("【{0}】 【{1}】OutXML={2}", adapterTrace, callMethodName, "[当前方法返回报文配置：不记录日志！]");
                }
            }
            catch (Exception ex)
            {
                //E#-1#错误信息
                string error = String.Format("【{0}】 AdapterHis错误：{1}【ret】={2}【InXML】={3}", adapterTrace, ex.ToString(), ret, inXml);
                resultCodeMsg = String.Format("E#{0}#{1}", "-1", error);
            }
            return ret;
        }

        /// <summary>
        /// BankCallResult
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="retVal"></param>
        /// <param name="resultCodeMsg"></param>
        /// <param name="log"></param>
        /// <param name="tradeCodeMsg"></param>
        /// <returns></returns>
        private static object BankCallResult(string adapterTrace, object retVal, out string resultCodeMsg, log4net.ILog log, string tradeCodeMsg)
        {
            object ret = new Dictionary<string, string>();
            resultCodeMsg = null;
            if (retVal == null || retVal.ToString() == "")
            {
                //E#-2#错误信息
                string error = String.Format("【{0}】 BankCallResult错误：{1}", adapterTrace, "返回值为空！");
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

            Dictionary<string, string> values = AnalyzeOutXmlToDictionary(retVal.ToString(), "Response");
            if (values.ContainsKey("ResultCode") && values.ContainsKey("ErrorMsg"))
            {
                if (values["ResultCode"].ToString() == "0000")
                {
                    values["ResultCode"] = "00";
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
                string error = String.Format("【{0}】 BankCallResult错误：{1}", adapterTrace, "返回值不合法！");
                resultCodeMsg = String.Format("E#{0}#{1}", "-3", error);
                log.Info(resultCodeMsg);
                throw new Exception(error);
            }

            return ret;
        }

        /// <summary>
        /// 组织输入报文【需要根据接口规范自定义】
        /// </summary>
        /// <param name="xmldoc">Xml文档</param>
        /// <param name="methodName">适配器方法名称</param>
        /// <param name="systemParams">WebUI传入参数集合</param>
        /// <param name="customParams">Adapter中自定义参数集合</param>
        /// <param name="callMethodName">【输出参数】：返回在Adapter.XXX.xml文件中对应的被调用方法名称</param>
        /// <returns></returns>
        private static string CreateInXml(XmlDocument xmldoc, string methodName, Dictionary<string, string> systemParams, Dictionary<string, string> customParams, out string callMethodName, out bool logFlag)
        {
            string inXml = @"<Request>{0}</Request>";
            callMethodName = null;
            logFlag = true;//默认记录日志
            string paramsXml = null;

            string to = null;
            string from = null;
            string mode = null;

            try
            {
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
            catch (Exception ex)
            {
                throw new Exception(String.Format("{0}-【to】={1},【from】={2},【mode】={3}", ex.Message, to, from, mode));
            }

            return String.Format(inXml, paramsXml);
        }

        /// <summary>
        /// 解析输出的报文【字典方式输出】
        /// </summary>
        /// <param name="outXml">需要解析的报文</param>
        /// <param name="root">根节点值</param>
        /// <returns>返回的各节点值</returns>
        private static Dictionary<string, string> AnalyzeOutXmlToDictionary(string outXml, string root)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

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

    }
}
