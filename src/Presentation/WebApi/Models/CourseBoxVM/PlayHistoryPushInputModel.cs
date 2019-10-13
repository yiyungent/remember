using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CourseBoxVM
{
    public class PlayHistoryPushInputModel
    {
        /// <summary>
        /// VideoInfo.ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 秒
        /// </summary>
        public double LastPlayAt { get; set; }

    }
}