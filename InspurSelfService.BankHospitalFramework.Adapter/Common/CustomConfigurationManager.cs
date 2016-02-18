/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：CustomConfigurationManager.cs
 * 描    述：用户配置管理类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;

/// <summary>
/// 自定义程序集配置管理类
/// </summary>
public class CustomConfigurationManager
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private static string configFile = "Adapter.config";

    /// <summary>
    /// 获取当前程序集ConnectionStrings配置
    /// </summary>
    public static ConnectionStringSettingsCollection ConnectionStrings
    {
        get
        {
            ConnectionStringSettingsCollection connectionStrings = new ConnectionStringSettingsCollection();

            try
            {
                XmlNodeList list = XMLHelper.GetXmlNodeListByXpath(GetConfigFileFullPath(), "//configuration//connectionStrings//add");
                if (list != null && list.Count > 0)
                {
                    foreach (XmlNode item in list)
                    {
                        string name = item.Attributes["name"] != null ? item.Attributes["name"].Value : "";
                        string connectionString = item.Attributes["connectionString"] != null ? item.Attributes["connectionString"].Value : null;
                        string providerName = item.Attributes["providerName"] != null ? item.Attributes["providerName"].Value : null;

                        if (!String.IsNullOrEmpty(name))//主键值不为null并且长度大于1
                            connectionStrings.Add(new ConnectionStringSettings(name, connectionString, providerName));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("【ConnectionStrings】", ex);
            }

            return connectionStrings;
        }
    }

    /// <summary>
    /// 获取当前程序集AppSettings配置
    /// </summary>
    public static NameValueCollection AppSettings
    {
        get
        {
            NameValueCollection appSettings = new NameValueCollection();
            try
            {
                XmlNodeList list = XMLHelper.GetXmlNodeListByXpath(GetConfigFileFullPath(), "//configuration//appSettings//add");
                if (list != null && list.Count > 0)
                {
                    foreach (XmlNode item in list)
                    {
                        string name = item.Attributes["key"] != null ? item.Attributes["key"].Value : null;
                        string value = item.Attributes["value"] != null ? item.Attributes["value"].Value : null;

                        appSettings.Add(name, value);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("【AppSettings】", ex);
            }

            return appSettings;
        }
    }

    /// <summary>
    /// 获取当前程序集配置文件
    /// </summary>
    /// <returns></returns>
    private static string GetConfigFileFullPath()
    {
        string path = null;

        if (System.Environment.CurrentDirectory == AppDomain.CurrentDomain.BaseDirectory)
        { path = AppDomain.CurrentDomain.BaseDirectory; }
        else
        { path = String.Format("{0}Bin\\{1}", AppDomain.CurrentDomain.BaseDirectory, configFile); }

        return path;
    }

}

