using Common;
using Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft;
using WebUI.Areas.Demo.Models;
using Service;
using Core;
using Domain;

namespace WebUI.Areas.Demo.Controllers
{
    public class BilibiliController : Controller
    {
        private CommentService commentService;
        private UserInfoService userInfoService;
        private IList<UserInfo> allUserInfo;

        public BilibiliController()
        {
            commentService = Container.Instance.Resolve<CommentService>();
            userInfoService = Container.Instance.Resolve<UserInfoService>();
            allUserInfo = userInfoService.GetAll();
        }

        public ViewResult Index()
        {
            return View();
        }

        public ActionResult GetVideo()
        {
            return View();
        }

        public JsonResult GetComments()
        {
            int pageNum = 1;
            long oid = 67500000;
            for (int j = 0; j < 100; j++)
            {
                long jsTimeStamp = DateTime.Now.ToTimeStamp13();
                string url = $"https://api.bilibili.com/x/v2/reply?pn={pageNum}&type=1&oid={oid}&sort=0&_={jsTimeStamp}";
                try
                {
                    string jsonStr = HttpAide.HttpGet(url: url);
                    CommentModel jsonModel = JsonConvert.DeserializeObject<CommentModel>(jsonStr);
                    Hot[] allReply = jsonModel.data.hots.Union(jsonModel.data.replies).ToArray();

                    for (int i = 0; i < allReply.Length; i++)
                    {
                        Hot hot = allReply[i];
                        try
                        {
                            commentService.Create(new Comment
                            {
                                Author = allUserInfo[new Random().Next(0, allUserInfo.Count - 1)],
                                Content = hot.content.message,
                                CreateTime = hot.ctime.ToDateTime10(),
                                LikeNum = hot.like,
                                LastUpdateTime = hot.ctime.ToDateTime10()
                            });
                        }
                        catch (Exception ex)
                        {
                        }
                        if (hot.replies != null && hot.replies.Length >= 1)
                        {
                            LoadSubComments(hot);
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                oid++;
            }

            return Json(new { code = 1, message = "success" }, JsonRequestBehavior.AllowGet);
        }

        private void LoadSubComments(Hot currentHot)
        {
            IList<Comment> allComment = Container.Instance.Resolve<CommentService>().GetAll();
            Hot[] hots = currentHot.replies;
            for (int i = 0; i < hots.Length; i++)
            {
                Hot hot = hots[i];
                try
                {
                    commentService.Create(new Comment
                    {
                        Author = allUserInfo[new Random().Next(0, allUserInfo.Count - 1)],
                        Content = hot.content.message,
                        CreateTime = hot.ctime.ToDateTime10(),
                        LikeNum = hot.like,
                        LastUpdateTime = hot.ctime.ToDateTime10(),
                        Parent = allComment.Where(m => m.Content.StartsWith(currentHot.content.message.Substring(0, 10))).FirstOrDefault()
                    });
                }
                catch (Exception ex)
                {
                }
                if (hot.replies != null && hot.replies.Length >= 1)
                {
                    LoadSubComments(hot);
                }
            }
        }
    }
}