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
            // TODO: 原本这样缓存 每个人拥有的缓存键，不合适，如果修改角色的权限，则此角色下的所有用户拥有的权限键也需要更新缓存
            //IList<string> authKeys = CacheHelper.Get<IList<string>>($"UserHaveAuthKeys({userId})");
            IList<string> authKeys = null;
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
                        var roleInfos = userInfo.Role_Users.Where(m => !m.IsDeleted).Select(m => m.RoleInfo);
                        foreach (var role in roleInfos)
                        {
                            var role_funcs = role.Role_Functions;
                            if (role_funcs != null && role_funcs.Count >= 1)
                            {
                                var funcs = role_funcs.Where(m => !m.IsDeleted).Select(m => m.FunctionInfo);
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
                }
                else
                {
                    // 游客
                    RoleInfo roleInfo = ContainerManager.Resolve<IRoleInfoService>().Find(m => m.ID == 2);
                    var role_funcs = roleInfo.Role_Functions;
                    if (role_funcs != null && role_funcs.Count >= 1)
                    {
                        var funcs = role_funcs.Where(m => !m.IsDeleted).Select(m => m.FunctionInfo);
                        foreach (var func in funcs)
                        {
                            if (!authKeys.Contains(func.AuthKey, authKeyCompare))
                            {
                                authKeys.Add(func.AuthKey);
                            }
                        }
                    }
                }

                //CacheHelper.Insert<IList<string>>($"UserHaveAuthKeys({userId})", authKeys);
            }

            return authKeys;
        }

        public IList<Sys_Menu> UserHaveSys_Menus(int userId)
        {
            IList<Sys_Menu> menuList = new List<Sys_Menu>();
            UserInfo userInfo = this.Find(m => m.ID == userId && !m.IsDeleted);
            if (userInfo != null)
            {
                var role_Users = userInfo.Role_Users.Where(m => !m.IsDeleted).Select(m => m.RoleInfo);
                IEnumerable<Sys_Menu> sys_Menus;
                foreach (RoleInfo role in role_Users)
                {
                    sys_Menus = role.Role_Menus.Where(m => !m.IsDeleted).Select(m => m.Sys_Menu);
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

        #region 批量删除
        /// <summary>
        /// 批量删除, 标记删除
        /// </summary>
        /// <param name="ids"></param>
        public int BatchDelete(string ids)
        {
            int successCount = 0;
            string[] idArr = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            int id = 0;
            foreach (var idStr in idArr)
            {
                id = Convert.ToInt32(idStr);
                var dbModel = this._repository.Find(m => m.ID == id && !m.IsDeleted);
                if (dbModel != null)
                {
                    dbModel.IsDeleted = true;
                    dbModel.DeletedAt = DateTime.Now;
                    this._repository.Update(dbModel);
                    successCount++;
                }
            }
            this._repository.SaveChanges();

            return successCount;
        }
        #endregion
    }
}
