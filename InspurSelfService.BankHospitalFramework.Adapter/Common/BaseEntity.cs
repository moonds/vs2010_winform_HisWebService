/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：BaseEntity.cs
 * 描    述：基础实体类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Data.SqlClient;
//using System.Data.OracleClient;
using System.Reflection;

//using Navi.Kernel.DbUtilities;


/// <summary>
/// Entity层服务类祖先
/// add by Vincent.Q 10.04.12
/// 只生成包含数据的insert,update,delete语句,其他事件一概不管 modify by Vincent.Q 10.05.23
/// 只保留Db表的字段信息,与后台Db是何种数据库无关 modify by Vincent.Q 10.09.18
/// 增加收集实体属性数据类型方法 modify by Vincent.Q 10.10.08
/// </summary>
public class BaseEntity : IBaseEntity
{
    #region 私有变量
    private string s_dbtablename = string.Empty;
    private IList<string> list_removepropertyname = null;
    private Dictionary<string, string> dic_dbcolumnnamelist;
    #endregion

    #region 构造函数
    /// <summary>
    /// 构造函数
    /// </summary>
    public BaseEntity()
    {
        if (this.list_removepropertyname == null)
            this.list_removepropertyname = new List<string>();

        this.list_removepropertyname.Add("DbTableName");
        this.list_removepropertyname.Add("RemovePropertyName");
        this.list_removepropertyname.Add("DbColumnNameList");

    }
    #endregion

    #region 私有方法
    #endregion

    #region IBaseEntity 属性和方法

    #region 公有属性

    /// <summary>
    /// Db表名称
    /// </summary>
    public string DbTableName
    {
        get { return s_dbtablename; }
        set { s_dbtablename = value; }
    }

    /// <summary>
    /// Sql语句栏目名称列表
    /// </summary>
    public Dictionary<string, string> DbColumnNameList
    {
        get
        {
            if (this.dic_dbcolumnnamelist == null)
            {
                this.dic_dbcolumnnamelist = new Dictionary<string, string>();
            }
            return this.dic_dbcolumnnamelist;
        }
        set
        {
            this.dic_dbcolumnnamelist = value;
        }
    }

    #endregion

    #region 公有方法

    #region 获取属性名称列表
    /// <summary>
    /// 获取属性名称列表
    /// add by Vincent.Q 10.04.13
    /// 过滤不是DbTable.Column的属性名称 modify by Vincent.Q 10.10.08
    /// </summary>
    /// <returns></returns>
    public IList<string> GetPropertyName()
    {
        IList<string> list_propertyname = null;

        Type t = this.GetType();
        PropertyInfo[] array_property = t.GetProperties();

        //检测是否有属性
        if (array_property.Length <= 0)
            return list_propertyname;

        list_propertyname = new List<string>();
        foreach (PropertyInfo property in array_property)
        {
            string s_propertyname = property.Name;

            //若在this.list_removepropertyname找到,则不必获取 modify by Vincent.Q 10.10.08
            if (this.list_removepropertyname.Contains(s_propertyname))
                continue;

            list_propertyname.Add(s_propertyname);
        }

        return list_propertyname;
    }
    #endregion

    #region 获取实体属性值,根据属性名称
    /// <summary>
    /// 获取实体属性值,根据属性名称
    /// add by Vincent.Q 10.12.30
    /// </summary>
    /// <param name="as_propertyname"></param>
    /// <returns></returns>
    public object GetPropertyValue(string as_propertyname)
    {
        object obj_propertyvalue = null;

        //参数检测
        if (string.IsNullOrEmpty(as_propertyname))
        {
            return obj_propertyvalue;
        }

        //反射实体本身类型
        Type t = this.GetType();
        obj_propertyvalue = t.GetProperty(as_propertyname).GetValue(this, null);

        return obj_propertyvalue;

    }
    #endregion

