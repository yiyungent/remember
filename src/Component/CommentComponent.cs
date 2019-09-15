using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class CommentComponent : BaseComponent<Comment, CommentManager>, CommentService
    {
    }
}
