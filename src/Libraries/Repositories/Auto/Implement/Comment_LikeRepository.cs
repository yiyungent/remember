// Code generated by a template
// Project: Remember
// LastUpadteTime: 2019-10-01 11:03:23
using Domain.Entities;
using Repositories.Core;
using Repositories.Interface;
using System.Data.Entity;
using System.Linq;

namespace Repositories.Implement
{
    public partial class Comment_LikeRepository : BaseRepository<Comment_Like>, IComment_LikeRepository
    {
        private readonly RemDbContext _context;

        public Comment_LikeRepository(RemDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}