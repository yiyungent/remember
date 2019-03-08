using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remember.Domain;
using NHibernate.Criterion;

namespace Remember.Service
{
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
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        T GetEntity(int id);
    }
}
