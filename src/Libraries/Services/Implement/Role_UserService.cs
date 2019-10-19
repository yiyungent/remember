using Domain.Entities;
using Repositories.Interface;
using Services.Core;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Implement
{
    public partial class Role_UserService : BaseService<Role_User>, IRole_UserService
    {
        public void UserAssignRoles(int userId, IList<int> roleIds, int operatorId)
        {
            // 计算 此用户拥有的角色 新旧差异
            var role_Users = this._repository.Filter(m => m.UserInfoId == userId && !m.IsDeleted);
            IList<int> old_Role_Ids = new List<int>();
            if (role_Users != null && role_Users.Count() >= 1)
            {
                old_Role_Ids = role_Users.Select(m => m.RoleInfoId).ToList();
            }
            IList<int> needDelete_Role_Ids = new List<int>();
            IList<int> needAdd_Role_Ids = new List<int>();
            // 旧中存在，但新中不存在，则删除
            foreach (var item in old_Role_Ids)
            {
                if (!roleIds.Contains(item))
                {
                    needDelete_Role_Ids.Add(item);
                }
            }
            // 旧的中不包含的就是需要 新增的
            foreach (var item in roleIds)
            {
                if (!old_Role_Ids.Contains(item))
                {
                    needAdd_Role_Ids.Add(item);
                }
            }
            // 删除
            IList<Role_User> needDelete_Role_Users = role_Users.Where(m => m.UserInfoId == userId && needDelete_Role_Ids.Contains(m.RoleInfoId)).ToList();
            foreach (var item in needDelete_Role_Users)
            {
                item.DeletedAt = DateTime.Now;
                item.IsDeleted = true;
                this._repository.Update(item);
            }
            // 新增
            foreach (var item in needAdd_Role_Ids)
            {
                this._repository.Create(new Role_User
                {
                    UserInfoId = userId,
                    RoleInfoId = item,
                    CreateTime = DateTime.Now,
                    OperatorId = operatorId
                });
            }
            // 统一保存
            this._repository.SaveChanges();
        }
    }
}
