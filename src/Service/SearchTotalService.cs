using Domain;
using Service.Base;
using System.Collections.Generic;

namespace Service
{
    public interface SearchTotalService : BaseService<SearchTotal>
    {
        IList<string> GetKeyWordList(string keyword);
    }
}
