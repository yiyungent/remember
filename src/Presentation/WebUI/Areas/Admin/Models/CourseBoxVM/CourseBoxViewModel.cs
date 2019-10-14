using Core;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.CourseBoxVM
{
    public class CourseBoxViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 有效日期 - 开始时间
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 有效日期 - 结束时间
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 有效学习天数
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public int LearnDay { get; set; }

        #region 数据库模型->视图模型
        public static explicit operator CourseBoxViewModel(CourseBox dbModel)
        {
            return null;
        }
        #endregion

        #region 输入模型->数据库模型
        public static explicit operator CourseBox(CourseBoxViewModel inputModel)
        {
            CourseBox dbModel = new CourseBox();
            dbModel.ID = inputModel.ID;
            dbModel.Name = inputModel.Name;
            dbModel.Description = inputModel.Description;
            dbModel.StartTime = inputModel.StartTime;
            dbModel.EndTime = inputModel.EndTime;
            dbModel.IsOpen = inputModel.IsOpen;

            return dbModel;
        }
        #endregion
    }
}