/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：BankTestData.cs
 * 描    述：银行测试数据
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace InspurSelfService.BankHospitalFramework.Adapter
{
    public class BankTestData
    {
        public static string GetValue(string _inStr, string _tradeName)
        {
            string outStr = "";

            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "Bin/BankTestData.xml");
                XmlNodeList nodes = xmldoc.SelectNodes("root/ROOT");

                string tradeCode = null;
                switch (_tradeName)
                {
                    case "Signin":
                        tradeCode = "2001";
                        break;
                    case "Bind":
                        tradeCode = "2002";
                        break;
                    case "Unbind":
                        tradeCode = "2003";
                        break;
                    case "Recharge":
                        tradeCode = "2006";
                        break;
                    case "RechargeReverse":
                        tradeCode = "2005";
                        break;
                    case "Refund":
                        tradeCode = "2006";
                        break;
                    case "QueryOriginalTradeRecord":
                        tradeCode = null;
                        break;
                    case "QueryBankCardAccountInfo":
                        tradeCode = "2010";
                        break;
                    case "BankBalance":
                        tradeCode = "2023";
                        break;
                    case "CheckBankPwd":
                        tradeCode = "2023";
                        break;
                    case "BankCardClassDownload":
                        tradeCode = null;
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < nodes.Count; i++)
                {
                    string code = nodes[i].SelectSingleNode("HEAD/TransCode").InnerText;
                    if (code == tradeCode)
                    {
                        outStr = nodes[i].OuterXml;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                outStr = String.Format("error:{0}", ex.Message);
            }

            return outStr;
        }
    }
}
