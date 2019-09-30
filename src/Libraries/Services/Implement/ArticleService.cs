using Domain.Entities;
using Repositories.Core;
using Repositories.Interface;
using Services.Core;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class ArticleService : BaseService<Article>, IArticleService
    {
        private readonly IArticleRepository _repository;
        public ArticleService(IArticleRepository repository) : base(repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询首页文章列表
        /// </summary>
        /// <param name="limit">要查询的记录数</param>
        /// <returns></returns>
        public IEnumerable<Article> FindHomePageArticles(int limit = 20)
        {
            return _repository.FindHomePageArticles(limit);
        }
    }
}
