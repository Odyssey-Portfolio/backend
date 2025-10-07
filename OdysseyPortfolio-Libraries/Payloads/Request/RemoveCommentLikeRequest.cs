using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Payloads.Request
{
    public class RemoveCommentLikeRequest
    {
        public string CommentId { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}
