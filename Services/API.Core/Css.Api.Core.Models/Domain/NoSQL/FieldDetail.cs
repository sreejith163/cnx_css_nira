namespace Css.Api.Core.Models.Domain.NoSQL
{
    public class FieldDetail
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the old value.
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Creates new value.
        /// </summary>
        public string NewValue { get; set; }
    }
}
