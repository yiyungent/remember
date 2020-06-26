// Code generated by a template
// Project: Remember
// https://github.com/yiyungent/Remember
// Author: yiyun <yiyungent@gmail.com>
// LastUpadteTime: 2020-06-26 09:47:00
using Domain.Entities;
using Repositories.Core;
using Repositories.Interface;

namespace Repositories.Implement
{
    public partial class Article_CommentRepository : BaseRepository<Article_Comment>, IArticle_CommentRepository
    {
        private readonly RemDbContext _context;

        public Article_CommentRepository(RemDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
