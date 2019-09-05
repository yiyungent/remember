using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseBoxTableComponent : BaseComponent<CourseBoxTable, CourseBoxTableManager>, CourseBoxTableService
    {
    }
}
