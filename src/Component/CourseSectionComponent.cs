using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseSectionComponent : BaseComponent<CourseSection, CourseSectionManager>, CourseSectionService
    {
    }
}
