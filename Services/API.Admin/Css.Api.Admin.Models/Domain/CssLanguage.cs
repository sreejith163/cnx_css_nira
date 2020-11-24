using System;
using System.Collections.Generic;

namespace Css.Api.AdminOps.Models.Domain
{
    public partial class CssLanguage
    {
        public CssLanguage()
        {
            LanguageTranslation = new HashSet<LanguageTranslation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<LanguageTranslation> LanguageTranslation { get; set; }
    }
}
