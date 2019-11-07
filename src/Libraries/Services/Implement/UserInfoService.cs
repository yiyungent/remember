using Core;
using Core.Common.Cache;
using Domain;
using Domain.Entities;
using Services.Core;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public bool Exists(string userName, int exceptId = 0)
        {
            bool isExist = this._repository.Count(
                m => m.UserName == userName
                && m.ID != exceptId
                && !m.IsDeleted
            ) > 0;

            return isExist;
        }

        public IList<string> UserHaveAuthKeys(int userId)
        {
            IList<string> authKeys = CacheHelper.Get<IList<string>>($"UserHaveAuthKeys({userId})");
            if (authKeys == null)
            {
                authKeys = new List<string>();
                var authKeyCompare = new AuthKeyCompare();
                if (userId != 0)
                {
                    // 非游客
                    UserInfo userInfo = this.Find(m => m.ID == userId && !m.IsDeleted);
                    if (userInfo.Role_Users != null && userInfo.Role_Users.Count >= 1)
                    {
                        var roleInfos = userInfo.Role_Users.Select(m => m.RoleInfo);
                        foreach (var role in roleInfos)
                        {
                            var funcs = role.FunctionInfos;
                            foreach (var func in funcs)
                            {
                                if (!authKeys.Contains(func.AuthKey, authKeyCompare))
                                {
                                    authKeys.Add(func.AuthKey);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // 游客
                    RoleInfo roleInfo = ContainerManager.Resolve<IRoleInfoService>().Find(m => m.ID == 2);
                    foreach (var func in roleInfo.FunctionInfos)
                    {
                        if (!authKeys.Contains(func.AuthKey, authKeyCompare))
                        {
                            authKeys.Add(func.AuthKey);
                        }
                    }
                }

                CacheHelper.Insert<IList<string>>($"UserHaveAuthKeys({userId})", authKeys);
            }

            return authKeys;
        }

        public IList<Sys_Menu> UserHaveSys_Menus(int userId)
        {
            IList<Sys_Menu> menuList = new List<Sys_Menu>();
            UserInfo userInfo = this.Find(m => m.ID == userId && !m.IsDeleted);
            if (userInfo != null)
            {
                var role_Users = userInfo.Role_Users.Select(m => m.RoleInfo);
                IEnumerable<Sys_Menu> sys_Menus;
                foreach (RoleInfo role in role_Users)
                {
                    sys_Menus = role.Role_Menus.Select(m => m.Sys_Menu);
                    foreach (Sys_Menu menu in sys_Menus)
                    {
                        if (!menuList.Contains(menu, new Sys_Menu_Compare()))
                        {
                            menuList.Add(menu);
                        }
                    }
                }
            }

            return menuList;
        }

        #region 关注/取消关注
        /// <summary>
        /// 关注/取消关注
        /// </summary>
        /// <param name="followerId">关注者用户ID</param>
        /// <param name="followUId">被关注者用户ID</param>
        /// <param name="act">关注：1，取消关注：2</param>
        public void Follow(int followerId, int followedId, out string message, int act = 1)
        {
            message = "";
            IFollower_FollowedService follower_FollowedService = ContainerManager.Resolve<IFollower_FollowedService>();
            bool isFollowed = follower_FollowedService.Contains(m => m.FollowerId == followerId && m.FollowedId == followedId && !m.IsDeleted);
            if (act == 1)
            {
                if (!isFollowed)
                {
                    // 未关注-》关注
                    follower_FollowedService.Create(new Follower_Followed
                    {
                        FollowerId = followerId,
                        FollowedId = followedId,
                        CreateTime = DateTime.Now,
                        IsDeleted = false
                    });

                    message = "关注成功";
                }
                else
                {
                    message = "已关注";
                }
            }
            else if (act == 2)
            {
                if (isFollowed)
                {
                    // 已经关注-》取消关注
                    Follower_Followed follower_Followed = follower_FollowedService.Find(m => m.FollowerId == followerId && m.FollowedId == followedId && !m.IsDeleted);
                    follower_Followed.DeletedAt = DateTime.Now;
                    follower_Followed.IsDeleted = true;
                    follower_FollowedService.Update(follower_Followed);

                    message = "取消关注成功";
                }
                else
                {
                    message = "已取消关注";
                }
            }
        }
        #endregion

        #region 获取关注关系
        /// <summary>
        /// 获取关注关系
        /// 0: 没有关系
        /// 1: 我单方面关注他
        /// 2: 他单方面关注我
        /// 3: 互相关注
        /// </summary>
        /// <param name="meUID">我的用户ID</param>
        /// <param name="himUID">他的用户ID</param>
        /// <param name="relation"></param>
        public int Relation(int meUID, int himUID)
        {
            int relation = 0;
            IFollower_FollowedService follower_FollowedService = ContainerManager.Resolve<IFollower_FollowedService>();

            // 我有 关注 他 吗？
            bool isMeFollowHim = follower_FollowedService.Contains(m => m.FollowerId == meUID && m.FollowedId == himUID && !m.IsDeleted);
            // 他 有 关注 我 吗？
            bool isHimFollowMe = follower_FollowedService.Contains(m => m.FollowedId == meUID && m.FollowerId == himUID && !m.IsDeleted);

            if (isMeFollowHim && !isHimFollowMe)
            {
                relation = 1;
            }
            else if (!isMeFollowHim && isHimFollowMe)
            {
                relation = 2;
            }
            else if (isMeFollowHim && isHimFollowMe)
            {
                relation = 3;
            }

            return relation;
        }
        #endregion

        #region 获取此人的关注数和粉丝数
        /// <summary>
        /// 获取此人的关注数和粉丝数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="following"></param>
        /// <param name="fans"></param>
        public void FollowAndFans(int uid, out int follow, out int fans)
        {
            follow = 0;
            fans = 0;

            IFollower_FollowedService follower_FollowedService = ContainerManager.Resolve<IFollower_FollowedService>();
            follow = follower_FollowedService.Count(m => m.FollowerId == uid && !m.IsDeleted);
            fans = follower_FollowedService.Count(m => m.FollowedId == uid && !m.IsDeleted);
        }
        #endregion


    }
}
