using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manager.Base
{
    public class BaseManager<T> : ActiveRecordBase<T>
        where T : class
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        public new void Create(T t)
        {
            ActiveRecordBase.Create(t);
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        public new void Delete(T t)
        {
            ActiveRecordBase.Delete(t);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        public void Delete(int id)
        {
            // 1.根据主键获取实体
            T t = GetEntity(id);
            // 2.根据实体删除
            if (t != null)
            {
                Delete(t);
            }
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        public void Edit(T t)
        {
            ActiveRecordBase.Update(t);
        }

        /// <summary>
        /// 查询
        /// </summary>
        public IList<T> Query(IList<ICriterion> condition)
        {
            // 1.IList --> Array
            // 2. Array --> IList
            // 3.强类型转换
            return (IList<T>)ActiveRecordBase.FindAll(typeof(T), condition.ToArray());
        }

        /// <summary>
        /// 获取全部实体
        /// </summary>
        public IList<T> GetAll()
        {
            // 强类型转换第二种写法
            return ActiveRecordBase.FindAll(typeof(T)) as IList<T>;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        public T GetEntity(int id)
        {
            return (T)ActiveRecordBase.FindByPrimaryKey(typeof(T), id);
        }

        public new int Count(params ICriterion[] criteria)
        {
            return ActiveRecordBase.Count(typeof(T), criteria);
        }

        public bool Exist(int id)
        {
            return ActiveRecordBase.Exists(typeof(T), id);
        }

        //分页区和取对象集合
        public IList<T> GetPaged(IList<ICriterion> queryConditions, IList<Order> orderList, int pageIndex, int pageSize, out int count)
        {
            if (queryConditions == null)//如果为null则赋值为一个总数为0的集合
            {
                queryConditions = new List<ICriterion>();
            }
            if (orderList == null)//如果为null则赋值为一个总数为0的集合
            {
                orderList = new List<Order>();
            }
            count = Count(typeof(T), queryConditions.ToArray());//根据查询条件获取满足条件的对象总数
            Array arr = SlicedFindAll(typeof(T), (pageIndex - 1) * pageSize, pageSize, orderList.ToArray(), queryConditions.ToArray());//根据查询条件分页获取对象集合
            return arr as IList<T>;
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
            //实例化一个hql查询语句对象
            StringBuilder hql = new StringBuilder(@"from " + typeof(T).Name + " d");
            //根据查询条件构造hql查询语句
            for (int i = 0; i < queryConditions.Count; i++)
            {
                KeyValuePair<string, string> keyv = queryConditions[i];//获取当前序号对应的条件
                if (!string.IsNullOrEmpty(keyv.Value))
                {
                    AddHqlSatements(hql);//增加where或and语句
                    hql.Append("d." + keyv.Key + " = : q_" + i.ToString());
                }
            }

            ISession session = ActiveRecordBase.holder.CreateSession(typeof(T));//获取管理DeliveryForm的session对象
            IQuery query = session.CreateQuery(hql.ToString());//获取满足条件的数据
            IQuery queryScalar = session.CreateQuery("select count(ID) " + hql.ToString());//获取满足条件的数据的总数
            for (int i = 0; i < queryConditions.Count; i++)
            {
                KeyValuePair<string, string> keyv = queryConditions[i];//获取当前序号对应的条件
                if (!string.IsNullOrEmpty(keyv.Value))
                {
                    queryScalar.SetString("q_" + i, keyv.Value);//用查询条件的值去填充hql，如d.Transportor.Name="michael"
                    query.SetString("q_" + i, keyv.Value);
                }
            }
            IList<object> result = queryScalar.List<object>();//执行查询条件总数的查询对象，返回一个集合（有一点怪异）
            int.TryParse(result[0].ToString(), out count);//尝试将返回值的第一个值转换为整形，并将转换成功的值赋值给count，如果转换失败,count=0
            query.SetFirstResult((pageIndex - 1) * pageSize);//设置获取满足条件实体的起点
            query.SetMaxResults(pageSize);//设置获取满足条件实体的终点
            IList<T> arr = query.List<T>();//返回当前页的数据
            session.Close();//关闭session
            return arr;
        }

        protected void AddHqlSatements(StringBuilder hql)
        {
            if (!hql.ToString().Contains("where"))//查询语句的开始条件是where
            {
                hql.Append(" where ");
            }
            else//当hql中有了一个where后再添加查询条件时就应该使用and了
            {
                hql.Append(" and ");
            }
        }
    }
}
