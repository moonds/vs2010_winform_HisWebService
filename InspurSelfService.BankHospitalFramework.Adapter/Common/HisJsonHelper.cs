/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：HisJsonHelper.cs
 * 描    述：JSON 数据转换接口
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;
using System.Net;


public class HisJsonHelper
{
    public static string GetJsonData(string Url)
    {
        Encoding enc;
        StreamReader sr;
        string json;
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            enc = Encoding.GetEncoding("utf-8"); // 如果是乱码就改成 utf-8 / GB2312
            sr = new StreamReader(resStream, enc); //命名空间:System.IO。 StreamReader 类实现一个 TextReader (TextReader类，表示可读取连续字符系列的读取器)，使其以一种特定的编码从字节流中读取字符。
            json = sr.ReadToEnd(); //输出(HTML代码)，ContentHtml为Multiline模式的TextBox控件
            resStream.Close();
            sr.Close();

            return json;
        }
        catch (WebException WE)
        {
            if (WE.Response != null)
            {
                Stream resStream = (WE.Response as WebResponse).GetResponseStream();
                enc = Encoding.GetEncoding("utf-8"); // 如果是乱码就改成 utf-8 / GB2312
                sr = new StreamReader(resStream, enc); //命名空间:System.IO。 StreamReader 类实现一个 TextReader (TextReader类，表示可读取连续字符系列的读取器)，使其以一种特定的编码从字节流中读取字符。
                json = sr.ReadToEnd(); //输出(HTML代码)，ContentHtml为Multiline模式的TextBox控件
                resStream.Close();
                sr.Close();
                return System.Web.HttpUtility.UrlDecode(json);
            }
            return WE.Message;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }

    /// <summary>
    /// 根据His提供jsonText转换DataTable
    /// </summary>
    /// <param name="jsonText">输入参数：json文本</param>
    /// <param name="Result">输出参数</param>
    /// <param name="ErrorMsg">输出参数</param>
    /// <param name="CRC32">输出参数</param>
    /// <param name="DateTime">输出参数</param>
    /// <returns></returns>
    public static DataTable GetTableByHisJsonText(string jsonText, out string Result, out string ErrorMsg, out string CRC32, out string DateTime)
    {
        DataTable ret = null;

        JsonHelper jh = new JsonHelper();
        Dictionary<string, object> objs = (Dictionary<string, object>)jh.GetObjectByJsonString(jsonText);

        Result = objs["Result"].ToString();
        ErrorMsg = objs["ErrorMsg"].ToString();
        CRC32 = objs["CRC32"].ToString();
        DateTime = objs["DateTime"].ToString();

        string data = null;
        foreach (var item in objs)
        {
            if (item.Key != "Result" && item.Key != "ErrorMsg" && item.Key != "CRC32" && item.Key != "DateTime" && item.Key != "TableName")
            {
                string str = jh.GetJsonStringByObject(item.Value).Replace("\"", "'");
                data += str + ",";
            }
        }

        if (!String.IsNullOrEmpty(data))
        {
            data = data.Substring(0, data.Length - 1);
            data = "{'DataTable':[" + data + "]}";
            ret = jh.GetTableByJsonString(data);
        }

        return ret;
    }

    /// <summary>
    /// 根据His提供jsonText转换DataTable
    /// </summary>
    /// <param name="jsonText">输入参数：json文本</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public static DataTable GetTableByHisJsonText(string jsonText, string tableName)
    {
        DataTable ret = null;

        JsonHelper jh = new JsonHelper();
        Dictionary<string, object> objs = (Dictionary<string, object>)jh.GetObjectByJsonString(jsonText);

        string dtStr = "{'" + tableName + "':" + jh.GetJsonStringByObject(objs[tableName]) + "}";
        ret = jh.GetTableByJsonString(dtStr, tableName);

        return ret;
    }


    ///// <summary>
    ///// 根据His提供jsonText转换DataTable
    ///// </summary>
    //public static DataTable GetTableByHisJsonText(string jsonText, out string Result, out string ErrorMsg, out string CRC32, out string DateTime)
    //{
    //    JObject data = (JObject)JsonConvert.DeserializeObject(jsonText);
    //    Result = data["Result"].ToString();
    //    data.Remove("Result");
    //    ErrorMsg = data["ErrorMsg"].ToString();
    //    data.Remove("ErrorMsg");
    //    CRC32 = data["CRC32"].ToString();
    //    data.Remove("CRC32");
    //    DateTime = data["DateTime"].ToString();
    //    data.Remove("DateTime");

    //    JToken[] obj = data["TableName"].ToArray();
    //    data.Remove("TableName");
    //    string str = data.ToString().Replace("\r\n", "").Replace(" ", "");
    //    str = str.Remove(str.Length - 1) + "]";
    //    str = "[" + str.Remove(0, 1);
    //    str = Regex.Replace(str, "\\\"\\d+\\\"([ ]?):", "");

    //    DataTable dt = JsonConvert.DeserializeObject<DataTable>(str);

    //    return dt;
    //}
}
