using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Entities
{
    public class CommentLike
    {
        public string Id { get; set; }
        public string CommentId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public Comment Comment { get; set; } = null!;
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
