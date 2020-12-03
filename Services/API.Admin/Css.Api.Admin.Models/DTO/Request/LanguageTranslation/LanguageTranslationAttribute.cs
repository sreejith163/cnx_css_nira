using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.LanguageTranslation
{
    public class LanguageTranslationAttribute
    {
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int VariableId { get; set; }
        public string VariableName { get; set; }
        public string Translation { get; set; }
        public string Description { get; set; }
    }
}
