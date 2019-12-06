using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.MySql;
using System.Collections.Generic;
using WebUI.Infrastructure;
using System.Diagnostics;
using Hangfire.Dashboard;
using Hangfire.Common;

[assembly: OwinStartup(typeof(WebUI.Startup))]

namespace WebUI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard("/admin/cornJob", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
                DashboardTitle = "计划任务",
            });

            // Let's also create a sample background job
            BackgroundJob.Enqueue(() => Debug.WriteLine("Hello world from Hangfire!"));

            // ...other configuration logic
        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new MySqlStorage("RemDbContext"));

            yield return new BackgroundJobServer();
        }
    }
}
