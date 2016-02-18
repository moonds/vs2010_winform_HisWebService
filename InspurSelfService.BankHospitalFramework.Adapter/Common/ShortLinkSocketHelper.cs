/************************************************************************/
/* Copyright (c) 2014, 银医产品研发部      All rights reserved.
 * 项目名称：银医V3.0
 * 作    者:
 * 文件名称：ShortLinkSocketHelper.cs
 * 描    述：短连接Socket 通信工具类
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class ShortLinkSocketHelper
{
    public static string GetValue(string IP, string Port, string EncodingType, string DecodingType, string SingleBuffSize, string SendTimeout, string ReceiveTimeout, string _inStr)
    {
        string outStr = "";

        try
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(IP), Convert.ToInt32(Port));
            Socket socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);
            socket.SendTimeout = Convert.ToInt32(SendTimeout);
            socket.ReceiveTimeout = Convert.ToInt32(ReceiveTimeout);

            Byte[] bytesSent = Encoding.GetEncoding(EncodingType).GetBytes(_inStr);
            socket.Send(bytesSent, bytesSent.Length, 0);

            string recvStr = "";
            byte[] recvBytes = new byte[1024 * 8];
            int bytes = 0;
            while (true)
            {
                bytes = socket.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息 　　　　     
                //将读取的字节转换成字符串
                recvStr += Encoding.GetEncoding(DecodingType).GetString(recvBytes, 0, bytes);
                if (bytes > 0)
                    break;
            }
            outStr = recvStr;
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception ex)
        {
            outStr = String.Format("error:{0}", ex.Message);
        }

        return outStr;
    }
}
