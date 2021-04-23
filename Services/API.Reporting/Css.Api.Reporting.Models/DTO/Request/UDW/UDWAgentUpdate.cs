using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Css.Api.Reporting.Models.DTO.Request.UDW
{
    public class UDWAgentUpdate : UDWAgentAttributes
    {
        [XmlElement(ElementName = "id")]
        public string SSN { get; set; }
    }
}
