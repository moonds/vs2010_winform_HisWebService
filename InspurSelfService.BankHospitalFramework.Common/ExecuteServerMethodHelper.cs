using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace InspurSelfService.BankHospitalFramework.Common
{
    public class ExecuteServerMethodHelper
    {
        static ExecuteServerServiceClient essc = null;

        /// <summary>
        /// 执行交易接口
        /// 确保Web.config或App.config的配置文件

        /// <client>
        ///     <endpoint address="http://XXX/ExecuteServerService.svc"
        ///     binding="basicHttpBinding" 
        ///     contract="TransactionServer.IExecuteServerService" name="BasicHttpBinding_IExecuteServerService" />
        /// </client>
        /// </summary>
        /// <param name="errorFlag">错误标志(等级#返回码#返回消息)</param>
        /// <param name="terminalIp">终端IP</param>
        /// <param name="pageName">页面名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="parameters">方法参数</param>
        /// <returns>返回值</returns>
        public static T ExecuteServerMethod<T>(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters)
        {
            //级别#返回码#返回消息【Page级别】
            errorFlag = "P#000#启动页面调用";
            object result = null;
            essc = new ExecuteServerServiceClient();
            try
            {
                if (typeof(T) == typeof(int?))
                {
                    int argsCount = parameters.Length;
                    if (parameters != null && parameters.Length > 0 && parameters[argsCount - 1].GetType() == typeof(DataTable))
                    {
                        List<object> args = parameters.ToList();
                        args.RemoveAt(argsCount - 1);
                        DataTable dt = (DataTable)parameters[argsCount - 1];
                        if (string.IsNullOrWhiteSpace(dt.TableName))
                            dt.TableName = "dt1";
                        result = essc.ExecuteServerMethodInt(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, dt, args.ToArray());
                    }
                    else
                        result = essc.ExecuteServerMethodInt(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
                }
                else if (typeof(T) == typeof(bool?))
                {
                    result = essc.ExecuteServerMethodBoolean(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
                }
                else if (typeof(T) == typeof(long?))
                {
                    result = essc.ExecuteServerMethodLong(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
                }
                else if (typeof(T) == typeof(float?))
                {
                    result = essc.ExecuteServerMethodFloat(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
                }
                else if (typeof(T) == typeof(string))
                {
                    result = essc.ExecuteServerMethodString(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
                }
                else if (typeof(T) == typeof(DataTable))
                {
                    result = essc.ExecuteServerMethodDataTable(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
                }
                else if (typeof(T) == typeof(DataSet))
                {
                    result = essc.ExecuteServerMethodDataSet(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
                }
            }
            catch (Exception ex)
            {
                errorFlag = "P#001#" + ex.Message;            
                //log4 记录日志
            }
            finally
            {
                essc.Close();
            }
            return (T)result.ChangeType(typeof(T));
        }
    }
}