using System;

namespace StocksApi.Services.Implementations
{
    public class CommentWorker
    {
        public void LogComment(string commentContent)
        {
            Console.WriteLine($"[Hangfire Worker] New comment added: {commentContent}");
        }
    }
}
