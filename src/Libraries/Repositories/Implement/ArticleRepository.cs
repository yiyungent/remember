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
        private readonly RemDbContext _context;

        public ArticleRepository(RemDbContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<Article> FindHomePageArticles(int limit = 20)
        {
            return this._context.Articles.OrderByDescending(m => m.CreateTime).Take(limit);
        }
    }
}
