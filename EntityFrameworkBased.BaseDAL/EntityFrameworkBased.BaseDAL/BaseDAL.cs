using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkBased.BaseDAL
{
    public class BaseDAL<T> where T : class
    {

        public string DbConnect = "";

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="t">数据实体</param>
        /// <returns>受影响行数</returns>
        public int Insert(T t)
        {
            if (t == null)
                return -1;
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                var dbset = context.Set(t.GetType());
                dbset.Add(t);
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="t">数据实体</param>
        /// <returns>执行添加操作后的数据实体</returns>
        public T Add(T t)
        {
            if (t == null)
                return null;
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                var dbset = context.Set(t.GetType());
                dbset.Add(t);
                int count = context.SaveChanges();
                if (count > 0)
                    return t;
                else
                    return null;
            }
        }

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="list">数据实体集合</param>
        /// <returns>受影响行数</returns>
        public int AddList(List<T> list)
        {
            if (list == null || list.Count < 1)
                return -1;
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                var dbset = context.Set(list[0].GetType());
                foreach (var item in list)
                {
                    dbset.Add(item);
                }
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// 更新数据记录
        /// </summary>
        /// <param name="t">数据实体</param>
        /// <returns>受影响行数</returns>
        public int Update(T t)
        {
            if (t == null)
                return -1;
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                context.Entry<T>(t).State = EntityState.Modified;
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// 更新多条数据记录
        /// </summary>
        /// <param name="list">数据记录集合</param>
        /// <returns>受影响行数</returns>
        public int UpdateList(List<T> list)
        {
            if (list == null || list.Count < 1)
                return -1;
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                foreach (var item in list)
                {
                    context.Entry<T>(item).State = EntityState.Modified;
                }
                return context.SaveChanges();
            }
        }


        /// <summary>
        /// 删除数据记录
        /// </summary>
        /// <param name="t">数据记录实体</param>
        /// <returns>受影响行数</returns>
        public int Delete(T t)
        {
            if (t == null)
                return -1;
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                context.Entry<T>(t).State = EntityState.Deleted;
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// 删除数据记录
        /// </summary>
        /// <param name="fieldName">数据查询的属性名（）</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Delete(string fieldName, object value)
        {
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                context.Configuration.ProxyCreationEnabled = false;
                List<Filter> filterList = new List<Filter>();
                filterList.Add(new Filter() { PropertyName = fieldName, Operation = FilterOption.Equals, Value = value });
                var query = GetQueryable(context, filterList);
                var model = query.FirstOrDefault();
                if (model == null)
                    return -1;
                context.Entry<T>(model).State = EntityState.Deleted;
                return context.SaveChanges();
            }
        }

        //public T GetModel(string fieldName, object value)
        //{
        //    using (DbContext context = new DbContextFactory().GetDbContext(DbConnect))
        //    {
        //        context.Configuration.ProxyCreationEnabled = false;
        //        Type type = typeof(T);
        //        //Assembly.Load(type.)
        //        var dbset = context.Set<T>();
        //        var list = dbset.AsQueryable<T>();
        //        ParameterExpression param = Expression.Parameter(typeof(T), "entity");
        //        Expression left = Expression.Property(param, typeof(T).GetProperty(fieldName));
        //        Expression right = Expression.Constant(value);


        //        Expression filter = Expression.Equal(left, right);
        //        Expression pred = Expression.Lambda(filter, param);
        //        Expression exp = Expression.Call(typeof(Queryable), "Where", new Type[] { typeof(T) }, Expression.Constant(dbset), pred);

        //        string sql = string.Empty;
        //        //list = list.Where(Expression.Lambda<Func<City, bool>>(filter, param));
        //        IQueryable<T> query = context.Set<T>().AsQueryable<T>().Provider.CreateQuery<T>(exp);
        //        //var testModel=
        //        var test = (context.Set<T>().AsQueryable<T>().Provider.CreateQuery<T>(exp) as ObjectQuery);
        //        return query.FirstOrDefault();
        //    }
        //}


        /// <summary>
        /// 获取一条数记录实体
        /// </summary>
        /// <param name="fieldName">用于查询的条件字段名</param>
        /// <param name="value">查询字段对应的值</param>
        /// <returns>数据实体</returns>
        public T GetModel(string fieldName, object value)
        {
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                context.Configuration.ProxyCreationEnabled = false;
                List<Filter> filterList = new List<Filter>();
                filterList.Add(new Filter() { PropertyName = fieldName, Operation = FilterOption.Equals, Value = value });
                var query = GetQueryable(context, filterList);
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取所有数据记录
        /// </summary>
        /// <returns>数据实体集</returns>
        public IEnumerable<T> GetAll()
        {
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                return context.Set<T>().ToList();
            }
        }

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="filters">用于查询过滤的条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="orderKey">排序字段名</param>
        /// <param name="orderType">排序方式 ASC DESC</param>
        /// <returns></returns>
        public ResultObjClass GetPageList(List<Filter> filters = null, int pageIndex = 1, int pageSize = 15, string orderKey = null, string orderType = "DESC")
        {
            using (var context = new DbContextFactory().GetDbContext(DbConnect))
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> query = null;
                if (filters != null)
                {
                    query = GetQueryable(context, filters);
                }
                else
                {
                    query = context.Set<T>().AsQueryable<T>();
                }
                return GetPageList(query, pageIndex, pageSize, orderKey, orderType);
            }
        }

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="query">添加过滤条件后的IQueryable</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="orderKey">排序字段名</param>
        /// <param name="orderType">排序方式 ASC DESC</param>
        /// <returns></returns>
        public ResultObjClass GetPageList(IQueryable<T> query, int pageIndex = 1, int pageSize = 15, string orderKey = null, string orderType = "DESC")
        {
            if (string.IsNullOrEmpty(orderKey))
            {
                PropertyInfo[] pis = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                orderKey = pis[0].Name;
            }
            query = query.PageSplitOrderBy(orderKey, orderType);
            var count = query.Count();
            query = query.PageSplit(pageIndex, pageSize);
            return new ResultObjClass()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                PageTotalCount = count,
                PageObject = query.ToList()
            };
        }


        public IQueryable<T> GetQueryable(DbContext context, List<Filter> filterList)
        {
            var deleg = ExpressionBuilder.GetExpression<T>(filterList);
            var query = context.Set<T>().Where(deleg);
            return query;
        }
    }
}
