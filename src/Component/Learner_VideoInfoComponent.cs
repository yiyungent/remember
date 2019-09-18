using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class Learner_VideoInfoComponent : BaseComponent<Learner_VideoInfo, Learner_VideoInfoManager>, Learner_VideoInfoService
    {
    }
}
