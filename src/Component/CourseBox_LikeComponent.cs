using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseBox_LikeComponent : BaseComponent<CourseBox_Like, CourseBox_LikeManager>, CourseBox_LikeService
    {
    }
}
