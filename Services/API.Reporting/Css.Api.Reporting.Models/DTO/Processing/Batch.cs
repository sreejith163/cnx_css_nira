using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class Batch<T>
        where T : class
    {
        public string BatchId { get; set; }
        public List<T> Items { get; set; }
    }
}
