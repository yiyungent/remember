using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CourseBoxVM
{
    public class HistoryDataModel
    {
        public IList<HistoryDataModelItem> List { get; set; }

    }

    public class HistoryDataModelItem
    {
        /// <summary>
        /// 课程盒ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 课程名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 课程描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// Unix时间戳
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public CreatorViewModel Creator { get; set; }

        /// <summary>
        /// 统计
        /// </summary>
        public StatViewModel Stat { get; set; }

        /// <summary>
        /// 课程总内容（课件）数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 最新进度
        /// </summary>
        public ProgressViewModel Progress { get; set; }

        public sealed class CreatorViewModel
        {
            public int ID { get; set; }

            public string Name { get; set; }

            public string Avatar { get; set; }
        }

        /// <summary>
        /// 最新进度
        /// </summary>
        public sealed class ProgressViewModel
        {
            /// <summary>
            /// 课程内容（课件）ID
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// 课件在课程内的序号
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// 课件标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 课件总的持续时间（视频则为总时间）
            /// 毫秒
            /// </summary>
            public long Duration { get; set; }

            /// <summary>
            /// 在此课件的最新播放位置
            /// 视频：毫秒
            /// </summary>
            public long LastPlayAt { get; set; }

            /// <summary>
            /// 在此课件的进度
            /// 毫秒
            /// </summary>
            public long ProgressAt { get; set; }

            /// <summary>
            /// 此课程的最新访问时间
            /// </summary>
            public long LastAccessTime { get; set; }
        }
    }
}