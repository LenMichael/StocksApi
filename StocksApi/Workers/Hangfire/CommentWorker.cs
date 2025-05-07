using System;

namespace StocksApi.Workers.Hangfire
{
    public class CommentWorkerHangfire
    {
        public void LogComment(string commentContent)
        {
            Console.WriteLine($"[Hangfire Worker] New comment added: {commentContent}");
        }
    }
}
