using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseComponent : BaseComponent<Course, CourseManager>, CourseService
    {
    }
}
