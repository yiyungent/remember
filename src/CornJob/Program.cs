using Quartz;
using Quartz.Impl;
using System;
using Topshelf;

namespace CornJob
{
    class Program
    {
        static void Main(string[] args)
        {
            // 配置和运行宿主服务
            HostFactory.Run(x =>
            {
                // 指定服务类型。这里设置为 TestSchedule
                x.Service<SearchSchedule>(s =>
                {
                    // 通过 new TestSchedule() 构建一个服务实例 
                    s.ConstructUsing(name => new SearchSchedule());
                    // 当服务启动后执行什么
                    s.WhenStarted(tc => tc.Start());
                    // 当服务停止后执行什么
                    s.WhenStopped(tc => tc.Stop());
                });

                // 服务用本地系统账号来运行
                x.RunAsLocalSystem();

                // 服务描述信息
                x.SetDescription("Remember 搜索相关服务");
                // 服务显示名称
                x.SetDisplayName("Remember 搜索");
                // 服务名称
                x.SetServiceName("RememberSearch");

            });
        }
    }
}
