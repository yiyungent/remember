using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.FavoriteVM
{
    /// <summary>
    /// 此课程的收藏统计
    /// </summary>
    public class FavStatInBookInfoViewModel
    {
        /// <summary>
        /// 此课程的总被收藏数（全部用户）
        /// </summary>
        public int BookInfoFavCount { get; set; }

        /// <summary>
        /// 对于此课程我的收藏统计
        /// </summary>
        public MyFavStatViewModel MyFavStat { get; set; }

        public sealed class MyFavStatViewModel
        {
            /// <summary>
            /// 我的这些收藏夹收藏了此课程
            /// 收藏夹的ID列表
            /// </summary>
            public IList<int> FavIds { get; set; }
        }
    }
}