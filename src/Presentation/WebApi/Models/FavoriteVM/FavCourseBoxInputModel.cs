using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.FavoriteVM
{
    public class FavCourseBoxInputModel
    {
        public int CourseBoxId { get; set; }

        ///// <summary>
        ///// 单个收藏夹收藏
        ///// </summary>
        //public int FavoriteId { get; set; }

        /// <summary>
        /// 收藏此课程的收藏夹的ID列表
        /// 如果我有收藏夹不在此列表内，则代表不收藏，有着移除它
        /// 1,2,5
        /// </summary>
        public string FavListIds { get; set; }
    }
}