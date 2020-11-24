using Css.Api.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Css.Api.Core.Utilities.Extensions
{
    public static class DataShaper
    {
		/// <summary>
		/// Shapes the data.
		/// </summary>
		/// <param name="entities">The entities.</param>
		/// <param name="fieldsString">The fields string.</param>
		/// <returns></returns>
		public static IEnumerable<Entity> ShapeData<T>(this IQueryable<T> entities, string fieldsString)
		{
			var requiredProperties = GetRequiredProperties<T>(fieldsString);
			return FetchData(entities, requiredProperties);
		}

		/// <summary>
		/// Shapes the data.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="fieldsString">The fields string.</param>
		/// <returns></returns>
		public static Entity ShapeData<T>(T entity, string fieldsString)
		{
			var requiredProperties = GetRequiredProperties<T>(fieldsString);
			return FetchDataForEntity(entity, requiredProperties);
		}

		/// <summary>
		/// Gets the required properties.
		/// </summary>
		/// <param name="fieldsString">The fields string.</param>
		/// <returns></returns>
		private static IEnumerable<PropertyInfo> GetRequiredProperties<T>(string fieldsString)
		{
			var requiredProperties = new List<PropertyInfo>();
			var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			if (!string.IsNullOrWhiteSpace(fieldsString))
			{
				var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

				foreach (var field in fields)
				{
					var property = properties.FirstOrDefault(pi => pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

					if (property == null)
						continue;

					requiredProperties.Add(property);
				}
			}
			else
			{
				requiredProperties = properties.ToList();
			}

			return requiredProperties;
		}

		/// <summary>
		/// Fetches the data.
		/// </summary>
		/// <param name="entities">The entities.</param>
		/// <param name="requiredProperties">The required properties.</param>
		/// <returns></returns>
		private static IEnumerable<Entity> FetchData<T>(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
		{
			var shapedData = new List<Entity>();

			foreach (var entity in entities)
			{
				var shapedObject = FetchDataForEntity(entity, requiredProperties);
				shapedData.Add(shapedObject);
			}

			return shapedData;
		}

		/// <summary>
		/// Fetches the data for entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="requiredProperties">The required properties.</param>
		/// <returns></returns>
		private static Entity FetchDataForEntity<T>(T entity, IEnumerable<PropertyInfo> requiredProperties)
		{
			var shapedObject = new Entity();

			foreach (var property in requiredProperties)
			{
				var objectPropertyValue = property.GetValue(entity);
				shapedObject.TryAdd(ToCamelCaseString(property.Name), objectPropertyValue);
			}

			return shapedObject;
		}

		/// <summary>
		/// Converts to camelcasestring.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <returns></returns>
		private static string ToCamelCaseString(string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				return char.ToLowerInvariant(str[0]) + str.Substring(1);
			}

			return str;
		}
	}
}
