using Common;
using Framework.Common;
using MiniCrawler.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniCrawler
{
    public class BiliVideoComment
    {

        /// <summary>
        /// 尝试从指定 oid 的视频号开始递进oid号，保存其视频下的热门评论
        /// </summary>
        /// <param name="oid">oid号，eg:67500000</param>
        /// <param name="length">一次性递进的长度</param>
        public static void TryVideosComments(long oid = 67500000, int length = 100)
        {
            try
            {
                EF.RemDbContext remDbContext = new EF.RemDbContext();
                IList<EF.userinfo> allUserInfo = null;
                try
                {
                    allUserInfo = remDbContext.userinfoes.ToList();

                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.InnerException.Message);
                    //throw;
                }

                int pageNum = 1;
                for (int j = 0; j < length; j++)
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
                                remDbContext.comments.Add(new EF.comment
                                {
                                    AuthorId = new Random().Next(0, allUserInfo.Count - 1),
                                    Content = hot.content.message,
                                    CreateTime = hot.ctime.ToDateTime10(),
                                    LikeNum = hot.like,
                                    LastUpdateTime = hot.ctime.ToDateTime10()
                                });
                                remDbContext.SaveChanges();

                                Console.WriteLine($"★★★★★★★★★★★★★★★★★★抓取 {hot.content.message}★★★★★★★★★★★★★★★★★★★★★★★★★★★");
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine(ex.Message);
                                if (ex.InnerException != null)
                                {
                                    //Console.WriteLine(ex.Message);
                                }
                            }
                            if (hot.replies != null && hot.replies.Length >= 1)
                            {
                                LoadSubComments(hot);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                        if (ex.InnerException != null)
                        {
                            //Console.WriteLine(ex.Message);
                        }
                    }

                    oid++;
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }


        private static void LoadSubComments(Hot currentHot)
        {
            EF.RemDbContext remDbContext = new EF.RemDbContext();
            var allComment = remDbContext.comments.ToList();
            Hot[] hots = currentHot.replies;
            for (int i = 0; i < hots.Length; i++)
            {
                Hot hot = hots[i];
                try
                {
                    remDbContext.comments.Add(new EF.comment
                    {
                        AuthorId = new Random().Next(0, remDbContext.userinfoes.Count() - 1),
                        Content = hot.content.message,
                        CreateTime = hot.ctime.ToDateTime10(),
                        LikeNum = hot.like,
                        LastUpdateTime = hot.ctime.ToDateTime10(),
                        ParentId = allComment.Where(m => m.Content.StartsWith(currentHot.content.message.Substring(0, 10))).FirstOrDefault().ID
                    });
                    remDbContext.SaveChanges();
                    Console.WriteLine($"★★★★★★★★★★★★★★★★★★抓取 {hot.content.message}★★★★★★★★★★★★★★★★★★★★★★★★★★★");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                    {
                        //Console.WriteLine(ex.InnerException.Message);
                    }
                }
                if (hot.replies != null && hot.replies.Length >= 1)
                {
                    LoadSubComments(hot);
                }
            }
        }

    }
}
