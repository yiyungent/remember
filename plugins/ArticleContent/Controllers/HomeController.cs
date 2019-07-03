using ArticleContent.Models;
using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArticleContent.Controllers
{
    public class HomeController : Controller
    {
        private IDbConnection _conn;

        #region Ctor
        public HomeController()
        {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDiagnosticsDb"].ConnectionString);
        }
        #endregion

        #region 列表
        public ActionResult Index()
        {
            IEnumerable<ArticleModel> list = _conn.Query<ArticleModel, UserInfo, ArticleModel>("SELECT * FROM Plugin_ArticleContent pa LEFT JOIN UserInfo ui ON pa.AuthorId = ui.ID", (post, user) => { post.AuthorId = user.ID; return post; });

            return View("~/plugins/ArticleContent/Views/Home/Index.cshtml", list);
        }
        #endregion
    }
}