using Core;
using Domain;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.CardBoxVM
{
    public class CardBoxViewModel
    {
        #region 数据库模型->视图模型
        public static explicit operator CardBoxViewModel(CardBox dbModel)
        {
            return null;
        }
        #endregion

        #region 输入模型->数据库模型
        public static explicit operator CardBox(CardBoxViewModel inputModel)
        {
            CardBox dbModel = null;


            return dbModel;
        }
        #endregion
    }
}