    #region 设置实体属性值,根据属性名称
    /// <summary>
    /// 设置实体属性值,根据属性名称
    /// add by Vincent.Q 10.12.30
    /// </summary>
    /// <param name="as_propertyname"></param>
    /// <param name="aobj_propertyvalue"></param>
    public void SetPropertyValue(string as_propertyname, object aobj_propertyvalue)
    {
        //获取实体类型
        Type t = this.GetType();

        //获取属性信息,并判断是否存在
        PropertyInfo property = t.GetProperty(as_propertyname.ToUpper());
        if (property == null)
            return;

        string s_datatype = property.PropertyType.Name.Trim().ToLower().Substring(0, 3);

        //属性赋值
        object obj_propertyvalue = null;
        switch (s_datatype)
        {
            case "int":
                int i_parm = 0;
                if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                    aobj_propertyvalue = 0;

                i_parm = Convert.ToInt16(aobj_propertyvalue);
                obj_propertyvalue = i_parm;
                break;
            case "dec":
                decimal dc_parm = 0.0m;
                if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                    aobj_propertyvalue = 0.0m;

                dc_parm = Convert.ToDecimal(aobj_propertyvalue);
                obj_propertyvalue = dc_parm;
                break;
            case "str":
                string s_parm = string.Empty;
                if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                    aobj_propertyvalue = "";

                s_parm = aobj_propertyvalue.ToString();
                obj_propertyvalue = s_parm;
                break;
            case "dat":
                DateTime dtm_parm = DateTime.MinValue;
                if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                    obj_propertyvalue = DateTime.MinValue;

                dtm_parm = Convert.ToDateTime(aobj_propertyvalue);
                obj_propertyvalue = dtm_parm;
                break;
            default:
                break;
        }

        t.GetProperty(as_propertyname).SetValue(this, obj_propertyvalue, null);

    }
    #endregion

    #region 收集实体属性值,生成Dictionary对象
    /// <summary>
    /// 收集实体属性值,生成Dictionary对象
    /// add by Vincent.Q 10.04.13
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, object> CollectPropertyValueToDictionary()
    {
        Dictionary<string, object> dic_propertyvalue = null;

        //获取属性列表
        IList<string> list_propertyname = this.GetPropertyName();
        if (list_propertyname == null || list_propertyname.Count <= 0)
            return dic_propertyvalue;

        //取属性值
        Type t = this.GetType();
        dic_propertyvalue = new Dictionary<string, object>();
        for (int i = 0; i < list_propertyname.Count; i++)
        {
            string s_propertyname = list_propertyname[i].Trim();
            object obj_propertyvalue = t.GetProperty(s_propertyname).GetValue(this, null);
            dic_propertyvalue.Add(s_propertyname, obj_propertyvalue);
        }

        return dic_propertyvalue;
    }
    #endregion

    #region 收集实体属性数据类型,生成Dictionary对象
    /// <summary>
    /// 收集实体属性数据类型,生成Dictionary对象
    /// add by Vincent.Q 10.10.08
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> CollectPropertyDataTypeToDictionary()
    {
        Dictionary<string, string> dic_datatype = null;

        //获取实体属性名称,即DbTable.Column
        IList<string> list_propertyname = this.GetPropertyName();
        if (list_propertyname == null || list_propertyname.Count <= 0)
            return dic_datatype;

        Type t = this.GetType();
        dic_datatype = new Dictionary<string, string>();
        for (int i = 0; i < list_propertyname.Count; i++)
        {
            string s_columnname = list_propertyname[i];
            PropertyInfo property_columninfo = t.GetProperty(s_columnname.ToUpper());
            string s_datatype = property_columninfo.PropertyType.Name.ToLower();
            dic_datatype.Add(s_columnname, s_datatype);
        }

        return dic_datatype;

    }
    #endregion

