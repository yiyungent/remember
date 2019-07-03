using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CardBoxComponent : BaseComponent<CardBox, CardBoxManager>, CardBoxService
    {
    }
}
