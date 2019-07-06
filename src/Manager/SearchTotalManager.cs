using Domain;
using Manager.Base;
using Manager.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Manager
{
    public class SearchTotalManager : BaseManager<SearchTotal>
    {
        public IList<string> GetKeyWordList(string keyword)
        {
            return _efDbContext.SearchTotal.Where(m => m.KeyWord.StartsWith(keyword)).Select(m => m.KeyWord).ToList();
        }
    }
}
