using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class VideoInfoComponent : BaseComponent<VideoInfo, VideoInfoManager>, VideoInfoService
    {
    }
}
