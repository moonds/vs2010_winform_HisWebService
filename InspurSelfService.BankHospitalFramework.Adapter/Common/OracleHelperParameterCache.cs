/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：OracleHelperParameterCache.cs
 * 描    述：Oracle 数据操作工具类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Data;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;

/// <summary>
///SqlHelperParameterCache 的摘要说明
/// </summary>
public sealed class OracleHelperParameterCache
{
    #region private methods, variables, and constructors

    private OracleHelperParameterCache() { }

    private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

    /// <summary>
    /// Resolve at run time the appropriate set of SqlParameters for a stored procedure
    /// </summary>
    /// <param name="connection">A valid OracleConnection object</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">Whether or not to include their return value parameter</param>
    /// <returns>The parameter array discovered.</returns>
    private static OracleParameter[] DiscoverSpParameterSet(OracleConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        using (OracleCommand cmd = new OracleCommand(spName, connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            OracleCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            //此处为SqlCommandBuilder.DeriveParameters的Bug
            //在OracleCommandBuilder.DeriveParameters中不需要
            //if (!includeReturnValueParameter)
            //{
            //    cmd.Parameters.RemoveAt(0);
            //}

            OracleParameter[] discoveredParameters = new OracleParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            foreach (OracleParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }
    }

    /// <summary>
    /// Deep copy of cached OracleParameter array
    /// </summary>
    /// <param name="originalParameters"></param>
    /// <returns></returns>
    private static OracleParameter[] CloneParameters(OracleParameter[] originalParameters)
    {
        OracleParameter[] clonedParameters = new OracleParameter[originalParameters.Length];

        for (int i = 0, j = originalParameters.Length; i < j; i++)
        {
            clonedParameters[i] = (OracleParameter)((ICloneable)originalParameters[i]).Clone();
        }

        return clonedParameters;
    }

    #endregion private methods, variables, and constructors

    #region caching functions

    /// <summary>
    /// Add parameter array to the cache
    /// </summary>
    /// <param name="connectionString">A valid connection string for a OracleConnection</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters to be cached</param>
    public static void CacheParameterSet(string connectionString, string commandText, params OracleParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

        string hashKey = connectionString + ":" + commandText;

        paramCache[hashKey] = commandParameters;
    }

    /// <summary>
    /// Retrieve a parameter array from the cache
    /// </summary>
    /// <param name="connectionString">A valid connection string for a OracleConnection</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>An array of SqlParamters</returns>
    public static OracleParameter[] GetCachedParameterSet(string connectionString, string commandText)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

        string hashKey = connectionString + ":" + commandText;

        OracleParameter[] cachedParameters = paramCache[hashKey] as OracleParameter[];
        if (cachedParameters == null)
        {
            return null;
        }
        else
        {
            return CloneParameters(cachedParameters);
        }
    }

    #endregion caching functions

    #region Parameter Discovery Functions

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a OracleConnection</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <returns>An array of SqlParameters</returns>
    public static OracleParameter[] GetSpParameterSet(string connectionString, string spName)
    {
        return GetSpParameterSet(connectionString, spName, false);
    }

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a OracleConnection</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
    /// <returns>An array of SqlParameters</returns>
    public static OracleParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
        }
    }

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connection">A valid OracleConnection object</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <returns>An array of SqlParameters</returns>
    internal static OracleParameter[] GetSpParameterSet(OracleConnection connection, string spName)
    {
        return GetSpParameterSet(connection, spName, false);
    }

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connection">A valid OracleConnection object</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
    /// <returns>An array of SqlParameters</returns>
    internal static OracleParameter[] GetSpParameterSet(OracleConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        using (OracleConnection clonedConnection = (OracleConnection)((ICloneable)connection).Clone())
        {
            return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
        }
    }

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <param name="connection">A valid OracleConnection object</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
    /// <returns>An array of SqlParameters</returns>
    private static OracleParameter[] GetSpParameterSetInternal(OracleConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

        OracleParameter[] cachedParameters;

        cachedParameters = paramCache[hashKey] as OracleParameter[];
        if (cachedParameters == null)
        {
            OracleParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
            paramCache[hashKey] = spParameters;
            cachedParameters = spParameters;
        }

        return CloneParameters(cachedParameters);
    }

    #endregion Parameter Discovery Functions
}
