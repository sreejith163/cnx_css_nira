using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Mappers
{
    public class ConfigSettings
    {
        public MuSettings MUs { get; set; }
        public AgentCategorySettings AgentCategoryFields { get; set; }

        public ConfigSettings()
        {
            MUs = new MuSettings();
            AgentCategoryFields = new AgentCategorySettings();
        }
    }
}
