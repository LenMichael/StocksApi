using Microsoft.EntityFrameworkCore;
using StocksApi.Data;
using StocksApi.Models;
using StocksApi.Repositories.Interfaces;

namespace StocksApi.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddAsync(Comment comment, CancellationToken cancellationToken)
        {
            await _context.Comments.AddAsync(comment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments.FindAsync(id, cancellationToken);
            if (comment == null)
                return null;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Comments.ToListAsync(cancellationToken); 
        }

        public async Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Comments.FindAsync(id, cancellationToken);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment, CancellationToken cancellationToken)
        {
            var existingComment = await _context.Comments.FindAsync(id, cancellationToken);
            if (existingComment == null)
                return null;

            //existingComment.Title = comment.Title;
            //existingComment.Content = comment.Content;
            //existingComment.CreatedAt = comment.CreatedAt;
            _context.Entry(existingComment).CurrentValues.SetValues(comment);

            await _context.SaveChangesAsync(cancellationToken);
            return existingComment;
        }
    }
}
