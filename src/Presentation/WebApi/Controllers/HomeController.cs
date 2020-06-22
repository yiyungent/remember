using Core;
using Domain;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Common;
using WebApi.Models.ArticleVM;
using WebApi.Models.HomeVM;
using System.Diagnostics;
using Domain.Entities;
using Services.Interface;
using Core.Common;
using Framework.Extensions;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Home")]
    public class HomeController : BaseApiController
    {
        #region Fields
        private readonly IArticleService _articleService;
        #endregion

        public HomeController(IArticleService articleService)
        {
            this._articleService = articleService;
        }

        #region 热门文章
        /// <summary>
        /// 热门文章
        /// </summary>
        /// <param name="number">前多少名</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Ranking")]
        public ResponseData Ranking(int number)
        {
            ResponseData responseData = null;
            IList<RankingCourseBoxViewModel> viewModel = new List<RankingCourseBoxViewModel>();


            responseData = new ResponseData
            {
                Code = 1,
                Message = "成功获取热门文章",
                Data = viewModel
            };

            return responseData;
        }
        #endregion

        #region 最新文章
        [HttpGet]
        [Route("LastBook")]
        public ResponseData LastArticles(int number)
        {
            ResponseData responseData = null;
            try
            {
                IList<LastArticleViewModel> viewModel = new List<LastArticleViewModel>();
                IList<Article> articles = this._articleService.Filter<DateTime>(1, number, out int count, m => !m.IsDeleted, m => m.CreateTime, false).ToList();


                for (int i = 0; i < articles.Count; i++)
                {
                    Article article = articles[i];

                    viewModel.Add(new LastArticleViewModel
                    {
                        Article = new LastArticleViewModel.ArticleItem
                        {
                            ID = article.ID,
                            CreateTime = article.CreateTime.ToTimeStamp13(),
                            Author = new LastArticleViewModel.Author
                            {
                                ID = article.Author.ID,
                                Avatar = article.Author.Avatar.ToHttpAbsoluteUrl(),
                                Desc = article.Author.Description,
                                UserName = article.Author.UserName
                            },
                            Desc = article.Description,
                            Title = article.Title,
                            PicUrl = article.PicUrl.ToHttpAbsoluteUrl()
                        },
                        RankingNum = i + 1
                    });
                }


                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "成功获取最新文章",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "获取最新文章失败 " + ex.Message + " " + ex.InnerException?.Message
                };
            }

            return responseData;
        }
        #endregion

    }
}
