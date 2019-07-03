using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.RequestResult
{
    public class CommonResult
    {
        /// <summary>
        /// <para> < 0 为 错误相关</para> 
        /// <para> > 0 为 正确相关，1 为一般成功</para>
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
