using Domain.Entities;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IArticleService : IService<Article>
    {
        /// <summary>
        /// 查询首页文章列表
        /// </summary>
        /// <param name="limit">要查询的记录数</param>
        /// <returns></returns>
        IEnumerable<Article> FindHomePageArticles(int limit = 20);
    }
}
