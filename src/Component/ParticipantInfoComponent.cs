using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class ParticipantInfoComponent : BaseComponent<ParticipantInfo, ParticipantInfoManager>, ParticipantInfoService
    {
    }
}
