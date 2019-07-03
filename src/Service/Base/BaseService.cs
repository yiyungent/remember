using Domain.Base;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace Service.Base
{
    /// <summary>
    /// 服务接口的基类
    /// </summary>
    public interface BaseService<T>
        where T : BaseEntity<T>, new()
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        void Create(T t);

        /// <summary>
        /// 根据实体删除
        /// </summary>
        void Delete(T t);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        void Delete(int id);

        /// <summary>
        /// 修改实体
        /// </summary>
        void Edit(T t);

        /// <summary>
        /// 查询
        /// </summary>
        IList<T> Query(IList<ICriterion> condition);

        /// <summary>
        /// 获取全部实体
        /// </summary>
        IList<T> GetAll();

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        T GetEntity(int id);

        int Count(params ICriterion[] criteria);

        bool Exist(int id);

        //分页区和取对象集合
        IList<T> GetPaged(IList<ICriterion> queryConditions, IList<Order> orderList, int pageIndex, int pageSize, out int count);

        /// <summary>
        /// 根据查询条件分页获取实体
        /// </summary>
        /// <param name="queryConditions">查询条件集合</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="count">返回满足查询条件</param>
        /// <returns>返回满足查询条件的实体</returns>
        IList<T> GetPaged(IList<KeyValuePair<string, string>> queryConditions, int pageIndex, int pageSize, out int count);
    }
}
