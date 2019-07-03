using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Config
{
    public class AppConfig
    {
        public static string LoginAccountSessionKey { get; set; } = "LoginUserInfo";

        public static string TokenCookieKey { get; set; } = "Token";

        public static int RememberMeDayCount { get; set; } = 7;
    }
}
