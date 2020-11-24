using System;
using System.Collections.Generic;

namespace Css.Api.Admin.Models.Domain
{
    public partial class CssMenu
    {
        public CssMenu()
        {
            CssVariable = new HashSet<CssVariable>();
            LanguageTranslation = new HashSet<LanguageTranslation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CssVariable> CssVariable { get; set; }
        public virtual ICollection<LanguageTranslation> LanguageTranslation { get; set; }
    }
}
