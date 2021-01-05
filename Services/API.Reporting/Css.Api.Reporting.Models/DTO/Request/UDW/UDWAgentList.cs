using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Css.Api.Reporting.Models.DTO.Request.UDW
{
    [XmlRoot("agentList")]
    public class UDWAgentList
    {
        [XmlElement(ElementName = "newAgent")]
        public List<UDWAgent> NewAgents { get; set; }

        [XmlElement(ElementName = "changedAgent")]
        public List<UDWAgentUpdate> ChangedAgents { get; set; }
    }
}
