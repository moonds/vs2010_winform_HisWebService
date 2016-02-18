/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：Adapter.Bank.cs
 * 描    述：银行接口适配处理类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;

namespace InspurSelfService.BankHospitalFramework.Adapter
{
    /// <summary>
    /// Bank适配器
    /// </summary>
    public class AdapterBank : BasePage
    {
        //private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);

        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string AdapterBankTest(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
            Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
            //object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);

            resultCodeMsg = "S#0#测试成功";
            ret = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return ret;
        }

        /// <summary>
        /// B001银行签到
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string Signin(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B001", "Signin", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = BankCustomInterfaceManager.BankTrade(adapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);

                if (retVal != null)
                {
                    adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B001", "Signin", retVal.ToString(), "1", "1");
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    string workKey = ds.Tables[1].Rows[0]["WorkKey"].ToString();
                    string pinKey = workKey.Substring(5, 16);
                    string macKey = workKey.Substring(21);
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    }
                    ret = String.Format("{0}|{1}|{2}|{3}", resultCode, resultMsg, pinKey, macKey);
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
                resultCodeMsg = "E#-1#银行签到失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                ////log.Error(sErrorMsg);
               // log.Logger.Repository.Shutdown();
                return resultCodeMsg;
            }

        }

        /// <summary>
        /// B002绑定
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string Bind(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                string adapterTrace = AdapterTrace;
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B002", "Bind", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = BankCustomInterfaceManager.BankTrade(adapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);

                if (retVal != null)
                {
                    adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B002", "Bind", retVal.ToString(), "1", "1");
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    }
                    ret = String.Format("{0}|{1}", resultCode, resultMsg);
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

                resultCodeMsg = "F#-1#银行绑定失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                ////log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }

        /// <summary>
        /// B003解绑
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string Unbind(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B003", "Unbind", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）
                object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
                if (retVal != null)
                {
                    adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B003", "Unbind", retVal.ToString(), "1", "1");
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    }
                    ret = String.Format("{0}|{1}", resultCode, resultMsg);
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
                resultCodeMsg = "F#B003#银行解绑失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                ////log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }

        /// <summary>
        /// B004充值（银行卡转账到诊疗卡）
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string Recharge(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B004", "Recharge", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）
                string tradeMoney = systemParamsDictionary["amt"];
                object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);

                if (retVal != null)
                {
                    adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B004", "Recharge", retVal.ToString(), "1", "1");
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetailsWithMoney("0301", TradeDetailId, tradeMoney);
                        adapterHss.UpdateTradeInfoWithMoney("0301", TradeDetailId, tradeMoney);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetailsWithMoney("0302", TradeDetailId, tradeMoney);
                        adapterHss.UpdateTradeInfoWithMoney("0302", TradeDetailId, tradeMoney);
                    }
                    ret = String.Format("{0}|{1}", resultCode, resultMsg);
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
                resultCodeMsg = "F#B004#银行充值失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                ////log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }

        /// <summary>
        /// B005充值冲正（转账诊疗卡失败时） 
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string RechargeReverse(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B005", "RechargeReverse", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）
                string tradeMoney = systemParamsDictionary["amt"];
                object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);

                if (retVal != null)
                {
                    adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B005", "RechargeReverse", retVal.ToString(), "1", "1");
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetailsWithMoney("0301", TradeDetailId, tradeMoney);
                        adapterHss.UpdateTradeInfoWithMoney("0301", TradeDetailId, tradeMoney);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetailsWithMoney("0302", TradeDetailId, tradeMoney);
                        adapterHss.UpdateTradeInfoWithMoney("0302", TradeDetailId, tradeMoney);
                    }
                    ret = String.Format("{0}|{1}", resultCode, resultMsg);
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
                resultCodeMsg = "F#B005#银行充值冲正失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                ////log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }

        /// <summary>
        /// B006退款（诊疗卡转账银行卡）
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string Refund(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B006", "Refund", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
                if (retVal != null)
                {
                    adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B006", "Refund", retVal.ToString(), "1", "1");
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    }
                    ret = String.Format("{0}|{1}", resultCode, resultMsg);
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
                resultCodeMsg = "F#B006#银行退款失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                //log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }

        /// <summary>
        /// B007查询原交易信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string QueryOriginalTradeRecord(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B007", "QueryOriginalTradeRecord", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B007", "QueryOriginalTradeRecord", retVal.ToString(), "1", "1");
                if (retVal != null)
                {
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    }
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
                resultCodeMsg = "F#B007#银行原交易查询失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                //log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }

        /// <summary>
        /// B008查询银行卡账户信息
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string QueryBankCardAccountInfo(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B008", "QueryBankCardAccountInfo", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B008", "QueryBankCardAccountInfo", retVal.ToString(), "1", "1");
                if (retVal != null)
                {
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    string Name = ds.Tables[1].Rows[0]["Name"].ToString();
                    string IDNo = ds.Tables[1].Rows[0]["IDNo"].ToString();
                    string Balance = ds.Tables[1].Rows[0]["Balance"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    }
                    ret = String.Format("{0}|{1}|{2}|{3}|{4}", resultCode, resultMsg, Name, IDNo, Balance);
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
                resultCodeMsg = "F#B008#查询银行卡账户信息失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                //log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }

        /// <summary>
        /// B009查询余额
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string BankBalance(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B009", "BankBalance", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）
                object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B009", "BankBalance", retVal.ToString(), "1", "1");
                if (retVal != null)
                {
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    string Balance = ds.Tables[1].Rows[0]["Balance"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    }
                    ret = String.Format("{0}|{1}|{2}", resultCode, resultMsg, Balance);
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
                resultCodeMsg = "F#B009#银行卡查询余额失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                //log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }

        /// <summary>
        /// B010验证银行卡密码
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string CheckBankPwd(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B010", "CheckBankPwd", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B011", "CheckBankPwd", retVal.ToString(), "1", "1");
                if (retVal != null)
                {
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    }
                    ret = String.Format("{0}|{1}", resultCode, resultMsg);
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
                resultCodeMsg = "F#B010#验证银行卡密码失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                //log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }

        /// <summary>
        /// B011卡表下载
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string BankCardClassDownload(out string resultCodeMsg, string terminalIp, string TradeDetailId, string systemParamsXml)
        {
            string ret = null;
            try
            {
                Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
                Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
                AdapterHss adapterHss = new AdapterHss();
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B011", "BankCardClassDownload", systemParamsXml, "0", "1");//报文类型int（0---发送 ，1---接收），日志类型Int（0---His 1---Bank）

                object retVal = BankCustomInterfaceManager.BankTrade(AdapterTrace, out resultCodeMsg, null, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
                adapterHss.InsertLogTrade(TradeDetailId, DateTime.Now.ToString(), "B011", "BankCardClassDownload", retVal.ToString(), "1", "1");
                if (retVal != null)
                {
                    DataSet ds = ConvertXMLToDataSet(retVal.ToString());
                    string resultCode = ds.Tables[0].Rows[0]["ResponseCode"].ToString();
                    string resultMsg = ds.Tables[0].Rows[0]["ResponseMsg"].ToString();
                    if (resultCode == "0000")
                    {
                        adapterHss.UpdateTradeDetails("0301", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0301", TradeDetailId);
                    }
                    else
                    {
                        adapterHss.UpdateTradeDetails("0302", TradeDetailId);
                        adapterHss.UpdateTradeInfo("0302", TradeDetailId);
                    }
                    ret = String.Format("{0}|{1}", resultCode, resultMsg);
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
                resultCodeMsg = "F#B011#银行卡卡表下载失败:" + e.Message;
                string sErrorMsg = string.Empty;
                sErrorMsg += "错误信息：\r\n" + @e.Message + "\r\n";
                sErrorMsg += "调试跟踪：" + @e.StackTrace.ToString();
                //log.Error(sErrorMsg);
                return resultCodeMsg;
            }
        }
    }
}
