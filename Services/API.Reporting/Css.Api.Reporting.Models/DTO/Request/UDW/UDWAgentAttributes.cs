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

        public int? MU { get { return string.IsNullOrWhiteSpace(MUString) ? null : (int?)int.Parse(MUString); } }

        [XmlElement(ElementName = "mu")]
        public string MUString { get; set; }

        [XmlElement(ElementName = "startID", IsNullable = true)]
        public long? StartId { get; set; }


        [XmlElement(ElementName = "agentStartDate")]
        public UDWAgentDate AgentStartDate { get; set; }

        [XmlArray(ElementName = "acdList")]
        [XmlArrayItem(ElementName = "acd")]
        public List<UDWAgentAcd> AcdList { get; set; }

        [XmlElement(ElementName = "senDate")]
        public UDWAgentDate SenDate { get; set; }

        public int? SenExt { get { return string.IsNullOrWhiteSpace(SenExtString) ? null : (int?) int.Parse(SenExtString); } }

        [XmlElement(ElementName = "senExt")]
        public string SenExtString { get; set; }

        [XmlArray(ElementName = "agentData")]
        [XmlArrayItem(ElementName = "group")]
        public List<UDWAgentDataGroup> AgentData { get; set; }
    }
}
