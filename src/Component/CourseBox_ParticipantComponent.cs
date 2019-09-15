using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CourseBox_ParticipantComponent : BaseComponent<CourseBox_Participant, CourseBox_ParticipantManager>, CourseBox_ParticipantService
    {
    }
}
