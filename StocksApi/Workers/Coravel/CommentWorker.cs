using System;

namespace StocksApi.Workers
{
    public class CommentWorkerCoravel
    {
        public Task LogComment(string commentTitle, string commentContent)
        {
            Console.WriteLine($"[Coravel Worker] New comment added: {commentContent}, with Title: {commentTitle}");
            return Task.CompletedTask;
        }
    }
}
