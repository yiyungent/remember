using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class Follower_FollowedComponent : BaseComponent<Follower_Followed, Follower_FollowedManager>, Follower_FollowedService
    {
    }
}
