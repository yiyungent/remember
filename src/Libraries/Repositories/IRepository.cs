﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repositories
{
    /// <summary>
    /// 仓储通用接口类
    /// </summary>
    /// <typeparam name="T">泛型实体类</typeparam>
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// Gets all objects from database
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets objects from database with filtering and paging.
        /// </summary>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <returns></returns>
        bool Contains(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        /// <returns></returns>
        T Find(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        /// <returns></returns>
        void Create(T t);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>
        void Delete(T t);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        /// <returns></returns>
        void Update(T t);

        /// <summary>
        /// Select Single Item by specified expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> expression);
    }
}