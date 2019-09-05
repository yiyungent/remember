using Core;
using Domain;
using Framework.Infrastructure.Concrete;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.CardBoxVM;
using WebUI.Areas.Admin.Models.CardInfoVM;
using WebUI.Extensions;
using WebUI.Models.CardBoxVM;

namespace WebUI.Controllers
{
    public class CardBoxController : Controller
    {
        #region Fields
        private CardInfoService _cardInfoService;

        private CardBoxService _cardBoxService;
        #endregion

        #region Ctor
        public CardBoxController()
        {
            this._cardInfoService = Container.Instance.Resolve<CardInfoService>();
            this._cardBoxService = Container.Instance.Resolve<CardBoxService>();
        }
        #endregion

        #region 卡片盒首页
        [HttpGet]
        /// <summary>
        /// 卡片盒首页
        /// </summary>
        /// <param name="q">搜索关键词</param>
        /// <param name="cat">分类：市场上的卡片盒,我创建的卡片盒，我阅读的卡片盒</param>
        /// <returns></returns>
        public ActionResult Index(string cat, string q = null)
        {
            return View();
        }
        #endregion

        #region 卡片盒描述页
        [HttpGet]
        /// <summary>
        /// 卡片盒描述页
        /// </summary>
        /// <param name="id">卡片盒Id</param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion

        #region 卡片盒的目录页
        public ViewResult Catalog(int id)
        {
            return View();
        }
        #endregion

        #region 卡片页
        /// <summary>
        /// 卡片页
        /// </summary>
        /// <param name="id">卡片Id</param>
        /// <returns></returns>
        public ActionResult Card(int id = 0)
        {
            CardInfoViewModel viewModel = null;
            if (_cardInfoService.Exist(id))
            {
                CardInfo dbModel = _cardInfoService.GetEntity(id);
                viewModel = (CardInfoViewModel)dbModel;
            }

            return View(viewModel);
        }
        #endregion

        #region 评论编辑页
        public ActionResult Comment()
        {
            return View();
        }
        #endregion

        #region 搜索卡片
        [HttpGet]
        public ActionResult Search()
        {
            string q = Request.QueryString["q"]?.Trim();
            IList<ICriterion> queryConditions = new List<ICriterion>();
            //if (cardBoxId != 0)
            //{
            //    queryConditions.Add(Expression.Eq("CardBox.ID", cardBoxId));
            //}
            SearchResultViewModel viewModel = new SearchResultViewModel();
            if (!string.IsNullOrEmpty(q))
            {
                queryConditions.Add(Expression.Like("Content", q, MatchMode.Anywhere));
                IList<CardInfo> cardInfos = _cardInfoService.Query(queryConditions);
                foreach (var item in cardInfos)
                {
                    SearchResultItem resultItem = new SearchResultItem();
                    resultItem.CardBoxId = item.CardBox.ID;
                    resultItem.CardInfoId = item.ID;
                    if (!string.IsNullOrEmpty(item.Content) && item.Content.Trim().Length > 10)
                    {
                        resultItem.Title = item.Content.Substring(0, 10);
                    }
                    else
                    {
                        resultItem.Title = item.Content;
                    }
                    if (!string.IsNullOrEmpty(item.Content) && item.Content.Trim().Length > 30)
                    {
                        resultItem.Description = item.Content.Substring(0, 30);
                    }
                    else
                    {
                        resultItem.Description = item.Content;
                    }
                    resultItem.Url = Url.Action("Card", new { id = item.ID });
                    viewModel.List.Add(resultItem);
                }
            }

            ViewBag.Q = q;

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Search(string q = null)
        {
            IList<ICriterion> queryConditions = new List<ICriterion>();
            SearchResultViewModel viewModel = new SearchResultViewModel();
            if (!string.IsNullOrEmpty(q))
            {
                queryConditions.Add(Expression.Like("Content", q, MatchMode.Anywhere));
                IList<CardInfo> cardInfos = _cardInfoService.Query(queryConditions);
                foreach (var item in cardInfos)
                {
                    SearchResultItem resultItem = new SearchResultItem();
                    resultItem.CardBoxId = item.CardBox.ID;
                    resultItem.CardInfoId = item.ID;
                    if (!string.IsNullOrEmpty(item.Content) && item.Content.Trim().Length > 10)
                    {
                        resultItem.Title = item.Content.Substring(0, 10);
                    }
                    else
                    {
                        resultItem.Title = item.Content;
                    }
                    if (!string.IsNullOrEmpty(item.Content) && item.Content.Trim().Length > 30)
                    {
                        resultItem.Description = item.Content.Substring(0, 30);
                    }
                    else
                    {
                        resultItem.Description = item.Content;
                    }
                    resultItem.Url = Url.Action("Card", new { id = item.ID });
                    viewModel.List.Add(resultItem);
                }
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 搜索卡片盒
        public ViewResult SearchCardBox()
        {
            return View();
        }
        #endregion

        #region 创建卡片盒
        [HttpGet]
        public ViewResult Create()
        {
            CardBoxViewModel viewModel = new CardBoxViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Create(CardBoxViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {
                    #region 数据有效效验

                    #endregion

                    UserInfo creator = AccountManager.GetCurrentUserInfo();
                    CardBox dbModel = (CardBox)inputModel;
                    dbModel.Creator = creator;
                    dbModel.CreateTime = DateTime.UtcNow;
                    dbModel.LastUpdateTime = DateTime.UtcNow;
                    
                    #region 失败
                    //CardInfo parentCardInfo = new CardInfo
                    //{
                    //    Title = "分类1",
                    //    CardBox = dbModel
                    //};
                    //_cardInfoService.Create(parentCardInfo);
                    //CardInfo cardInfo = new CardInfo
                    //{
                    //    Title = "示例1",
                    //    Content = "示例内容，可尝试编辑本页面",
                    //    Parent = parentCardInfo,
                    //    CardBox = dbModel
                    //};
                    //dbModel.CardInfoList.Add(parentCardInfo);
                    //dbModel.CardInfoList.Add(cardInfo); 
                    #endregion

                    _cardBoxService.Create(dbModel);

                    CardBox createdCardBox = _cardBoxService.Query(new List<ICriterion>
                    {
                        Expression.Eq("Creator.ID", creator.ID)
                    }).OrderByDescending(m => m.ID).Take(1).ToList()[0];

                    #region 添加示例内容
                    CardInfo cardInfo = new CardInfo
                    {
                        Content = "示例内容，可尝试编辑本页面",
                        CardBox = createdCardBox
                    };
                    _cardInfoService.Create(cardInfo);
                    #endregion



                    return Json(new { code = 1, message = "添加成功" });
                }
                else
                {
                    string errorMessage = ModelState.GetErrorMessage();
                    return Json(new { code = -1, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "添加失败" });
            }
        }
        #endregion
    }
}