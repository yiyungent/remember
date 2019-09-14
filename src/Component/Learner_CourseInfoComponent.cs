using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class Learner_CourseInfoComponent : BaseComponent<Learner_CourseInfo, Learner_CourseInfoManager>, Learner_CourseInfoService
    {
    }
}
