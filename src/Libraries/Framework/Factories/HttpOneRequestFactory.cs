using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework.Factories
{
    /// <summary>
    /// Http请求-线程内唯一
    /// </summary>
    public static class HttpOneRequestFactory
    {
        public static object Get(Type type)
        {
            object instance = CallContext.GetData("Resolve:" + type.ToString());
            if (instance == null)
            {
                // 先试图从框架本身提供的DI中拿
                instance = DependencyResolverProvider.Get(type);
                if (instance == null)
                {
                    // 再试图从 框架外部设置的DI中拿
                    instance = DependencyResolver.Current.GetService(type);
                }

                CallContext.SetData("Resolve:" + type.ToString(), instance);
            }

            return instance;
        }

        public static T Get<T>()
        {
            Type type = typeof(T);

            return (T)Get(type);
        }
    }
}
