using System;
using System.Collections.Generic;

namespace Css.Api.Admin.Models.Domain
{
    public partial class CssVariable
    {
        public CssVariable()
        {
            LanguageTranslation = new HashSet<LanguageTranslation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MenuId { get; set; }

        public virtual CssMenu Menu { get; set; }
        public virtual ICollection<LanguageTranslation> LanguageTranslation { get; set; }
    }
}
