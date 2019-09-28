using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：日志信息
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class LogInfo : BaseEntity<LogInfo>
    {
        /// <summary>
        /// 访问者的用户ID
        /// 如果未登录，则为 0
        /// </summary>
        [Property]
        public int AccessUserId { get; set; }

        /// <summary>
        /// 访问者的IP
        /// </summary>
        [Property(Length = 50)]
        public string AccessIp { get; set; }

        /// <summary>
        /// 解析 UserAgent json字符串
        /// {
        ///      ua: "",
        ///      browser: {
        ///          name: "",
        ///          version: ""
        ///      },
        ///      engine: {
        ///          name: "",
        ///          version: ""
        ///      },
        ///      os: {
        ///          name: "",
        ///          version: ""
        ///      },
        ///      device: {
        ///          model: "",
        ///          type: "",
        ///          vendor: ""
        ///      },
        ///      cpu: {
        ///          architecture: ""
        ///      }
        //} }
        /// </summary>
        public string UserAgent { get; set; }

        #region UserAgent 解析出的数据

        [Property(Length = 100)]
        public string Browser { get; set; }

        [Property(Length = 100)]
        public string BrowserEngine { get; set; }

        [Property(Length = 100)]
        public string OS { get; set; }

        [Property(Length = 100)]
        public string Device { get; set; }

        [Property(Length = 100)]
        public string Cpu { get; set; }

        #endregion

        /// <summary>
        /// 访问时间：进入网页，加载完的时间
        /// </summary>
        [Property]
        public DateTime AccessTime { get; set; }

        /// <summary>
        /// 跳出网页时间
        /// </summary>
        [Property]
        public DateTime JumpTime { get; set; }

        /// <summary>
        /// 在页面的持续时间 = JumpTime - AccessTime
        /// 总秒数
        /// </summary>
        [Property]
        public long Duration { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        [Property(Length = 500)]
        public string AccessUrl { get; set; }

        /// <summary>
        /// 来源URL
        /// </summary>
        [Property(Length = 500)]
        public string RefererUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property]
        public DateTime CreateTime { get; set; }

    }
}
