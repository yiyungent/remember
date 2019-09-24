using Core;
using Domain;
using Framework.Extensions;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Attributes;
using WebApi.Infrastructure;
using WebApi.Models.Common;
using WebApi.Models.FavoriteVM;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Favorite")]
    public class FavoriteController : ApiController
    {
        #region 我的收藏夹列表
        [HttpGet]
        [Route("MyFavList")]
        [NeedAuth]
        public ResponseData MyFavList()
        {
            ResponseData responseData = null;
            MyFavListViewModel viewModel = null;
            try
            {
                IList<Favorite> favorites = Container.Instance.Resolve<FavoriteService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Creator.ID", ((UserIdentity)User.Identity).ID)
                }).OrderByDescending(m => m.CreateTime).ToList();

                viewModel = new MyFavListViewModel();
                viewModel.Groups = new List<MyFavListViewModel.Group>();

                List<MyFavListViewModel.Favorite> favList = new List<MyFavListViewModel.Favorite>();
                foreach (var item in favorites)
                {
                    // 此收藏夹的课程列表 - 按时间倒序排序
                    IList<Favorite_CourseBox> favorite_CourseBoxes = Container.Instance.Resolve<Favorite_CourseBoxService>().Query(new List<ICriterion>
                    {
                        Expression.Eq("Favorite.ID", item.ID)
                    }).OrderByDescending(m => m.CreateTime).ToList();

                    favList.Add(new MyFavListViewModel.Favorite
                    {
                        ID = item.ID,
                        Name = item.Name,
                        IsOpen = item.IsOpen,
                        Count = item.CourseBoxList?.Count ?? 0,
                        // 取 最新收藏的课程  的 PicUrl
                        PicUrl = favorite_CourseBoxes.FirstOrDefault()?.CourseBox?.PicUrl.ToHttpAbsoluteUrl()
                    });
                }

                viewModel.Groups.Add(new MyFavListViewModel.Group
                {
                    ID = 1,
                    GroupName = "我的创建",
                    IsFolder = true,
                    FavList = favList
                });

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "获取我的收藏夹列表成功",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "获取我的收藏夹列表失败"
                };
            }

            return responseData;
        }
        #endregion



        #region 收藏指定课程
        [HttpPost]
        [Route("FavCourseBox")]
        [NeedAuth]
        public ResponseData FavCourseBox(int courseBoxId, int favoriteId)
        {
            ResponseData responseData = null;
            try
            {
                // 先检验是否此收藏夹是我创建的，只有是我创建的才能操作此收藏夹
                if (Container.Instance.Resolve<FavoriteService>().Exist(favoriteId))
                {
                    Favorite favorite = Container.Instance.Resolve<FavoriteService>().GetEntity(favoriteId);
                    if (favorite.Creator.ID == ((UserIdentity)User.Identity).ID)
                    {
                        // 该收藏夹是你创建的
                        // 将课程放入此收藏夹
                        // 查询此课程是否已经放入此收藏夹，防止再次插入出现脏数据
                        bool isExist = Container.Instance.Resolve<Favorite_CourseBoxService>().Count(Expression.And(
                             Expression.Eq("CourseBox.ID", courseBoxId),
                             Expression.Eq("Favorite.ID", favoriteId)
                        )) >= 1;

                        if (!isExist)
                        {
                            // 此收藏夹 还未收藏 此课程
                            try
                            {
                                Container.Instance.Resolve<Favorite_CourseBoxService>().Create(new Favorite_CourseBox
                                {
                                    CourseBox = new CourseBox { ID = courseBoxId },
                                    Favorite = new Favorite { ID = favoriteId },
                                    CreateTime = DateTime.Now
                                });

                                responseData = new ResponseData
                                {
                                    Code = 1,
                                    Message = "收藏课程成功"
                                };
                            }
                            catch (Exception ex)
                            {
                                responseData = new ResponseData
                                {
                                    Code = -4,
                                    Message = "收藏课程失败"
                                };
                            }
                        }
                        else
                        {
                            // 此收藏夹已经收藏此课程
                            responseData = new ResponseData
                            {
                                Code = -5,
                                Message = "收藏课程失败，此收藏夹已经收藏此课程"
                            };
                        }
                    }
                    else
                    {
                        // 该收藏夹不是我创建的
                        responseData = new ResponseData
                        {
                            Code = -3,
                            Message = "该收藏夹非你创建，你无权操作"
                        };
                    }
                }
                else
                {
                    responseData = new ResponseData
                    {
                        Code = -2,
                        Message = "不存在此收藏夹"
                    };
                }
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "收藏课程失败"
                };
            }

            return responseData;
        }
        #endregion
    }
}
