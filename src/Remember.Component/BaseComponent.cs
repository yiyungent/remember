using System;
using System.Collections.Generic;
using Remember.Domain;
using Remember.Manager;
using Remember.Service;
using NHibernate.Criterion;

namespace Remember.Component
{
    /// <summary>
    /// Component负责将 Service,Manager联系在一起，Service定义有哪些，Manager负责实现
    /// Base负责所有实体共有的
    /// </summary>
    /// <typeparam name="T">具体的实体模型</typeparam>
    /// <typeparam name="M">具体的Manager实现</typeparam>
    public class BaseComponent<T, M> : BaseService<T>
        // T 应当是具体的实体模型
        where T : BaseEntity<T>, new()
        // M 应当是具体的Manager 实现
        where M : BaseManager<T>, new()
    {
        protected M _manager = (M)typeof(M).GetConstructor(Type.EmptyTypes).Invoke(null);

        public void Create(T t)
        {
            _manager.Create(t);
        }

        public void Delete(T t)
        {
            _manager.Delete(t);
        }

        public void Delete(int id)
        {
            _manager.Delete(id);
        }

        public void Edit(T t)
        {
            _manager.Edit(t);
        }

        public IList<T> Query(IList<ICriterion> condition)
        {
            return _manager.Query(condition);
        }

        public IList<T> GetAll()
        {
            return _manager.GetAll();
        }

        public T GetEntity(int id)
        {
            return _manager.GetEntity(id);
        }
    }
}
