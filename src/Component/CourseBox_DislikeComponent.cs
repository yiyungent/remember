using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseBox_DislikeComponent : BaseComponent<CourseBox_Dislike, CourseBox_DislikeManager>, CourseBox_DislikeService
    {
    }
}
