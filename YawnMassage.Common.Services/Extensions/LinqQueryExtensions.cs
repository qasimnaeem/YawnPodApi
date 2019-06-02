using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace YawnMassage.Common.Services.Extensions
{
    public static class LinqQueryExtensions
    {
        public static IQueryable<TEntity> OrderByFieldName<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                       bool asc)
        {
            if (string.IsNullOrEmpty(orderByProperty))
                return source;

            string command = asc ? "OrderBy" : "OrderByDescending";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                            source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
