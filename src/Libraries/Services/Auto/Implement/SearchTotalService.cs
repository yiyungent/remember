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
    public partial class SearchTotalService : BaseService<SearchTotal>, ISearchTotalService
    {
        private readonly ISearchTotalRepository _repository;
        public SearchTotalService(ISearchTotalRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}
