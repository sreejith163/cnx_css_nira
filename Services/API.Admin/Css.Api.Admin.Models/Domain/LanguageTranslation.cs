using System;
using System.Collections.Generic;

namespace Css.Api.Admin.Models.Domain
{
    public partial class LanguageTranslation
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int MenuId { get; set; }
        public int VariableId { get; set; }
        public string Translation { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual CssLanguage Language { get; set; }
        public virtual CssMenu Menu { get; set; }
        public virtual CssVariable Variable { get; set; }
    }
}
