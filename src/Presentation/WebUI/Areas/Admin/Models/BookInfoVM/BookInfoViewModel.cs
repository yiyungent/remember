using Core;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.BookInfoVM
{
    public class BookInfoViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsOpen { get; set; }

        #region 数据库模型->视图模型
        public static explicit operator BookInfoViewModel(BookInfo dbModel)
        {
            return null;
        }
        #endregion

        #region 输入模型->数据库模型
        public static explicit operator BookInfo(BookInfoViewModel inputModel)
        {
            BookInfo dbModel = new BookInfo();
            dbModel.ID = inputModel.ID;
            dbModel.Name = inputModel.Name;
            dbModel.Description = inputModel.Description;
            dbModel.IsOpen = inputModel.IsOpen;

            return dbModel;
        }
        #endregion
    }
}