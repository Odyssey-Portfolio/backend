using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OdysseyPortfolio_Libraries.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Entities
{
    public class User : IdentityUser
    {        
        public override string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public override string Email { get; set; } = null!;
        public int NumberOfCommentsLeft { get; set; } = CommentConstants.USER_DAILY_COMMENT_LIMIT;
        public DateTime OutOfCommentLimitTime { get; set; } 
        public ICollection<Comment> Comments { get; } = new List<Comment>();
        public ICollection<Blog> Blogs { get; } = new List<Blog>();
        public ICollection<CommentLike> CommentLikes{ get; } = new List<CommentLike>();
    }
}
