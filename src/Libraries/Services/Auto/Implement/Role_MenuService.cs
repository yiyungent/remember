// Code generated by a template
// Project: Remember
// https://github.com/yiyungent/Remember
// Author: yiyun <yiyungent@gmail.com>
// LastUpadteTime: 2020-06-26 09:49:55
using Domain.Entities;
using Repositories.Interface;
using Services.Core;
using Services.Interface;

namespace Services.Implement
{
    public partial class Role_MenuService : BaseService<Role_Menu>, IRole_MenuService
    {
        private readonly IRole_MenuRepository _repository;
        public Role_MenuService(IRole_MenuRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}
