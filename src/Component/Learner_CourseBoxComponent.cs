using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class Learner_CourseBoxComponent : BaseComponent<Learner_CourseBox, Learner_CourseBoxManager>, Learner_CourseBoxService
    {
    }
}
