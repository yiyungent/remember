using Core;
using Core.Common.Cache;
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

        #region 权限(操作)键相等比较器
        public class AuthKeyCompare : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return x.ToLower() == y.ToLower();
            }

            public int GetHashCode(string obj)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
