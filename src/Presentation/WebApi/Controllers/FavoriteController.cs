using Core;
using Core.Common;
using Domain;
using Domain.Entities;
using Framework.Extensions;
using Framework.Infrastructure.Concrete;
using Services.Interface;
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
    public class FavoriteController : BaseApiController
    {
        #region Fields
        private readonly IFavoriteService _favoriteService;
        private readonly IFavorite_ArticleService _favorite_ArticleService;
        #endregion

        #region Ctor
        public FavoriteController(IFavoriteService favoriteService, IFavorite_ArticleService favorite_ArticleService)
        {
            this._favoriteService = favoriteService;
            this._favorite_ArticleService = favorite_ArticleService;
        }
        #endregion

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
                //if (Container.Instance.Resolve<FavoriteService>().Exist(id))
                if (this._favoriteService.Contains(m => m.ID == id && !m.IsDeleted))
                {
                    //Favorite favorite = Container.Instance.Resolve<FavoriteService>().GetEntity(id);
                    Favorite favorite = this._favoriteService.Find(id);
                    if (favorite.IsOpen)
                    {
                        // 公开：谁都可以访问，无需登录

                        #region 获取收藏夹内容数据->视图模型
                        // 此收藏夹内的课程-按加入收藏夹的时间-从最新到最早
                        //IList<Favorite_BookInfo> favorite_BookInfos = Container.Instance.Resolve<Favorite_BookInfoService>().Query(new List<ICriterion>
                        //{
                        //    Expression.Eq("Favorite.ID", favorite.ID)
                        //}).OrderByDescending(m => m.CreateTime).ToList();
                        IList<Favorite_Article> favorite_BookInfos = this._favorite_ArticleService.Filter(m => m.FavoriteId == favorite.ID && !m.IsDeleted).OrderByDescending(m => m.CreateTime).ToList();

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
                        if (favorite.Articles == null || favorite.Articles.Count <= 0)
                        {
                            // 无收藏内容，默认封面图
                            viewModel.PicUrl = ":WebApiSite:/assets/images/default-favorite-pic.jpg".ToHttpAbsoluteUrl();
                        }
                        else
                        {
                            viewModel.PicUrl = favorite_BookInfos.FirstOrDefault().Article.PicUrl.ToHttpAbsoluteUrl();
                        }
                        viewModel.Articles = new List<FavoriteViewModel.ArticleItem>();
                        foreach (var item in favorite_BookInfos)
                        {
                            // 此课程的学习人数
                            //int learnNum = Container.Instance.Resolve<User_BookInfoService>().Count(Expression.Eq("BookInfo.ID", item.BookInfo.ID));
                            //int learnNum = this._learner_BookInfoService.Count(m => m.BookInfoId == item.ArticleId && !m.IsDeleted);

                            viewModel.Articles.Add(new FavoriteViewModel.ArticleItem
                            {
                                ID = item.Article.ID,
                                Creator = new FavoriteViewModel.CreatorViewModel
                                {
                                    ID = item.Article.AuthorId,
                                    UserName = item.Article.Author.UserName
                                },
                                Title = item.Article.Title,
                                PicUrl = item.Article.PicUrl.ToHttpAbsoluteUrl(),
                                FavTime = item.CreateTime.ToTimeStamp13(),
                                Stat = new FavoriteViewModel.StatModel
                                {
                                    FavNum = item.Article.Favorite_Articles.Count
                                }
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
                            //IList<Favorite_BookInfo> favorite_BookInfos = Container.Instance.Resolve<Favorite_BookInfoService>().Query(new List<ICriterion>
                            //{
                            //    Expression.Eq("Favorite.ID", favorite.ID)
                            //}).OrderByDescending(m => m.CreateTime).ToList();
                            IList<Favorite_Article> favorite_BookInfos = this._favorite_ArticleService.Filter(m => m.FavoriteId == favorite.ID && !m.IsDeleted).OrderByDescending(m => m.CreateTime).ToList();

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
                            if (favorite.Favorite_Articles == null || favorite.Favorite_Articles.Count <= 0)
                            {
                                // 无收藏内容，默认封面图
                                viewModel.PicUrl = ":WebApiSite:/assets/images/default-favorite-pic.jpg".ToHttpAbsoluteUrl();
                            }
                            else
                            {
                                viewModel.PicUrl = favorite_BookInfos.FirstOrDefault().Article.PicUrl.ToHttpAbsoluteUrl();
                            }
                            viewModel.Articles = new List<FavoriteViewModel.ArticleItem>();
                            foreach (var item in favorite_BookInfos)
                            {
                                // 此课程的学习人数
                                //int learnNum = Container.Instance.Resolve<User_BookInfoService>().Count(Expression.Eq("BookInfo.ID", item.BookInfo.ID));
                                //int learnNum = this._learner_BookInfoService.Count(m => m.BookInfoId == item.ArticleId && !m.IsDeleted);

                                viewModel.Articles.Add(new FavoriteViewModel.ArticleItem
                                {
                                    ID = item.Article.ID,
                                    Creator = new FavoriteViewModel.CreatorViewModel
                                    {
                                        ID = item.Article.AuthorId,
                                        UserName = item.Article.Author.UserName
                                    },
                                    Title = item.Article.Title,
                                    PicUrl = item.Article.PicUrl.ToHttpAbsoluteUrl(),
                                    FavTime = item.CreateTime.ToTimeStamp13(),
                                    Stat = new FavoriteViewModel.StatModel
                                    {
                                        FavNum = item.Article.Favorite_Articles.Count
                                    }
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
                //IList<Favorite> favorites = Container.Instance.Resolve<FavoriteService>().Query(new List<ICriterion>
                //{
                //    Expression.Eq("Creator.ID", ((UserIdentity)User.Identity).ID)
                //}).OrderByDescending(m => m.CreateTime).ToList();
                int currentUserId = ((UserIdentity)User.Identity).ID;
                IList<Favorite> favorites = this._favoriteService.Filter(m => m.CreatorId == currentUserId && !m.IsDeleted).OrderByDescending(m => m.CreateTime).ToList();

                viewModel = new MyFavListViewModel();
                viewModel.Groups = new List<MyFavListViewModel.Group>();

                List<MyFavListViewModel.Favorite> favList = new List<MyFavListViewModel.Favorite>();
                foreach (var item in favorites)
                {
                    // 此收藏夹的课程列表 - 按时间倒序排序
                    //int favorite_BookInfo_Num = Container.Instance.Resolve<Favorite_BookInfoService>().Count(Expression.Eq("Favorite.ID", item.ID));
                    int favorite_BookInfo_Num = this._favorite_ArticleService.Count(m => m.FavoriteId == item.ID && !m.IsDeleted);
                    string picUrl = "";
                    if (favorite_BookInfo_Num >= 1)
                    {
                        //IList<Favorite_BookInfo> favorite_BookInfos = Container.Instance.Resolve<Favorite_BookInfoService>().Query(new List<ICriterion>
                        //{
                        //    Expression.Eq("Favorite.ID", item.ID)
                        //}).OrderByDescending(m => m.CreateTime).ToList();
                        IList<Favorite_Article> favorite_BookInfos = this._favorite_ArticleService.Filter(m => m.FavoriteId == item.ID && !m.IsDeleted).OrderByDescending(m => m.CreateTime).ToList();
                        picUrl = favorite_BookInfos.FirstOrDefault()?.Article?.PicUrl.ToHttpAbsoluteUrl();
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
                        Count = favorite_BookInfo_Num,
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
        [Route("FavBookInfo")]
        [NeedAuth]
        public ResponseData FavBookInfo(FavBookInfoInputModel inputModel)
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
                //        bool isExist = Container.Instance.Resolve<Favorite_BookInfoService>().Count(Expression.And(
                //             Expression.Eq("BookInfo.ID", inputModel.BookInfoId),
                //             Expression.Eq("Favorite.ID", inputModel.FavoriteId)
                //        )) >= 1;

                //        if (!isExist)
                //        {
                //            // 此收藏夹 还未收藏 此课程
                //            try
                //            {
                //                Container.Instance.Resolve<Favorite_BookInfoService>().Create(new Favorite_BookInfo
                //                {
                //                    BookInfo = new BookInfo { ID = inputModel.BookInfoId },
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
                // TODO: 不安全未效验

                try
                {
                    string[] favIdsStr = inputModel.FavListIds?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                    IList<int> favIds = new List<int>();
                    foreach (var item in favIdsStr)
                    {
                        favIds.Add(Convert.ToInt32(item));
                    }

                    //IList<Favorite> myAllFavList = Container.Instance.Resolve<FavoriteService>().Query(new List<ICriterion>
                    //{
                    //    Expression.Eq("Creator.ID", ((UserIdentity)User.Identity).ID)
                    //});
                    int currentUserId = ((UserIdentity)User.Identity).ID;
                    IList<Favorite> myAllFavList = this._favoriteService.Filter(m => m.CreatorId == currentUserId && !m.IsDeleted).ToList();

                    // 先将我的收藏夹列表与此课程的所有关系都删除
                    //IList<Favorite_BookInfo> favorite_BookInfos = Container.Instance.Resolve<Favorite_BookInfoService>().Query(new List<ICriterion>
                    //{
                    //    Expression.And(
                    //        Expression.Eq("BookInfo.ID", inputModel.BookInfoId),
                    //        Expression.In("Favorite.ID", myAllFavList.Select(m=>m.ID).ToArray())
                    //    )
                    //});
                    List<int> tempIds = myAllFavList.Select(t => t.ID).ToList();
                    IList<Favorite_Article> favorite_BookInfos = this._favorite_ArticleService.Filter(m => m.ArticleId == inputModel.BookInfoId && tempIds.Contains(m.FavoriteId) && !m.IsDeleted).ToList();
                    foreach (var item in favorite_BookInfos)
                    {
                        //Container.Instance.Resolve<Favorite_BookInfoService>().Delete(item.ID);
                        item.IsDeleted = true;
                        item.DeletedAt = DateTime.Now;
                        this._favorite_ArticleService.Update(item);
                    }
                    if (favIds != null && favIds.Count >= 1)
                    {
                        // 在此列表内的 - 此课程加入收藏
                        foreach (var favId in favIds)
                        {
                            //Container.Instance.Resolve<Favorite_BookInfoService>().Create(new Favorite_BookInfo
                            //{
                            //    BookInfo = new BookInfo { ID = inputModel.BookInfoId },
                            //    Favorite = new Favorite { ID = favId },
                            //    CreateTime = DateTime.Now
                            //});
                            this._favorite_ArticleService.Create(new Favorite_Article
                            {
                                ArticleId = inputModel.BookInfoId,
                                FavoriteId = favId,
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
        [Route("FavStatInBookInfo")]
        public ResponseData FavStatInBookInfo(int courseBoxId)
        {
            ResponseData responseData = null;
            FavStatInBookInfoViewModel viewModel = null;
            try
            {
                //int courseBoxFavCount = Container.Instance.Resolve<Favorite_BookInfoService>().Count(Expression.Eq("BookInfo.ID", courseBoxId));
                int courseBoxFavCount = this._favorite_ArticleService.Count(m => m.ArticleId == courseBoxId && !m.IsDeleted);
                viewModel = new FavStatInBookInfoViewModel
                {
                    BookInfoFavCount = courseBoxFavCount,
                };

                // 登录用户，追加登录用户对于此课程的收藏统计
                var user = AccountManager.GetCurrentUserInfo(false);
                if (user != null)
                {
                    // 查询我有哪些收藏夹
                    //IList<Favorite> myFavList = Container.Instance.Resolve<FavoriteService>().Query(new List<ICriterion>
                    //{
                    //    Expression.Eq("Creator.ID", user.ID)
                    //});
                    IList<Favorite> myFavList = this._favoriteService.Filter(m => m.CreatorId == user.ID).ToList();
                    IList<int> myFavListIds = myFavList.Select(m => m.ID).ToList();

                    // 关于这门课程，我的这些收藏夹收藏了，附带每个收藏夹多久收藏了这么课程的详细情况
                    //IList<Favorite_BookInfo> fav_BookInfo_InMyFav = Container.Instance.Resolve<Favorite_BookInfoService>().Query(new List<ICriterion>
                    //{
                    //    Expression.And(
                    //        Expression.Eq("BookInfo.ID", courseBoxId),
                    //        Expression.In("Favorite.ID", myFavListIds.ToArray())
                    //    )
                    //});
                    IList<Favorite_Article> fav_BookInfo_InMyFav = this._favorite_ArticleService.Filter(m => m.ArticleId == courseBoxId && myFavListIds.Contains(m.FavoriteId) && !m.IsDeleted).ToList();
                    // 关于这门课程我的这些收藏夹收藏了，按创建收藏夹的时间倒序排序，****与获取我的收藏夹列表的顺序相同******
                    IList<Favorite> myFavListInBookInfo = fav_BookInfo_InMyFav.Select(m => m.Favorite).OrderByDescending(m => m.CreateTime).ToList();

                    viewModel.MyFavStat = new FavStatInBookInfoViewModel.MyFavStatViewModel();
                    viewModel.MyFavStat.FavIds = myFavListInBookInfo.Select(m => m.ID).ToList();
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
                int currentUserId = ((UserIdentity)User.Identity).ID;
                this._favoriteService.Create(new Favorite
                {
                    IsOpen = inputModel.IsOpen,
                    Name = inputModel.Name,
                    Description = inputModel.Desc,
                    CreatorId = currentUserId
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
