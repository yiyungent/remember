using Domain.Entities;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public partial interface IUserInfoService : IService<UserInfo>
    {
        bool Exists(string userName, int exceptId = 0);

        IList<string> UserHaveAuthKeys(int userId);

        IList<Sys_Menu> UserHaveSys_Menus(int userId);

        /// <summary>
        /// 关注/取消关注
        /// </summary>
        /// <param name="uid">关注者用户ID</param>
        /// <param name="followUId">被关注者用户ID</param>
        /// <param name="act">关注：1，取消关注：2</param>
        void Follow(int followerId, int followedId, out string message, int act = 1);

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
        int Relation(int meUID, int himUID);

        /// <summary>
        /// 获取此人的关注数和粉丝数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="following"></param>
        /// <param name="fans"></param>
        void FollowAndFans(int uid, out int follow, out int fans);

        /// <summary>
        /// 批量删除, 标记删除
        /// </summary>
        /// <param name="ids"></param>
        int BatchDelete(string ids);
    }
}
