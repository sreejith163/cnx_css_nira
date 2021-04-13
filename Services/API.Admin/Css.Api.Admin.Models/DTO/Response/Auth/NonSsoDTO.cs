using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Response.Auth
{
   public class NonSsoDTO
    {
        public int Id { get; set; }
     
        public string EmployeeId { get; set; }
   
        public int RoleId { get; set; }
  
        public string Username { get; set; }
   
        public string Sso { get; set; }
   
        public string Firstname { get; set; }
      
        public string Lastname { get; set; }

  

   
    }
}
