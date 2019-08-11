using Core;
using Domain;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.CardInfoVM;

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

        #region 卡片盒市场首页
        /// <summary>
        /// 卡片盒市场首页
        /// </summary>
        /// <param name="q">搜索关键词</param>
        /// <returns></returns>
        public ActionResult Index(string q = null)
        {
            return View();
        }
        #endregion

        #region 卡片盒描述页
        /// <summary>
        /// 卡片盒描述页
        /// </summary>
        /// <param name="id">卡片盒Id</param>
        /// <returns></returns>
        public ActionResult Index(int id, string q = null)
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
        public ActionResult Card(int id = 0, string q = null)
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
    }
}