using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor;
using Castle.MicroKernel;

namespace Remember.Core
{
    /// <summary>
    /// IOC容器——控制反转
    /// </summary>
    public sealed class Container
    {
        private XmlInterpreter _interpreter; // 拦截器
        private WindsorContainer _windsor; // IOC容器
        private static readonly Container _instance = new Container();
        private IKernel _kernel;

        public static Container Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 私有化构造方法，实现单例模式
        /// </summary>
        private Container()
        {
            try
            {
                // 拦截器
                _interpreter = new XmlInterpreter("Config/Service.xml");
                _windsor = new WindsorContainer(_interpreter);
                _kernel = _windsor.Kernel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Resolve<T>()
        {
            return _kernel.Resolve<T>();
        }

        public T Resolve<T>(string key)
        {
            return _kernel.Resolve<T>(key);
        }

        public void Dispose()
        {
            _kernel.Dispose();
        }
    }
}
