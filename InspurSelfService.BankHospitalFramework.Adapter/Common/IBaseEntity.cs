/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：IBaseEntity.cs
 * 描    述：实体类接口
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Reflection;

//using Navi.Kernel.DbUtilities;

public interface IBaseEntity
{
    //公有属性        
    string DbTableName { get; set; }
    Dictionary<string, string> DbColumnNameList { get; set; }

    //基础方法
    IList<string> GetPropertyName();
    object GetPropertyValue(string as_propertyname);
    void SetPropertyValue(string as_propertyname, object aobj_propertyvalue);

    Dictionary<string, object> CollectPropertyValueToDictionary();
    Dictionary<string, string> CollectPropertyDataTypeToDictionary();
    void SetPropertyValueByDataRow(DataRow arow_data);
    void SetPropertyValueByDictionary(Dictionary<string, object> adic_propertyvalue);

}