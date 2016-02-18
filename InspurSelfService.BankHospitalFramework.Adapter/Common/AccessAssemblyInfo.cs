/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：AccessAssemblyInfo.cs
 * 描    述：
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using InspurSelfService.BankHospitalFramework.Adapter;

/// <summary>
/// 程序集属性访问类
/// </summary>
public class AccessAssemblyInfo
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// 访问程序集信息
    /// </summary>
    public AccessAssemblyInfo()
    {
      Type t= System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        AssemblyInfo<BasePage> infoClass = new AssemblyInfo<BasePage>();
        log.InfoFormat("AssemblyFullName : {0}", infoClass.AssemblyFullName);
        log.InfoFormat("AssemblyName : {0}", infoClass.AssemblyName);
        log.InfoFormat("CodeBase : {0}", infoClass.CodeBase);
        log.InfoFormat("Company : {0}", infoClass.Company);
        log.InfoFormat("Copyright : {0}", infoClass.Copyright);
        log.InfoFormat("Description : {0}", infoClass.Description);
        log.InfoFormat("Product : {0}", infoClass.Product);
        log.InfoFormat("Title : {0}", infoClass.Title);
        log.InfoFormat("Version : {0}", infoClass.Version);
        log.InfoFormat("Configration : {0}", infoClass.Configration);
        log.InfoFormat("TradeMark : {0}", infoClass.TradeMark);
        log.InfoFormat("Culture : {0}", infoClass.Culture);
    }
}

/// <summary>
/// 程序集类集合
/// </summary>
/// <typeparam name="T"></typeparam>
public class AssemblyInfo<T>
{
    private Type myType;

    /// <summary>
    /// 构造函数
    /// </summary>
    public AssemblyInfo()
    {
        myType = typeof(T);
    }

    /// <summary>
    /// 程序集名称
    /// </summary>
    public String AssemblyName
    {
        get
        {
            return myType.Assembly.GetName().Name.ToString();
        }
    }

    /// <summary>
    /// 程序集全名
    /// </summary>
    public String AssemblyFullName
    {
        get
        {
            return myType.Assembly.GetName().FullName.ToString();
        }
    }

    /// <summary>
    /// 代码库
    /// </summary>
    public String CodeBase
    {
        get
        {
            return myType.Assembly.CodeBase;
        }
    }

    /// <summary>
    /// 版本
    /// </summary>
    public String Version
    {
        get
        {
            return myType.Assembly.GetName().Version.ToString();
        }
    }

    /// <summary>
    /// 版权
    /// </summary>
    public String Copyright
    {
        get
        {
            Type att = typeof(AssemblyCopyrightAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyCopyrightAttribute copyattr = (AssemblyCopyrightAttribute)r[0];
            return copyattr.Copyright;
        }
    }

    /// <summary>
    /// 公司
    /// </summary>
    public String Company
    {
        get
        {
            Type att = typeof(AssemblyCompanyAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyCompanyAttribute compattr = (AssemblyCompanyAttribute)r[0];
            return compattr.Company;
        }
    }

    /// <summary>
    /// 配置
    /// </summary>
    public String Configration
    {
        get
        {
            Type att = typeof(AssemblyConfigurationAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyConfigurationAttribute configattr = (AssemblyConfigurationAttribute)r[0];
            return configattr.Configuration;
        }
    }

    /// <summary>
    /// 商标
    /// </summary>
    public string TradeMark
    {
        get
        {
            Type att = typeof(AssemblyTrademarkAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyTrademarkAttribute aa = (AssemblyTrademarkAttribute)r[0];
            return aa.Trademark;
        }
    }

    /// <summary>
    /// 文化
    /// </summary>
    public string Culture
    {
        get
        {
            Type att = typeof(AssemblyCultureAttribute);
            object[] a = myType.Assembly.GetCustomAttributes(att, false);
            if (a.Length > 0)
            {
                AssemblyCultureAttribute aa = (AssemblyCultureAttribute)a[0];
                return aa.Culture;
            }
            else
            {
                return "No value";
            }
        }
    }

    /// <summary>
    /// 描述
    /// </summary>
    public String Description
    {
        get
        {
            Type att = typeof(AssemblyDescriptionAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyDescriptionAttribute descattr = (AssemblyDescriptionAttribute)r[0];
            return descattr.Description;
        }
    }

    /// <summary>
    /// 产品
    /// </summary>
    public String Product
    {
        get
        {
            Type att = typeof(AssemblyProductAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyProductAttribute prodattr = (AssemblyProductAttribute)r[0];
            return prodattr.Product;
        }
    }

    /// <summary>
    /// 标题
    /// </summary>
    public String Title
    {
        get
        {
            Type att = typeof(AssemblyTitleAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyTitleAttribute titleattr = (AssemblyTitleAttribute)r[0];
            return titleattr.Title;
        }
    }
}
