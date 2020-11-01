using Newtonsoft.Json;

namespace Css.Api.Core.Models.Domain
{
    public class Paging
    {
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the has next.
        /// </summary>
        [JsonProperty("hasNext")]
        public bool HasNext { get; set; }

        /// <summary>
        /// Gets or sets the has previous.
        /// </summary>
        [JsonProperty("hasPrevious")]
        public bool HasPrevious { get; set; }
    }
}
