using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class Favorite_CourseBoxComponent : BaseComponent<Favorite_CourseBox, Favorite_CourseBoxManager>, Favorite_CourseBoxService
    {
    }
}
