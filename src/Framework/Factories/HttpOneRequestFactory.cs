using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

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
                instance = DependencyResolverProvider.Get(type);
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
