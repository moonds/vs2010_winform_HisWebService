/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者：
 * 文件名称：LogUtility.cs
 * 描    述：日志处理工具类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using log4net.Appender;


public class LogUtility
{
    #region 记录日志函数
    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="log">日志</param>
    /// <returns></returns>
    public void WritePageErrorLog(string log)
    {
        string logFolder = "D:\\AdapterError\\";
        //try
        //{
        //    logFolder += TerminalIP;
        //}
        //catch (Exception)
        //{
        //    logFolder += "无IP错误日志";
        //}
        //finally
        //{
        DateTime date = DateTime.Now;
        string dateMonth = date.ToString("yyyy-MM");
        string dateDay = date.ToString("yyyy-MM-dd");

        WriteLogFile(log, logFolder, String.Format("{0}", dateMonth), dateDay + ".txt");
        //}
    }

    /// <summary>
    /// 写入日志文件
    /// </summary>
    /// <param name="input">文件内容</param>
    /// <param name="folder">文件夹</param>
    /// <param name="yearMonth">【子文件夹名】</param>
    /// <param name="file">文件名【日志文件】</param>
    private void WriteLogFile(string input, string folder, string yearMonth, string file)
    {
        ///指定日志文件的目录
        string fname = String.Format("{0}\\{1}\\{2}", folder, yearMonth, file);
        //文件夹
        string sPath = String.Format("{0}\\{1}", folder, yearMonth);
        if (!Directory.Exists(sPath))
        {
            Directory.CreateDirectory(sPath);
        }

        StreamWriter sw = File.AppendText(fname);

        sw.WriteLine(input);
        sw.Flush();
        sw.Close();
    }
    /// <summary>
    /// HIS错误日志
    /// </summary>
    /// <param name="log"></param>
    /// <param name="strErrorMessage"></param>
    /// <param name="terminalIp"></param>
    public void HisLogError(log4net.ILog log, string strErrorMessage, string terminalIp)
    {
        var repository = log4net.LogManager.GetRepository().GetAppenders();
        var targetApder = repository.First(p => p.Name == "AdapterHisErrorAppender") as RollingFileAppender;
        targetApder.File = "D:/LogTrade/CallHisTradeLog/" + terminalIp + "/Error/" + DateTime.Now.Date.ToString("yyyyMMdd") + ".log";
        targetApder.ActivateOptions();
        log.Error(strErrorMessage);
    }
    /// <summary>
    /// HIS交易日志
    /// </summary>
    /// <param name="log"></param>
    /// <param name="strErrorMessage"></param>
    /// <param name="terminalIp"></param>
    public void HisLogInfo(log4net.ILog log, string strErrorMessage, string terminalIp)
    {
        var repository = log4net.LogManager.GetRepository().GetAppenders();
        var targetApder1 = repository.First(p => p.Name == "AdapterHisInfoAppender") as RollingFileAppender;
        targetApder1.File = "D:/LogTrade/CallHisTradeLog/" + terminalIp + "/Info/" + DateTime.Now.Date.ToString("yyyyMMdd") + ".log";
        targetApder1.ActivateOptions();
        log.Info(strErrorMessage);
    }
    public void HisLogInfoFormat(log4net.ILog log, string strMessage, string adapterTrace, string methodName, string callMethodName, object result, string terminalIp)
    {
        var repository = log4net.LogManager.GetRepository().GetAppenders();
        var targetApder1 = repository.First(p => p.Name == "AdapterHisInfoAppender") as RollingFileAppender;
        targetApder1.File = "D:/LogTrade/CallHisTradeLog/" + terminalIp + "/Info/" + DateTime.Now.Date.ToString("yyyyMMdd") + ".log";
        targetApder1.ActivateOptions();
        log.InfoFormat("【{0}】 【{1}->{2}】 Result={3}", adapterTrace, methodName, callMethodName, result);
    }
    /// <summary>
    /// 银行错误日志
    /// </summary>
    /// <param name="log"></param>
    /// <param name="strErrorMessage"></param>
    /// <param name="terminalIp"></param>
    public void BankLogError(log4net.ILog log, string strErrorMessage, string terminalIp)
    {
        var repository = log4net.LogManager.GetRepository().GetAppenders();
        var targetApder = repository.First(p => p.Name == "AdapterHisErrorAppender") as RollingFileAppender;
        targetApder.File = "D:/LogTrade/CallBankTradeLog/" + terminalIp + "/Error/" + DateTime.Now.Date.ToString("yyyyMMdd") + ".log";
        targetApder.ActivateOptions();
        log.Error(strErrorMessage);
    }
    /// <summary>
    /// 银行交易日志
    /// </summary>
    /// <param name="log"></param>
    /// <param name="strErrorMessage"></param>
    /// <param name="terminalIp"></param>
    public void BankLogInfo(log4net.ILog log, string strErrorMessage, string terminalIp)
    {
        var repository = log4net.LogManager.GetRepository().GetAppenders();
        var targetApder1 = repository.First(p => p.Name == "AdapterHisInfoAppender") as RollingFileAppender;
        targetApder1.File = "D:/LogTrade/CallBankTradeLog/" + terminalIp + "/Info/" + DateTime.Now.Date.ToString("yyyyMMdd") + ".log";
        targetApder1.ActivateOptions();
        log.Info(strErrorMessage);
    }
    #endregion

    public void WriteHisLog(string TerminalIP, string FunName, string TradeName, string Params, string Socket, string LogValue)
    {
        string logFolder = "D:\\LogTrade\\CallHisTradeLog";
        DateTime date = DateTime.Now;
        string dateMonth = TerminalIP + "\\" + date.ToString("yyyy-MM");
        string dateDay = date.ToString("yyyy-MM-dd");
        if (string.IsNullOrEmpty(LogValue))
        {
            string log = string.Format("TerminalIP=[{5}],TradeTime=[{4}],FunName=[{0}],TradeName=[{1}],Params=[{2}],Socket=[{3}]", FunName, TradeName, Params, Socket,
                DateTime.Now.ToString("HH:mm:ss"), TerminalIP);
            WriteLogFile(log, logFolder, dateMonth, "CallBank" + dateDay + ".log");
        }
        else
        {
            string log = string.Format("TerminalIP=[{4}],TradeTime=[{0}],FunName=[{1}],TradeName=[{2}],Log=[{3}]", DateTime.Now.ToString("HH:mm:ss"), FunName, TradeName, LogValue, TerminalIP);
            WriteLogFile(log, logFolder, dateMonth, "CallBank" + dateDay + ".log");
        }
    }

    
}
