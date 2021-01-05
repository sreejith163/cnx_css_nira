using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Css.Api.Reporting.Models.DTO.Request.UDW
{
    public class UDWAgentDataGroup
    {
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        
        [XmlElement(ElementName = "adgStartDate")]
        public UDWAgentDate ADGStartDate { get; set; }
        
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }
}
