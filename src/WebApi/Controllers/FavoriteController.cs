using Common;
using Core;
using Domain;
using Framework.Extensions;
using Framework.Infrastructure.Concrete;
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
        #region 指定收藏夹内容
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;
            FavoriteViewModel viewModel = null;
            try
            {
                // 此收藏夹是否公开
                // 公开：无需登录，谁都可以看
                // 私密：只有创建者可以看
                if (Container.Instance.Resolve<FavoriteService>().Exist(id))
                {
                    Favorite favorite = Container.Instance.Resolve<FavoriteService>().GetEntity(id);
                    if (favorite.IsOpen)
                    {
                        // 公开：谁都可以访问，无需登录

                        #region 获取收藏夹内容数据->视图模型
                        // 此收藏夹内的课程-按加入收藏夹的时间-从最新到最早
                        IList<Favorite_CourseBox> favorite_CourseBoxes = Container.Instance.Resolve<Favorite_CourseBoxService>().Query(new List<ICriterion>
                        {
                            Expression.Eq("Favorite.ID", favorite.ID)
                        }).OrderByDescending(m => m.CreateTime).ToList();

                        viewModel = new FavoriteViewModel();
                        viewModel.ID = favorite.ID;
                        viewModel.Name = favorite.Name;
                        viewModel.Desc = favorite.Description;
                        viewModel.CreateTime = favorite.CreateTime.ToTimeStamp13();
                        viewModel.Creator = new FavoriteViewModel.CreatorViewModel
                        {
                            ID = favorite.Creator.ID,
                            UserName = favorite.Creator.UserName
                        };
                        if (favorite.CourseBoxList == null || favorite.CourseBoxList.Count <= 0)
                        {
                            // 无收藏内容，默认封面图
                            viewModel.PicUrl = ":WebApi:/assets/images/default-favorite-pic.jpg".ToHttpAbsoluteUrl();
                        }
                        else
                        {
                            viewModel.PicUrl = favorite_CourseBoxes.FirstOrDefault().CourseBox.PicUrl.ToHttpAbsoluteUrl();
                        }
                        viewModel.CourseBoxs = new List<FavoriteViewModel.CourseBox>();
                        foreach (var item in favorite_CourseBoxes)
                        {
                            // 此课程的学习人数
                            int learnNum = Container.Instance.Resolve<Learner_CourseBoxService>().Count(Expression.Eq("CourseBox.ID", item.CourseBox.ID));

                            viewModel.CourseBoxs.Add(new FavoriteViewModel.CourseBox
                            {
                                ID = item.CourseBox.ID,
                                Creator = new FavoriteViewModel.CreatorViewModel
                                {
                                    ID = item.CourseBox.Creator.ID,
                                    UserName = item.CourseBox.Creator.UserName
                                },
                                Name = item.CourseBox.Name,
                                PicUrl = item.CourseBox.PicUrl.ToHttpAbsoluteUrl(),
                                FavTime = item.CreateTime.ToTimeStamp13(),
                                LearnNum = learnNum
                            });
                        }
                        #endregion


                        responseData = new ResponseData
                        {
                            Code = 1,
                            Message = "获取收藏夹内容成功",
                            Data = viewModel
                        };
                    }
                    else
                    {
                        // 私密：判断是否当前用户为此收藏夹的创建者
                        if (favorite.Creator.ID == (AccountManager.GetCurrentUserInfo()?.ID ?? 0))
                        {
                            // 此收藏夹虽然私密，当为当前用户创建，可以查看

                            #region 获取收藏夹内容数据->视图模型
                            // 此收藏夹内的课程-按加入收藏夹的时间-从最新到最早
                            IList<Favorite_CourseBox> favorite_CourseBoxes = Container.Instance.Resolve<Favorite_CourseBoxService>().Query(new List<ICriterion>
                            {
                                Expression.Eq("Favorite.ID", favorite.ID)
                            }).OrderByDescending(m => m.CreateTime).ToList();

                            viewModel = new FavoriteViewModel();
                            viewModel.ID = favorite.ID;
                            viewModel.Name = favorite.Name;
                            viewModel.Desc = favorite.Description;
                            viewModel.CreateTime = favorite.CreateTime.ToTimeStamp13();
                            viewModel.Creator = new FavoriteViewModel.CreatorViewModel
                            {
                                ID = favorite.Creator.ID,
                                UserName = favorite.Creator.UserName
                            };
                            if (favorite.CourseBoxList == null || favorite.CourseBoxList.Count <= 0)
                            {
                                // 无收藏内容，默认封面图
                                viewModel.PicUrl = ":WebApi:/assets/images/default-favorite-pic.jpg".ToHttpAbsoluteUrl();
                            }
                            else
                            {
                                viewModel.PicUrl = favorite_CourseBoxes.FirstOrDefault().CourseBox.PicUrl.ToHttpAbsoluteUrl();
                            }
                            viewModel.CourseBoxs = new List<FavoriteViewModel.CourseBox>();
                            foreach (var item in favorite_CourseBoxes)
                            {
                                // 此课程的学习人数
                                int learnNum = Container.Instance.Resolve<Learner_CourseBoxService>().Count(Expression.Eq("CourseBox.ID", item.CourseBox.ID));

                                viewModel.CourseBoxs.Add(new FavoriteViewModel.CourseBox
                                {
                                    ID = item.CourseBox.ID,
                                    Creator = new FavoriteViewModel.CreatorViewModel
                                    {
                                        ID = item.CourseBox.Creator.ID,
                                        UserName = item.CourseBox.Creator.UserName
                                    },
                                    Name = item.CourseBox.Name,
                                    PicUrl = item.CourseBox.PicUrl.ToHttpAbsoluteUrl(),
                                    FavTime = item.CreateTime.ToTimeStamp13(),
                                    LearnNum = learnNum
                                });
                            }
                            #endregion

                            responseData = new ResponseData
                            {
                                Code = 1,
                                Message = "获取收藏夹内容成功",
                                Data = viewModel
                            };
                        }
                        else
                        {
                            responseData = new ResponseData
                            {
                                Code = -3,
                                Message = "此收藏夹为私密，你无权查看"
                            };
                        }
                    }
                }
                else
                {
                    responseData = new ResponseData
                    {
                        Code = -2,
                        Message = "此收藏夹不存在"
                    };
                }
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "获取收藏夹内容失败"
                };
            }

            return responseData;
        }
        #endregion

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
        public ResponseData FavCourseBox(FavCourseBoxInputModel inputModel)
        {
            ResponseData responseData = null;
            try
            {
                // 先检验是否此收藏夹是我创建的，只有是我创建的才能操作此收藏夹
                if (Container.Instance.Resolve<FavoriteService>().Exist(inputModel.FavoriteId))
                {
                    Favorite favorite = Container.Instance.Resolve<FavoriteService>().GetEntity(inputModel.FavoriteId);
                    if (favorite.Creator.ID == ((UserIdentity)User.Identity).ID)
                    {
                        // 该收藏夹是你创建的
                        // 将课程放入此收藏夹
                        // 查询此课程是否已经放入此收藏夹，防止再次插入出现脏数据
                        bool isExist = Container.Instance.Resolve<Favorite_CourseBoxService>().Count(Expression.And(
                             Expression.Eq("CourseBox.ID", inputModel.CourseBoxId),
                             Expression.Eq("Favorite.ID", inputModel.FavoriteId)
                        )) >= 1;

                        if (!isExist)
                        {
                            // 此收藏夹 还未收藏 此课程
                            try
                            {
                                Container.Instance.Resolve<Favorite_CourseBoxService>().Create(new Favorite_CourseBox
                                {
                                    CourseBox = new CourseBox { ID = inputModel.CourseBoxId },
                                    Favorite = new Favorite { ID = inputModel.FavoriteId },
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

        #region 此课程 被我创建的哪些收藏夹 收藏/或没有被收藏
        /// <summary>
        /// 对于此课程，我的收藏情况
        /// </summary>
        /// <param name="courseBoxId"></param>
        /// <returns></returns>
        [NeedAuth]
        [HttpGet]
        [Route("FavStatInCourseBox")]
        public ResponseData FavStatInCourseBox(int courseBoxId)
        {
            ResponseData responseData = null;
            try
            {
                // TODO:
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }
        #endregion
    }
}
