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
    public class AdapterHisCall : BasePage
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);

        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="resultCodeMsg"></param>
        /// <param name="terminalIp"></param>
        /// <param name="systemParamsXml"></param>
        /// <returns></returns>
        public string AdapterHisCallTest(out string resultCodeMsg, string terminalIp, string systemParamsXml)
        {
            string ret = null;
            Dictionary<string, string> systemParamsDictionary = GetSystemParamsDictionary(systemParamsXml);
            Dictionary<string, string> customParamsDictionary = new Dictionary<string, string>();
            object obj = HisCallCustomInterfaceManager.HisCallTrade(AdapterTrace, out resultCodeMsg, log, terminalIp, System.Reflection.MethodBase.GetCurrentMethod(), systemParamsDictionary, customParamsDictionary);
            resultCodeMsg = "S#0#测试成功";
            ret = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return ret;
        }
    }
}
