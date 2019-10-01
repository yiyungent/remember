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

        // TODO: 改为从数据库设置表中获取，可更改
        public static int RememberMeDayCount { get; set; } = 7;
    }
}
