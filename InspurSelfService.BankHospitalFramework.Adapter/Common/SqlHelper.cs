/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：SqlHelper.cs
 * 描    述：SQL 操作类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Collections;
using System.Configuration;

/// <summary>
///SqlHelper 的摘要说明
/// </summary>
public sealed class SqlHelper
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SqlHelper()
    { }
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public static readonly string connectionString = ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"] == null ? "" : ConfigurationManager.ConnectionStrings["BankHospitalConnectionString"].ConnectionString;

    private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    #region 私有构造函数和方法

    /// <summary>
    /// 将SqlParameters参数数组（参数值）分配给SqlCommand命令
    /// 这个方法将给任何一个参数分配DBNull.Value
    /// 该操作将阻止默认值的使用（大爷的，没看懂）
    /// </summary>
    /// <param name="command">要分配SqlParameters参数的SqlCommand命令</param>
    /// <param name="commandParameters">SqlParameters参数数组</param>
    private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
    {
        if (command == null) throw new ArgumentNullException("command");
        if (commandParameters != null)
        {
            foreach (SqlParameter p in commandParameters)
            {
                if (p != null)
                {
                    if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && (p.Value == null))
                        p.Value = DBNull.Value;
                    command.Parameters.Add(p);
                }
            }
        }
    }

    /// <summary>
    /// 将DataRow类型的列值分配到SqlParameter参数数组
    /// </summary>
    /// <param name="commandParameters">要分配的SqlParameter参数数组</param>
    /// <param name="dataRow">将要分配给存储过程参数的DataRow</param>
    private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
    {
        if ((commandParameters == null) || (dataRow == null))
            return;

        int i = 0;
        foreach (SqlParameter commandParameter in commandParameters)
        {
            if (commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1)
                throw new Exception(string.Format("请提供参数{0},一个有效的名称{1}.", i, commandParameter.ParameterName));
            // 如果存在和参数名称相同的列,则将列值赋给当前名称的参数.
            if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
            i++;
        }
    }

    /// <summary>
    /// 将一个对象数组分配给SqlParameter参数数组.（重载方法）
    /// </summary>
    /// <param name="commandParameters">要分配的SqlParameter参数数组</param>
    /// <param name="parameterValues">将要分配给存储过程参数的对象数组</param>
    private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
    {
        if ((commandParameters == null) || (parameterValues == null))
            return;

        // 确保对象数组个数与参数个数匹配,如果不匹配,抛出一个异常
        if (commandParameters.Length != parameterValues.Length)
            throw new ArgumentException("参数值个数与参数不匹配.");

        for (int i = 0, j = commandParameters.Length; i < j; i++)
        {
            if (parameterValues[i] is IDbDataParameter)
            {
                IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                if (paramInstance.Value == null)
                    commandParameters[i].Value = DBNull.Value;
                else
                    commandParameters[i].Value = paramInstance.Value;
            }
            else if (parameterValues[i] == null)
                commandParameters[i].Value = DBNull.Value;
            else
                commandParameters[i].Value = parameterValues[i];
        }
    }

    /// <summary>
    /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
    /// </summary>
    /// <param name="command">要处理的SqlCommand</param>
    /// <param name="connection">有效的数据库连接</param>
    /// <param name="transaction">一个有效的事务或者是null值</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
    /// <param name="commandText">存储过程名或都T-SQL命令文本</param>
    /// <param name="commandParameters">和命令相关联的SqlParameter参数数组,如果没有参数为'null'</param>
    /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param>
    private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
    {
        if (command == null) throw new ArgumentNullException("command");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

        if (connection.State != ConnectionState.Open)
        {
            mustCloseConnection = true;
            connection.Open();
        }
        else
            mustCloseConnection = false;

        command.CommandTimeout = 1000 * 60 * 60; // 60分钟
        command.Connection = connection;
        command.CommandText = commandText;

        // 分配事务
        if (transaction != null)
        {
            if (transaction.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            command.Transaction = transaction;
        }

        command.CommandType = commandType;

        if (commandParameters != null)
            AttachParameters(command, commandParameters);
        return;
    }

    #endregion 私有构造函数和方法结束


    #region ExecuteNonQuery命令

    /// <summary>
    /// 执行指定连接字符串,类型的SqlCommand.
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
    /// <param name="commandText">存储过程名称或SQL语句</param>
    /// <returns>返回命令影响的行数</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
    {
        return ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定连接字符串,类型的SqlCommand.如果没有提供参数,不返回结果
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
    /// <param name="commandText">存储过程名称或SQL语句</param>
    /// <param name="commandParameters">SqlParameter参数数组</param>
    /// <returns>返回命令影响的行数</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
        }
    }

    /// <summary>
    /// 执行指定连接字符串的存储过程,将对象数组的值赋给存储过程参数.
    /// 此方法需要在参数缓存方法中探索参数并生成参数.
    /// </summary>
    /// <remarks>
    /// 这个方法没有提供访问输出参数和返回值.
    /// 示例:  
    /// int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="parameterValues">分配到存储过程输入参数的对象数组</param>
    /// <returns>返回受影响的行数</returns>
    public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令. 
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <returns>返回影响的行数</returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
    {
        return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
    /// <param name="commandText">T存储过程名称或T-SQL语句</param>
    /// <param name="commandParameters">SqlParamter参数数组</param>
    /// <returns>返回影响的行数</returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");

        using (SqlCommand cmd = new SqlCommand())
        {
            bool mustCloseConnection = false;
            int retval = 0;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
            try
            {
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", connection.ConnectionString, commandText), ex);
                throw ex;
            }
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,将对象数组的值赋给存储过程参数.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值
    /// 示例:  
    /// int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回影响的行数</returns>
    public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行带事务的SqlCommand. 
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="transaction">一个有效的事物</param>
    /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <returns>返回影响的行数</returns>
    public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行带事务的SqlCommand(指定参数).
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">一个有效的数据库事物</param>
    /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="commandParameters">SqlParamter参数数组</param>
    /// <returns>返回影响的行数</returns>
    public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        using (SqlCommand cmd = new SqlCommand())
        {
            bool mustCloseConnection = false;
            int retval = 0;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            try
            {
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", transaction.Connection.ConnectionString, commandText), ex);
                throw ex;
            }
            cmd.Parameters.Clear();
            return retval;
        }
    }

    /// <summary>
    /// 执行带事务的SqlCommand(指定参数值).
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值
    /// 示例:  
    /// int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回受影响的行数</returns>
    public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
    }

    #endregion ExecuteNonQuery命令


    #region ExecuteDataset方法

    /// <summary>
    /// 执行指定数据库连接字符串的命令,返回DataSet. 
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型(存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <returns>返回一个包含结果集的DataSet</returns>
    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
    {
        return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定数据库连接字符串的命令,返回DataSet.
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型(存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="commandParameters">SqlParamters参数数组</param>
    /// <returns>返回一个包含结果集的DataSet</returns>
    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return ExecuteDataset(connection, commandType, commandText, commandParameters);
        }
    }

    /// <summary>
    /// 执行指定数据库连接字符串的命令,直接提供参数值,返回DataSet.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值.
    /// 示例:  
    /// DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="spName">存储过程名</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回一个包含结果集的DataSet</returns>
    public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,返回DataSet. 
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名或T-SQL语句</param>
    /// <returns>返回一个包含结果集的DataSet</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
    {
        return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataSet.
    /// </summary>
    /// <remarks>
    /// 示例:  
    /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名或T-SQL语句</param>
    /// <param name="commandParameters">SqlParamter参数数组</param>
    /// <returns>返回一个包含结果集的DataSet</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");

        using (SqlCommand cmd = new SqlCommand())
        {
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", connection.ConnectionString, commandText), ex);
                    ds.Dispose();
                    ds = null;
                    throw ex;
                }
                cmd.Parameters.Clear();
                if (mustCloseConnection)
                    connection.Close();
                return ds;
            }
        }
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,指定参数值,返回DataSet.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输入参数和返回值.
    /// 示例.: 
    ///  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回一个包含结果集的DataSet</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定事务的命令,返回DataSet.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名或T-SQL语句</param>
    /// <returns>返回一个包含结果集的DataSet</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定事务的命令,指定参数,返回DataSet.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名或T-SQL语句</param>
    /// <param name="commandParameters">SqlParamter参数数组</param>
    /// <returns>返回一个包含结果集的DataSet</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        using (SqlCommand cmd = new SqlCommand())
        {
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", transaction.Connection.ConnectionString, commandText), ex);
                    ds.Dispose();
                    ds = null;
                    throw ex;
                }
                cmd.Parameters.Clear();
                return ds;
            }
        }
    }

    /// <summary>
    /// 执行指定事务的命令,指定参数值,返回DataSet.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输入参数和返回值.
    /// 示例.: 
    ///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">事务</param>
    /// <param name="spName">存储过程名</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回一个包含结果集的DataSet</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
    }

    #endregion ExecuteDataset方法


    #region ExecuteReader 数据阅读器
    /// <summary>
    /// 枚举,标识数据库连接是由SqlHelper提供还是由调用者提供
    /// </summary>
    private enum SqlConnectionOwnership
    {
        /// <summary>由SqlHelper提供连接</summary>
        Internal,
        /// <summary>由调用者提供连接</summary>
        External
    }

    /// <summary>
    /// 执行指定数据库连接对象的数据阅读器.
    /// </summary>
    /// <remarks>
    /// 如果是SqlHelper打开连接,当连接关闭DataReader也将关闭.
    /// 如果是调用都打开连接,DataReader由调用都管理.
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="transaction">一个有效的事务,或者为 'null'</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名或T-SQL语句</param>
    /// <param name="commandParameters">SqlParameters参数数组,如果没有参数则为'null'</param>
    /// <param name="connectionOwnership">标识数据库连接对象是由调用者提供还是由SqlHelper提供</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        bool mustCloseConnection = false;
        // 创建命令
        SqlCommand cmd = new SqlCommand();
        try
        {
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            SqlDataReader dataReader;
            if (connectionOwnership == SqlConnectionOwnership.External)
                dataReader = cmd.ExecuteReader();
            else
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            bool canClear = true;
            foreach (SqlParameter commandParameter in cmd.Parameters)
            {
                if (commandParameter.Direction != ParameterDirection.Input)
                    canClear = false;
            }

            if (canClear)
                cmd.Parameters.Clear();
            return dataReader;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", connection.ConnectionString, commandText), ex);
            if (mustCloseConnection)
                connection.Close();
            throw ex;
        }
    }

    /// <summary>
    /// 执行指定数据库连接字符串的数据阅读器.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名或T-SQL语句</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
    {
        return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定数据库连接字符串的数据阅读器,指定参数.
    /// </summary>
    /// <remarks>
    /// 示例: 
    ///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名或T-SQL语句</param>
    /// <param name="commandParameters">SqlParamter参数数组(new SqlParameter("@prodid", 24))</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            return ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", connectionString, commandText), ex);
            if (connection != null)
                connection.Close();
            throw ex;
        }
    }

    /// <summary>
    /// 执行指定数据库连接字符串的数据阅读器,指定参数值.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// SqlDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="spName">存储过程名</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定数据库连接对象的数据阅读器.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名或T-SQL语句</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
    {
        return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// [调用者方式]执行指定数据库连接对象的数据阅读器,指定参数.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名或T-SQL语句</param>
    /// <param name="commandParameters">SqlParamter参数数组</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
    }

    /// <summary>
    /// [调用者方式]执行指定数据库连接对象的数据阅读器,指定参数值.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">T存储过程名</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteReader(connection, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数值.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数.
    /// </summary>
    /// <remarks>
    /// 示例: 
    ///  SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
    }

    /// <summary>
    /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数值.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
    }

    #endregion ExecuteReader 数据阅读器


    #region ExecuteScalar 返回结果集中的第一行第一列

    /// <summary>
    /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
    {
        return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定数据库连接字符串的命令,指定参数,返回结果集中的第一行第一列.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return ExecuteScalar(connection, commandType, commandText, commandParameters);
        }
    }

    /// <summary>
    /// 执行指定数据库连接字符串的命令,指定参数值,返回结果集中的第一行第一列.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,返回结果集中的第一行第一列.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
    {
        return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,指定参数,返回结果集中的第一行第一列.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");

        using (SqlCommand cmd = new SqlCommand())
        {
            bool mustCloseConnection = false;
            object retval = null;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
            try
            {
                retval = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", connection.ConnectionString, commandText), ex);
                throw ex;
            }
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,指定参数值,返回结果集中的第一行第一列.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定数据库事务的命令,返回结果集中的第一行第一列.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定数据库事务的命令,指定参数,返回结果集中的第一行第一列.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        using (SqlCommand cmd = new SqlCommand())
        {
            bool mustCloseConnection = false;
            object retval = null;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            try
            {
                retval = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", transaction.Connection.ConnectionString, commandText), ex);
                throw ex;
            }
            cmd.Parameters.Clear();
            return retval;
        }
    }

    /// <summary>
    /// 执行指定数据库事务的命令,指定参数值,返回结果集中的第一行第一列.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
    }

    #endregion ExecuteScalar


    #region ExecuteXmlReader XML阅读器

    /// <summary>
    /// 执行指定数据库连接对象的SqlCommand命令,并产生一个XmlReader对象做为结果集返回.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句 using "FOR XML AUTO"</param>
    /// <returns>返回XmlReader结果集对象.</returns>
    public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
    {
        return ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定数据库连接对象的SqlCommand命令,并产生一个XmlReader对象做为结果集返回,指定参数.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句 using "FOR XML AUTO"</param>
    /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>
    /// <returns>返回XmlReader结果集对象.</returns>
    public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        bool mustCloseConnection = false;

        SqlCommand cmd = new SqlCommand();
        XmlReader retval = null;

        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
        try
        {
            retval = cmd.ExecuteXmlReader();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", connection.ConnectionString, commandText), ex);
            if (mustCloseConnection)
                connection.Close();
            throw ex;
        }
        cmd.Parameters.Clear();
        return retval;
    }

    /// <summary>
    /// 执行指定数据库连接对象的SqlCommand命令,并产生一个XmlReader对象做为结果集返回,指定参数值.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名称 using "FOR XML AUTO"</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回XmlReader结果集对象.</returns>
    public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定数据库事务的SqlCommand命令,并产生一个XmlReader对象做为结果集返回.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句 using "FOR XML AUTO"</param>
    /// <returns>返回XmlReader结果集对象.</returns>
    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        return ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// 执行指定数据库事务的SqlCommand命令,并产生一个XmlReader对象做为结果集返回,指定参数.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句 using "FOR XML AUTO"</param>
    /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>
    /// <returns>返回XmlReader结果集对象.</returns>
    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        XmlReader retval = null;
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        try
        {
            retval = cmd.ExecuteXmlReader();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", transaction.Connection.ConnectionString, commandText), ex);
            if (mustCloseConnection)
                transaction.Connection.Close();
            throw ex;
        }

        cmd.Parameters.Clear();
        return retval;
    }

    /// <summary>
    /// 执行指定数据库事务的SqlCommand命令,并产生一个XmlReader对象做为结果集返回,指定参数值.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    /// <returns>返回一个包含结果集的DataSet.</returns>
    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
    }

    #endregion ExecuteXmlReader 阅读器结束


    #region FillDataset 填充数据集

    /// <summary>
    /// 执行指定数据库连接字符串的命令,映射数据表并填充数据集.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="dataSet">要填充结果集的DataSet实例</param>
    /// <param name="tableNames">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)</param>
    public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (dataSet == null) throw new ArgumentNullException("dataSet");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            FillDataset(connection, commandType, commandText, dataSet, tableNames);
        }
    }

    /// <summary>
    /// 执行指定数据库连接字符串的命令,映射数据表并填充数据集.指定命令参数.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="dataSet">分配给命令的SqlParamter参数数组</param>
    /// <param name="tableNames">要填充结果集的DataSet实例</param>
    /// <param name="commandParameters">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)
    /// </param>
    public static void FillDataset(string connectionString, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames,
        params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (dataSet == null) throw new ArgumentNullException("dataSet");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
        }
    }

    /// <summary>
    /// 执行指定数据库连接字符串的命令,映射数据表并填充数据集,指定存储过程参数值.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, 24);
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataSet">要填充结果集的DataSet实例</param>
    /// <param name="tableNames">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)
    /// </param>   
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    public static void FillDataset(string connectionString, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (dataSet == null) throw new ArgumentNullException("dataSet");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            FillDataset(connection, spName, dataSet, tableNames, parameterValues);
        }
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,映射数据表并填充数据集.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="dataSet">要填充结果集的DataSet实例</param>
    /// <param name="tableNames">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)
    /// </param>   
    public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
    {
        FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,映射数据表并填充数据集,指定参数.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="dataSet">要填充结果集的DataSet实例</param>
    /// <param name="tableNames">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)
    /// </param>
    /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>
    public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
    {
        FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
    }

    /// <summary>
    /// 执行指定数据库连接对象的命令,映射数据表并填充数据集,指定存储过程参数值.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataSet">要填充结果集的DataSet实例</param>
    /// <param name="tableNames">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)
    /// </param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    public static void FillDataset(SqlConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (dataSet == null) throw new ArgumentNullException("dataSet");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
        }
        else
            FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
    }

    /// <summary>
    /// 执行指定数据库事务的命令,映射数据表并填充数据集.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="dataSet">要填充结果集的DataSet实例</param>
    /// <param name="tableNames">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)
    /// </param>
    public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
    {
        FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
    }

    /// <summary>
    /// 执行指定数据库事务的命令,映射数据表并填充数据集,指定参数.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="dataSet">要填充结果集的DataSet实例</param>
    /// <param name="tableNames">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)
    /// </param>
    /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>
    public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
    {
        FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
    }

    /// <summary>
    /// 执行指定数据库事务的命令,映射数据表并填充数据集,指定存储过程参数值.
    /// </summary>
    /// <remarks>
    /// 此方法不提供访问存储过程输出参数和返回值参数.
    /// 示例: 
    /// FillDataset(trans, "GetOrders", ds, new string[]{"orders"}, 24, 36);
    /// </remarks>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataSet">要填充结果集的DataSet实例</param>
    /// <param name="tableNames">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)
    /// </param>
    /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
    public static void FillDataset(SqlTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (dataSet == null) throw new ArgumentNullException("dataSet");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
        }
        else
            FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
    }

    /// <summary>
    /// [私有方法][内部调用]执行指定数据库连接对象/事务的命令,映射数据表并填充数据集,DataSet/TableNames/SqlParameters.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="transaction">一个有效的连接事务</param>
    /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
    /// <param name="commandText">存储过程名称或T-SQL语句</param>
    /// <param name="dataSet">要填充结果集的DataSet实例</param>
    /// <param name="tableNames">表映射的数据表数组
    /// 用户定义的表名 (可有是实际的表名.)
    /// </param>
    /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>
    private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (dataSet == null) throw new ArgumentNullException("dataSet");

        using (SqlCommand command = new SqlCommand())
        {
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    for (int index = 0; index < tableNames.Length; index++)
                    {
                        if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString();
                    }
                }
                try
                {
                    dataAdapter.Fill(dataSet);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("链接[{0}]执行SQL\r\n{1}\r\n失败", connection.ConnectionString, commandText), ex);
                    throw ex;
                }
                command.Parameters.Clear();
            }
            if (mustCloseConnection)
                connection.Close();
        }
    }

    #endregion FillDataset 填充数据集


    #region UpdateDataset 更新数据集

    /// <summary>
    /// 执行数据集更新到数据库,指定inserted, updated, or deleted命令.
    /// </summary>
    /// <remarks>
    /// 示例: 
    /// UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
    /// </remarks>
    /// <param name="insertCommand">[追加记录]一个有效的T-SQL语句或存储过程</param>
    /// <param name="deleteCommand">[删除记录]一个有效的T-SQL语句或存储过程</param>
    /// <param name="updateCommand">[更新记录]一个有效的T-SQL语句或存储过程</param>
    /// <param name="dataSet">要更新到数据库的DataSet</param>
    /// <param name="tableName">要更新到数据库的DataTable</param>
    public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
    {
        if (insertCommand == null) throw new ArgumentNullException("insertCommand");
        if (deleteCommand == null) throw new ArgumentNullException("deleteCommand");
        if (updateCommand == null) throw new ArgumentNullException("updateCommand");
        if (tableName == null || tableName.Length == 0) throw new ArgumentNullException("tableName");

        using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
        {
            dataAdapter.UpdateCommand = updateCommand;
            dataAdapter.InsertCommand = insertCommand;
            dataAdapter.DeleteCommand = deleteCommand;
            try
            {
                dataAdapter.Update(dataSet, tableName);
                dataSet.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("链接[{0}]执行Command\r\nTable{1}\r\n失败", insertCommand.Connection.ConnectionString, tableName), ex);
                throw ex;
            }
        }
    }

    #endregion UpdateDataset 更新数据集


    #region CreateCommand 创建一条SqlCommand命令

    /// <summary>
    /// 创建SqlCommand命令,指定数据库连接对象,存储过程名和参数.
    /// </summary>
    /// <remarks>
    /// 示例: 
    ///  SqlCommand command = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName");
    /// </remarks>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="sourceColumns">源表的列名称数组</param>
    /// <returns>返回SqlCommand命令</returns>
    public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        SqlCommand cmd = new SqlCommand(spName, connection);
        cmd.CommandType = CommandType.StoredProcedure;

        if ((sourceColumns != null) && (sourceColumns.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            for (int index = 0; index < sourceColumns.Length; index++)
                commandParameters[index].SourceColumn = sourceColumns[index];
            AttachParameters(cmd, commandParameters);
        }
        return cmd;
    }

    #endregion CreateCommand 创建一条SqlCommand命令


    #region ExecuteNonQueryTypedParams 类型化参数(DataRow)

    /// <summary>
    /// 执行指定连接数据库连接字符串的存储过程,使用DataRow做为参数值,返回受影响的行数.
    /// </summary>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回影响的行数</returns>
    public static int ExecuteNonQueryTypedParams(String connectionString, String spName, DataRow dataRow)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回受影响的行数.
    /// </summary>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回影响的行数</returns>
    public static int ExecuteNonQueryTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
    }



    /// <summary>
    /// 执行指定连接数据库事物的存储过程,使用DataRow做为参数值,返回受影响的行数.
    /// </summary>
    /// <param name="transaction">一个有效的连接事务 object</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回影响的行数</returns>
    public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
    }

    #endregion ExecuteNonQueryTypedParams 类型化参数(DataRow)


    #region ExecuteDatasetTypedParams 类型化参数(DataRow)

    /// <summary>
    /// 执行指定连接数据库连接字符串的存储过程,使用DataRow做为参数值,返回DataSet.
    /// </summary>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回一个包含结果集的DataSet.</returns>
    public static DataSet ExecuteDatasetTypedParams(string connectionString, String spName, DataRow dataRow)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回DataSet.
    /// </summary>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回一个包含结果集的DataSet.</returns>
    public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定连接数据库事务的存储过程,使用DataRow做为参数值,返回DataSet.
    /// </summary>
    /// <param name="transaction">一个有效的连接事务 object</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回一个包含结果集的DataSet.</returns>
    public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
    }

    #endregion ExecuteDatasetTypedParams 类型化参数(DataRow)


    #region ExecuteReaderTypedParams 类型化参数(DataRow)

    /// <summary>
    /// 执行指定连接数据库连接字符串的存储过程,使用DataRow做为参数值,返回DataReader.
    /// </summary>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReaderTypedParams(String connectionString, String spName, DataRow dataRow)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回DataReader.
    /// </summary>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定连接数据库事物的存储过程,使用DataRow做为参数值,返回DataReader.
    /// </summary>
    /// <param name="transaction">一个有效的连接事务 object</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回包含结果集的SqlDataReader</returns>
    public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
    }

    #endregion ExecuteReaderTypedParams 类型化参数(DataRow)


    #region ExecuteScalarTypedParams 类型化参数(DataRow)

    /// <summary>
    /// 执行指定连接数据库连接字符串的存储过程,使用DataRow做为参数值,返回结果集中的第一行第一列.
    /// </summary>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalarTypedParams(String connectionString, String spName, DataRow dataRow)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回结果集中的第一行第一列.
    /// </summary>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalarTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定连接数据库事务的存储过程,使用DataRow做为参数值,返回结果集中的第一行第一列.
    /// </summary>
    /// <param name="transaction">一个有效的连接事务 object</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回结果集中的第一行第一列</returns>
    public static object ExecuteScalarTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
    }

    #endregion ExecuteScalarTypedParams 类型化参数(DataRow)


    #region ExecuteXmlReaderTypedParams 类型化参数(DataRow)

    /// <summary>
    /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回XmlReader类型的结果集.
    /// </summary>
    /// <param name="connection">一个有效的数据库连接对象</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回XmlReader结果集对象.</returns>
    public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
    }

    /// <summary>
    /// 执行指定连接数据库事务的存储过程,使用DataRow做为参数值,返回XmlReader类型的结果集.
    /// </summary>
    /// <param name="transaction">一个有效的连接事务 object</param>
    /// <param name="spName">存储过程名称</param>
    /// <param name="dataRow">使用DataRow作为参数值</param>
    /// <returns>返回XmlReader结果集对象.</returns>
    public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, dataRow);
            return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
            return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);

    }

    #endregion ExecuteXmlReaderTypedParams 类型化参数(DataRow)
}
