/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：XMLHelper.cs
 * 描    述：XML 操作工具类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

///<summary>
/// XMLHelper XML文档操作管理器
///</summary>
public class XMLHelper
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// 构造函数
    /// </summary>
    public XMLHelper()
    {
    }

    #region XML文档节点查询和读取
    ///<summary>
    /// 选择匹配XPath表达式的第一个节点XmlNode.
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
    ///<returns>返回XmlNode</returns>
    public static XmlNode GetXmlNodeByXpath(string xmlFileName, string xpath)
    {
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            return xmlNode;
        }
        catch (Exception ex)
        {
            log.Error(String.Format("{0}【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Reflection.MethodBase.GetCurrentMethod()), ex);
            return null;
        }
    }

    ///<summary>
    /// 选择匹配XPath表达式的节点列表XmlNodeList.
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
    ///<returns>返回XmlNodeList</returns>
    public static XmlNodeList GetXmlNodeListByXpath(string xmlFileName, string xpath)
    {
        XmlDocument xmlDoc = new XmlDocument();

        try
        {
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
            return xmlNodeList;
        }
        catch (Exception ex)
        {
            log.Error(String.Format("{0}【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Reflection.MethodBase.GetCurrentMethod()), ex);
            return null;
        }
    }

    ///<summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute.
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    ///<param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    ///<returns>返回xmlAttributeName</returns>
    public static XmlAttribute GetXmlAttribute(string xmlFileName, string xpath, string xmlAttributeName)
    {
        string content = string.Empty;
        XmlDocument xmlDoc = new XmlDocument();
        XmlAttribute xmlAttribute = null;
        try
        {
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                if (xmlNode.Attributes.Count > 0)
                {
                    xmlAttribute = xmlNode.Attributes[xmlAttributeName];
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(String.Format("{0}【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Reflection.MethodBase.GetCurrentMethod()), ex);
            return null;
        }
        return xmlAttribute;
    }
    #endregion

    #region XML文档创建和节点或属性的添加、修改
    ///<summary>
    /// 创建一个XML文档
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="rootNodeName">XML文档根节点名称(须指定一个根节点名称)</param>
    ///<param name="version">XML文档版本号(必须为:"1.0")</param>
    ///<param name="encoding">XML文档编码方式</param>
    ///<param name="standalone">该值必须是"yes"或"no",如果为null,Save方法不在XML声明上写出独立属性</param>
    ///<returns>成功返回true,失败返回false</returns>
    public static bool CreateXmlDocument(string xmlFileName, string rootNodeName, string version, string encoding, string standalone)
    {
        bool isSuccess = false;
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration(version, encoding, standalone);
            XmlNode root = xmlDoc.CreateElement(rootNodeName);
            xmlDoc.AppendChild(xmlDeclaration);
            xmlDoc.AppendChild(root);
            xmlDoc.Save(xmlFileName);
            isSuccess = true;
        }
        catch (Exception ex)
        {
            log.Error(String.Format("{0}【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Reflection.MethodBase.GetCurrentMethod()), ex);
        }
        return isSuccess;
    }

    ///<summary>
    /// 依据匹配XPath表达式的第一个节点来创建它的子节点(如果此节点已存在则追加一个新的同名节点
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    ///<param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
    ///<param name="innerText">节点文本值</param>
    ///<param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    ///<param name="value">属性值</param>
    ///<returns>成功返回true,失败返回false</returns>
    public static bool CreateXmlNodeByXPath(string xmlFileName, string xpath, string xmlNodeName, string innerText, string xmlAttributeName, string value)
    {
        bool isSuccess = false;
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //存不存在此节点都创建
                XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                subElement.InnerXml = innerText;

                //如果属性和值参数都不为空则在此新节点上新增属性
                if (!string.IsNullOrEmpty(xmlAttributeName) && !string.IsNullOrEmpty(value))
                {
                    XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                    xmlAttribute.Value = value;
                    subElement.Attributes.Append(xmlAttribute);
                }

                xmlNode.AppendChild(subElement);
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            isSuccess = true;
        }
        catch (Exception ex)
        {
            log.Error(String.Format("{0}【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Reflection.MethodBase.GetCurrentMethod()), ex);
        }
        return isSuccess;
    }

    ///<summary>
    /// 依据匹配XPath表达式的第一个节点来创建或更新它的子节点(如果节点存在则更新,不存在则创建)
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    ///<param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
    ///<param name="innerText">节点文本值</param>
    ///<returns>成功返回true,失败返回false</returns>
    public static bool CreateOrUpdateXmlNodeByXPath(string xmlFileName, string xpath, string xmlNodeName, string innerText)
    {
        bool isSuccess = false;
        bool isExistsNode = false;//标识节点是否存在
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //遍历xpath节点下的所有子节点
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    if (node.Name.ToLower() == xmlNodeName.ToLower())
                    {
                        //存在此节点则更新
                        node.InnerXml = innerText;
                        isExistsNode = true;
                        break;
                    }
                }
                if (!isExistsNode)
                {
                    //不存在此节点则创建
                    XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                    subElement.InnerXml = innerText;
                    xmlNode.AppendChild(subElement);
                }
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            isSuccess = true;
        }
        catch (Exception ex)
        {
            log.Error(String.Format("{0}【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Reflection.MethodBase.GetCurrentMethod()), ex);
        }
        return isSuccess;
    }

    ///<summary>
    /// 依据匹配XPath表达式的第一个节点来创建或更新它的属性(如果属性存在则更新,不存在则创建)
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    ///<param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    ///<param name="value">属性值</param>
    ///<returns>成功返回true,失败返回false</returns>
    public static bool CreateOrUpdateXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName, string value)
    {
        bool isSuccess = false;
        bool isExistsAttribute = false;//标识属性是否存在
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //遍历xpath节点中的所有属性
                foreach (XmlAttribute attribute in xmlNode.Attributes)
                {
                    if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                    {
                        //节点中存在此属性则更新
                        attribute.Value = value;
                        isExistsAttribute = true;
                        break;
                    }
                }
                if (!isExistsAttribute)
                {
                    //节点中不存在此属性则创建
                    XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                    xmlAttribute.Value = value;
                    xmlNode.Attributes.Append(xmlAttribute);
                }
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            isSuccess = true;
        }
        catch (Exception ex)
        {
            log.Error(String.Format("{0}【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Reflection.MethodBase.GetCurrentMethod()), ex);
        }
        return isSuccess;
    }
    #endregion

    #region XML文档节点或属性的删除
    ///<summary>
    /// 删除匹配XPath表达式的第一个节点(节点中的子元素同时会被删除)
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    ///<returns>成功返回true,失败返回false</returns>
    public static bool DeleteXmlNodeByXPath(string xmlFileName, string xpath)
    {
        bool isSuccess = false;
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //删除节点
                xmlNode.ParentNode.RemoveChild(xmlNode);
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            isSuccess = true;
        }
        catch (Exception ex)
        {
            log.Error(String.Format("{0}【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Reflection.MethodBase.GetCurrentMethod()), ex);
        }
        return isSuccess;
    }

    ///<summary>
    /// 删除匹配XPath表达式的第一个节点中的匹配参数xmlAttributeName的属性
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    ///<param name="xmlAttributeName">要删除的xmlAttributeName的属性名称</param>
    ///<returns>成功返回true,失败返回false</returns>
    public static bool DeleteXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName)
    {
        bool isSuccess = false;
        bool isExistsAttribute = false;
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            XmlAttribute xmlAttribute = null;
            if (xmlNode != null)
            {
                //遍历xpath节点中的所有属性
                foreach (XmlAttribute attribute in xmlNode.Attributes)
                {
                    if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                    {
                        //节点中存在此属性
                        xmlAttribute = attribute;
                        isExistsAttribute = true;
                        break;
                    }
                }
                if (isExistsAttribute)
                {
                    //删除节点中的属性
                    xmlNode.Attributes.Remove(xmlAttribute);
                }
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            isSuccess = true;
        }
        catch (Exception ex)
        {
            log.Error(String.Format("{0}【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Reflection.MethodBase.GetCurrentMethod()), ex);
        }
        return isSuccess;
    }

    ///<summary>
    /// 删除匹配XPath表达式的第一个节点中的所有属性
    ///</summary>
    ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
    ///<param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    ///<returns>成功返回true,失败返回false</returns>
    public static bool DeleteAllXmlAttributeByXPath(string xmlFileName, string xpath)
    {
        bool isSuccess = false;
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //遍历xpath节点中的所有属性
                xmlNode.Attributes.RemoveAll();
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            isSuccess = true;
        }
        catch (Exception ex)
        {
            throw ex; //这里可以定义你自己的异常处理
        }
        return isSuccess;
    }
    #endregion

    /// <summary>
    /// XML转换给DataTable
    /// </summary>
    /// <param name="xmlFileName"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    public static DataTable GetDataTable(string xmlFileName, string xpath)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFileName); //加载XML文档

        //XmlNodeList xlist = doc.SelectNodes("//DataTable/Rows");
        XmlNodeList xlist = doc.SelectNodes(xpath);
        DataTable Dt = new DataTable();
        DataRow Dr;

        for (int i = 0; i < xlist.Count; i++)
        {
            Dr = Dt.NewRow();
            XmlElement xe = (XmlElement)xlist.Item(i);
            for (int j = 0; j < xe.Attributes.Count; j++)
            {
                if (!Dt.Columns.Contains("@" + xe.Attributes[j].Name))
                    Dt.Columns.Add("@" + xe.Attributes[j].Name);
                Dr["@" + xe.Attributes[j].Name] = xe.Attributes[j].Value;
            }
            for (int j = 0; j < xe.ChildNodes.Count; j++)
            {
                if (!Dt.Columns.Contains(xe.ChildNodes.Item(j).Name))
                    Dt.Columns.Add(xe.ChildNodes.Item(j).Name);
                Dr[xe.ChildNodes.Item(j).Name] = xe.ChildNodes.Item(j).InnerText;
            }
            Dt.Rows.Add(Dr);
        }
        return Dt;
    }

    /// <summary>
    /// 获取DataTable转化XML
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string GetXml(DataTable dt)
    {
        string strXml = @"<?xml version='1.0' encoding='UTF-8' ?><DataTable />";
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(strXml);
        XmlNode root = doc.SelectSingleNode("//DataTable");
        // 创建子节点     
        for (int j = 0; j < dt.Rows.Count; j++)
        {
            XmlElement xe = doc.CreateElement("Rows");
            XmlElement xeChild = null;
            if (!Object.Equals(dt, null))
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.StartsWith("@"))
                    {
                        string AttributeName = dt.Columns[i].ColumnName.Replace("@", "");
                        // 为该子节点设置属性     
                        xe.SetAttribute(AttributeName, dt.Rows[j][i].ToString());
                    }
                    else
                    {
                        xeChild = doc.CreateElement(dt.Columns[i].ColumnName);

                        try
                        {
                            xeChild.InnerXml = dt.Rows[j][i].ToString();
                        }
                        catch
                        {
                            xeChild.InnerText = dt.Rows[j][i].ToString();
                        }
                        xe.AppendChild(xeChild);
                    }
                }
            }
            // 保存子节点设置     
            root.AppendChild(xe);
        }
        return doc.InnerXml.ToString();
    }



    /// <summary>
    /// 组织XML
    /// </summary>
    /// <param name="root">根结点</param>
    /// <param name="tags">标签列表 ,隔开</param>
    /// <param name="values">内容</param>
    /// <returns></returns>
    public static string GetXml(string root, string tags, string[] values)
    {
        string inXml = @"<{0}>{1}</{0}>";
        StringBuilder sb = new StringBuilder();
        int count = 0;
        foreach (string item in tags.Split(','))
        {
            sb.AppendFormat("<{0}>{1}</{0}>", item.Trim(), values[count]);
            count++;
        }
        if (string.IsNullOrEmpty(root))
        {
            return sb.ToString();
        }
        else
        {
            return string.Format(inXml, root, sb.ToString());
        }
    }

    /// <summary>
    /// 组织XML
    /// </summary>
    /// <param name="root">根结点</param>
    /// <param name="args">内容</param>
    /// <returns></returns>
    public static string GetXml(string root, Dictionary<string, string> args)
    {
        string inXml = @"<{0}>{1}</{0}>";
        StringBuilder sb = new StringBuilder();
        foreach (var item in args)
        {
            sb.AppendFormat("<{0}>{1}</{0}>", item.Key, item.Value);
        }
        if (string.IsNullOrEmpty(root))
        {
            return sb.ToString();
        }
        else
        {
            return string.Format(inXml, root, sb.ToString());
        }
    }
    //
    /// <summary>
    /// 取XML中path处InnerXml
    /// </summary>
    /// <param name="strXml">XML字符串</param>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public static string GetValueFromXML(string strXml, string path)
    {
        string r = "";
        XmlDocument xd = new XmlDocument();
        xd.LoadXml(strXml);
        XmlNode xn = xd.SelectSingleNode(path);
        if (xn != null)
        {
            r = xn.InnerXml;
        }
        return r;
    }

    /// <summary>
    /// 取XML中path处子节点InnerXml，转为object[]
    /// </summary>
    /// <param name="strXml"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static object[] GetObjctArrayFromXML(string strXml, string path)
    {
        object[] objs;
        XmlDocument xd = new XmlDocument();
        xd.LoadXml(strXml);
        XmlNode root = xd;
        if (path != "") { root = xd.SelectSingleNode(path); }
        if (root != null)
        {
            objs = new object[root.ChildNodes.Count];
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                objs[i] = root.ChildNodes[i].InnerXml;
            }
        }
        else
        { objs = new object[0]; }
        return objs;
    }
    /// <summary>
    /// 取XML中path处子节点InnerXml，转为object[]
    /// </summary>
    /// <param name="strXml"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static object[] GetObjctArrayFromXML(XmlDocument XmlDoc, string path)
    {
        object[] objs;
        XmlNode root = XmlDoc;
        if (path != "") { root = XmlDoc.SelectSingleNode(path); }
        if (root != null)
        {
            objs = new object[root.ChildNodes.Count];
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                objs[i] = root.ChildNodes[i].InnerXml;
            }
        }
        else
        { objs = new object[0]; }
        return objs;
    }
    /// <summary>
    /// 取XML中path处数据，转为DataTable
    /// 表名为Path最后一层标签名
    /// 无行标签，仅限一行
    /// </summary>
    /// <param name="strXml"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DataTable GetDataRowFromXML(string strXml, string path)
    {
        DataTable r = new DataTable();
        XmlDocument xd = new XmlDocument();
        xd.LoadXml(strXml);
        XmlNode root = xd;
        if (path != "") { root = xd.SelectSingleNode(path); }
        if (root != null)
        {
            r.TableName = root.LocalName;
            r.Rows.Add(r.NewRow());
            foreach (XmlNode xn in root.ChildNodes)
            {
                r.Columns.Add(xn.LocalName, xn.InnerXml.GetType());
                r.Rows[0][xn.LocalName] = xn.InnerXml;
            }
        }
        return r;
    }
    /// <summary>
    /// 取XML中path处数据，并转为DataTable
    /// 有行标签，支持多行
    /// </summary>
    /// <param name="strXml"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DataTable GetDataTableFromXML(string strXml, string path)
    {
        DataTable r = new DataTable();
        XmlDocument xd = new XmlDocument();
        xd.LoadXml(strXml);
        XmlNode root = xd;
        if (path != "") { root = xd.SelectSingleNode(path); }
        if (root != null)
        {
            r.TableName = root.LocalName;
            foreach (XmlNode xn in root.ChildNodes)
            {
                r.Rows.Add(r.NewRow());
                foreach (XmlNode xn2 in xn.ChildNodes)
                {
                    if (!r.Columns.Contains(xn2.LocalName))
                    {
                        r.Columns.Add(xn2.LocalName, xn2.InnerXml.GetType());
                    }
                    r.Rows[r.Rows.Count - 1][xn2.LocalName] = xn2.InnerXml;
                }
            }
        }
        else
        {
            r.TableName = path.Split('/')[path.Split('/').Length - 1];
        }
        return r;
    }
    /// <summary>
    /// 取XML中path处数据，并转为DataTable
    /// 有行标签，支持多行,可去标题行
    /// </summary>
    /// <param name="strXml"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DataTable GetDataTableFromXML(string strXml, string path, int HeadRow)
    {
        DataTable r = new DataTable();
        XmlDocument xd = new XmlDocument();
        xd.LoadXml(strXml);
        XmlNode root = xd;
        if (path != "") { root = xd.SelectSingleNode(path); }
        if (root != null)
        {
            r.TableName = root.LocalName;
            for (int i = HeadRow; i < root.ChildNodes.Count; i++)
            {
                XmlNode xn = root.ChildNodes[i];
                r.Rows.Add(r.NewRow());
                foreach (XmlNode xn2 in xn.ChildNodes)
                {
                    if (!r.Columns.Contains(xn2.LocalName))
                    {
                        r.Columns.Add(xn2.LocalName, xn2.InnerXml.GetType());
                    }
                    r.Rows[r.Rows.Count - 1][xn2.LocalName] = xn2.InnerXml;
                }
            }
        }
        else
        {
            r.TableName = path.Split('/')[path.Split('/').Length - 1];
        }
        return r;
    }

    /// <summary>
    /// 取XML中path处数据，并转为DataTable
    /// 有行标签，支持多行
    /// 和行名相同的节点转为行，其他节点放弃
    /// </summary>
    /// <param name="strXml"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DataTable GetDataTableFromXML(string strXml, string path, string rowName)
    {
        DataTable r = new DataTable();
        XmlDocument xd = new XmlDocument();
        xd.LoadXml(strXml);
        XmlNode root = xd;
        if (path != "") { root = xd.SelectSingleNode(path); }
        if (root != null)
        {
            r.TableName = root.LocalName;
            foreach (XmlNode xn in root.ChildNodes)
            {
                if (xn.Name == rowName)
                {
                    r.Rows.Add(r.NewRow());
                    foreach (XmlNode xn2 in xn.ChildNodes)
                    {
                        if (!r.Columns.Contains(xn2.LocalName))
                        {
                            r.Columns.Add(xn2.LocalName, xn2.InnerXml.GetType());
                        }
                        r.Rows[r.Rows.Count - 1][xn2.LocalName] = xn2.InnerXml;
                    }
                }

            }
        }
        return r;
    }
}