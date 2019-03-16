using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Remember.Common
{
    public class StringHelper
    {
        #region MD5加密
        public static string EncodeMD5(string crude)
        {
            if (crude == null) { return string.Empty; }
            // 实例化MD5构造器
            MD5 md5 = MD5.Create();
            // MD5加密
            byte[] target = md5.ComputeHash(Encoding.UTF8.GetBytes(crude));
            // 字节数组 ---> 字符串
            StringBuilder sb = new StringBuilder(null);
            for (int i = 0; i < target.Length; i++)
            {
                sb.Append(target[i].ToString("X2"));
            }
            return sb.ToString();
        } 
        #endregion
    }
}
