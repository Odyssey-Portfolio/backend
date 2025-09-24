using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Payloads.Request
{
    public class GetBlogByIdRequest
    {        
        public string Id { get; set; }       
        public string? UserRole { get; set; } 
    }
}
