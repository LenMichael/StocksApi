using StocksApi.Models;

namespace StocksApi.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken);
        Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Comment> AddAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment?> UpdateAsync(int id, Comment comment, CancellationToken cancellationToken);
        Task<Comment?> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