    #region 填充实体类属性值
    /// <summary>
    /// 填充实体类属性值,根据DataRow对象
    /// add by Vincent.Q 10.04.13
    /// </summary>
    /// <param name="atable_data"></param>
    public void SetPropertyValueByDataRow(DataRow arow_data)
    {
        //参数检测
        if (arow_data == null)
            return;

        //获取属性列表
        IList<string> list_propertyname = this.GetPropertyName();
        if (list_propertyname == null || list_propertyname.Count <= 0)
            return;

        Type t = this.GetType();
        for (int i = 0; i < arow_data.ItemArray.Length; i++)
        {
            foreach (DataColumn column in arow_data.Table.Columns)
            {
                string s_columnname = column.ColumnName.Trim().ToUpper();
                if (!list_propertyname.Contains(s_columnname))
                    continue;

                //属性信息
                PropertyInfo property_columninfo = t.GetProperty(s_columnname.ToUpper());
                string s_columntype = property_columninfo.PropertyType.Name.Trim().ToLower().Substring(0, 3);

                //属性取值
                object obj_columnvalue = arow_data[column];

                //属性赋值
                switch (s_columntype)
                {
                    case "int":
                        int i_parm = 0;
                        if (obj_columnvalue == null || string.IsNullOrEmpty(obj_columnvalue.ToString()))
                            obj_columnvalue = 0;

                        i_parm = Convert.ToInt32(obj_columnvalue);
                        obj_columnvalue = i_parm;
                        break;
                    case "dec":
                        decimal dc_parm = 0.0m;
                        if (obj_columnvalue == null || string.IsNullOrEmpty(obj_columnvalue.ToString()))
                            obj_columnvalue = 0.0m;

                        dc_parm = Convert.ToDecimal(obj_columnvalue);
                        obj_columnvalue = dc_parm;
                        break;
                    case "str":
                        string s_parm = string.Empty;
                        if (obj_columnvalue == null || string.IsNullOrEmpty(obj_columnvalue.ToString()))
                            obj_columnvalue = "";

                        s_parm = obj_columnvalue.ToString();
                        obj_columnvalue = s_parm;
                        break;
                    case "dat":
                        DateTime dtm_parm = DateTime.MinValue;
                        if (obj_columnvalue == null || string.IsNullOrEmpty(obj_columnvalue.ToString()))
                            obj_columnvalue = DateTime.MinValue;

                        dtm_parm = Convert.ToDateTime(obj_columnvalue);
                        obj_columnvalue = dtm_parm;
                        break;
                    default:
                        break;
                }

                //oracle数据库,id流水号数据类型为decimal,需要额外处理 add by Vincent.Q 10.06.05
                if (s_columnname == "id" && obj_columnvalue.GetType().Name.Trim().ToLower().Substring(0, 3) == "dec")
                {

                }

                t.GetProperty(s_columnname).SetValue(this, obj_columnvalue, null);

            }
        }

    }

    /// <summary>
    /// 填充实体类属性值,根据Dictionary对象
    /// add by Vincent.Q 10.04.13
    /// </summary>
    /// <param name="adic_propertyvalue"></param>
    public void SetPropertyValueByDictionary(Dictionary<string, object> adic_propertyvalue)
    {
        //参数检测
        if (adic_propertyvalue == null || adic_propertyvalue.Count <= 0)
            return;

        //获取属性列表
        IList<string> list_propertyname = this.GetPropertyName();
        if (list_propertyname == null || list_propertyname.Count <= 0)
            return;

        Type t = this.GetType();
        foreach (string item in adic_propertyvalue.Keys)
        {
            string s_columnname = item.Trim();
            if (!list_propertyname.Contains(s_columnname.ToUpper()))
                continue;

            //属性赋值
            object obj_columnvalue = null;
            adic_propertyvalue.TryGetValue(s_columnname, out obj_columnvalue);

            //需要将obj_columnvalue数据类型修改为与属性数据类型一致,不止不为空情况 modify by Vincent.Q 10.06.05
            PropertyInfo property_columninfo = t.GetProperty(s_columnname.ToUpper());
            string s_columntype = property_columninfo.PropertyType.Name.Trim().ToLower().Substring(0, 3);
            switch (s_columntype)
            {
                case "int":
                    int i_parm = 0;
                    if (obj_columnvalue == null || string.IsNullOrEmpty(obj_columnvalue.ToString()))
                        obj_columnvalue = 0;

                    i_parm = Convert.ToInt16(obj_columnvalue);
                    obj_columnvalue = i_parm;
                    break;
                case "dec":
                    decimal dc_parm = 0.0m;
                    if (obj_columnvalue == null || string.IsNullOrEmpty(obj_columnvalue.ToString()))
                        obj_columnvalue = 0.0m;

                    dc_parm = Convert.ToDecimal(obj_columnvalue);
                    obj_columnvalue = dc_parm;
                    break;
                case "str":
                    string s_parm = string.Empty;
                    if (obj_columnvalue == null || string.IsNullOrEmpty(obj_columnvalue.ToString()))
                        obj_columnvalue = "";

                    s_parm = obj_columnvalue.ToString();
                    obj_columnvalue = s_parm;
                    break;
                case "dat":
                    DateTime dtm_parm = DateTime.MinValue;
                    if (obj_columnvalue == null || string.IsNullOrEmpty(obj_columnvalue.ToString()))
                        obj_columnvalue = DateTime.Now;

                    dtm_parm = Convert.ToDateTime(obj_columnvalue);
                    obj_columnvalue = dtm_parm;
                    break;
                default:
                    break;
            }

            t.GetProperty(s_columnname.ToUpper()).SetValue(this, obj_columnvalue, null);
        }

    }
    #endregion

    #endregion

    #endregion
}
