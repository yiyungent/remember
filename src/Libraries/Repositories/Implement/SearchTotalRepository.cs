using Domain.Entities;
using Repositories.Core;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public partial class SearchTotalRepository : BaseRepository<SearchTotal>, ISearchTotalRepository
    {
        /// <summary>
        /// 获取热词列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IList<string> GetKeyWordList(string keyword)
        {
            return this._context.SearchTotal.Where(m => m.KeyWord.StartsWith(keyword)).Select(m => m.KeyWord).ToList();
        }
    }
}
