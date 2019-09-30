using Domain.Entities;
using Repositories.Core;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        private readonly RemDbContext _dbContext;

        public ArticleRepository(RemDbContext context) : base(context)
        {
            this._dbContext = context;
        }

        public IQueryable<Article> FindHomePageArticles(int limit = 20)
        {
            return this._dbContext.Articles.OrderByDescending(m => m.CreateTime).Take(limit);
        }
    }
}
