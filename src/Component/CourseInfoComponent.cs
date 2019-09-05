using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseInfoComponent : BaseComponent<CourseInfo, CourseInfoManager>, CourseInfoService
    {
    }
}
