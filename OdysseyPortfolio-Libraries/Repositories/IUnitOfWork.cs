using OdysseyPortfolio_Libraries.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Repositories
{

    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Blog> BlogRepository { get; }
        IGenericRepository<Comment> CommentRepository { get; }
        IGenericRepository<Image> ImageRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<CommentLike> CommentLikeRepository { get; }
        void Save();
    }

}
