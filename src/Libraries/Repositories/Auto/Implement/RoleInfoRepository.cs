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
    public partial class RoleInfoRepository : BaseRepository<RoleInfo>, IRoleInfoRepository
    {
        private readonly RemDbContext _context;

        public RoleInfoRepository(RemDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
