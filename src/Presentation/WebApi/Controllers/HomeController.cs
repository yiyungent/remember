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
using WebApi.Models.CourseBoxVM;
using WebApi.Models.HomeVM;
using System.Diagnostics;
using Domain.Entities;
using Services.Interface;
using Core.Common;
using Framework.Extensions;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Home")]
    public class HomeController : BaseController
    {
        #region Fields
        private readonly ILearner_CourseBoxService _learner_CourseBoxService;
        private readonly ICourseBoxService _courseBoxService;
        #endregion

        public HomeController(ILearner_CourseBoxService learner_CourseBoxService, ICourseBoxService courseBoxService)
        {
            this._learner_CourseBoxService = learner_CourseBoxService;
            this._courseBoxService = courseBoxService;
        }

        #region 热门课程
        /// <summary>
        /// 热门课程
        /// </summary>
        /// <param name="number">前多少名</param>
        /// <returns></returns>
        [HttpGet]
        [Route("RankingCourseBox")]
        public ResponseData RankingCourseBox(int number)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ResponseData responseData = null;
            IList<RankingCourseBoxViewModel> viewModel = new List<RankingCourseBoxViewModel>();
            //IList<Learner_CourseBox> allCourseBoxTable = Container.Instance.Resolve<Learner_CourseBoxService>().GetAll().Where(m => m.CourseBox.IsOpen = true).ToList();
            IList<Learner_CourseBox> allCourseBoxTable = this._learner_CourseBoxService.Filter(m => m.CourseBox.IsOpen == true && !m.IsDeleted).ToList();
            var query = from a in allCourseBoxTable
                        group a by a.CourseBox.ID
                      into g
                        select new
                        {
                            CourseBox = new RankingCourseBoxViewModel.RankingCourseBoxItem
                            {
                                ID = g.Key,
                                Creator = new RankingCourseBoxViewModel.Creator
                                {
                                    ID = g.First().CourseBox.Creator.ID,
                                    UserName = g.First().CourseBox.Creator.UserName,
                                    Avatar = g.First().CourseBox.Creator.Avatar.ToHttpAbsoluteUrl(),
                                },
                                Desc = g.First().CourseBox.Description,
                                Name = g.First().CourseBox.Name,
                                PicUrl = g.First().CourseBox.PicUrl.ToHttpAbsoluteUrl()
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
                    TotalSpendTime = rankingQuery[i].TotalSpendTime ?? 0,
                    RankingNum = i + 1
                });
            }

            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds;

            responseData = new ResponseData
            {
                Code = 1,
                Message = "成功获取热门课程",
                Data = viewModel
            };

            return responseData;
        }
        #endregion

        #region 最新课程
        [HttpGet]
        [HttpOptions]
        [Route("LastCourseBox")]
        public ResponseData LastCourseBox(int number)
        {
            ResponseData responseData = null;
            try
            {
                IList<LastCourseBoxViewModel> viewModel = new List<LastCourseBoxViewModel>();
                //List<Order> orders = new List<Order> { new Order("CreateTime", false) };
                //IList<CourseBox> courseBoxes = Container.Instance.Resolve<CourseBoxService>().GetPaged(new List<ICriterion>(), orders, 1, number, out int count).ToList();
                IList<CourseBox> courseBoxes = this._courseBoxService.Filter<DateTime>(1, number, out int count, m => !m.IsDeleted, m => m.CreateTime ?? DateTime.MinValue, false).ToList();


                for (int i = 0; i < courseBoxes.Count; i++)
                {
                    CourseBox courseBox = courseBoxes[i];

                    //int learnNum = Container.Instance.Resolve<Learner_CourseBoxService>().Count(Expression.Eq("CourseBox.ID", courseBox.ID));
                    int learnNum = this._learner_CourseBoxService.Count(m => m.CourseBoxId == courseBox.ID && !m.IsDeleted);

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
                    Message = "成功获取最新课程",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "获取最新课程失败"
                };
            }

            return responseData;
        }
        #endregion

    }
}
