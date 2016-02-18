/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：IJsonHelper.cs
 * 描    述：Json 处理接口
/************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;


public interface IJsonHelper
{
    //生成json格式字符串
    string GetJsonStringByObject(object aobj_data);
    string GetJsonStringByDataTable(DataTable atable_data);
    string GetJsonStringByDataTable(DataTable atable_data, string as_tablename);

    //json格式字符串转为各对象
    object GetObjectByJsonString(string as_jsonstring);
    object GetObjectByJsonString(string as_jsonstring, Type t);
    BaseEntity GetEntityByJsonString(string as_jsonstring, BaseEntity aentity);
    DataTable GetTableByJsonString(string as_jsonstring);
    DataTable GetTableByJsonString(string as_jsonstring, string as_tablename);

}

