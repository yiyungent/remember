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
using WebApi.Models.BookInfoVM;
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
        private readonly IUser_BookInfoService _user_BookInfoService;
        private readonly IBookInfoService _bookInfoService;
        #endregion

        public HomeController(IUser_BookInfoService user_BookInfoService, IBookInfoService bookInfoService)
        {
            this._user_BookInfoService = user_BookInfoService;
            this._bookInfoService = bookInfoService;
        }

        #region 热门文库
        /// <summary>
        /// 热门文库
        /// </summary>
        /// <param name="number">前多少名</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Ranking")]
        public ResponseData Ranking(int number)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ResponseData responseData = null;
            IList<RankingCourseBoxViewModel> viewModel = new List<RankingCourseBoxViewModel>();
            //IList<Learner_CourseBox> allCourseBoxTable = Container.Instance.Resolve<Learner_CourseBoxService>().GetAll().Where(m => m.CourseBox.IsOpen = true).ToList();
            IList<User_BookInfo> allCourseBoxTable = this._user_BookInfoService.Filter(m => m.BookInfo.IsOpen == true && !m.IsDeleted).ToList();
            var query = from a in allCourseBoxTable
                        group a by a.BookInfo.ID
                      into g
                        select new
                        {
                            CourseBox = new RankingCourseBoxViewModel.RankingCourseBoxItem
                            {
                                ID = g.Key,
                                Creator = new RankingCourseBoxViewModel.Creator
                                {
                                    ID = g.First().BookInfo.Creator.ID,
                                    UserName = g.First().BookInfo.Creator.UserName,
                                    Avatar = g.First().BookInfo.Creator.Avatar.ToHttpAbsoluteUrl(),
                                },
                                Desc = g.First().BookInfo.Description,
                                Name = g.First().BookInfo.Name,
                                PicUrl = g.First().BookInfo.PicUrl.ToHttpAbsoluteUrl()
                            },
                            LearnNum = g.Count(),
                            TotalSpendTime = g.Sum(s => s.SpendTime)
                        };
            // 倒序排序：学习人数越多越靠前
            var rankingQuery = query.OrderByDescending(m => m.LearnNum).Take(number).ToList();
            for (int i = 0; i < rankingQuery.Count(); i++)
            {
                viewModel.Add(new RankingCourseBoxViewModel
                {
                    CourseBox = rankingQuery[i].CourseBox,
                    LearnNum = rankingQuery[i].LearnNum,
                    TotalSpendTime = rankingQuery[i].TotalSpendTime,
                    RankingNum = i + 1
                });
            }

            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds;

            responseData = new ResponseData
            {
                Code = 1,
                Message = "成功获取热门文库",
                Data = viewModel
            };

            return responseData;
        }
        #endregion

        #region 最新文库
        [HttpGet]
        [Route("LastBook")]
        public ResponseData LastBook(int number)
        {
            ResponseData responseData = null;
            try
            {
                IList<LastCourseBoxViewModel> viewModel = new List<LastCourseBoxViewModel>();
                IList<BookInfo> courseBoxes = this._bookInfoService.Filter<DateTime>(1, number, out int count, m => !m.IsDeleted, m => m.CreateTime, false).ToList();


                for (int i = 0; i < courseBoxes.Count; i++)
                {
                    BookInfo courseBox = courseBoxes[i];

                    int learnNum = this._user_BookInfoService.Count(m => m.BookInfoId == courseBox.ID && !m.IsDeleted);

                    viewModel.Add(new LastCourseBoxViewModel
                    {
                        CourseBox = new LastCourseBoxViewModel.CourseBoxItem
                        {
                            ID = courseBox.ID,
                            CreateTime = courseBox.CreateTime.ToTimeStamp13(),
                            Creator = new LastCourseBoxViewModel.Creator
                            {
                                ID = courseBox.Creator.ID,
                                Avatar = courseBox.Creator.Avatar.ToHttpAbsoluteUrl(),
                                Desc = courseBox.Creator.Description,
                                UserName = courseBox.Creator.UserName
                            },
                            Desc = courseBox.Description,
                            Name = courseBox.Name,
                            PicUrl = courseBox.PicUrl.ToHttpAbsoluteUrl()
                        },
                        LearnNum = learnNum,
                        RankingNum = i + 1
                    });
                }


                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "成功获取最新文库",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "获取最新文库失败 " + ex.Message + " " + ex.InnerException?.Message
                };
            }

            return responseData;
        }
        #endregion

    }
}
