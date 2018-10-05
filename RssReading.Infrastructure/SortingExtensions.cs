using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RssReading.Infrastructure
{
    public static class SortingExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, Sorting sorting)
        {
            if (sorting?.SortFields == null)
            {
                return query;
            }

            if (sorting.SortFields.Any())
            {
                query = query.OrderBy(x => true);
            }

            foreach (var sortField in sorting.SortFields)
            {
                var parameter = Expression.Parameter(typeof(T), "p");

                try
                {
                    var aggregate = sortField.FieldName.Split('.').Aggregate((Expression)parameter, Expression.Property);

                    if (aggregate == null) continue;

                    var orderExpression = Expression.Lambda(aggregate, parameter);

                    var childPropertyType = ((PropertyInfo)((MemberExpression)aggregate).Member).PropertyType;
                    var methodToInvoke = sortField.Direction == SortDirection.Ascending
                        ? "ThenBy"
                        : "ThenByDescending";

                    if (aggregate.Type.IsNullable() && sortField.Direction == SortDirection.Ascending)
                    {
                        query = BuildQuery(query, methodToInvoke, aggregate, parameter);
                    }

                    var orderByCall = Expression.Call(typeof(Queryable), methodToInvoke,
                        new[]
                        {
                            typeof(T), childPropertyType
                        }, query.Expression, Expression.Quote(orderExpression));

                    query = (IOrderedQueryable<T>)query.Provider.CreateQuery(orderByCall);

                    if (aggregate.Type.IsNullable() && sortField.Direction == SortDirection.Descending)
                    {
                        query = BuildQuery(query, methodToInvoke, aggregate, parameter);
                    }
                }
                catch
                {

                }
            }

            return query;
        }

        public static IQueryable<T> BuildQuery<T>(IQueryable<T> query, string methodToInvoke, Expression aggregate, ParameterExpression parameter)
        {
            var orderNullExpression =
                Expression.Lambda(Expression.Equal(aggregate, Expression.Constant(null)),
                    parameter);
            var orderByNullCall = Expression.Call(typeof(Queryable), methodToInvoke,
                new[]
                {
                    typeof(T), typeof(bool)
                },
                query.Expression, Expression.Quote(orderNullExpression));

            return (IOrderedQueryable<T>)query.Provider.CreateQuery(orderByNullCall);
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}