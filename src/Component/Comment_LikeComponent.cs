using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class Comment_LikeComponent : BaseComponent<Comment_Like, Comment_LikeManager>, Comment_LikeService
    {
    }
}
