using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseBox_CommentComponent : BaseComponent<CourseBox_Comment, CourseBox_CommentManager>, CourseBox_CommentService
    {
    }
}
