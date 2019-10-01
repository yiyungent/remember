using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common
{
    public class HttpTemplate
    {
        public T GetUseGet<T>(string url)
        {
            T rtnResult = default(T);
            string jsonStr = HttpAide.HttpGet(url: url);
            rtnResult = JsonConvert.DeserializeObject<T>(jsonStr);

            return rtnResult;
        }

        public T GetUsePost<T>(string url, string postDataStr)
        {
            T rtnResult = default(T);
            string jsonStr = HttpAide.HttpPost(url: url, postDataStr: postDataStr);
            rtnResult = JsonConvert.DeserializeObject<T>(jsonStr);

            return rtnResult;
        }
    }
}
