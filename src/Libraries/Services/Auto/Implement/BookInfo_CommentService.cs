// Code generated by a template
// Project: Remember
// LastUpadteTime: 2019-12-03 10:35:07
using Domain.Entities;
using Repositories.Interface;
using Services.Core;
using Services.Interface;

namespace Services.Implement
{
    public partial class BookInfo_CommentService : BaseService<BookInfo_Comment>, IBookInfo_CommentService
    {
        private readonly IBookInfo_CommentRepository _repository;
        public BookInfo_CommentService(IBookInfo_CommentRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}
