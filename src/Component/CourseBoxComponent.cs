using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseBoxComponent : BaseComponent<CourseBox, CourseBoxManager>, CourseBoxService
    {
    }
}
