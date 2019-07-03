using Domain;
using Manager.Base;
using NHibernate.Criterion;

namespace Manager
{
    public class UserInfoManager : BaseManager<UserInfo>
    {
        public bool Exist(string userName, int exceptId = 0)
        {
            bool isExist = Count(Expression.And(
                                Expression.Eq("UserName", userName),
                                Expression.Not(Expression.Eq("ID", exceptId))
                            )) > 0;

            return isExist;
        }
    }
}
