using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Payloads.Request
{
    public class CreateCommentRequest
    {        
        public string Content { get; set; }     
        public string BlogId { get; set; }        
        public string UserId { get; set; } = null!;        
    }
}
