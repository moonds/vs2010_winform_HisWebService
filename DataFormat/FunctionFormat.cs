using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFormat
{
    public class FunctionFormat
    {
        public string FormatBySymbol(string inString, char symbol, out bool isSuccess)
        {
            if (string.IsNullOrEmpty(inString))
            {
                isSuccess = false;
                return "";
            }
            try
            {
                string[] arrayStr = inString.Split(symbol);
                string finalString = "";
                for (int i = 0; i < arrayStr.Length; i++)
                {
                    finalString += arrayStr[i];
                    if (i != (arrayStr.Length - 1))
                        finalString += "\r\n";
                }
                isSuccess = true;
                return finalString;
            }
            catch (Exception)
            {
                isSuccess = false;
                return "";
            }

        }
    }
}
