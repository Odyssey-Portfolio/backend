using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly OdysseyPortfolioDbContext? _dbContext;
        private bool disposed = false;
        private IGenericRepository<Blog> _blogRepository { get; set; }
        private IGenericRepository<Comment> _commentRepository { get; set; }
        private IGenericRepository<Image> _imageRepository { get; set; }
        private IGenericRepository<User> _userRepository { get; set; }
        private IGenericRepository<CommentLike> _commentLikeRepository { get; set; }
        public UnitOfWork(OdysseyPortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<Blog> BlogRepository
        {
            get
            {
                return _blogRepository ??= new GenericRepository<Blog>(_dbContext!);
            }
        }
        public IGenericRepository<Comment> CommentRepository
        {
            get
            {
                return _commentRepository ??= new GenericRepository<Comment>(_dbContext!);
            }
        }

        public IGenericRepository<Image> ImageRepository
        {
            get
            {
                return _imageRepository ??= new GenericRepository<Image>(_dbContext!);
            }
        }
        public IGenericRepository<User> UserRepository
        {
            get
            {
                return _userRepository ??= new GenericRepository<User>(_dbContext!);
            }
        }
        public IGenericRepository<CommentLike> CommentLikeRepository
        {
            get
            {
                return _commentLikeRepository ??= new GenericRepository<CommentLike>(_dbContext!);
            }
        }

        public void Save()
        {
            _dbContext!.SaveChanges();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext!.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
