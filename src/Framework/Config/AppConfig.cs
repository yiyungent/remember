using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Config
{
    public class AppConfig
    {
        public static readonly string LoginAccountSessionKey = "LoginUserInfo";

        public static readonly string JwtName = System.Configuration.ConfigurationManager.AppSettings["JwtName"];

        public static int RememberMeDayCount { get; set; } = 7;
    }
}
