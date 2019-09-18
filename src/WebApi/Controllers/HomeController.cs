using Core;
using Domain;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Common;
using WebApi.Models.CourseBoxVM;
using WebApi.Models.HomeVM;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Home")]
    public class HomeController : ApiController
    {
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
            ResponseData responseData = null;
            IList<RankingCourseBoxViewModel> viewModel = new List<RankingCourseBoxViewModel>();
            IList<Learner_CourseBox> allCourseBoxTable = Container.Instance.Resolve<Learner_CourseBoxService>().GetAll().Where(m => m.CourseBox.IsOpen = true).ToList();
            var query = from a in allCourseBoxTable
                        group a by a.CourseBox.ID
                      into g
                        select new
                        {
                            CourseBox = new RankingCourseBoxItem
                            {
                                ID = g.Key,
                                Creator = new Models.UserInfoVM.UserInfoViewModel
                                {
                                    ID = g.First().CourseBox.Creator.ID,
                                    UserName = g.First().CourseBox.Creator.UserName,
                                    Name = g.First().CourseBox.Creator.Name,
                                    Avatar = g.First().CourseBox.Creator.Avatar,
                                },
                                Description = g.First().CourseBox.Description,
                                Name = g.First().CourseBox.Name,
                                PicUrl = g.First().CourseBox.PicUrl
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

            responseData = new ResponseData
            {
                Code = 1,
                Message = "成功获取热门课程",
                Data = viewModel
            };

            return responseData;
        }
        #endregion
    }
}
