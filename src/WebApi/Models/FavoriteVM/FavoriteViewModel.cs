using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.FavoriteVM
{
    public class FavoriteViewModel
    {
        /// <summary>
        /// 收藏夹ID
        /// </summary>
        public int ID { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public long CreateTime { get; set; }

        public CreatorViewModel Creator { get; set; }

        public IList<CourseBox> CourseBoxs { get; set; }

        /// <summary>
        /// 收藏夹的封面图由内容决定，若无内容则默认图，否则选择此收藏夹最新收藏的内容的封面图为此收藏夹的封面图
        /// </summary>
        public string PicUrl { get; set; }


        public sealed class CourseBox
        {
            /// <summary>
            /// 课程ID
            /// </summary>
            public int ID { get; set; }

            public string Name { get; set; }

            public string PicUrl { get; set; }

            public CreatorViewModel Creator { get; set; }

            public int LearnNum { get; set; }

            /// <summary>
            /// 收藏此课程的时间
            /// </summary>
            public long FavTime { get; set; }
        }

        public sealed class CreatorViewModel
        {
            public int ID { get; set; }

            public string UserName { get; set; }
        }

    }
}