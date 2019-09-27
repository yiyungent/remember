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
        [Property]
        public string AccessIp { get; set; }

        /// <summary>
        /// 访客浏览器信息
        /// </summary>
        [Property]
        public string UserAgent { get; set; }

        /// <summary>
        /// 访问时间：进入网页，加载完的时间
        /// </summary>
        [Property]
        public string AccessTime { get; set; }

        /// <summary>
        /// 跳出网页时间
        /// </summary>
        [Property]
        public string JumpTime { get; set; }

        /// <summary>
        /// 在页面的持续时间 = JumpTime - AccessTime
        /// 总秒数
        /// </summary>
        [Property]
        public long Duration { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        [Property]
        public string AccessUrl { get; set; }

        /// <summary>
        /// 来源URL
        /// </summary>
        [Property]
        public string RefererUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property]
        public DateTime CreateTime { get; set; }

    }
}
