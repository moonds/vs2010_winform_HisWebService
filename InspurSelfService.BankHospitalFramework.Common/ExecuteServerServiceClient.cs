using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;

namespace InspurSelfService.BankHospitalFramework.Common
{
    [ServiceContract(ConfigurationName = "IExecuteServerService")]
    public interface IExecuteServerService
    {
        [OperationContract]
        System.Nullable<int> ExecuteServerMethodInt(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters);

        [OperationContract(Name = "ExecuteServerMethodIntDataTable")]
        System.Nullable<int> ExecuteServerMethodInt(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, DataTable dt, object[] parameters);

        [OperationContract]
        System.Nullable<bool> ExecuteServerMethodBoolean(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters);

        [OperationContract]
        System.Nullable<long> ExecuteServerMethodLong(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters);

        [OperationContract]
        System.Nullable<float> ExecuteServerMethodFloat(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters);

        [OperationContract]
        void ExecuteServerMethodVoid(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters);

        [OperationContract]
        string ExecuteServerMethodString(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters);

        [OperationContract]
        System.Data.DataTable ExecuteServerMethodDataTable(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters);

        [OperationContract]
        System.Data.DataSet ExecuteServerMethodDataSet(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters);
    }

    public interface IExecuteServerServiceChannel : IExecuteServerService, System.ServiceModel.IClientChannel
    {
    }

    public partial class ExecuteServerServiceClient : System.ServiceModel.ClientBase<IExecuteServerService>, IExecuteServerService
    {

        public ExecuteServerServiceClient()
        {
        }

        public ExecuteServerServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public ExecuteServerServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public ExecuteServerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public ExecuteServerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public System.Nullable<int> ExecuteServerMethodInt(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters)
        {
            return base.Channel.ExecuteServerMethodInt(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
        }

        public System.Nullable<int> ExecuteServerMethodInt(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, DataTable dt, object[] parameters)
        {
            return base.Channel.ExecuteServerMethodInt(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, dt, parameters);
        }

        public System.Nullable<long> ExecuteServerMethodLong(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters)
        {
            return base.Channel.ExecuteServerMethodLong(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
        }

        public System.Nullable<bool> ExecuteServerMethodBoolean(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters)
        {
            return base.Channel.ExecuteServerMethodBoolean(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
        }

        public System.Nullable<float> ExecuteServerMethodFloat(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters)
        {
            return base.Channel.ExecuteServerMethodFloat(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
        }

        public void ExecuteServerMethodVoid(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters)
        {
            base.Channel.ExecuteServerMethodVoid(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
        }

        public string ExecuteServerMethodString(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters)
        {
            return base.Channel.ExecuteServerMethodString(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
        }

        public System.Data.DataTable ExecuteServerMethodDataTable(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters)
        {
            return base.Channel.ExecuteServerMethodDataTable(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
        }

        public System.Data.DataSet ExecuteServerMethodDataSet(out string errorFlag, string businessTrace, string terminalIp, string pageName, string methodName, string userName, string pwd, object[] parameters)
        {
            return base.Channel.ExecuteServerMethodDataSet(out errorFlag, businessTrace, terminalIp, pageName, methodName, userName, pwd, parameters);
        }
    }
}
