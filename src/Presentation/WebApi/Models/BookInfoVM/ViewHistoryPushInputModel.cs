using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.BookInfoVM
{
    public class ViewHistoryPushInputModel
    {
        /// <summary>
        /// VideoInfo.ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 秒
        /// </summary>
        public double LastViewAt { get; set; }

    }
}