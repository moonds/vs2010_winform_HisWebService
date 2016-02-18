/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：BankCustomInterfaceManager.cs
 * 描    述：银行接口调用管理类
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
    public class BankCustomInterfaceManager : BasePage
    {
        /// <summary>
        /// BankTrade
        /// </summary>
        /// <param name="adapterTrace"></param>
        /// <param name="resultCodeMsg"></param>
        /// <param name="log"></param>
        /// <param name="terminalIp"></param>
        /// <param name="method"></param>
        /// <param name="systemParams"></param>
        /// <param name="customParams"></param>
        /// <returns></returns>
        public static object BankTrade(string adapterTrace, out string resultCodeMsg, log4net.ILog log, string terminalIp, System.Reflection.MethodBase method, Dictionary<string, string> systemParams, Dictionary<string, string> customParams)
        {
            resultCodeMsg = null;
            object ret = null;
            log.Info(FormatIpMethod(adapterTrace, terminalIp, method));
            string className = method.DeclaringType.Name;
            string methodName = method.Name;
            string xmlPath = String.Format(@"{0}Bin\{1}", AppDomain.CurrentDomain.BaseDirectory, "Adapter.Bank.xml");

            //返回值类型
            ParameterInfo returnType = ((MethodInfo)method).ReturnParameter;
            Type type = returnType.ParameterType;
            string inXml = null;
            string outXml = null;
            //被调用方法名称：在Adapter.Bank.xml文件中一一对应
            string callMethodName = null;
            //是否记录返回报文
            bool logFlag = true;

            try
            {
                //加载XML配置文档
                string xmlContent = ReadFile(xmlPath).Replace(" xmlns=\"http://inspur.com/ihss/Validation\"", "");
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlContent);

                //如下方式Socket接口调用
                //服务地址：在Adapter.config中配置：IP、Port等                
                string IP = CustomConfigurationManager.AppSettings["IP"];
                string Port = CustomConfigurationManager.AppSettings["Port"];
                string EncodingType = CustomConfigurationManager.AppSettings["EncodingType"];
                string DecodingType = CustomConfigurationManager.AppSettings["DecodingType"];
                string SingleBuffSize = CustomConfigurationManager.AppSettings["SingleBuffSize"];
                string SendTimeout = CustomConfigurationManager.AppSettings["SendTimeout"];
                string ReceiveTimeout = CustomConfigurationManager.AppSettings["ReceiveTimeout"];

                inXml = CreateInXml(xmldoc, methodName, systemParams, customParams, out callMethodName, out logFlag);
                log.InfoFormat("【{0}】 {1}:{2}  SendTimeOut={3}ms,ReceiveTimeOut={4}ms", adapterTrace, IP, Port, SendTimeout, ReceiveTimeout);
                log.InfoFormat("【{0}】 【{1}->{2}】InXML={3}", adapterTrace, methodName, callMethodName, inXml);

                //Socket连接方式：短连接或长连接
                //object result = ShortLinkSocketHelper.GetValue(IP, Port, EncodingType, DecodingType, SingleBuffSize, SendTimeout, ReceiveTimeout, inXml);
                object result = BankTestData.GetValue(inXml, methodName);

                outXml = Regex.Replace(ReplaceLowOrderASCIICharacters(result.ToString()), @"\r\n", "");//去掉换行;
                ret = outXml.StartsWith("error") ? null : outXml;

                DataSet ds = ConvertXMLToDataSet(ret.ToString());
                string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                resultCodeMsg = resultCode == "00" ? String.Format("S#{0}#{1}", "00", resultMsg) : String.Format("F#{0}#{1}", resultCode, resultMsg);

                if (logFlag)
                {
                    log.InfoFormat("【{0}】 【{1}->{2}】 OutXML={3}", adapterTrace, methodName, callMethodName, outXml);
                }
                else
                {
                    log.InfoFormat("【{0}】 【{1}->{2}】 OutXML={2}", adapterTrace, methodName, callMethodName, "[当前方法返回报文配置：不记录日志！]");
                }
            }
            catch (Exception ex)
            {
                //E#-1#错误信息
                string error = String.Format("【{0}】 【{1}->{2}】 AdapterBank错误：{3}【ret】={4}【InXML】={5}", adapterTrace, methodName, callMethodName, ex.Message, ret, inXml);
                resultCodeMsg = String.Format("E#{0}#{1}", "-1", error);
                log.Error(resultCodeMsg);
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
        /// 解析输出的报文【数组方式输出】
        /// </summary>
        /// <param name="outXml">需要解析的报文</param>
        /// <param name="root">根节点值</param>
        /// <returns>返回的各节点值</returns>
        private static string[] AnalyzeOutXmlToArray(string outXml, string root)
        {
            string[] values = new string[] { null, null, null, null, null };

            if (!String.IsNullOrEmpty(outXml))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(outXml);

                XmlNodeList nodes = xmldoc.SelectSingleNode(root).ChildNodes;

                for (int i = 0; i < nodes.Count; i++)
                {
                    values[i] = nodes.Item(i).InnerXml;
                }
            }

            return values;
        }
    }
}
