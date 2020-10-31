using Css.Api.Core.Utilities.Interfaces;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Css.Api.Core.Utilities
{
	/// <summary>
	/// Implementaion for sorting the data based on provided keywords
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SortHelper<T> : ISortHelper<T>
	{
		public IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString)
		{
			if (!entities.Any())
				return entities;

			if (string.IsNullOrWhiteSpace(orderByQueryString))
			{
				return entities;
			}

			var orderParams = orderByQueryString.Trim().Split(',');
			var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var orderQueryBuilder = new StringBuilder();

			foreach (var param in orderParams)
			{
				if (string.IsNullOrWhiteSpace(param))
					continue;

				var propertyFromQueryName = param.Split(" ")[0];
				var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

				if (objectProperty == null)
					continue;

				var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

				orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
			}

			var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

			return string.IsNullOrEmpty(orderQuery) ? entities : entities.OrderBy(orderQuery);
		}
	}
}
