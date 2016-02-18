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
    public class AdapterBankCall : BasePage
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);

        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string AdapterBankCallTest(out string resultCodeMsg, string terminalIp, string systemParamsXml)
        {
            string ret = null;
            Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
            Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
            object obj = BankCallCustomInterfaceManager.BankCallTrade(AdapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
            return ret;
        }
    }
}
