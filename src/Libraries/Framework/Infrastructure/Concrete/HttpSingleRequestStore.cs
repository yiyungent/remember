using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Concrete
{
    /// <summary>
    /// Http一次请求一个线程内单例 存储器
    /// </summary>
    public class HttpSingleRequestStore
    {
        public static void SetData(string key, object value)
        {
            CallContext.SetData("Resolve:" + key, value);
        }

        public static object GetData(string key)
        {
            return CallContext.GetData("Resolve:" + key);
        }
    }
}
