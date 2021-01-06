using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Css.Api.Reporting.Models.DTO.Request.UDW
{
    public class UDWAgentAcd
    {
        [XmlElement(ElementName = "acdID")]
        public int AcdId { get; set; }
        
        [XmlElement(ElementName = "logon")]
        public string LogOn { get; set; }

        [XmlElement(ElementName = "agentGroup")]
        public string AgentGroup { get; set; }
    }
}
