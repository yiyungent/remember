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
    public partial class LogInfoService : BaseService<LogInfo>, ILogInfoService
    {
        private readonly ILogInfoRepository _repository;
        public LogInfoService(ILogInfoRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}
