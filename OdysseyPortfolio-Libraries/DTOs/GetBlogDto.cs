using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.DTOs
{
    public class GetBlogDto
    {
        public string Id { get; set; } = null!;
        public string Image { get; set; } = "";
        public string Title { get; set; } = null!;        
        public string Description { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}
