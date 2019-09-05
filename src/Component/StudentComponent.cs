using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class StudentComponent : BaseComponent<Student, StudentManager>, StudentService
    {
    }
}
