using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace Framework.Common
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public partial class CommonHelper
    {
        #region 确保邮箱
        /// <summary>
        /// Ensures the subscriber email or throw.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            string output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new Exception("Email is not valid.");
            }

            return output;
        } 
        #endregion

        #region 验证邮箱是否合理
        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        } 
        #endregion

        #region 随机数字码
        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        } 
        #endregion

        #region 随机整数
        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }
        #endregion

        #region 限制字符串长度
        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        } 
        #endregion

        #region 确保字符串中只含有数字
        /// <summary>
        /// Ensures that a string only contains numeric values
        /// <para>过滤 str, 返回的字符串中只含有数字</para>
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        } 
        #endregion

        #region 保证一个字符串不为 null
        /// <summary>
        /// Ensure that a string is not null
        /// <para>当 str 为 null， 则返回 string.Empty, 否则原样返回</para>
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return string.Empty;

            return str;
        } 
        #endregion

        #region 给定的参数字符串数组中是否存在有满足 IsNullOrEmpty 的项
        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// <para>遍历字符串数组，只要其中有一项IsNullOrEmpty,则返回 true</para>
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            bool result = false;
            Array.ForEach(stringsToValidate, str =>
            {
                if (string.IsNullOrEmpty(str)) result = true;
            });
            return result;
        } 
        #endregion

        #region 两数组是否相等
        /// <summary>
        /// Compare two arrasy
        /// <para>两数组是否相等</para>
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>Result</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        } 
        #endregion

        private static AspNetHostingPermissionLevel? _trustLevel;
        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                //set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in new[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal
                            })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; //we've set the highest permission we can
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }
            return _trustLevel.Value;
        }
    }
}
