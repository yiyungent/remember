// Code generated by a template
// Project: remember
// https://github.com/yiyungent/remember
// Author: yiyun <yiyungent@gmail.com>
// LastUpadteTime: 2020-07-06 10:36:54
using Domain.Entities;
using Repositories.Interface;
using Services.Core;
using Services.Interface;

namespace Services.Implement
{
    public partial class Follower_FollowedService : BaseService<Follower_Followed>, IFollower_FollowedService
    {
        private readonly IFollower_FollowedRepository _repository;
        public Follower_FollowedService(IFollower_FollowedRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}
