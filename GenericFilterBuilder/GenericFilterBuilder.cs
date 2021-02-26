/// <summary>
///   Author:    Denis Jakus
///    email:    djakus@outlook.com
///      web:    https://www.denisjakus.com
/// linkedIn:    https://www.linkedin.com/in/denisjakus/
/// </summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GenericFilterBuilder
{
    public class GenericFilterBuilder<T> where T : class
    {
        private Expression<Func<T, bool>> _expression;

        public GenericFilterBuilder<T> AddFilter(FilterValueItem filter)
        {
            if (filter == null)
                return null;

            var param = Expression.Parameter(typeof(T), "t");

            var exp = GetExpression(param, filter);

            _expression = Expression.Lambda<Func<T, bool>>(exp, param);

            return this;
        }

        public GenericFilterBuilder<T> AddAndFilters(string filterValues)
        {
            _expression = GetExpressionAndOrFilters(filterValues);
            return this;
        }

        public GenericFilterBuilder<T> AddOrFilters(string filterValues)
        {
            _expression = GetExpressionAndOrFilters(filterValues, false);
            return this;
        }

        public Expression<Func<T, bool>> Build()
        {
            return _expression;
        }

        #region helper methods
        private Expression<Func<T, bool>> GetExpressionAndOrFilters(string filterValues, bool isAndFilter = true)
        {
            var filters = JsonConverter.TryParse<List<FilterValueItem>>(filterValues);
            if (filters == null) return null;

            var numberOfUsedFilters = filters.Where(f => !string.IsNullOrEmpty(f.FilterValue));
            if (!numberOfUsedFilters.Any()) return null;

            var param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;


            if (numberOfUsedFilters.Count() == 1)
            {
                var filterValue = numberOfUsedFilters.ToList()[0];
                exp = GetExpression(param, filterValue);
            }
            else
            {
                exp = GetExpression(param, numberOfUsedFilters.ToList()[0]);
                for (var i = 1; i < numberOfUsedFilters.Count(); i++)
                {
                    var filterValue = numberOfUsedFilters.ToList()[i];

                    exp = isAndFilter
                        ? Expression.AndAlso(exp, GetExpression(param, filterValue))
                        : Expression.Or(exp, GetExpression(param, filterValue));
                }
            }
            return Expression.Lambda<Func<T, bool>>(exp, param);
        }
        private Expression GetExpression(ParameterExpression param, FilterValueItem filter)
        {

            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var member = Expression.Property(param, filter.FilterKey);
            var constant = Expression.Constant(filter.FilterValue.ToString());

            if (member.Type == typeof(string)) return Expression.Call(member, containsMethod, constant);


            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType);
            var propertyValue = converter.ConvertFromInvariantString(filter.FilterValue.ToString());
            constant = Expression.Constant(propertyValue);

            return Expression.Equal(member, constant);

        }
        #endregion
    }

}
