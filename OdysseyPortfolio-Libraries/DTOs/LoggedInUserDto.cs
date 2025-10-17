using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.DTOs
{
    public class LoggedInUserDto
    {
        public string Id { get; set; }  
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string[] Roles {  get; set; }    
        public string? Token { get; set; }

    }
}
