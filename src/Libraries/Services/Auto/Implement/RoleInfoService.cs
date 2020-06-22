// Code generated by a template
// Project: Remember
// LastUpadteTime: 2020-06-22 05:06:48
using Domain.Entities;
using Repositories.Interface;
using Services.Core;
using Services.Interface;

namespace Services.Implement
{
    public partial class RoleInfoService : BaseService<RoleInfo>, IRoleInfoService
    {
        private readonly IRoleInfoRepository _repository;
        public RoleInfoService(IRoleInfoRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}
