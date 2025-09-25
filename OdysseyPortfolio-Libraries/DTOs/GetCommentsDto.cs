using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.DTOs
{
    public class GetCommentsDto
    {
        public string UserId { get; set; }  
        public string UserName { get; set; }    
        public string ElapsedTime { get; set; }
        public string Content { get; set; }
    }
}
