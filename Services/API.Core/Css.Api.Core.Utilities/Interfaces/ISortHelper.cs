using System.Linq;

namespace Css.Api.Core.Utilities.Interfaces
{
    /// <summary>
    /// Interface for sorting the data based on provided keywords
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISortHelper<T>
    {
        /// <summary>
        /// Applies the sort.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="orderByQueryString">The order by query string.</param>
        /// <returns></returns>
        IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString);
    }
}
