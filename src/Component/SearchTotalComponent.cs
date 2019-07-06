using Component.Base;
using Domain;
using Manager;
using Service;
using System.Collections.Generic;

namespace Component
{
    public class SearchTotalComponent : BaseComponent<SearchTotal, SearchTotalManager>, SearchTotalService
    {
        public IList<string> GetKeyWordList(string keyword)
        {
            return _manager.GetKeyWordList(keyword);
        }
    }
}
