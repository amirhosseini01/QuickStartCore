using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Server.Core.Commons.Datatables;

public static class DataTableQuery
{
    public static async Task<DataTableResult<T>> ToDataTableAsync<T>(this IQueryable<T> source, DataTableFilter dataTableFilter, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        if (dataTableFilter.Length == -1)
        {
            dataTableFilter.Length = count;
            dataTableFilter.Start = 0;
        }

        return new DataTableResult<T>
        {
            Draw = dataTableFilter.Draw,
            RecordsFiltered = count,
            RecordsTotal = count,
            Data = await source.OrderByIndex(dataTableFilter).Skip(dataTableFilter.Start).Take(dataTableFilter.Length).ToListAsync(cancellationToken)
        };
    }
    public static IQueryable<T> OrderByIndex<T>(this IQueryable<T> source, DataTableFilter dataTableFilter)
    {
        if (dataTableFilter.Order == null || dataTableFilter.Length == 0)
        {
            return source;
        }

        var orderedQueryable = (!(dataTableFilter.Order[0].Dir == "desc")) ? OrderBy(source, dataTableFilter.Columns[dataTableFilter.Order[0].Column].Data) : OrderByDescending(source, dataTableFilter.Columns[dataTableFilter.Order[0].Column].Data);
        for (var i = 1; i < dataTableFilter.Order.Count; i++)
        {
            orderedQueryable = (!(dataTableFilter.Order[i].Dir == "desc")) ? ThenBy(orderedQueryable, dataTableFilter.Columns[dataTableFilter.Order[i].Column].Data) : ThenByDescending(orderedQueryable, dataTableFilter.Columns[dataTableFilter.Order[i].Column].Data);
        }

        return orderedQueryable;
        static IOrderedQueryable<T> OrderBy(IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda(propertyName));
        }

        static IOrderedQueryable<T> OrderByDescending(IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda(propertyName));
        }

        static IOrderedQueryable<T> ThenBy(IOrderedQueryable<T> source, string propertyName)
        {
            return source.ThenBy(ToLambda(propertyName));
        }

        static IOrderedQueryable<T> ThenByDescending(IOrderedQueryable<T> source, string propertyName)
        {
            return source.ThenByDescending(ToLambda(propertyName));
        }

        static Expression<Func<T, object>> ToLambda(string propertyName)
        {
            var parameterExpression = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(parameterExpression, propertyName), typeof(object)), [parameterExpression]);
        }
    }
}