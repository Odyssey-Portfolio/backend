using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Payloads.Request
{
    public class GetCommentsRequest
    {
        public string BlogId { get; set; } = "";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;     
    }
}
