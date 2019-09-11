using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// JWT 帮助器
    /// </summary>
    public class JwtHelper
    {
        public static readonly string Secret = System.Configuration.ConfigurationManager.AppSettings["JwtSecret"];

        #region 加密
        public static string Encode(object payload, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Secret;
            }
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            try
            {
                return encoder.Encode(payload, key);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 解密
        public static T Decode<T>(string token, out bool verifyPass, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Secret;
            }
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            try
            {
                verifyPass = true;

                return decoder.DecodeToObject<T>(token, key, true);
            }
            catch (Exception)
            {
                verifyPass = false;

                return default(T);
            }
        }
        #endregion

    }
}
