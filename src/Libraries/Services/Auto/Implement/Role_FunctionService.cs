// Code generated by a template
// Project: Remember
// LastUpadteTime: 2019-12-03 10:35:07
using Domain.Entities;
using Repositories.Interface;
using Services.Core;
using Services.Interface;

namespace Services.Implement
{
    public partial class Role_FunctionService : BaseService<Role_Function>, IRole_FunctionService
    {
        private readonly IRole_FunctionRepository _repository;
        public Role_FunctionService(IRole_FunctionRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}
