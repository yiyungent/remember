using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Core;
using Domain;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private Container _container;

        protected void Application_Start()
        {
            #region 容器初始化
            try
            {
                // 获取 web.config 配置
                IConfigurationSource source = (IConfigurationSource)System.Configuration.ConfigurationManager.GetSection("ActiveRecord");
                ActiveRecordStarter.Initialize(typeof(UserInfo).Assembly, source);
                _container = Container.Instance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
