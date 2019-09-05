using Core;
using Domain;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.CardBoxVM
{
    public class CardBoxViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsOpen { get; set; }

        #region 数据库模型->视图模型
        public static explicit operator CardBoxViewModel(CardBox dbModel)
        {
            return null;
        }
        #endregion

        #region 输入模型->数据库模型
        public static explicit operator CardBox(CardBoxViewModel inputModel)
        {
            CardBox dbModel = new CardBox();
            dbModel.ID = inputModel.ID;
            dbModel.Name = inputModel.Name;
            dbModel.Description = inputModel.Description;
            dbModel.IsOpen = inputModel.IsOpen;

            return dbModel;
        }
        #endregion
    }
}