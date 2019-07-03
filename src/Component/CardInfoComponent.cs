using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CardInfoComponent : BaseComponent<CardInfo, CardInfoManager>, CardInfoService
    {
    }
}
