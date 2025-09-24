using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.DTOs
{
    public class GetBlogDtoAdmin : GetBlogDto
    {
        public bool IsDeleted { get; set; }
    }
}
