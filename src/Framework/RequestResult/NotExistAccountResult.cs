using Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Framework.RequestResult
{

    #region ViewResult
    public class View_NotExistAccountResult : ViewResult
    {
        public View_NotExistAccountResult()
        {
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            this.ViewData.Model = new ErrorRedirectViewModel
            {
                Title = "不存在此账号",
                Message = "账号不存在",
                RedirectUrl = url.Action("Index", "Home", new { area = "" }),
                RedirectUrlName = "首页",
                WaitSecond = 8
            };

            this.ViewName = "_ErrorRedirect";
        }
    }
    #endregion
}
