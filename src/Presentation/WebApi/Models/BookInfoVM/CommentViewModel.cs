using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.BookInfoVM
{
    public class CommentViewModel
    {
        public int ID { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// js时间戳
        /// </summary>
        public long CreateTime { get; set; }

        public int Like { get; set; }

        public int Dislike { get; set; }

        public bool IsLogin { get; set; }

        /// <summary>
        /// 我赞？
        /// </summary>
        public bool IsMeLike { get; set; }

        /// <summary>
        /// 我踩？
        /// </summary>
        public bool IsMeDislike { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// 此条评论回复某条评论的ID
        /// 没有回复某条评论，则为 0
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 回复此条评论的评论列表
        /// 没有评论回复此条评论，则为 null
        /// </summary>
        public IList<CommentViewModel> Children { get; set; }

        public sealed class AuthorViewModel
        {
            public int ID { get; set; }

            public string UserName { get; set; }

            public string Desc { get; set; }

            public string Avatar { get; set; }
        }
    }

    public class CommentListLoadViewModel
    {
        public PageViewModel Page { get; set; }

        public IList<CommentViewModel> Comments { get; set; }

        public sealed class PageViewModel
        {
            /// <summary>
            /// 第几页
            /// </summary>
            public int PageNum { get; set; }

            /// <summary>
            /// 每页大小
            /// </summary>
            public int PageSize { get; set; }

            /// <summary>
            /// 总共大小
            /// </summary>
            public int Count { get; set; }
        }
    }
}