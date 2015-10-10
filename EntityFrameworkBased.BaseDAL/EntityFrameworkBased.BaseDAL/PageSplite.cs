using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace EntityFrameworkBased.BaseDAL
{
    public static class PageSplite
    {
        public static IQueryable<T> PageSplitOrderBy<T>(this IQueryable<T> source, string sortName, string sortType)
        {
            if (string.IsNullOrEmpty(sortName))
                return source.AsQueryable();

            sortType = sortType ?? "ASC";

            var sortingDir = string.Empty;
            switch (sortType.ToUpper().Trim())
            {
                case "ASC":
                    sortingDir = "OrderBy";
                    break;
                case "DESC":
                    sortingDir = "OrderByDescending";
                    break;
            }

            var param = Expression.Parameter(typeof(T), sortName);
            var pi = typeof(T).GetProperty(sortName);
            var types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, source.Expression,Expression.Lambda(Expression.Property(param, sortName), param));
            var query = source.AsQueryable().Provider.CreateQuery<T>(expr);
            return query;
        }

        public static IQueryable<T> PageSplit<T>(this IQueryable<T> source, int pageIndex = 1, int pageSize = 15)
        {
            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<T> SortingAndPaging<T>(this IQueryable<T> source, string sortName,
            string sortType, int pageIndex, int pageSize)
        {
            return source.PageSplitOrderBy(sortName, sortType).PageSplit(pageIndex, pageSize);
        }

        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        public static Expression<Func<T, bool>> PageSpliteOr<T>(this Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expression1.Body, invokedExpression),
                expression1.Parameters);
        }

        public static Expression<Func<T, bool>> PageSpliteAnd<T>(this Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters);

            return Expression.Lambda<Func<T, bool>>(Expression.And(expression1.Body,
                invokedExpression), expression1.Parameters);
        }
    }
}
