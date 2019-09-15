using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseInfo_CommentComponent : BaseComponent<CourseInfo_Comment, CourseInfo_CommentManager>, CourseInfo_CommentService
    {
    }
}
