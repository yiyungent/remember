using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Extensions
{
    public static class ModelStateDicExt
    {
        #region 获取模型格式错误
        public static string GetErrorMessage(this ModelStateDictionary modelStateDictionary)
        {
            string errorMessage = string.Empty;
            foreach (ModelState item in modelStateDictionary.Values)
            {
                if (item.Errors != null && item.Errors.Count > 0)
                {
                    errorMessage += item.Errors.FirstOrDefault().ErrorMessage + ", ";
                }
            }
            // 移除末尾 ", "
            errorMessage = "不合理的输入: " + errorMessage.Remove(errorMessage.Length - 2);

            return errorMessage;
        }
        #endregion
    }
}