using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class Comment_DislikeComponent : BaseComponent<Comment_Dislike, Comment_DislikeManager>, Comment_DislikeService
    {
    }
}
