using StocksApi.Models;

namespace StocksApi.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken);
        Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Comment> CreateAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment?> UpdateAsync(int id, Comment comment, CancellationToken cancellationToken);
        Task<Comment?> DeleteAsync(int id, CancellationToken cancellationToken);
        //Task<bool> CommentExists(int id);
    }
}
