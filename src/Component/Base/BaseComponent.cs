using Domain;
using Domain.Base;
using Manager;
using Manager.Base;
using Service.Base;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NHibernate;
using System.Linq;

namespace Component.Base
{
    public class BaseComponent<T, M> : BaseService<T>
        where T : BaseEntity<T>, new()
        where M : BaseManager<T>, new()
    {
        protected M _manager = (M)typeof(M).GetConstructor(Type.EmptyTypes).Invoke(null);

        /// <summary>
        /// 新增实体
        /// </summary>
        public void Create(T t)
        {
            _manager.Create(t);
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        public void Delete(T t)
        {
            _manager.Delete(t);
        }

        /// <summary>
        /// 根据主键删除
        /// 假删除
        /// </summary>
        public void Delete(int id)
        {
            try
            {
                T t = _manager.GetEntity(id);
                t.Status = StatusEnum.Deleted;
                _manager.Edit(t);
            }
            catch (Exception ex)
            { }

            //_manager.Delete(id);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        public void Edit(T t)
        {
            _manager.Edit(t);
        }

        /// <summary>
        /// 查询
        /// </summary>
        public IList<T> Query(IList<ICriterion> condition)
        {
            return _manager.Query(condition).Where(m => m.Status != StatusEnum.Deleted).ToList();
        }

        /// <summary>
        /// 获取全部实体
        /// </summary>
        public IList<T> GetAll()
        {
            return _manager.GetAll();
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        public T GetEntity(int id)
        {
            return _manager.GetEntity(id);
        }

        public int Count(params ICriterion[] criteria)
        {
            return _manager.Count(criteria);
        }

        public bool Exist(int id)
        {
            return _manager.Exist(id);
        }

        //分页区和取对象集合
        public IList<T> GetPaged(IList<ICriterion> queryConditions, IList<Order> orderList, int pageIndex, int pageSize, out int count)
        {
            queryConditions.Add(Expression.Not(Expression.Eq("Status", Domain.Base.StatusEnum.Deleted)));
            return _manager.GetPaged(queryConditions, orderList, pageIndex, pageSize, out count);
        }

        /// <summary>
        /// 根据查询条件分页获取实体
        /// </summary>
        /// <param name="queryConditions">查询条件集合</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="count">返回满足查询条件</param>
        /// <returns>返回满足查询条件的实体</returns>
        public IList<T> GetPaged(IList<KeyValuePair<string, string>> queryConditions, int pageIndex, int pageSize, out int count)
        {
            // TODO:
            //queryConditions.Add(Expression.Not(Expression.Eq("Status", Domain.Base.StatusEnum.Deleted)));
            return _manager.GetPaged(queryConditions, pageIndex, pageSize, out count);
        }

        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            return _manager.ExecuteNonQuery(sql, parameters);
        }

        public int GetLastId()
        {
            return _manager.GetLastId();
        }
    }
}
