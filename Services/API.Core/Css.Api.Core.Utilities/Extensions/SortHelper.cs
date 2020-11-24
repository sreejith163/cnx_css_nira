using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Css.Api.Core.Utilities.Extensions
{
	public static class SortHelper
	{
		public static IQueryable<T> ApplySort<T>(this IQueryable<T> entities, string orderByQueryString)
		{
			if (!entities.Any() || string.IsNullOrWhiteSpace(orderByQueryString))
			{
				return entities;
			}

			var orderParams = orderByQueryString.Trim().Split(',');
			var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var orderQueryBuilder = new StringBuilder();

			foreach (var param in orderParams)
			{
				if (string.IsNullOrWhiteSpace(param))
				{
					continue;
				}

				var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
				var propertyFromQueryName = param.Split(" ")[0];
				dynamic objectProperty;

				objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

				if (objectProperty == null)
				{
					var innerPropertyFromQueryName = propertyFromQueryName.Split('.')[0];
					var innerObjectProperty = propertyInfos.FirstOrDefault(x => x.Name.Equals(innerPropertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
					if (innerObjectProperty == null)
					{
						continue;
					}

					var innerPropertyInfos = innerObjectProperty.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
					objectProperty = innerPropertyInfos.FirstOrDefault(x => x.Name.Equals(propertyFromQueryName.Split('.')[1], StringComparison.InvariantCultureIgnoreCase));

					if (objectProperty == null)
					{
						continue;
					}

					orderQueryBuilder.Append($"{propertyFromQueryName} {sortingOrder}, ");
				}
				else
				{
					orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
				}
			}

			var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

			return string.IsNullOrEmpty(orderQuery) ? entities : entities.OrderBy(orderQuery);
		}
	}
}
