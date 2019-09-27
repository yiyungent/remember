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
                            viewModel.PicUrl = ":WebApiSite:/assets/images/default-favorite-pic.jpg".ToHttpAbsoluteUrl();
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
                                viewModel.PicUrl = ":WebApiSite:/assets/images/default-favorite-pic.jpg".ToHttpAbsoluteUrl();
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
                    int favorite_CourseBox_Num = Container.Instance.Resolve<Favorite_CourseBoxService>().Count(Expression.Eq("Favorite.ID", item.ID));
                    string picUrl = "";
                    if (favorite_CourseBox_Num >= 1)
                    {
                        IList<Favorite_CourseBox> favorite_CourseBoxes = Container.Instance.Resolve<Favorite_CourseBoxService>().Query(new List<ICriterion>
                        {
                            Expression.Eq("Favorite.ID", item.ID)
                        }).OrderByDescending(m => m.CreateTime).ToList();
                        picUrl = favorite_CourseBoxes.FirstOrDefault()?.CourseBox?.PicUrl.ToHttpAbsoluteUrl();
                    }
                    else
                    {
                        picUrl = ":WebApiSite:/assets/images/default-favorite-pic.jpg".ToHttpAbsoluteUrl();
                    }

                    favList.Add(new MyFavListViewModel.Favorite
                    {
                        ID = item.ID,
                        Name = item.Name,
                        IsOpen = item.IsOpen,
                        Count = favorite_CourseBox_Num,
                        // 取 最新收藏的课程  的 PicUrl
                        PicUrl = picUrl
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
                #region 废弃
                //// 先检验是否此收藏夹是我创建的，只有是我创建的才能操作此收藏夹
                //if (Container.Instance.Resolve<FavoriteService>().Exist(inputModel.FavoriteId))
                //{
                //    Favorite favorite = Container.Instance.Resolve<FavoriteService>().GetEntity(inputModel.FavoriteId);
                //    if (favorite.Creator.ID == ((UserIdentity)User.Identity).ID)
                //    {
                //        // 该收藏夹是你创建的
                //        // 将课程放入此收藏夹
                //        // 查询此课程是否已经放入此收藏夹，防止再次插入出现脏数据
                //        bool isExist = Container.Instance.Resolve<Favorite_CourseBoxService>().Count(Expression.And(
                //             Expression.Eq("CourseBox.ID", inputModel.CourseBoxId),
                //             Expression.Eq("Favorite.ID", inputModel.FavoriteId)
                //        )) >= 1;

                //        if (!isExist)
                //        {
                //            // 此收藏夹 还未收藏 此课程
                //            try
                //            {
                //                Container.Instance.Resolve<Favorite_CourseBoxService>().Create(new Favorite_CourseBox
                //                {
                //                    CourseBox = new CourseBox { ID = inputModel.CourseBoxId },
                //                    Favorite = new Favorite { ID = inputModel.FavoriteId },
                //                    CreateTime = DateTime.Now
                //                });

                //                responseData = new ResponseData
                //                {
                //                    Code = 1,
                //                    Message = "收藏课程成功"
                //                };
                //            }
                //            catch (Exception ex)
                //            {
                //                responseData = new ResponseData
                //                {
                //                    Code = -4,
                //                    Message = "收藏课程失败"
                //                };
                //            }
                //        }
                //        else
                //        {
                //            // 此收藏夹已经收藏此课程
                //            responseData = new ResponseData
                //            {
                //                Code = -5,
                //                Message = "收藏课程失败，此收藏夹已经收藏此课程"
                //            };
                //        }
                //    }
                //    else
                //    {
                //        // 该收藏夹不是我创建的
                //        responseData = new ResponseData
                //        {
                //            Code = -3,
                //            Message = "该收藏夹非你创建，你无权操作"
                //        };
                //    }
                //}
                //else
                //{
                //    responseData = new ResponseData
                //    {
                //        Code = -2,
                //        Message = "不存在此收藏夹"
                //    };
                //} 
                #endregion


                try
                {
                    string[] favIdsStr = inputModel.FavListIds?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                    IList<int> favIds = new List<int>();
                    foreach (var item in favIdsStr)
                    {
                        favIds.Add(Convert.ToInt32(item));
                    }

                    IList<Favorite> myAllFavList = Container.Instance.Resolve<FavoriteService>().Query(new List<ICriterion>
                    {
                        Expression.Eq("Creator.ID", ((UserIdentity)User.Identity).ID)
                    });

                    // 先将我的收藏夹列表与此课程的所有关系都删除
                    IList<Favorite_CourseBox> favorite_CourseBoxes = Container.Instance.Resolve<Favorite_CourseBoxService>().Query(new List<ICriterion>
                    {
                        Expression.And(
                            Expression.Eq("CourseBox.ID", inputModel.CourseBoxId),
                            Expression.In("Favorite.ID", myAllFavList.Select(m=>m.ID).ToArray())
                        )
                    });
                    foreach (var item in favorite_CourseBoxes)
                    {
                        Container.Instance.Resolve<Favorite_CourseBoxService>().Delete(item.ID);
                    }
                    if (favIds != null && favIds.Count >= 1)
                    {
                        // 在此列表内的 - 此课程加入收藏
                        foreach (var favId in favIds)
                        {
                            Container.Instance.Resolve<Favorite_CourseBoxService>().Create(new Favorite_CourseBox
                            {
                                CourseBox = new CourseBox { ID = inputModel.CourseBoxId },
                                Favorite = new Favorite { ID = favId },
                                CreateTime = DateTime.Now
                            });
                        }
                    }


                    #region 废弃
                    //// 不在此列表内的，移除 此收藏夹与此课程的关系
                    //IList<int> myAllFavListIds = myAllFavList.Select(m => m.ID).ToList();
                    //// myAllFavListIds - inputModel.FavListIds
                    //foreach (var favId in myAllFavListIds)
                    //{

                    //} 
                    #endregion

                    responseData = new ResponseData
                    {
                        Code = 1,
                        Message = "成功"
                    };
                }
                catch (Exception ex)
                {
                    responseData = new ResponseData
                    {
                        Code = -2,
                        Message = "失败"
                    };
                }

            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "失败"
                };
            }

            return responseData;
        }
        #endregion

        #region 对于此课程的收藏情况（包括我的收藏统计对于此课程）
        /// <summary>
        /// 对于此课程的收藏情况
        /// </summary>
        /// <param name="courseBoxId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("FavStatInCourseBox")]
        public ResponseData FavStatInCourseBox(int courseBoxId)
        {
            ResponseData responseData = null;
            FavStatInCourseBoxViewModel viewModel = null;
            try
            {
                int courseBoxFavCount = Container.Instance.Resolve<Favorite_CourseBoxService>().Count(Expression.Eq("CourseBox.ID", courseBoxId));
                viewModel = new FavStatInCourseBoxViewModel
                {
                    CourseBoxFavCount = courseBoxFavCount,
                };

                // 登录用户，追加登录用户对于此课程的收藏统计
                var user = AccountManager.GetCurrentUserInfo(false);
                if (user != null)
                {
                    // 查询我有哪些收藏夹
                    IList<Favorite> myFavList = Container.Instance.Resolve<FavoriteService>().Query(new List<ICriterion>
                    {
                        Expression.Eq("Creator.ID", user.ID)
                    });
                    IList<int> myFavListIds = myFavList.Select(m => m.ID).ToList();

                    // 关于这门课程，我的这些收藏夹收藏了，附带每个收藏夹多久收藏了这么课程的详细情况
                    IList<Favorite_CourseBox> fav_CourseBox_InMyFav = Container.Instance.Resolve<Favorite_CourseBoxService>().Query(new List<ICriterion>
                    {
                        Expression.And(
                            Expression.Eq("CourseBox.ID", courseBoxId),
                            Expression.In("Favorite.ID", myFavListIds.ToArray())
                        )
                    });
                    // 关于这门课程我的这些收藏夹收藏了，按创建收藏夹的时间倒序排序，****与获取我的收藏夹列表的顺序相同******
                    IList<Favorite> myFavListInCourseBox = fav_CourseBox_InMyFav.Select(m => m.Favorite).OrderByDescending(m => m.CreateTime).ToList();

                    viewModel.MyFavStat = new FavStatInCourseBoxViewModel.MyFavStatViewModel();
                    viewModel.MyFavStat.FavIds = myFavListInCourseBox.Select(m => m.ID).ToList();
                }


                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "对于此课程，获取我的收藏情况成功",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "对于此课程，获取我的收藏情况失败"
                };
            }

            return responseData;
        }
        #endregion


        #region 创建收藏夹
        [HttpPost]
        [Route("Create")]
        [NeedAuth]
        public ResponseData Create(CreateInputModel inputModel)
        {
            ResponseData responseData = null;
            try
            {
                Container.Instance.Resolve<FavoriteService>().Create(new Favorite
                {
                    IsOpen = inputModel.IsOpen,
                    Name = inputModel.Name,
                    Description = inputModel.Desc,
                    Creator = new UserInfo { ID = ((UserIdentity)User.Identity).ID }
                });

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "创建收藏夹成功"
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "创建收藏夹失败"
                };
            }

            return responseData;
        }
        #endregion
    }
}
