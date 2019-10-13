using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Demo.Models
{
    public class CommentModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public int ttl { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public Page page { get; set; }
        public Hot[] replies { get; set; }
        public Hot[] hots { get; set; }
    }

    public class Page
    {
        public int num { get; set; }
        public int size { get; set; }
        public int count { get; set; }
        public int acount { get; set; }
    }

    public class Hot
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public int rpid { get; set; }
        /// <summary>
        /// AV号(视频号)
        /// </summary>
        public int oid { get; set; }
        /// <summary>
        /// 父级评论ID
        /// </summary>
        public int parent { get; set; }
        /// <summary>
        /// 10位时间戳
        /// </summary>
        public long ctime { get; set; }
        public int like { get; set; }
        public Content content { get; set; }
        public Hot[] replies { get; set; }
    }

    public class Content
    {
        public string message { get; set; }
        public int plat { get; set; }
    }
}