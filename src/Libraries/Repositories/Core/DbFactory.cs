using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Core
{
    /// <summary>
    /// 数据库工厂
    /// TODO: 暂时未使用：而是使用 Autofac 依赖注入 DbContext，并设置每个请求一个DbContext实例，也可改为从这里获取，也可以实现线程内唯一
    /// </summary>
    public class DbFactory
    {
        /// <summary>
        /// 负责创建EF数据操作上下文实例，必须保证线程内唯一
        /// </summary>
        public static DbContext GetDbContext()
        {
            DbContext dbContext = (DbContext)CallContext.GetData("DbContext");
            if (dbContext == null)
            {
                dbContext = new RemDbContext();
                CallContext.SetData("DbContext", dbContext);
            }

            return dbContext;
        }
    }
}
