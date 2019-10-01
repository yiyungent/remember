using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Mvc.WebViewPages.Auth
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {

        public string Auth(string authKey)
        {
            return @"auth-need=""delete-product-cat""";
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}
