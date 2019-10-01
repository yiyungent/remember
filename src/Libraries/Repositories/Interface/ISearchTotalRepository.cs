using Domain.Entities;
using Repositories.Core;
using System.Collections.Generic;

namespace Repositories.Interface
{
    public partial interface ISearchTotalRepository : IRepository<SearchTotal>
    {
        /// <summary>
        /// 获取热词列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        IList<string> GetKeyWordList(string keyword);
    }
}
