using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Core
{
    /// <summary>
    /// TODO: 目前所有 SaveChanges()都在这里完成，
    /// 而一个业务中经常涉及到对多张表操作，我们期望连接一次数据库，完成对多张表数据的操作，提高性能
    /// 所以，可增加数据会话层，来统一 SaveChanges()
    /// 数据会话层：就是一个工厂类，负责完成所有数据操作类实例的创建，然后业务层通过数据会话层来获取要操作数据类的实例,所以数据会话层将业务层与数据层解耦。
    /// 在数据会话层中提供一个方法：完成所有数据的保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : IDependency, IRepository<T> where T : class, new()
    {
        protected readonly DbContext DbContext;

        public BaseRepository(DbContext context)
        {
            DbContext = context;
        }

        /// <summary>
        /// Gets all objects from database
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> All()
        {
            return DbContext.Set<T>().AsQueryable();
        }

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <returns></returns>
        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
        }

        /// <summary>
        /// Gets objects from database with filtering and paging.
        /// </summary>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0,
            int size = 50)
        {
            var skipCount = index * size;
            var resetSet = filter != null
                ? DbContext.Set<T>().Where<T>(filter).AsQueryable()
                : DbContext.Set<T>().AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <returns></returns>
        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Any(predicate);
        }

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        /// <returns></returns>
        public virtual T Find(params object[] keys)
        {
            return DbContext.Set<T>().Find(keys);
        }

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().FirstOrDefault<T>(predicate);
        }

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        /// <returns></returns>
        public virtual void Create(T t)
        {
            DbContext.Set<T>().Add(t);

            DbContext.SaveChanges();
        }

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>
        public virtual void Delete(T t)
        {
            DbContext.Set<T>().Remove(t);

            DbContext.SaveChanges();
        }

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<T, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbContext.Set<T>().Remove(obj);
            return DbContext.SaveChanges();
        }

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        /// <returns></returns>
        public virtual void Update(T t)
        {
            try
            {
                var entry = DbContext.Entry(t);
                DbContext.Set<T>().Attach(t);
                entry.State = EntityState.Modified;

                DbContext.SaveChanges();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Select Single Item by specified expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return All().FirstOrDefault(expression);
        }
    }
}
