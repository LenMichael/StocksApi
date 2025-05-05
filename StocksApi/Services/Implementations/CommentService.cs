using StocksApi.Models;
using StocksApi.Repositories.Interfaces;
using StocksApi.Services.Interfaces;

namespace StocksApi.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepo;
        public CommentService(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }
        public async Task<Comment> CreateAsync(Comment comment)
        {
            comment.CreatedAt = DateTime.Now;
            return await _commentRepo.AddAsync(comment);
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var existingComment = await _commentRepo.GetByIdAsync(id);
            if (existingComment == null)
                return null;
            return await _commentRepo.DeleteAsync(id);
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _commentRepo.GetAllAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _commentRepo.GetByIdAsync(id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            var existingComment = await _commentRepo.GetByIdAsync(id);
            if (existingComment == null)
                return null;

            existingComment.Title = comment.Title;
            existingComment.Content = comment.Content;
            existingComment.CreatedAt = DateTime.Now;

            return await _commentRepo.UpdateAsync(id, existingComment);
        }
    }
}
