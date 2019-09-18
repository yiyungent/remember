using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class VideoInfo_CommentComponent : BaseComponent<VideoInfo_Comment, VideoInfo_CommentManager>, VideoInfo_CommentService
    {
    }
}
