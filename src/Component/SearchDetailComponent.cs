using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class SearchDetailComponent : BaseComponent<SearchDetail, SearchDetailManager>, SearchDetailService
    {
    }
}
