#if ODBC
using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Odbc;
using System.Collections;
using System.Data;

/// <summary>
///SqlHelperParameterCache 的摘要说明
/// </summary>
public sealed class OdbcHelperParameterCache
{
    #region private methods, variables, and constructors

    private OdbcHelperParameterCache() { }

    private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

    /// <summary>
    /// Resolve at run time the appropriate set of SqlParameters for a stored procedure
    /// </summary>
    /// <param name="connection">A valid OdbcConnection object</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">Whether or not to include their return value parameter</param>
    /// <returns>The parameter array discovered.</returns>
    private static OdbcParameter[] DiscoverSpParameterSet(OdbcConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        using (OdbcCommand cmd = new OdbcCommand(spName, connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            OdbcCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            OdbcParameter[] discoveredParameters = new OdbcParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            foreach (OdbcParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }
    }

    /// <summary>
    /// Deep copy of cached OdbcParameter array
    /// </summary>
    /// <param name="originalParameters"></param>
    /// <returns></returns>
    private static OdbcParameter[] CloneParameters(OdbcParameter[] originalParameters)
    {
        OdbcParameter[] clonedParameters = new OdbcParameter[originalParameters.Length];

        for (int i = 0, j = originalParameters.Length; i < j; i++)
        {
            clonedParameters[i] = (OdbcParameter)((ICloneable)originalParameters[i]).Clone();
        }

        return clonedParameters;
    }

    #endregion private methods, variables, and constructors

    #region caching functions

    /// <summary>
    /// Add parameter array to the cache
    /// </summary>
    /// <param name="connectionString">A valid connection string for a OdbcConnection</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters to be cached</param>
    public static void CacheParameterSet(string connectionString, string commandText, params OdbcParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

        string hashKey = connectionString + ":" + commandText;

        paramCache[hashKey] = commandParameters;
    }

    /// <summary>
    /// Retrieve a parameter array from the cache
    /// </summary>
    /// <param name="connectionString">A valid connection string for a OdbcConnection</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>An array of SqlParamters</returns>
    public static OdbcParameter[] GetCachedParameterSet(string connectionString, string commandText)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

        string hashKey = connectionString + ":" + commandText;

        OdbcParameter[] cachedParameters = paramCache[hashKey] as OdbcParameter[];
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
    /// <param name="connectionString">A valid connection string for a OdbcConnection</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <returns>An array of SqlParameters</returns>
    public static OdbcParameter[] GetSpParameterSet(string connectionString, string spName)
    {
        return GetSpParameterSet(connectionString, spName, false);
    }

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a OdbcConnection</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
    /// <returns>An array of SqlParameters</returns>
    public static OdbcParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        using (OdbcConnection connection = new OdbcConnection(connectionString))
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
    /// <param name="connection">A valid OdbcConnection object</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <returns>An array of SqlParameters</returns>
    internal static OdbcParameter[] GetSpParameterSet(OdbcConnection connection, string spName)
    {
        return GetSpParameterSet(connection, spName, false);
    }

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connection">A valid OdbcConnection object</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
    /// <returns>An array of SqlParameters</returns>
    internal static OdbcParameter[] GetSpParameterSet(OdbcConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        using (OdbcConnection clonedConnection = (OdbcConnection)((ICloneable)connection).Clone())
        {
            return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
        }
    }

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <param name="connection">A valid OdbcConnection object</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
    /// <returns>An array of SqlParameters</returns>
    private static OdbcParameter[] GetSpParameterSetInternal(OdbcConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

        OdbcParameter[] cachedParameters;

        cachedParameters = paramCache[hashKey] as OdbcParameter[];
        if (cachedParameters == null)
        {
            OdbcParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
            paramCache[hashKey] = spParameters;
            cachedParameters = spParameters;
        }

        return CloneParameters(cachedParameters);
    }

    #endregion Parameter Discovery Functions
}
#endif