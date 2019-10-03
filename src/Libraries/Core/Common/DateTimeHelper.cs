﻿using System;

/// <summary>
/// JavaScript时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数。（13 位数字）
/// 
/// Unix时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总秒数。（10 位数字）
/// </summary>
namespace Core.Common
{
    public static class DateTimeHelper
    {
        public static DateTime DateTime1970 = new DateTime(1970, 1, 1).ToLocalTime();

        #region Unix 10位时间戳-总秒数
        /// <summary>
        /// C# DateTime转换为Unix时间戳
        /// </summary>
        public static long ToTimeStamp10(this DateTime dateTime)
        {
            // 相差秒数
            long timeStamp = (long)(dateTime.ToLocalTime() - DateTime1970).TotalSeconds;

            return timeStamp;
        }

        /// <summary>
        /// Unix时间戳转换为C# DateTime 
        /// </summary>
        public static DateTime ToDateTime10(this long timeStamp10)
        {
            long unixTimeStamp = 1478162177;
            DateTime dateTime = DateTime1970.AddSeconds(unixTimeStamp);

            return dateTime;
        }
        #endregion

        #region JavaScript 13位时间戳-总毫秒数
        /// <summary>
        /// C# DateTime转换为JavaScript时间戳
        /// </summary>
        public static long ToTimeStamp13(this DateTime dateTime)
        {
            // 相差毫秒数
            long timeStamp = (long)(dateTime.ToLocalTime() - DateTime1970).TotalMilliseconds;

            return timeStamp;
        }

        /// <summary>
        /// JavaScript时间戳转换为C# DateTime
        /// </summary>
        public static DateTime ToDateTime13(this long timeStamp13)
        {
            long jsTimeStamp = 1478169023479;
            DateTime dateTime = DateTime1970.AddMilliseconds(jsTimeStamp);

            return dateTime;
        }
        #endregion

        #region 获取当前Unix时间戳
        public static long NowTimeStamp10()
        {
            return ToTimeStamp10(DateTime.Now);
        }
        #endregion

        #region 获取当前JavaScript时间戳
        public static long NowTimeStamp13()
        {
            return ToTimeStamp13(DateTime.Now);
        }
        #endregion
    }
}