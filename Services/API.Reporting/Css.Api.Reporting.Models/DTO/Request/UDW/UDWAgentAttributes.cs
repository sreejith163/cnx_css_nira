using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Css.Api.Reporting.Models.DTO.Request.UDW
{
    public class UDWAgentAttributes
    {
        [XmlElement(ElementName = "sso")]
        public string SSO { get; set; }

        [XmlElement(ElementName = "UUID")]
        public string UUID { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "firstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "lastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "nameSuffix")]
        public string NameSuffix { get; set; }

        [XmlElement(ElementName = "mu")]
        public string MU { get; set; }

        [XmlElement(ElementName = "startID", IsNullable = true)]
        public long? StartId { get; set; }


        [XmlElement(ElementName = "agentStartDate")]
        public UDWAgentDate AgentStartDate { get; set; }

        [XmlArray(ElementName = "acdList")]
        [XmlArrayItem(ElementName = "acd")]
        public List<UDWAgentAcd> AcdList { get; set; }

        [XmlElement(ElementName = "senDate")]
        public UDWAgentDate SenDate { get; set; }

        [XmlElement(ElementName = "senExt")]
        public UDWAgentDate SenExt { get; set; }

        [XmlArray(ElementName = "agentData")]
        [XmlArrayItem(ElementName = "group")]
        public List<UDWAgentDataGroup> AgentData { get; set; }
    }
}
