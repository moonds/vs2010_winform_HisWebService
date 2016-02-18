/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：DatabaseAccess.cs
 * 描    述：数据操作类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;
using System.Reflection;

namespace InspurSelfService.BankHospitalFramework.Common
{
    public class DatabaseAccess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //static DatabaseAccess()
        //{
        //    log4net.LogManager.GetCurrentLoggers();
        //    log = log4net.LogManager.GetLogger(Assembly.GetCallingAssembly(), "logDb");
        //}

        private String _connectionString;

        public String ConnectionString
        {
            get { return _connectionString; }
            set
            {
                _connectionString = value;
            }
        }

        public static DbProviderFactory Factory
        {
            get
            {
                return SqlClientFactory.Instance;
            }
        }

        public DatabaseAccess(string connectionString)
        {
            this._connectionString = connectionString;
        }
        /// <summary>
        /// 获得单张表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteSelectDataTable(String sql)
        {
            DataTable result = new DataTable("dt");
            using (DbDataAdapter da = Factory.CreateDataAdapter())
            {
                using (da.SelectCommand = Factory.CreateCommand())
                {
                    da.SelectCommand.CommandText = sql;
                    using (da.SelectCommand.Connection = Factory.CreateConnection())
                    {
                        da.SelectCommand.Connection.ConnectionString = ConnectionString;
                        try
                        {
                            da.Fill(result);
                        }
                        catch (Exception ex)
                        {
                            log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                            result.Dispose();
                            result = null;
                            throw ex;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获得多张表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecuteSelectDataSet(String sql)
        {
            DataSet result = new DataSet();
            using (DbDataAdapter da = Factory.CreateDataAdapter())
            {
                using (da.SelectCommand = Factory.CreateCommand())
                {
                    da.SelectCommand.CommandText = sql;
                    using (da.SelectCommand.Connection = Factory.CreateConnection())
                    {
                        da.SelectCommand.Connection.ConnectionString = ConnectionString;
                        try
                        {
                            da.Fill(result);
                        }
                        catch (Exception ex)
                        {
                            log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                            result.Dispose();
                            result = null;
                            throw ex;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获得第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public String ExecuteSelect(String sql)
        {
            using (DbConnection conn = Factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    try
                    {
                        object obj = cmd.ExecuteScalar();
                        if (obj == DBNull.Value || obj == null)
                            return null;
                        return obj.ToString();
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                        return null;
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 获得第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T ExecuteSelect<T>(String sql, DbParameter[] parameters)
        {
            using (DbConnection conn = Factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);
                    try
                    {
                        object obj = cmd.ExecuteScalar();
                        if (obj == DBNull.Value || obj == null)
                            return default(T);
                        return (T)Convert.ChangeType(obj,typeof(T));
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                        return default(T);
                        throw ex;
                    }
                }
            }
        }
        /// <summary>
        /// 执行存储过程获得多张表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteStoreProcedureDataSet(String sql, DbParameter[] parameters)
        {
            DataSet result = new DataSet();
            using (DbDataAdapter da = Factory.CreateDataAdapter())
            {
                using (da.SelectCommand = Factory.CreateCommand())
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandText = sql;
                    if (parameters != null && parameters.Length > 0)
                        da.SelectCommand.Parameters.AddRange(parameters);
                    using (da.SelectCommand.Connection = Factory.CreateConnection())
                    {
                        da.SelectCommand.Connection.ConnectionString = ConnectionString;
                        try
                        {
                            da.Fill(result);
                        }
                        catch (Exception ex)
                        {
                            log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                            result.Dispose();
                            result = null;
                            throw ex;
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 执行存储过程获得第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public String ExecuteStoreProcedureString(String sql, SqlParameter[] parameters)
        {
            using (DbConnection conn = Factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sql;
                    if(parameters != null && parameters.Length>0)
                        cmd.Parameters.AddRange(parameters);
                    try
                    {
                        object obj = cmd.ExecuteScalar();
                        if (obj == DBNull.Value || obj == null)
                            return null;
                        return obj.ToString();
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                        return null;
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 执行插入返回行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteStoreProcedureInsert(String sql, SqlParameter[] parameters)
        {
            using (DbConnection conn = Factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sql;
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);
                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                        return 0;
                        throw ex;
                    }
                }
            }
        }
        /// <summary>
        /// 更新行
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteUpdate(String sql)
        {
            using (DbConnection conn = Factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                        return 0;
                        throw ex;
                    }
                }
            }
        }
        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteInsert(String sql)
        {
            using (DbConnection conn = Factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                        return 0;
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 插入带有自增列行并获得新的自增编号
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteInsertIdentity(String sql)
        {
            using (DbConnection conn = Factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    if (sql.Trim().IndexOf("SELECT @@IDENTITY") <= 0)
                        sql = string.Format("{0} SELECT @@IDENTITY;", sql);
                    cmd.CommandText = sql;
                    try
                    {
                        object obj = cmd.ExecuteScalar();
                        if (obj == DBNull.Value || obj == null)
                            return -1;
                        return Convert.ToInt32(obj);
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                        return 0;
                        throw ex;
                    }
                }
            }
        }
        /// <summary>
        /// 通过dataReader获得表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteSelect(string sql, DbParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);
                    try
                    {
                        IDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        dt.Load(dr);
                        if (dt.Rows.Count <= 0)
                        {
                            dt.Dispose();
                            return null;
                        }
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                        dt.Dispose();
                        return null;
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 获得多个DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecuteSelectTables(string sql)
        {
            DataSet ds = new DataSet();
            using (DbDataAdapter da = Factory.CreateDataAdapter())
            {
                using (da.SelectCommand = Factory.CreateCommand())
                {
                    da.SelectCommand.CommandText = sql;
                    using (da.SelectCommand.Connection = Factory.CreateConnection())
                    {
                        da.SelectCommand.Connection.ConnectionString = ConnectionString;
                        try
                        {
                            da.Fill(ds);
                        }
                        catch (Exception ex)
                        {
                            log.Error(string.Format("执行SQL\r\n{0}\r\n失败", sql), ex);
                            ds.Dispose();
                            ds = null;
                            throw ex;
                        }
                    }
                }
            }
            return ds;
        }
    }
}