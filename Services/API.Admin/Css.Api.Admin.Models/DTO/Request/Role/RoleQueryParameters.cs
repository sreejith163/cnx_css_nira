using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Admin.Models.DTO.Request.Role
{
    public class RoleQueryParameters : QueryStringParameters
    {
        public RoleQueryParameters()
        {
            OrderBy = "RoleId";
        }
    }
}
