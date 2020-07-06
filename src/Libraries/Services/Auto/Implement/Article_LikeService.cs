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
    public partial class Article_LikeService : BaseService<Article_Like>, IArticle_LikeService
    {
        private readonly IArticle_LikeRepository _repository;
        public Article_LikeService(IArticle_LikeRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}
