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
            // ���� ���û�ӵ�еĽ�ɫ �¾ɲ���
            var role_Users = this._repository.Filter(m => m.UserInfoId == userId);
            IList<int> old_Role_Ids = new List<int>();
            if (role_Users != null && role_Users.Count() >= 1)
            {
                old_Role_Ids = role_Users.Select(m => m.RoleInfoId).ToList();
            }
            IList<int> needDelete_Role_Ids = new List<int>();
            IList<int> needAdd_Role_Ids = new List<int>();
            // ���д��ڣ������в����ڣ���ɾ��
            foreach (var item in old_Role_Ids)
            {
                if (!roleIds.Contains(item))
                {
                    needDelete_Role_Ids.Add(item);
                }
            }
            // �ɵ��в������ľ�����Ҫ ������
            foreach (var item in roleIds)
            {
                if (!old_Role_Ids.Contains(item))
                {
                    needAdd_Role_Ids.Add(item);
                }
            }
            // ɾ��
            IList<Role_User> needDelete_Role_Users = role_Users.Where(m => m.UserInfoId == userId && needDelete_Role_Ids.Contains(m.RoleInfoId)).ToList();
            foreach (var item in needDelete_Role_Users)
            {
                this._repository.Delete(item);
            }
            // ����
            foreach (var item in needAdd_Role_Ids)
            {
                this._repository.Create(new Role_User
                {
                    UserInfoId = userId,
                    RoleInfoId = item,
                    CreateTime = DateTime.Now,
                });
            }
            // ͳһ����
            this._repository.SaveChanges();
        }
    }
}
