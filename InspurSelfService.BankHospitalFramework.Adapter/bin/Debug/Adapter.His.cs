/************************************************************************
 * Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者: 
 * 文件名称：Adapter.His.cs
 * 描    述：His 接口适配处理类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace InspurSelfService.BankHospitalFramework.Adapter
{
    /// <summary>
    /// His适配器
    /// </summary>
    public class AdapterHis : BasePage
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        LogUtility HisLog = new LogUtility();
        #region 1.测试
        /// <summary>
        /// 接口调用测试（仅作调用测试，代码不必修改）
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string AdapterHisTest(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            try
            {
                //Adapter.AdapterHss adapterHss = new Adapter.AdapterHss();
                //adapterHss.InsertLogTrade()
                string ret = null;
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H201", "GetRealTimeDepartmentSchedule", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                //object retVal =    object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary); HisCustomInterfaceManager.HisTrade(adapterTrace,out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);

                resultCodeMsg = "S#0#测试成功";
                ret = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#测试错误";
                return "error:请查看日志";
            }

        }

        /// <summary>
        /// 1)	H000测试
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? NetTest(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            try
            {
                bool? ret = null;
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();

                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H000", "NetTest", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H000", "NetTest", resultCodeMsg, "1", "0");
                try
                {
                    Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                    if (resultCodeMsg.StartsWith("S#"))
                    {
                        ret = true;
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                }
                catch (Exception ex)
                {
                    //log.Info(ex);
                    //Adapter_Error(ex);
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }

                return ret;
            }
            catch (Exception e)
            {
                Adapter_Error(e);
                resultCodeMsg = "S#-1#H000测试";
                return false;
            }

        }
        #endregion

        #region 2.卡管理
        /// <summary>
        /// H001判断病人是否已经办卡
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? CheckPatientByIdCardNo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            try
            {
                bool? ret = null;
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H201", "CheckPatientByIdCardNo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）
                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H201", "CheckPatientByIdCardNo", "true", "1", "0");
                if (dict["resultCode"].ToString() == "00" && dict["status"].ToString() == "0")
                {
                    ret = false;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else if (dict["resultCode"].ToString() == "00" && dict["status"].ToString() == "1")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H001判断病人是否已经办卡";
                return false;
            }

        }

        /// <summary>
        /// H002建卡/激活
        /// </summary>
        /// <param name="resultCodeMsg">返回消息</param>
        /// <param name="terminalIp">终端IP</param>
        /// <param name="systemParamsXml">参数键值对</param>
        /// <returns></returns>
        public string CreateCard(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H002", "CreateCard", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H002", "CreateCard", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = dict["ResultData"].ToString();
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H002建卡/激活";
                return ret;
            }

        }

        /// <summary>
        /// H003获取门诊病人信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>·
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetPatientInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H003", "GetPatientInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                // ret = ConvertDictionaryToDataTable(dict);
                DataSet ds = ConvertXMLToDataSet(dict["ResultData"].ToString());
                ret = ds.Tables[0];
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H003", "GetPatientInfo", XMLHelper.GetXml(ret), "1", "0");
                ret = ColumnConvert(returnParamsDictionary, ret);
                if (dict["ResultCode"].ToString() == "0")
                {
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    //adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    // adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H003获取门诊病人信息";
                return ret;
            }

        }


        /// <summary>
        /// H004注销卡
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? RevocationCard(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            try
            {
                bool? ret = null;
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H004", "RevocationCard", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H004", "RevocationCard", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H004注销卡";
                return false;
            }

        }

        /// <summary>
        /// H005补卡
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? SupplementaryCard(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            bool? ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H005", "SupplementaryCard", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H005", "SupplementaryCard", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1# H005补卡";
                return false;
            }

        }

        /// <summary>
        /// H006修改卡密码
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? ChangeCardPassword(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            try
            {
                bool? ret = null;
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H006", "ChangeCardPassword", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H006", "ChangeCardPassword", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {

                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H006修改卡密码";
                return false;
            }

        }

        /// <summary>
        /// H007补录卡信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? MakeUpCardInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            bool? ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H007", "MakeUpCardInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H007", "MakeUpCardInfo", "true", "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H007补录卡信息";
                return false;
            }

        }

        /// <summary>
        /// H008获取预留就诊卡号（银行端申请就诊卡）
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetReservePatientCardNo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H008", "GetReservePatientCardNo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H008", "GetReservePatientCardNo", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H007补录卡信息";
                return ret;
            }

        }
        #endregion

        #region 3.充值/转账交易
        /// <summary>
        /// H101就诊卡充值
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string PatientCardRecharge(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();

                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H101", "PatientCardRecharge", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                string tradeMoney = systemParamsDictionary["amt"];
                if (retVal == null)
                {
                    adapterHss.UpdateTradeDetailsWithMoney("0302", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0302", TradeDetailId, tradeMoney);
                    adapterHss.writeAccount(TradeDetailId, tradeMoney, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), DateTime.Now.ToString("yyyyMMddhhmmss"), "0302", "his未返回数据（失败）");
                    return ret;
                }
                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H101", "PatientCardRecharge", dict["ResultData"].ToString(), "1", "0");

                if (dict["ResultCode"].ToString() == "0")
                {
                    //  ret = dict["ResultData"].ToString();

                    DataTable dt = ConvertXMLToDataSet("<moon>" + dict["ResultData"].ToString() + "</moon>").Tables[0];
                    dt = ColumnConvert(returnParamsDictionary, dt);
                    ret = dt.Rows[0]["CardYuE"].ToString();

                    adapterHss.UpdateTradeDetailsWithMoney("0301", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0301", TradeDetailId, tradeMoney);//记录账面信息
                    adapterHss.writeAccount(TradeDetailId, tradeMoney, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), DateTime.Now.ToString("yyyyMMddhhmmss"), "0301", "成功");
                }
                else
                {
                    adapterHss.UpdateTradeDetailsWithMoney("0302", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0302", TradeDetailId, tradeMoney);
                    adapterHss.writeAccount(TradeDetailId, tradeMoney, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), DateTime.Now.ToString("yyyyMMddhhmmss"), "0302", "失败");
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H101就诊卡充值";
                return ret;
            }

        }

        /// <summary>
        /// H102获取就诊卡在自助机可退款金额
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string GetPatientCardRefundBalance(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H102", "GetPatientCardRefundBalance", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H102", "GetPatientCardRefundBalance", dict["resultCode"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = dict["可退款金额"].ToString();
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }

                return ret;
            }
            catch (Exception e)
            {

                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H102获取就诊卡在自助机可退款金额";
                return ret;
            }

        }

        /// <summary>
        /// H103就诊卡退款（转账银行卡）
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string PatientCardRefund(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H103", "PatientCardRefund", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H103", "PatientCardRefund", dict["His交易流水号"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = dict["His交易流水号"].ToString();
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H103就诊卡退款（转账银行卡）";
                return ret;
            }

        }

        /// <summary>
        /// H104就诊卡退款冲正（转账银行卡失败时冲正）
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? PatientCardRefundReverse(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            bool? ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H104", "PatientCardRefundReverse", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H104", "PatientCardRefundReverse", dict["resultCode"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H104就诊卡退款冲正（转账银行卡失败时冲正）";
                return ret;
            }
        }

        /// <summary>
        /// H105查询就诊卡交易明细记录
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetPatientCardTradeRecord(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H105", "GetPatientCardTradeRecord", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H105", "GetPatientCardTradeRecord", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet(dict["ResultData"].ToString()).Tables[0];
                    ret = ColumnConvert(returnParamsDictionary, ret);
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H105查询就诊卡交易明细记录";
                return ret;
            }

        }

        /// <summary>
        /// H106查询就诊卡单笔原交易明细记录（验证His是否到账）
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetPatientCardOriginalTradeRecord(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H106", "GetPatientCardOriginalTradeRecord", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H106", "GetPatientCardOriginalTradeRecord", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H106查询就诊卡单笔原交易明细记录（验证His是否到账）";
                return ret;
            }

        }

        /// <summary>
        /// H107查询自助机交易明细记录
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetTerminalTradeRecord(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H107", "GetTerminalTradeRecord", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H107", "GetTerminalTradeRecord", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H107查询自助机交易明细记录";
                return ret;
            }

        }

        /// <summary>
        /// H108自助机交易记录汇总查询
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetTerminalTradeRecordSummary(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H108", "GetTerminalTradeRecordSummary", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H108", "GetTerminalTradeRecordSummary", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H108自助机交易记录汇总查询";
                return ret;
            }

        }
        #endregion

        #region 4.当日挂号
        /// <summary>
        /// H201获取当日实时科室排班表
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="TradeDetailId"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetRealTimeDepartmentSchedule(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;

            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H201", "GetRealTimeDepartmentSchedule", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）
                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                //20150803 moon 返回报文过长时截取前200个字符进行存储
                string trademsg = dict["List"].ToString().Length > 200 ? dict["List"].ToString().Substring(0, 200) : dict["List"].ToString();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H201", "GetRealTimeDepartmentSchedule", trademsg, "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H201获取当日实时科室排班表";
                return ret;
            }

        }

        /// <summary>
        /// H202获取当日实时医生排班表
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetRealTimeDoctorSchedule(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;

            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                customParamsDictionary.Add("RegDate", DateTime.Now.ToString("yyyy-MM-dd"));//挂号日期
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H202", "GetRealTimeDoctorSchedule", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                //20150803 moon 返回报文过长时截取前200个字符进行存储
                string trademsg = dict["List"].ToString().Length > 200 ? dict["List"].ToString().Substring(0, 200) : dict["List"].ToString();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H202", "GetRealTimeDoctorSchedule", trademsg, "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H202获取当日实时医生排班表";
                return ret;
            }

        }

        /// <summary>
        /// H203挂号
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable RealTimeRegister(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H203", "RealTimeRegister", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H203", "RealTimeRegister", dict["List"].ToString(), "1", "0");
                string tradeMoney = systemParamsDictionary["totalCost"];
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetailsWithMoney("0301", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0301", TradeDetailId, tradeMoney);
                }
                else
                {
                    adapterHss.UpdateTradeDetailsWithMoney("0302", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0302", TradeDetailId, tradeMoney);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H203挂号";
                return ret;
            }

        }

        /// <summary>
        /// H204获取挂号信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetRegisterInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H204", "GetRegisterInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H204", "GetRegisterInfo", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H204获取挂号信息";
                return ret;
            }

        }

        /// <summary>
        /// H205退号
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? BackNumber(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            bool? ret = null;

            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H205", "BackNumber", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H205", "BackNumber", "true", "1", "0");
                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H205退号";
                return ret;
            }

        }
        #endregion

        #region 5.预约挂号/取号
        /// <summary>
        /// H301获取预约科室排班表
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetReservationDepartmentSchedule(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H301", "GetReservationDepartmentSchedule", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H301", "GetReservationDepartmentSchedule", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H301获取预约科室排班表";
                return ret;
            }

        }

        /// <summary>
        /// H302获取预约医生排班表
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetReservationDoctorSchedule(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H302", "GetReservationDoctorSchedule", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H302", "GetReservationDoctorSchedule", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H302获取预约医生排班表";
                return ret;
            }

        }

        /// <summary>
        /// H303预约挂号
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable ReservationRegister(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H303", "ReservationRegister", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H303", "ReservationRegister", dict["List"].ToString(), "1", "0");
                string tradeMoney = systemParamsDictionary["totalCost"];
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetailsWithMoney("0301", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0301", TradeDetailId, tradeMoney);
                }
                else
                {
                    adapterHss.UpdateTradeDetailsWithMoney("0302", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0302", TradeDetailId, tradeMoney);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H303预约挂号";
                return ret;
            }

        }

        /// <summary>
        /// H304获取预约信息列表
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetReservationList(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H304", "GetReservationList", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H304", "GetReservationList", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H304获取预约信息列表";
                return ret;
            }

        }

        /// <summary>
        /// H305预约信息确认
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable ReservationRegisterConfirm(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H305", "ReservationRegisterConfirm", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H305", "ReservationRegisterConfirm", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H305预约信息确认";
                return ret;
            }

        }

        /// <summary>
        /// H306预约信息取消
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? ReservationRegisterCancel(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            bool? ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H306", "GetRealTimeDepartmentSchedule", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H306", "GetPrescriptionList", "true", "1", "0");
                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H306预约信息取消";
                return ret;
            }

        }
        #endregion

        #region 6.缴费
        /// <summary>
        /// H401获取需要缴费的处方列表
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetPrescriptionList(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H401", "GetPrescriptionList", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H401", "GetPrescriptionList", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H401获取需要缴费的处方列表";
                return ret;
            }

        }

        /// <summary>
        /// H402获取需要缴费的处方信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetPrescriptionInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H402", "GetPrescriptionInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H402", "GetPrescriptionInfo", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }

                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H402获取需要缴费的处方信息";
                return ret;
            }

        }

        /// <summary>
        /// H403缴费
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable PayConfirm(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            //DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H403", "PayConfirm", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H403", "PayConfirm", retVal.ToString(), "1", "0");
                string tradeMoney = systemParamsDictionary["amt"];
                tradeMoney = "0";
                if (dict["resultCode"].ToString() == "00")
                {
                    //ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetailsWithMoney("0301", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0301", TradeDetailId, tradeMoney);
                }
                else
                {
                    adapterHss.UpdateTradeDetailsWithMoney("0302", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0302", TradeDetailId, tradeMoney);
                }
                //项目开发人员根据具体需要修改一下代码。
                DataTable dt = XMLHelper.GetDataRowFromXML(dict["List"].ToString(), "Item");
                dt.Columns.Add("resultCode");
                dt.Columns.Add("resultMsg");
                dt.Rows[0]["resultCode"] = dict["resultCode"].ToString();
                dt.Rows[0]["resultMsg"] = dict["resultMsg"].ToString(); //XMLHelper.GetValueFromXML(retVal.ToString(), "Response/resultMsg");
                return dt;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H403缴费";
                return null;
            }

            //return ret;
        }

        /// <summary>
        /// H404获取就诊卡费用明细
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetPatientCardFeesDetail(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H404", "GetPatientCardFeesDetail", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H404", "GetPatientCardFeesDetail", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet(dict["ResultData"].ToString()).Tables[0];
                    ret = ColumnConvert(returnParamsDictionary, ret);
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H404获取就诊卡费用明细";
                return ret;
            }

        }
        #endregion

        #region 7.查询
        /// <summary>
        /// H501医院简介
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string GetHospitalInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;

            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H501", "GetHospitalInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H501", "GetHospitalInfo", dict["医院简介信息"].ToString(), "1", "0");

                if (dict["resultCode"].ToString() == "00")
                {
                    ret = dict["医院简介信息"].ToString();
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H501医院简介";
                return ret;
            }

        }

        /// <summary>
        /// H502科室简介
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetDepartmentInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;

            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H502", "GetDepartmentInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H502", "GetDepartmentInfo", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H502科室简介";
                return ret;
            }

        }

        /// <summary>
        /// H503专家简介
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetExpertInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;

            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H503", "GetExpertInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H503", "GetExpertInfo", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H503专家简介";
                return ret;
            }

        }

        /// <summary>
        /// H504查询药品类别
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetDrugType(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H504", "GetDrugType", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H504", "GetDrugType", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H504查询药品类别";
                return ret;
            }

        }

        /// <summary>
        /// H505查询药品信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetDrugInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H505", "GetDrugInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                //20150729 moon 返回报文过长时截取前200个字符进行存储
                string trademsg = dict["ResultData"].ToString().Length > 200 ? dict["ResultData"].ToString().Substring(0, 200) : dict["ResultData"].ToString();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H505", "GetDrugInfo", trademsg, "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet(dict["ResultData"].ToString()).Tables[0];
                    ret = ColumnConvert(returnParamsDictionary, ret);
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H505查询药品信息";
                return ret;
            }

        }

        /// <summary>
        /// H509查询药品医保比例
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetDrugYbbl(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H509", "GetDrugYbbl", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                //20150729 moon 返回报文过长时截取前200个字符进行存储
                // string trademsg = dict["ResultData"].ToString().Length > 200 ? dict["ResultData"].ToString().Substring(0, 200) : dict["ResultData"].ToString();
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H509", "GetDrugYbbl", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet("<moon>" + dict["ResultData"].ToString() + "</moon>").Tables[0];
                    ret = ColumnConvert(returnParamsDictionary, ret);
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H505查询药品信息";
                return ret;
            }

        }


        /// <summary>
        /// H506查询诊疗服务项目类别
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetMedicalServicesType(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H506", "GetMedicalServicesType", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H506", "GetMedicalServicesType", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H506查询诊疗服务项目类别";
                return ret;
            }

        }

        /// <summary>
        /// H507查询诊疗服务项目信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetMedicalServicesInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H507", "GetMedicalServicesInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H507", "GetMedicalServicesInfo", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet(dict["ResultData"].ToString()).Tables[0];
                    ret = ColumnConvert(returnParamsDictionary, ret);
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H507查询诊疗服务项目信息";
                return ret;
            }

        }
        /// <summary>
        /// H508查询消费明细
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="TradeDetailId"></param>
        /// <param name="systemParamXml"></param>
        /// <returns></returns>
        public DataTable GetPayedList(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H508", "GetPayedList", systemParamXml, "0", "0");
                object retVal = HisCustomInterfaceManager.HisTrade(AdapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H508", "GetPayedList", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {

                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                resultCodeMsg = "S#-1#H508查询消费明细";
                return ret;
            }
        }
        /// <summary>
        /// H509查询发药窗口
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetWindowOrder(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H509", "GetWindowOrder", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H509", "GetWindowOrder", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H509查询发药窗口信息";
                return ret;
            }

        }
        #endregion

        #region 8.检验单
        /// <summary>
        /// H601获取检验单列表
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetReportList(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;

            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H601", "GetReportList", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H601", "GetReportList", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet(dict["ResultData"].ToString()).Tables[0];
                    ret = ColumnConvert(returnParamsDictionary, ret);
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H601获取检验单列表";
                return ret;
            }

        }

        /// <summary>
        /// H602获取检验单项目结果
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetReportItems(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H602", "GetReportItems", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H602", "GetReportItems", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet(dict["ResultData"].ToString()).Tables[0];
                    ret = ColumnConvert(returnParamsDictionary, ret);
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H602获取检验单项目结果";
                return ret;
            }

        }

        /// <summary>
        /// H603更新检验单打印状态
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? UpdateReportPrintState(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            bool? ret = null;

            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H603", "UpdateReportPrintState", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H603", "GetDocSignature", "true", "1", "0");
                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H603更新检验单打印状态";
                return ret;
            }

        }

        /// <summary>
        /// H604获取医生签名
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetDocSignature(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H604", "GetDocSignature", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H604", "GetDocSignature", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H604获取医生签名";
                return ret;
            }

        }

        /// <summary>
        /// H605获取检验单图像
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetReportImage(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H605", "GetReportImage", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H605", "GetReportImage", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H605获取检验单图像";
                return ret;
            }

        }
        #endregion

        #region 9.绑定/解绑
        /// <summary>
        /// H701查询绑定信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetBindingInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H701", "GetBindingInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H701", "GetBindingInfo", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1# H701查询绑定信息";
                return ret;
            }

        }

        /// <summary>
        /// H702就诊卡银行卡绑定
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? PatientCardBind(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            bool? ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H702", "PatientCardBind", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H702", "PatientCardBind", "true", "1", "0");

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1# H702就诊卡银行卡绑定";
                return ret;
            }

        }

        /// <summary>
        /// H703就诊卡银行卡解绑
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public bool? PatientCardUnBind(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            bool? ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H703", "PatientCardUnBind", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H703", "PatientCardUnBind", "true", "1", "0");

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = true;
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H703就诊卡银行卡解绑";
                return ret;
            }

        }
        #endregion

        #region 10.住院
        /// <summary>
        /// H801获取住院病人信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetInPatientInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H801", "GetInPatientInfo", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H801", "GetInPatientInfo", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet(dict["ResultData"].ToString()).Tables[0];
                    ret = ColumnConvert(returnParamsDictionary, ret);
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }

                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H801获取住院病人信息";
                return ret;
            }

        }

        /// <summary>
        /// H802获取在院病人费用明细
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetInPatientInHospitalFeesDetail(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H802", "GetInPatientInHospitalFeesDetail", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H802", "GetInPatientInHospitalFeesDetail", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet(dict["ResultData"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                ret = ColumnConvert(returnParamsDictionary, ret);
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H802获取在院病人费用明细";
                return ret;
            }

        }

        /// <summary>
        /// H803获取出院病人费用明细
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetInPatientDischargedFeesDetail(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H803", "GetInPatientDischargedFeesDetail", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H803", "GetInPatientDischargedFeesDetail", dict["List"].ToString(), "1", "0");
                if (dict["resultCode"].ToString() == "00")
                {
                    ret = ConvertXMLToDataSet(dict["List"].ToString()).Tables[0];
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                // Adapter_Error(e);
                resultCodeMsg = "S#-1#H803获取出院病人费用明细";
                return ret;
            }

        }

        /// <summary>
        /// H804住院预交金充值
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string InPatientRecharge(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H804", "InPatientRecharge", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);
                string tradeMoney = systemParamsDictionary["amt"];
                if (retVal == null)
                {
                    adapterHss.UpdateTradeDetailsWithMoney("0302", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfoWithMoney("0302", TradeDetailId, tradeMoney);
                    adapterHss.writeAccount(TradeDetailId, tradeMoney, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), DateTime.Now.ToString("yyyyMMddhhmmss"), "0302", "his未返回数据（失败）");
                    return ret;
                }
                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H804", "InPatientRecharge", dict["ResultData"].ToString(), "1", "0");

                if (dict["ResultCode"].ToString() == "0")
                {
                    DataTable dt = ConvertXMLToDataSet("<moon>" + dict["ResultData"].ToString() + "</moon>").Tables[0];
                    dt = ColumnConvert(returnParamsDictionary, dt);
                    ret = dt.Rows[0]["CardYuE"].ToString();

                    adapterHss.UpdateTradeDetailsWithMoney("0301", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    adapterHss.writeAccount(TradeDetailId, tradeMoney, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), DateTime.Now.ToString("yyyyMMddhhmmss"), "0301", "成功");
                }
                else
                {
                    adapterHss.UpdateTradeDetailsWithMoney("0301", TradeDetailId, tradeMoney);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    adapterHss.writeAccount(TradeDetailId, tradeMoney, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), DateTime.Now.ToString("yyyyMMddhhmmss"), "0302", "失败");
                }
                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1#H804住院预交金充值";
                return ret;
            }

        }

        /// <summary>
        /// H805获取住院账户缴费明细
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public DataTable GetInPatientTradeRecord(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            DataTable ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                Dictionary<string, string> returnParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;

                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H805", "GetInPatientTradeRecord", systemParamsXml, "0", "0");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = HisCustomInterfaceManager.HisTrade(adapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary, ref returnParamsDictionary);

                Dictionary<string, object> dict = (Dictionary<string, object>)retVal;
                if (dict["ResultCode"].ToString() != "0")
                {
                    dict["ResultData"] = dict["ErrorMsg"];
                }
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "H805", "GetInPatientTradeRecord", dict["ResultData"].ToString(), "1", "0");
                if (dict["ResultCode"].ToString() == "0")
                {
                    ret = ConvertXMLToDataSet(dict["ResultData"].ToString()).Tables[0];
                    ret = ColumnConvert(returnParamsDictionary, ret);
                    adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0303", TradeDetailId);
                }
                else
                {
                    adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                    adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                }

                return ret;
            }
            catch (Exception e)
            {
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                HisLog.HisLogError(log, sErrorMsg, terminalIp);
                //Adapter_Error(e);
                resultCodeMsg = "S#-1# H805获取住院账户缴费明细";
                return ret;
            }

        }

        #endregion

    }
}
