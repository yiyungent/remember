using JWT;
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

        public static string Encode(Dictionary<string, object> payload, string key)
        {
            return "";
        }

        public static Dictionary<string, object> Decode(string token, string key)
        {
            string secret = key;
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                return decoder.DecodeToObject<Dictionary<string, object>>(token, secret, true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
