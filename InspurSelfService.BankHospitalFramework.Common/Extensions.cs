﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace InspurSelfService.BankHospitalFramework.Common
{
    public static class Extensions
    {
        /// <summary>
        /// 数据库类型转C#类型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string DataCellToString(this object o)
        {
            if (o == DBNull.Value || o == null)
                return "";
            else
                return o.ToString().Trim();
        }

        /// <summary>
        /// 数据库类型转C#类型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int DataCellToInt(this object o)
        {
            int i = -1;
            if (o == DBNull.Value || o == null)
                return i;
            if (int.TryParse(o.ToString().Trim(), out i))
                return i;
            return -1;
        }

        /// <summary>
        /// Convert.ChangeType的替代品
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(this object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value != null)
                {
                    NullableConverter nullableConverter = new NullableConverter(conversionType);
                    conversionType = nullableConverter.UnderlyingType;
                }
                else
                    return null;
            }
            return Convert.ChangeType(value, conversionType);
        }
    }
}
