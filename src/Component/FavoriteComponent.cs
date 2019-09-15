using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class FavoriteComponent : BaseComponent<Favorite, FavoriteManager>, FavoriteService
    {
    }
}
