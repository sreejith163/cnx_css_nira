using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Core.Common.Models.Domain
{
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Gets the current page.
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets the total count.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has previous.
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Gets a value indicating whether this instance has next.
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="count">The count.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        /// <summary>
        /// Converts to json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToJson(object value)
        {
            var results = value as PagedList<Entity>;
            var metadata = new
            {
                results.TotalCount,
                results.PageSize,
                results.CurrentPage,
                results.TotalPages,
                results.HasNext,
                results.HasPrevious
            };

            return JsonConvert.SerializeObject(metadata);
        }

        /// <summary>
        /// Converts to pagedlist.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static async Task<PagedList<T>> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return await Task.FromResult(new PagedList<T>(items, count, pageNumber, pageSize));
        }
    }
}
