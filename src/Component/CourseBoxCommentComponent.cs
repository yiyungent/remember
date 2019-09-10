using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseBoxCommentComponent : BaseComponent<CourseBoxComment, CourseBoxCommentManager>, CourseBoxCommentService
    {
    }
}
