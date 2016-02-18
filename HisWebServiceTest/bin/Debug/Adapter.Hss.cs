/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：Adapter.Hss.cs
 * 描    述：Hss 处理类
/************************************************************************/
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace InspurSelfService.BankHospitalFramework.Adapter
{
    /// <summary>
    /// 银医系统后台标准交易
    /// </summary>
    public class AdapterHss
    {
        #region Y000 1.1添加病人信息至Hss数据库病人主索引信息表
        /// <summary>
        /// Y000  添加病人信息至Hss数据库病人主索引信息表
        /// </summary>
        /// <param name="patientId">病人主索引Id</param>
        /// <param name="patientName">病人姓名</param>
        /// <param name="birthDate">出生日期</param>
        /// <param name="sex">性别（取值：1男、2女、9未知）</param>
        /// <param name="socialNo">身份证号</param>
        /// <param name="homeStreet">住址</param>
        /// <param name="relationTel">联系电话</param>
        /// <param name="micNo">医保号</param>
        /// <returns>信息添加成功返回影响行数，失败返回null</returns>
        public int? InsertPatientMIToHss(string patientId, string patientName, string birthDate, string sex, string socialNo, string homeStreet, string relationTel, string micNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"INSERT INTO [PatientMasterIndex] 
                ([patient_id],[name],[birth_date],[sex],[social_no],[home_street],[relation_tel],[base_insure_no]) 
                VALUES( @patient_id, @name, @birth_date, @sex, @social_no, @home_street, @relation_tel, @base_insure_no)";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patient_id",patientId),
                                                new SqlParameter("@name",patientName),
                                                new SqlParameter("@birth_date",birthDate),
                                                new SqlParameter("@sex",sex),
                                                new SqlParameter("@social_no",socialNo),
                                                new SqlParameter("@home_street",homeStreet),
                                                new SqlParameter("@relation_tel",relationTel),
                                                new SqlParameter("@base_insure_no",micNo)
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y000；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y001 1.2通过病人主索引Id判断Hss数据库中病人主索引信息是否存在
        /// <summary>
        /// Y001  通过病人主索引Id判断Hss数据库中病人主索引信息是否存在
        /// </summary>
        /// <param name="patientId">病人主索引Id</param>
        /// <returns>查询数据库失败返回null，病人信息存在返回true，否则返回false</returns>
        public bool? ExistPatientIdByPatientId(string patientId)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT COUNT(*) FROM [PatientMasterIndex] WHERE [patient_id] = @patient_id ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patient_id",patientId)                                                
                                            };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return false;
                else
                    if (Convert.ToInt32(obj) > 0)
                        return true;
                    else
                        return false;
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y001；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y002 1.3修改Hss数据库中病人主索引信息
        /// <summary>
        /// Y002  修改Hss数据库中病人主索引信息
        /// </summary>
        /// <param name="patientId">病人主索引Id</param>
        /// <param name="patientName">病人姓名</param>
        /// <param name="birthDate">出生日期</param>
        /// <param name="sex">性别（取值：1男、2女、9未知）</param>
        /// <param name="socialNo">身份证号</param>
        /// <param name="homeStreet">住址</param>
        /// <param name="relationTel">联系电话</param>
        /// <param name="micNo">医保号</param>
        /// <returns>信息添加成功返回影响行数，失败返回null</returns>        
        public int? UpdatePatientMI(string patientId, string patientName, string birthDate, string sex, string socialNo, string homeStreet, string relationTel, string micNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"UPDATE [PatientMasterIndex] 
                SET [name] = @name, 
                [birth_date] = @birth_date, 
                [sex] = @sex, 
                [social_no] = @social_no, 
                [home_street] = @home_street, 
                [relation_tel] = @relation_tel, 
                [base_insure_no] = @base_insure_no  
                 WHERE [patient_id] = @patient_id ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patient_id",patientId),
                                                new SqlParameter("@name",patientName),
                                                new SqlParameter("@birth_date",birthDate),
                                                new SqlParameter("@sex",sex),
                                                new SqlParameter("@social_no",socialNo),
                                                new SqlParameter("@home_street",homeStreet),
                                                new SqlParameter("@relation_tel",relationTel),
                                                new SqlParameter("@base_insure_no",micNo)
                                            };

            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y002；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y003 1.4根据身份证号判断病人主索引信息是否注册
        /// <summary>
        /// Y003  根据身份证号判断病人主索引信息是否注册
        /// </summary>
        /// <param name="socialNo">身份证号</param>
        /// <returns>查询数据库失败返回null，病人信息存在返回true，否则返回false</returns>
        public bool? ExistPatientBySocialCardNo(string socialNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT COUNT(*) FROM [PatientMasterIndex]  WHERE [social_no] = @social_no ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@social_no",socialNo)                                                
                                            };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return false;
                else
                    if (Convert.ToInt32(obj) > 0)
                        return true;
                    else
                        return false;
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y003；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y004 1.5判断病人就诊卡信息是否存在
        /// <summary>
        /// Y004  判断病人就诊卡信息是否存在
        /// </summary>
        /// <param name="patientCardId">病人就诊卡号</param>
        /// <returns>查询数据库失败返回null，病人信息存在返回true，否则返回false</returns>
        public bool? ExistPatientCardId(string patientCardId)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT COUNT(*) FROM [PatientCardBlance] WHERE [PatientCardId] = @PatientCardId ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@PatientCardId",patientCardId)                                                
                                            };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return false;
                else
                    if (Convert.ToInt32(obj) > 0)
                        return true;
                    else
                        return false;
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y004；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y005 1.6写入一条病人就诊卡信息（发卡时调用）
        /// <summary>
        /// Y005  写入一条病人就诊卡信息（发卡时调用）
        /// </summary>
        /// <param name="patientCardId">病人就诊卡号</param>
        /// <param name="patientName">病人姓名</param>
        /// <param name="birthDate">出生日期</param>
        /// <param name="sex">性别（取值：1男、2女、9未知）</param>
        /// <param name="socialNo">身份证号</param>
        /// <param name="homeStreet">住址</param>
        /// <param name="relationTel">联系电话</param>
        /// <returns>信息添加成功返回影响行数，失败返回null</returns>
        public int? InsertPatientCard(string patientCardId, string patientName, string birthDate, string sex, string socialNo, string homeStreet, string relationTel, string terminalIp)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"INSERT INTO [PatientCardBlance] 
                ([PatientCardId], [InputDate], [CurrentMoney], [PatientCardState], [PatientName], [BirthDate], [SexId], [SocialNo], [HomeStreet], [RelationTel],[terminalIp] ) 
                VALUES( @PatientCardId, @InputDate, @CurrentMoney, @PatientCardState, @PatientName, @BirthDate, @SexId, @SocialNo, @HomeStreet, @RelationTel, @terminalIp)";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@PatientCardId",patientCardId),
                                                new SqlParameter("@InputDate",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                                                new SqlParameter("@CurrentMoney",0),//就诊卡初值现金为0元
                                                new SqlParameter("@PatientCardState",1),//就诊卡状态为1
                                                new SqlParameter("@PatientName",patientName),
                                                new SqlParameter("@BirthDate",birthDate),
                                                new SqlParameter("@SexId",sex),
                                                new SqlParameter("@SocialNo",socialNo),
                                                new SqlParameter("@HomeStreet",homeStreet),
                                                new SqlParameter("@RelationTel",relationTel),
                                                new SqlParameter("@terminalIp",terminalIp)
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y005；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y006 1.7删除一条病人就诊卡信息
        /// <summary>
        /// Y006  删除一条病人就诊卡信息
        /// </summary>
        /// <param name="patientCardId">病人就诊卡号</param>
        /// <returns>信息添加成功返回影响行数，失败返回null</returns>
        public int? DeletePatientCard(string patientCardId)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"DELETE FROM [PatientCardBlance] WHERE [PatientCardId] = @PatientCardId";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@PatientCardId",patientCardId)                                                
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y006；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y007 1.8更新一条就诊卡信息
        /// <summary>
        /// Y007  更新一条就诊卡信息
        /// </summary>
        /// <param name="patientCardId">病人就诊卡号</param>
        /// <param name="patientName">病人姓名</param>
        /// <param name="birthDate">出生日期</param>
        /// <param name="sex">性别（取值：1男、2女、9未知）</param>
        /// <param name="socialNo">身份证号</param>
        /// <param name="homeStreet">住址</param>
        /// <param name="relationTel">联系电话</param>
        /// <returns>信息添加成功返回影响行数，失败返回null</returns>
        public int? UpdatePatientCard(string patientCardId, string patientName, string birthDate, string sex, string socialNo, string homeStreet, string relationTel)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"UPDATE [PatientCardBlance] 
                SET [PatientName] = @PatientName,                  
                [BirthDate] = @BirthDate, 
                [SexId] = @SexId, 
                [SocialNo] = @SocialNo, 
                [HomeStreet] = @HomeStreet, 
                [RelationTel] = @RelationTel 
                 WHERE [PatientCardId] = @PatientCardId ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@PatientCardId",patientCardId),
                                                new SqlParameter("@PatientName",patientName),
                                                new SqlParameter("@BirthDate",birthDate),
                                                new SqlParameter("@SexId",sex),
                                                new SqlParameter("@SocialNo",socialNo),
                                                new SqlParameter("@HomeStreet",homeStreet),
                                                new SqlParameter("@RelationTel",relationTel)                                              
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y007；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y008 1.9根据病人就诊卡号查询病人就诊信息
        /// <summary>
        /// Y008  根据病人就诊卡号查询病人就诊信息
        /// </summary>
        /// <param name="patientCardId">病人就诊卡号</param>
        /// <returns>信息查询成功返回datatable对象，否则返回null</returns>
        public DataTable SelectPatientCard(string patientCardId)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [PatientCardId], [PatientName], [BirthDate], [SexId], [SocialNo], [HomeStreet], [RelationTel] 
                 FROM [PatientCardBlance] 
                 WHERE [PatientCardId] = @PatientCardId ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@PatientCardId",patientCardId)                                                                                              
                                            };
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlString, parameterValues).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y008；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y009 1.10写入一条充值信息到银医
        /// <summary>
        /// Y009  写入一条充值信息到银医
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="fillTypeId">充值类型（1现金，2,银行卡）</param>
        /// <param name="jinE">金额</param>
        /// <param name="fillDate">充值日期</param>
        /// <param name="bankCardNo">银行卡号（现金充值时为””）</param>
        /// <param name="transactionTypeId">交易类型(1充值，2退款)</param>
        /// <returns>信息添加成功返回1，失败返回null</returns>
        public int? InsertFillMoneyDetail(string patientCardId, string fillTypeId, string jinE, string fillDate, string bankCardNo, string transactionTypeId, string description, string terminalIp, string trace, string bankSettlementDate)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "p_FillMoneyToMedicalCard";
            SqlParameter[] parameters = { 
                                        new SqlParameter("@PatientCardId",patientCardId),
                                        new SqlParameter("@FillTypeId",fillTypeId),
                                        new SqlParameter("@JinE",jinE),
                                        new SqlParameter("@FillDate",fillDate),
                                        new SqlParameter("@BankCardNo",bankCardNo),
                                        new SqlParameter("@TransactionTypeId",transactionTypeId),    
                                        new SqlParameter("@description",description),
                                        new SqlParameter("@terminalIp",terminalIp), //暂时制定new OracleParameter("@czgh","2967"),//
                                        new SqlParameter("@trace",trace), //医院流水号
                                        new SqlParameter("@bankSettlementDate",bankSettlementDate),
                                        };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, sqlString, parameters);
                if (obj == DBNull.Value || obj == null)
                    return 0;
                else
                    return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y009；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y010 1.11获取就诊卡日期范围交易列表
        /// <summary>
        /// Y010  获取就诊卡日期范围交易列表
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>就诊卡号，充值类型(现金，银行卡)，交易金额，交易日期</returns>
        public DataTable GetPatientCardDetailCheckHss(string startDate, string endDate)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [tab1].[PatientCardId] AS '就诊卡号' 
                                        ,[tab2].[FillType] AS '充值类型' 
                                        ,[tab1].[JinE] AS '交易金额' 
                                        ,[tab1].[FillDate] AS '交易日期' 
                                        FROM [PatientCardDetail] AS tab1 INNER JOIN [PatientCardFillMoneyType] AS tab2 
                                        ON tab1.FillTypeId=tab2.Id 
                                        WHERE tab1.FillDate BETWEEN @startDate AND @endDate ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@startDate",startDate),
                                                new SqlParameter("@endDate",endDate)                                            
                                            };
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlString, parameterValues).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y010；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y011 1.12根据就诊卡号获取病人姓名
        /// <summary>
        /// Y011  根据就诊卡号获取病人姓名
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <returns>信息查询成功返回病人姓名，否则返回null</returns>
        public String GetPatientName(string patientCardId)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [PatientName] FROM [PatientCardBlance] WHERE [PatientCardId] = @PatientCardId";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@PatientCardId",patientCardId)                                                                                              
                                            };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y011；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y012 1.13执行病人费用退款(银行卡、医保卡)未实现
        /// <summary>
        /// Y012**未实现**执行病人费用退款(银行卡、医保卡)
        /// </summary>
        /// <param name="patientCardId">病人就诊卡号</param>
        /// <param name="returnFeeType">退费类型(1,银行卡，2.医保卡)</param>
        /// <param name="jinE">退款金额</param>
        /// <returns>信息添加成功返回影响行数，否则返回null</returns>
        public int? ExecutePatientReturnFeeHss(string patientCardId, string returnFeeType, string jinE)
        {
            throw new Exception("交易码：Y012；交易操作：未实现");
        }
        #endregion

        #region Y013 1.14插入一条就诊卡银行卡绑定信息
        /// <summary>
        /// Y013  插入一条就诊卡银行卡绑定信息
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="bankCardNo">银行卡号</param>
        /// <param name="bankId">银行Id</param>
        /// <param name="inputDate">插入日期</param>
        /// <param name="bindingstate">绑定状态(1.绑定，0.取消绑定)</param>
        /// <returns>信息添加成功返回影响行数，否则返回null</returns>
        public int? InsertBankCardBinding(string patientCardId, string bankCardNo, string bankId, string inputDate, string bindingstate)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            //string sqlupdate = @"update [PatientCardBinding] set [BindingState]=0 where [BankCardNo]=@BankCardNo and [BindingState]=1";
            // SqlParameter[] parametervalue = { new SqlParameter("@BankCardNo", bankCardNo) };
            string sqlString = @"INSERT INTO [PatientCardBinding] 
                ([PatientCardId],[BankCardNo],[BankId],[InputDate],[BindingState])
                VALUES( @patientCardId, @bankCardNo, @bankId, @inputDate, @bindingstate) ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patientCardId",patientCardId),
                                                new SqlParameter("@bankCardNo",bankCardNo),
                                                new SqlParameter("@bankId",bankId),
                                                new SqlParameter("@inputDate",inputDate),
                                                new SqlParameter("@bindingstate",bindingstate),
                                            };
            try
            {
                //  SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlupdate, parametervalue);
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y013；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y014 1.15绑定就诊卡银行卡
        /// <summary>
        /// Y014 绑定就诊卡银行卡
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="bankCardNo">银行卡号</param>
        /// <returns>操作执行成功返回影响行数，否则返回null</returns>
        public int? BindingBankCard(string patientCardId, string bankCardNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"UPDATE [PatientCardBinding] SET [BindingState] = 1 WHERE [PatientCardId] = @patientCardId 
                 AND [BankCardNo] = @bankCardNo ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patientCardId",patientCardId),
                                                new SqlParameter("@bankCardNo",bankCardNo)                                               
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y014；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y015 1.16就诊卡银行卡解绑定
        /// <summary>
        /// Y015  就诊卡银行卡解绑定
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="bankCardNo">银行卡号</param>
        /// <returns>操作执行成功返回影响行数，否则返回null</returns>
        public int? UnBindingBankCard(string patientCardId, string bankCardNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"UPDATE [PatientCardBinding] SET [BindingState] = 0 WHERE [PatientCardId] = @patientCardId 
                 AND [BankCardNo] = @bankCardNo ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patientCardId",patientCardId),
                                                new SqlParameter("@bankCardNo",bankCardNo)                                               
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y015；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y016 1.17根据绑定记录Id号解绑定 遗弃方法
        /// <summary>
        /// Y016**遗弃方法**根据绑定记录Id号解绑定
        /// </summary>
        /// <param name="id">绑定记录Id号</param>
        /// <returns>操作执行成功返回影响行数，否则返回null</returns>
        public int? JieBindingBankCardById(string id)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"UPDATE [PatientCardBinding] SET [BindingState] = 0 WHERE [Id] = @id ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@id",id)                                                                                               
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception("交易码：Y015；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y017 1.18就诊卡银行卡绑定是否存在
        /// <summary>
        /// Y017  就诊卡银行卡绑定是否存在
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="bankCardNo">银行卡号</param>
        /// <returns>查询数据库失败返回null，信息存在返回true，否则返回false</returns>
        public bool? ExistBindingBankCard(string patientCardId, string bankCardNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT COUNT(*) FROM [PatientCardBinding] WHERE [PatientCardId]= @patientCardId 
                 AND [BankCardNo] = @bankCardNo ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patientCardId",patientCardId),
                                                new SqlParameter("@bankCardNo",bankCardNo)                                                                                               
                                            };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (Convert.ToInt32(obj) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y017；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y018 1.19获取就诊卡银行卡绑定状态
        /// <summary>
        /// Y018  获取就诊卡银行卡绑定状态
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="bankCardNo">银行卡号</param>
        /// <returns>信息查询失败返回null，未绑定返回0，绑定返回1</returns>
        public int? GetBindingState(string patientCardId, string bankCardNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [BindingState] FROM [PatientCardBinding] 
                 WHERE [PatientCardId] = @patientCardId 
                  AND [BankCardNo] = @bankCardNo ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patientCardId",patientCardId),
                                                new SqlParameter("@bankCardNo",bankCardNo)                                                                                               
                                            };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y018；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y019 1.20根据就诊卡号和状态获取绑定记录
        /// <summary>
        /// Y019  根据就诊卡号和状态获取绑定记录
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="bindingState">绑定状态(1.已绑定，0.未绑定)</param>
        /// <returns>信息查询成功返回datatable对象，否则返回null</returns>
        public DataTable GetBindingRecordByPatientIdBindingState(string patientCardId, string bindingState)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [Id] 
                ,[PatientCardId] 
                ,[BankCardNo] 
                ,[BankId] 
                ,[InputDate] 
                ,[BindingState] 
                 FROM [PatientCardBinding] 
                 WHERE [PatientCardId] = @patientCardId 
                 AND [BindingState] = @bindingState ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patientCardId",patientCardId),
                                                new SqlParameter("@bindingState",bindingState)                                                                                               
                                            };
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlString, parameterValues).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y019；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y020 1.21获取就诊卡绑定银行卡记录
        /// <summary>
        ///Y020  获取就诊卡绑定银行卡记录
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <returns>信息查询成功返回datatable对象，否则返回null</returns>
        public DataTable GetBindingRecordByPatientId(string patientCardId)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [Id] 
                ,[PatientCardId] 
                ,[BankCardNo] 
                ,[BankId] 
                ,[InputDate] 
                ,[BindingState] 
                 FROM [PatientCardBinding] 
                 WHERE [PatientCardId] = @patientCardId ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patientCardId",patientCardId)                                                                                                                                              
                                            };
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlString, parameterValues).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y020；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y021 1.22根据记录Id解绑定就诊卡
        /// <summary>
        /// Y021  根据记录Id解绑定就诊卡
        /// </summary>
        /// <param name="id">绑定记录Id</param>
        /// <returns>操作执行成功返回影响行数，否则返回null</returns>
        public int? UnBindingBankCardById(string id)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"UPDATE [PatientCardBinding] 
                SET [BindingState] = 0 
                 WHERE [Id] = @id ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@id",id)                                                                                                                                              
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y021；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y022 1.23根据就诊卡号和银行卡号绑定就诊卡
        /// <summary>
        /// Y022  根据记录Id绑定就诊卡
        /// </summary>
        /// <param name="id">绑定记录Id</param>
        /// <returns>操作执行成功返回影响行数，否则返回null</returns>
        public int? BindingBankCardById(string pid, string bid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"UPDATE [PatientCardBinding] 
                SET [BindingState] = 1 
                 WHERE [PatientCardId] = @pid and [BankCardNo] = @bid ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@pid",pid), 
                                                new SqlParameter("@bid",bid)                                                                                             
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y022；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y023 1.24插入一条就诊卡医保卡绑定信息
        /// <summary>
        /// Y023  插入一条就诊卡医保卡绑定信息
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="micNo">医保卡号</param>
        /// <returns>操作执行成功返回影响行数，否则返回null</returns>
        public int? InsertMICPatientCardBinding(string patientCardId, string micNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"INSERT INTO [MedicalInsuranceCardBinding] 
           ([PatientCardId],[MedicalInsuranceCardNo],[InputDate],[BindingState]) 
           VALUES( @PatientCardId, @MedicalInsuranceCardNo, @BankId, @InputDate, @BindingState )";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@PatientCardId",patientCardId), 
                                                new SqlParameter("@MedicalInsuranceCardNo",micNo), 
                                                new SqlParameter("@BankId",micNo),
                                                new SqlParameter("@InputDate",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") ), 
                                                new SqlParameter("@BindingState",1)                                                                                             
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y023；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y024 1.25根据就诊号获取医保卡号
        /// <summary>
        /// Y024  根据就诊号获取医保卡号
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <returns>操作成功返回医保卡号，否则返回null</returns>
        public string GetMedicalInsuranceCardNoByPid(string patientCardId)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [MedicalInsuranceCardNo] 
                 FROM [MedicalInsuranceCardBinding] 
                 WHERE ltrim(rtrim([PatientCardId])) = @patientCardId 
                 AND [BindingState] = @BindingState";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@patientCardId",patientCardId), 
                                                new SqlParameter("@BindingState",1)                                                                                             
                                            };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y024；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y025 1.26根据医保卡号获取就诊号
        /// <summary>
        /// Y025  根据医保卡号获取就诊号
        /// </summary>
        /// <param name="micNo">医保卡号</param>
        /// <returns>操作成功返回就诊卡号，否则返回null</returns>
        public string GetPatientCardIdByMiNo(string micNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [PatientCardId] 
                 FROM [MedicalInsuranceCardBinding] 
                 WHERE ltrim(rtrim([MedicalInsuranceCardNo])) = @micNo 
                 AND [BindingState] = @BindingState";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@micNo",micNo), 
                                                new SqlParameter("@BindingState",1)                                                                                             
                                            };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y025；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y026 1.27根据银行卡号获取就诊卡号
        /// <summary>
        /// Y026  根据银行卡号获取就诊卡号
        /// </summary>
        /// <param name="bankCardNo">银行卡号</param>
        /// <returns>操作成功返回就诊卡号，否则返回null</returns>
        public string GetPatientCardIdByBankCardNo(string bankCardNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [PatientCardId] 
                 FROM [PatientCardBinding] 
                 WHERE ltrim(rtrim([BankCardNo])) = @bankCardNo 
                 AND [BindingState] = @BindingState ";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@bankCardNo",bankCardNo), 
                                                new SqlParameter("@BindingState",1)                                                                                             
                                            };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y026；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y027 1.28根据终端号获取终端子设备配置串
        /// <summary>
        /// Y027  根据终端号获取终端子设备配置串
        /// </summary>
        /// <param name="terminalNo">终端号</param>
        /// <returns>操作成功返回终端子设备配置串，否则返回null</returns>
        public string GetTerninalSubDeviceStartConfigByDevNo(string terminalNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "GetTerminalSubDeviceStartConfig";
            SqlParameter[] parameterValues = {
                                      new SqlParameter("@DeviNo", terminalNo)
                                  };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y027；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y028 1.29根据Ip获取终端号
        /// <summary>
        /// Y028  根据Ip获取终端号
        /// </summary>
        /// <param name="terminalIp">终端Ip</param>
        /// <returns>操作成功返回终端号，否则返回null</returns>
        public string GetTerminalNoByIp(string terminalIp)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [TerminalNo] 
                 FROM [Terminals] 
                 WHERE ltrim(rtrim([TerminalIp])) = @terminalIp ";
            SqlParameter[] parameterValues = {
                                      new SqlParameter("@terminalIp", terminalIp)
                                  };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y028；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y029 1.30根据终端号获取当前发票号
        /// <summary>
        /// Y029  根据终端号获取当前发票号
        /// </summary>
        /// <param name="terminalNo">终端号</param>
        /// <returns>操作成功返回终端当前发票号，否则返回null</returns>
        public string GetCurrentInvoiceNo(string terminalNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"SELECT [CurrentNo] FROM [InvoiceBooks] WHERE [State] = @State AND ltrim(rtrim([TerminalNo])) = @terminalNo ";
            SqlParameter[] parameterValues = {
                                                 new SqlParameter("@State",1),
                                                 new SqlParameter("@terminalNo", terminalNo)
                                             };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y029；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y030 1.31发票号增1，已打发票记入明细
        /// <summary>
        /// Y030  发票号增1，已打发票记入明细
        /// </summary>
        /// <param name="terminalNo">终端号</param>
        /// <returns>操作成功返回影响行数，否则返回null</returns>
        public int? AddInvoiceNo(string terminalNo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"UPDATE [InvoiceBooks] SET [CurrentNo] = [CurrentNo] + 1 where [State] = @State AND TerminalNo = @terminalNo";
            SqlParameter[] parameterValues = {
                                                 new SqlParameter("@State",1),
                                                 new SqlParameter("@terminalNo", terminalNo)
                                             };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y030；交易操作：" + sqlString);
            }
        }
        #endregion

        #region  Y031 1.32更新记录设备当前状态
        /// <summary>
        /// Y031  更新记录设备当前状态
        /// </summary>
        /// <param name="terminalNo">终端号</param>
        /// <param name="deviceStateStr">监控获取的设备串</param>
        /// <param name="dateTime">记录时间</param>
        /// <returns>操作成功返回首行首列，否则返回null</returns>
        public int? UpdateDeviceState(string terminalNo, string deviceStateStr, string dateTime)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "UpdateDeviceState";
            SqlParameter[] parameterValues = {
                                      new SqlParameter("@TerminalNo", terminalNo),
                                      new SqlParameter("@DeviceStateStr", deviceStateStr),
                                      new SqlParameter("@Datetime", dateTime)
                                  };

            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y031；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y032  1.33记录钞箱增加放入一张纸币
        /// <summary>
        /// Y032  记录钞箱增加放入一张纸币
        /// </summary>
        /// <param name="terminalNo">终端号</param>
        /// <param name="mianZhi">面值</param>
        /// <param name="zhangShu">张数</param>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="idCardNo">身份证号或空字符串</param>
        /// <param name="startDate">写入日期</param>
        /// <param name="moneyCode">币种</param>
        /// <param name="maxZhangshu">最大满箱张数</param>
        /// <returns>操作成功返回首行首列，否则返回null</returns>
        public int? CashBoxDetailInsert(string terminalNo, string mianZhi, string zhangShu, string patientCardId, string idCardNo, string startDate, string moneyCode, string maxZhangshu, string description)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "CashBoxDetailInsert";
            SqlParameter[] parameterValues = {
                                      new SqlParameter("@TerminalNo", terminalNo),
                                      new SqlParameter("@MianZhi", mianZhi),
                                      new SqlParameter("@ZhangShu", zhangShu),
                                      new SqlParameter("@PatientCardId", patientCardId),
                                      new SqlParameter("@IdCardNo", idCardNo),
                                      new SqlParameter("@StartDate", startDate),
                                      new SqlParameter("@MoneyCode", moneyCode),
                                      new SqlParameter("@MaxZhangShu", maxZhangshu),
                                      new SqlParameter("@Description",description)
                                  };

            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y032；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y033  1.34插入发卡明细
        /// <summary>
        /// Y033  插入发卡明细
        /// </summary>
        /// <param name="terminalNo">终端号</param>
        /// <param name="addCardAmount">加卡或发卡量 ，加卡为正值，发卡为负值</param>
        /// <param name="patientCardId">就诊卡号，加卡时为“”</param>
        /// <param name="idCardNo">身份证号，加卡时为“”</param>
        /// <param name="inputDate">写入日期</param>
        /// <param name="userId">加卡时为管理用户Id，发卡时为“”</param>
        /// <param name="description">描述</param>
        /// <param name="maxCardStockZhangShu">最大容量张数</param>
        /// <returns>操作成功返回首行首列，否则返回null</returns>
        public int? CardStockDetailInsert(string terminalNo, string addCardAmount, string patientCardId, string idCardNo, string inputDate, string userId, string description, string maxCardStockZhangShu)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "CardStockDetailInsert";
            SqlParameter[] parameterValues = {
                                      new SqlParameter("@TerminalNo", terminalNo),
                                      new SqlParameter("@AddCardAmount", addCardAmount),
                                      new SqlParameter("@PatientCardId", patientCardId),
                                      new SqlParameter("@IdCardNo", idCardNo),
                                      new SqlParameter("@InputDate", inputDate),
                                      new SqlParameter("@UserId", userId),
                                      new SqlParameter("@Description", description),
                                      new SqlParameter("@MaxCardStockZhangShu", maxCardStockZhangShu)
                                  };
            if (string.IsNullOrWhiteSpace(userId))
                parameterValues[5].Value = DBNull.Value;

            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y033；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y034 1.35获取发卡机状态
        /// <summary>
        /// Y034  获取发卡机状态（判断发卡机卡箱是否已满）该存储过程并不是获取卡机状态
        /// </summary>
        /// <param name="terminalNo">终端号</param>
        /// <param name="maxZhangShu">最大容量张数</param>
        /// <returns>操作成功返回值：0.正常，1卡箱满，2.卡箱空，3.异常；失败返回null</returns>
        public string CardStockGetState(string terminalNo, string maxZhangShu)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "CardStockGetState";
            SqlParameter[] parameterValues = {
                                      new SqlParameter("@TerminalNo", terminalNo),
                                      new SqlParameter("@CardStockAlertDownLimit", maxZhangShu)
                                  };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y034；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y035 1.36获取当前钞箱状态
        /// <summary>
        /// Y035  获取当前钞箱状态
        /// </summary>
        /// <param name="terminalNo">终端号</param>
        /// <param name="maxZhangShu">最大容量张数</param>
        /// <returns>操作成功返回值：0.正常，1卡箱满，2.卡箱空，3.异常；失败返回null</returns>
        public string CashBoxGetState0(string terminalNo, string maxZhangShu)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "CashBoxGetState0";
            SqlParameter[] parameterValues = {
                                      new SqlParameter("@TerminalNo", terminalNo),
                                      new SqlParameter("@MaxZhangShu", maxZhangShu)
                                  };
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, sqlString, parameterValues);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y035；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y036插入交易报文明细
        /// <summary>
        /// 插入交易报文明细
        /// </summary>
        /// <param name="TradeDetailID"></param>
        /// <param name="TradeTime"></param>
        /// <param name="TradeCode"></param>
        /// <param name="TradeName"></param>
        /// <param name="TradeMsg"></param>
        /// <param name="MsgType"></param>
        /// <param name="LogType"></param>
        /// <returns></returns>
        public string InsertLogTrade(string TradeDetailID, string TradeTime, string TradeCode, string TradeName, string TradeMsg, string MsgType, string LogType)
        {
            string strConn = "Data Source=127.0.0.1;Initial Catalog=New_BankHospital;uid=sa;pwd=inspur123!@#";// ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "insert into LogTrade (TradeDetailID , TradeTime , TradeCode , TradeName , TradeMsg , MsgType , LogType) Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
            sqlString = string.Format(sqlString, TradeDetailID, TradeTime, TradeCode, TradeName, TradeMsg, MsgType, LogType);
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj != DBNull.Value && obj != null)
                    return obj.ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：H031；交易操作：" + sqlstring);
            }
        }
        #endregion

        #region Y037 插入交易明细
        /// <summary>
        /// 插入交易明细
        /// </summary>
        /// <param name="InterfaceTypeID"></param>
        /// <param name="InterfaceNameID"></param>
        /// <param name="TradeTime"></param>
        /// <param name="TraceNo"></param>
        /// <param name="TerminalNo"></param>
        /// <param name="TradeState"></param>
        /// <param name="ActivityID"></param>
        /// <param name="CurrentMoney"></param>
        /// <param name="TradeMoney"></param>
        /// <param name="TradeID"></param>
        /// <returns></returns>
        public string InsertTradeDetail(string InterfaceTypeID, string InterfaceNameID, string TradeTime, string TraceNo, string TerminalNo, string TradeState, string pageName, string CurrentMoney, string TradeMoney, string TradeID)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "insert into TradeDetails (InterfaceTypeID , InterfaceNameID  , TradeTime , TraceNo , TerminalNo , TradeState , pageName , CurrentMoney , TradeMoney , TradeID) Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')   select @@identity";
            sqlString = string.Format(sqlString, InterfaceTypeID, InterfaceNameID, TradeTime, TraceNo, TerminalNo, TradeState, pageName, CurrentMoney, TradeMoney, TradeID);
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj != DBNull.Value && obj != null)
                    return obj.ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：H031；交易操作：" + sqlstring);
            }
        }
        #endregion

        #region Y038获取医院简介信息    GetHospitalIntroduction
        /// <summary>
        /// 获取医院简介信息
        /// </summary>
        /// <returns>返回值：DataTable对象.成功、null.失败</returns>
        public DataTable GetHospitalIntroduction(string hospitalName)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlstring = String.Format("select HospitalName,Introduction from HospitalIntroduction where HospitalName='{0}'", hospitalName);
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlstring).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：H031；交易操作：" + sqlstring);
            }
        }
        #endregion


        #region Y039 1.40查询就诊卡交易信息
        /// <summary>
        /// Y039  查询就诊卡交易信息
        /// </summary>
        /// <param name="patientCardId">病人就诊卡号</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>查询成功返回datatable（就诊卡号，姓名，充值类型(1.现金，2.银行卡，3.就诊卡)，交易类型(1.终端充值，2.终端退款，3.终端扣款)，金额，交易日期，备注），失败返回null</returns>
        public DataTable GetPatientCardTransactionDetail(string patientCardId, string startDate, string endDate)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = string.Format(@"select a.PatientCardId '就诊卡号',a.PatientName '姓名',f.FillType '充值类型',t.TransactionType '交易类型',d.JinE '金额',d.FillDate '交易日期',Description '备注'  
                                                from PatientCardDetail d 
                                                join PatientCardFillMoneyType  f on d.FillTypeId=f.Id 
                                                join PatientCardFillTransactionType t on d.TransactionTypeId=t.Id 
                                                join PatientCardBlance a on d.PatientCardId=a.PatientCardId 
                                                WHERE d.[PatientCardId] = '{0}' AND d.[FillDate] BETWEEN '{1}' AND '{2}' 
                                                order by  '交易日期' desc", patientCardId, startDate, endDate);
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlString).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y039；交易操作：" + sqlString);
            }
        }
        #endregion
        #region 1.40--44用户权限菜单

        public DataTable GetMenuItemByLoginName(string loginName)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "SELECT * FROM View_UserMenuItem where LoginName='" + loginName + "'";
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlString).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y042；交易操作：" + sqlString);
            }
        }

        public DataTable GetMenuItemByLoginNameParentMenuId(string loginName, string parentMenuId)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "SELECT * FROM View_UserMenuItem where LoginName='" + loginName + "' and ParentMenuId=" + parentMenuId;
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlString).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y041；交易操作：" + sqlString);
            }
        }

        public string GetTellerNoByIp(string terminalIp)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "select TellerNo from Terminals where TerminalIp='" + terminalIp + "'";
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y040；交易操作：" + sqlString);
            }
        }

        public string GetMainPageByLoginName(string loginName)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "select FileName from View_LoginNameMainPage where LoginName='" + loginName + "'";
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y043；交易操作：" + sqlString);
            }
        }

        public string GetEmpNoByIp(string terminalIp)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "select EmpNo from Terminals where TerminalIp='" + terminalIp + "'";
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj == DBNull.Value || obj == null)
                    return null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y040；交易操作：" + sqlString);
            }
        }
        #endregion
        #region Y045 1.46写一条待冲正信息到Hss
        /// <summary>
        /// Y044  写一条待冲正信息到Hss
        /// </summary>
        /// <param name="TransCode">交易码</param>
        /// <param name="Message">交易报文</param>
        /// <param name="ResultFlag">冲正标志</param>
        /// <param name="ExecuteDate">执行时间</param>
        /// <param name="Count">冲正次数</param>
        /// <returns>信息添加成功返回影响行数，否则返回null</returns>
        public int? InsertChongZheng(string TransCode, string Message, int ResultFlag, DateTime ExecuteDate, int Count)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = @"INSERT INTO [CorrectTradeQueue] 
            ([TransCode] ,[Message] ,[ResultFlag] ,[ExecuteDate] ,[ExecuteCount],[FillDate]) 
            VALUES ( @TransCode, @Message, @ResultFlag, @ExecuteDate,@Count)";
            SqlParameter[] parameterValues ={
                                                new SqlParameter("@TransCode",TransCode),
                                                new SqlParameter("@Message",Message),
                                                new SqlParameter("@ResultFlag",ResultFlag),
                                                new SqlParameter("@ExecuteDate",ExecuteDate),
                                                new SqlParameter("@Count",Count),
                                                new SqlParameter("@FillDate",DateTime.Now)
                                            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameterValues);
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：Y038；交易操作：" + sqlString);
            }
        }
        #endregion

        #region Y047 1.47根据银医流水号、流水号和交易时间更新医院返回的流水号
        /// <summary>
        /// 根据银医流水号、流水号和交易时间更新医院返回的流水号
        /// </summary>
        /// <param name="patientCardId">就诊卡号</param>
        /// <param name="terminalIp">终端ip</param>
        /// <param name="trace">银医流水号</param>
        /// <param name="hisTrace">his流水号</param>
        /// <returns></returns>
        public int? UpdatePatientCardDetail_hisTrace(string patientCardId, string terminalIp, string trace, string hisTrace)
        {
            int ret = -1;

            try
            {
                string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
                string sqlString = @"UPDATE PatientCardDetail SET hisTrace=@hisTrace WHERE PatientCardId = @PatientCardId AND terminalIp = @terminalIp AND trace = @trace";
                SqlParameter[] parameters = {                                                 
                                        new SqlParameter("@hisTrace",hisTrace),
                                        new SqlParameter("@PatientCardId",patientCardId),
                                        new SqlParameter("@terminalIp",terminalIp),
                                        new SqlParameter("@trace",trace),
                                        };

                ret = SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlString, parameters);
            }
            catch (Exception ex)
            {

            }

            return ret;
        }
        #endregion


        /// <summary>
        /// 更新交易信息
        /// </summary>
        /// <param name="TradeState"></param>
        /// <param name="CurrentMoney"></param>
        /// <param name="TradeTime"></param>
        /// <param name="TradeMoney"></param>
        /// <param name="TradeID"></param>
        /// <returns></returns>
        public string UpdateTradeInfo(string TradeState, string TradeDetailID)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "UPDATE  TradeInfo SET  TradeState  = '{0}' where  TradeID=(select TradeID from TradeDetails  where TradeDetailID='{1}')";
            sqlString = string.Format(sqlString, TradeState, TradeDetailID);
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj != DBNull.Value && obj != null)
                    return obj.ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：H031；交易操作：" + sqlstring);
            }
        }
        /// <summary>
        /// 更新交易明细信息
        /// </summary>
        /// <param name="TradeState"></param>
        /// <param name="CurrentMoney"></param>
        /// <param name="TradeTime"></param>
        /// <param name="TradeMoney"></param>
        /// <param name="TradeID"></param>
        /// <returns></returns>
        public string UpdateTradeDetails(string TradeState, string TradeDetailID)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "UPDATE  TradeDetails SET  TradeState  = '{0}'  WHERE  TradeDetailID='{1}'";
            sqlString = string.Format(sqlString, TradeState, TradeDetailID);
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj != DBNull.Value && obj != null)
                    return obj.ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：H031；交易操作：" + sqlstring);
            }
        }

        /// <summary>
        #region 更新交易信息(涉及交易金额的)
        /// </summary>
        /// <param name="TradeState"></param>
        /// <param name="TradeDetailID"></param>
        /// <param name="TradeMoney"></param>
        /// <returns></returns>
        public string UpdateTradeInfoWithMoney(string TradeState, string TradeDetailID, string TradeMoney)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "UPDATE  TradeInfo SET  TradeState  = '{0}', TradeMoney='{2}' where  TradeID=(select TradeID from TradeDetails  where TradeDetailID='{1}')";
            sqlString = string.Format(sqlString, TradeState, TradeDetailID, TradeMoney);
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj != DBNull.Value && obj != null)
                    return obj.ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：H031；交易操作：" + sqlstring);
            }
        }
        #endregion

        /// <summary>
        #region  更新交易明细信息（涉及交易金额的）
        /// </summary>
        /// <param name="TradeState"></param>
        /// <param name="TradeDetailID"></param>
        /// <param name="TradeMoney"></param>
        /// <returns></returns>
        public string UpdateTradeDetailsWithMoney(string TradeState, string TradeDetailID, string TradeMoney)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "UPDATE  TradeDetails SET  TradeState  = '{0}', TradeMoney='{2}'  WHERE  TradeDetailID='{1}'";
            sqlString = string.Format(sqlString, TradeState, TradeDetailID, TradeMoney);
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj != DBNull.Value && obj != null)
                    return obj.ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：H031；交易操作：" + sqlstring);
            }
        }
        #endregion

        /// <summary>
        #region 判断是不是当前流程中第一次获取就诊人信息
        /// </summary>
        /// cj 2014年9月30日15:35:37
        /// <param name="TradeDetailID"></param>
        /// <returns></returns>
        public bool IsFirstGetPatientInfo(string TradeDetailID)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;

            string sqlString = @"select COUNT(*)as Num from TradeDetails 
                       where TradeID=(select TradeID from TradeDetails where TradeDetailID='{0}')
                       and TradeDetailID<>'{0}'";
            sqlString = string.Format(sqlString, TradeDetailID);
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj == "0")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;//throw new Exception("交易码：H031；交易操作：" + sqlstring);
            }
        }
        #endregion

        #region 记录账面记录
        public string writeAccount(String TradeDetailID, String TradeAmt, String TradeTime, String TradeTraceNo, String TradeState, String TradeResult)
        {
            //string strConn = ConfigurationManager.AppSettings["BankHospitalConnectionString"];
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            String sqlString = "insert into account(TradeDetailID,TradeAmt,TradeTime,TradeTraceNo,TradeState,TradeResult)values('" + TradeDetailID + "','" + TradeAmt + "','" + TradeTime + "','" + TradeTraceNo + "','" + TradeState + "','" + TradeResult + "')";

            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
                if (obj != DBNull.Value && obj != null)
                    return obj.ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

#if DEBUG
        /// <summary>
        /// Y999 测试方法
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int? Sum(int i)
        {
            string strConn = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;
            string sqlString = "select FileName from View_LoginNameMainPage where LoginName='0' or 1=1 --";

            object obj = SqlHelper.ExecuteScalar(strConn, CommandType.Text, sqlString);
            int sum = 0;
            for (int j = 0; j < 3000; j++)
            {
                sum += j;
                for (int k = 0; k < int.MaxValue / 1000; k++)
                {

                }
            }
            return sum + i;
        }
#endif
    }
}
