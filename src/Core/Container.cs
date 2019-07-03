using Castle.MicroKernel;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using System;

namespace Core
{
    /// <summary>
    /// IOC容器——控制反转
    /// </summary>
    public sealed class Container
    {
        private XmlInterpreter interpreter; // 拦截器
        private WindsorContainer windsor; // IOC容器
        private static readonly Container instance = new Container();
        private IKernel kernel;

        public static Container Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 私有化构造方法，实现单例模式
        /// </summary>
        private Container()
        {
            try
            {
                interpreter = new XmlInterpreter("Config/Service.xml");
                windsor = new WindsorContainer(interpreter);
                kernel = windsor.Kernel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Resolve<T>()
        {
            return kernel.Resolve<T>();
        }

        public T Resolve<T>(string key)
        {
            return kernel.Resolve<T>(key);
        }

        public void Dispose()
        {
            kernel.Dispose();
        }
    }
}
