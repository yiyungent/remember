using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.CardInfoVM
{
    public class CardInfoViewModel
    {
        #region 数据库模型->视图模型
        public static explicit operator CardInfoViewModel(CardInfo dbModel)
        {
            return null;
        }
        #endregion

        #region 输入模型->数据库模型
        public static explicit operator CardInfo(CardInfoViewModel inputModel)
        {
            CardInfo dbModel = null;


            return dbModel;
        }
        #endregion
    }
}