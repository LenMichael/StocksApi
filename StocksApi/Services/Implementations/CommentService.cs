using Hangfire;
using StocksApi.Models;
using StocksApi.Repositories.Interfaces;
using StocksApi.Services.Interfaces;

namespace StocksApi.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepo;
        private readonly CommentWorker _commentWorker;
        public CommentService(ICommentRepository commentRepo, CommentWorker commentWorker)
        {
            _commentRepo = commentRepo;
            _commentWorker = commentWorker;
        }
        public async Task<Comment> CreateAsync(Comment comment, CancellationToken cancellationToken)
        {
            comment.CreatedAt = DateTime.Now;
            BackgroundJob.Enqueue(() => _commentWorker.LogComment(comment.Content));
            return await _commentRepo.AddAsync(comment, cancellationToken);
        }

        public async Task<Comment?> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var existingComment = await _commentRepo.GetByIdAsync(id, cancellationToken);
            if (existingComment == null)
                return null;
            return await _commentRepo.DeleteAsync(id, cancellationToken);
        }

        public async Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _commentRepo.GetAllAsync(cancellationToken);
        }

        public async Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _commentRepo.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment, CancellationToken cancellationToken)
        {
            var existingComment = await _commentRepo.GetByIdAsync(id, cancellationToken);
            if (existingComment == null)
                return null;

            existingComment.Title = comment.Title;
            existingComment.Content = comment.Content;
            existingComment.CreatedAt = DateTime.Now;

            return await _commentRepo.UpdateAsync(id, existingComment, cancellationToken);
        }
    }
}
