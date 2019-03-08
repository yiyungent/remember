using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace Remember.Manager
{
    public class BaseManager<T> : ActiveRecordBase<T>
        where T : class
    {
        public new void Create(T t)
        {
            ActiveRecordBase.Create(t);
        }

        public new void Delete(T t)
        {
            ActiveRecordBase.Delete(t);
        }

        public void Delete(int id)
        {
            T t = GetEntity(id);
            if (t != null)
            {
                Delete(t);
            }
        }

        public void Edit(T t)
        {
            ActiveRecordBase.Update(t);
        }

        public IList<T> Query(IList<ICriterion> condition)
        {
            return (IList<T>)ActiveRecordBase.FindAll(typeof(T), condition.ToArray());
        }

        public IList<T> GetAll()
        {
            return ActiveRecordBase.FindAll(typeof(T)) as IList<T>;
        }

        public T GetEntity(int id)
        {
            return (T)ActiveRecordBase.FindByPrimaryKey(typeof(T), id);
        }
    }
}
