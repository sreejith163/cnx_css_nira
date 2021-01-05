using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Css.Api.Reporting.Models.DTO.Request.UDW
{
    public class UDWAgentDate
    {
        [XmlElement(ElementName = "day")]
        public int Day { get; set; }
        
        [XmlElement(ElementName = "month")]
        public int Month { get; set; }

        [XmlElement(ElementName = "year")]
        public int Year { get; set; }
    }
}
