using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CourseBoxVM
{
    public class CourseBoxViewModel
    {
        /// <summary>
        /// 课程ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 课程名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public CreatorViewModel Creator { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        public long LastUpdateTime { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 有效日期 - 开始时间
        /// </summary>
        public long StartTime { get; set; }

        /// <summary>
        /// 有效日期 - 结束时间
        /// </summary>
        public long EndTime { get; set; }

        /// <summary>
        /// 有效学习天数
        /// </summary>
        public int LearnDay { get; set; }

        /// <summary>
        /// 此学习者在此课程总学习时间: 花费时间
        /// 毫秒
        /// <para>注意：不一定是学习者在此课程上的所有课件的进度时间，因为课程存在反复看，反复看时间也要计算在内</para>
        /// </summary>
        public long SpendTime { get; set; }

        /// <summary>
        /// 统计信息
        /// </summary>
        public StatViewModel Stat { get; set; }

        /// <summary>
        /// 课件列表
        /// </summary>
        public IList<VideoInfoViewModel> VideoInfos { get; set; }

        #region 需登录

        /// <summary>
        /// 最新播放的视频课件
        /// </summary>
        public VideoInfoViewModel LastPlayVideoInfo { get; set; }

        /// <summary>
        /// 加入学习的时间
        /// js时间戳(毫秒)
        /// </summary>
        public long JoinTime { get; set; }

        #endregion

        /// <summary>
        /// 视频课件
        /// </summary>
        public sealed class VideoInfoViewModel
        {
            /// <summary>
            /// 视频课件ID
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// 此课件在课程内的序号
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// 课件标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 视频播放url地址
            /// </summary>
            public string PlayUrl { get; set; }

            #region 需登录
            /// <summary>
            /// 最新播放位置
            /// </summary>
            public long LastPlayAt { get; set; }

            /// <summary>
            /// 学习进度
            /// </summary>
            public long ProgressAt { get; set; }
            #endregion
        }

        public sealed class StatViewModel
        {
            public int LikeNum { get; set; }
            public int DislikeNum { get; set; }
            public int Coin { get; set; }
            public int FavNum { get; set; }
            public int ShareNum { get; set; }
            public int CommentNum { get; set; }
            public int ViewNum { get; set; }
        }

        public sealed class CreatorViewModel
        {
            public int ID { get; set; }

            public string UserName { get; set; }

            public string Desc { get; set; }

            public string Avatar { get; set; }

            public int FansNum { get; set; }
        }
    }
}