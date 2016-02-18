/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：JsonHelper.cs
 * 描    述：Json 处理工具类
/************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


public class JsonHelper : IJsonHelper
{
    #region 单例构造
    private static JsonHelper instance;
    private static readonly object syncroot = new object();
    public static JsonHelper GetInstance()
    {
        if (instance == null)
        {
            lock (syncroot)
            {
                if (instance == null)
                {
                    instance = new JsonHelper();
                }
            }
        }
        return instance;
    }
    #endregion

    #region 私有方法
    #endregion

    #region IJsonHelper 方法和属性

    /// <summary>
    /// Object对象转为Json格式字符串
    /// add by Vincent.Q 11.01.12
    /// </summary>
    /// <param name="aobj_data"></param>
    /// <returns></returns>
    public string GetJsonStringByObject(object aobj_data)
    {
        string s_jsonstring = string.Empty;
        s_jsonstring = Newtonsoft.Json.JavaScriptConvert.SerializeObject(aobj_data);

        return s_jsonstring;

    }

    /// <summary>
    /// DataTable对象转为Json格式字符串
    /// add by Vincent.Q 11.01.12
    /// </summary>
    /// <param name="atable_data"></param>
    /// <returns></returns>
    public string GetJsonStringByDataTable(DataTable atable_data)
    {
        return this.GetJsonStringByDataTable(atable_data, "DataTable");

    }

    /// <summary>
    /// DataTable对象转为Json格式字符串
    /// add by Vincent.Q 11.01.12
    /// </summary>
    /// <param name="atable_data"></param>
    /// <param name="as_tablename"></param>
    /// <returns></returns>
    public string GetJsonStringByDataTable(DataTable atable_data, string as_tablename)
    {
        string s_jsonstring = string.Empty;

        //参数检测
        if (atable_data == null)
            return s_jsonstring;

        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        using (JsonWriter jw = new JsonWriter(sw))
        {
            JsonSerializer ser = new JsonSerializer();
            jw.WriteStartObject();
            jw.WritePropertyName(as_tablename);
            jw.WriteStartArray();

            foreach (DataRow dr in atable_data.Rows)
            {
                jw.WriteStartObject();

                foreach (DataColumn dc in atable_data.Columns)
                {
                    jw.WritePropertyName(dc.ColumnName);
                    ser.Serialize(jw, dr[dc].ToString());
                }

                jw.WriteEndObject();
            }
            jw.WriteEndArray();
            jw.WriteEndObject();

            sw.Close();
            jw.Close();
        }

        return sb.ToString();

    }

    /// <summary>
    /// Json格式字符串转为Object对象
    /// add by Vincent.Q 11.01.12
    /// </summary>
    /// <param name="as_jsonstring"></param>
    /// <returns></returns>
    public object GetObjectByJsonString(string as_jsonstring)
    {
        object obj_return = null;

        obj_return = Newtonsoft.Json.JavaScriptConvert.DeserializeObject(as_jsonstring);
        return obj_return;

    }

    /// <summary>
    /// Json格式字符串转为Object对象
    /// add by Vincent.Q 11.01.12
    /// </summary>
    /// <param name="as_jsonstring"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public object GetObjectByJsonString(string as_jsonstring, Type t)
    {
        object obj_return = null;

        obj_return = Newtonsoft.Json.JavaScriptConvert.DeserializeObject(as_jsonstring, t);
        return obj_return;
    }

    /// <summary>
    /// Json格式字符串转为Entity对象
    /// add by Vincent.Q 11.01.12
    /// </summary>
    /// <param name="as_jsonstring"></param>
    /// <param name="aentity"></param>
    /// <returns></returns>
    public BaseEntity GetEntityByJsonString(string as_jsonstring, BaseEntity aentity)
    {
        BaseEntity entity_return = null;

        //参数检测
        if (string.IsNullOrEmpty(as_jsonstring) || aentity == null)
        {
            return entity_return;
        }

        //获取实体属性名称列表
        entity_return = aentity;
        IList<string> list_propertyname = entity_return.GetPropertyName();

        //获取json对象
        object obj_parm = this.GetObjectByJsonString(as_jsonstring);
        Newtonsoft.Json.JavaScriptObject jso = (JavaScriptObject)obj_parm;
        for (int i = 0; i < list_propertyname.Count; i++)
        {
            string s_propertyname = list_propertyname[i];
            object obj_propertyvalue = jso[s_propertyname];

            //动态写入实体类中
            entity_return.SetPropertyValue(s_propertyname, obj_propertyvalue);
        }

        return entity_return;

    }

    /// <summary>
    /// Json格式字符串转为DataTable对象
    /// add by Vincent.Q 11.01.12
    /// </summary>
    /// <param name="as_jsonstring"></param>
    /// <returns></returns>
    public DataTable GetTableByJsonString(string as_jsonstring)
    {
        return this.GetTableByJsonString(as_jsonstring, "DataTable");

    }

    /// <summary>
    /// Json格式字符串转为DataTable对象
    /// add by Vincent.Q 11.01.12
    /// </summary>
    /// <param name="as_jsonstring"></param>
    /// <param name="as_tablename"></param>
    /// <returns></returns>
    public DataTable GetTableByJsonString(string as_jsonstring, string as_tablename)
    {
        bool b_initcolumn = false;
        DataTable table_return = null;

        //参数检测
        if (string.IsNullOrEmpty(as_jsonstring) || string.IsNullOrEmpty(as_tablename))
        {
            return table_return;
        }

        //获取json对象
        object obj_parm = this.GetObjectByJsonString(as_jsonstring);

        //转换后,有且只有一个对象,即DataTable
        Newtonsoft.Json.JavaScriptObject jso_parm = (JavaScriptObject)obj_parm;

        //转为json数组
        Newtonsoft.Json.JavaScriptArray jsa_parm = (JavaScriptArray)jso_parm[as_tablename];

        //循环获取
        for (int i = 0; i < jsa_parm.Count; i++)
        {
            Newtonsoft.Json.JavaScriptObject jso_item = (JavaScriptObject)jsa_parm[i];

            //转为泛型处理
            Dictionary<string, object> dic_item = (Dictionary<string, object>)jso_item;

            //加载DataTable栏目信息
            if (!b_initcolumn)
            {
                table_return = new DataTable();
                foreach (string s_key in dic_item.Keys)
                {
                    object obj_value = null;
                    dic_item.TryGetValue(s_key, out obj_value);

                    table_return.Columns.Add(s_key, typeof(string));
                }
                b_initcolumn = true;
            }

            //加载DataTable数据
            DataRow row_add = table_return.NewRow();
            foreach (string s_key in dic_item.Keys)
            {
                object obj_value = null;
                dic_item.TryGetValue(s_key, out obj_value);

                row_add[s_key] = obj_value;
            }
            table_return.Rows.Add(row_add);

        }

        return table_return;

    }

    #endregion

}
