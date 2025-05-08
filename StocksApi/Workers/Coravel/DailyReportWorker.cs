using Coravel.Invocable;
using System;

namespace StocksApi.Workers.Coravel
{
    public class DailyReportWorker : IInvocable
    {
        public Task Invoke()
        {
            SendDailyReport();
            return Task.CompletedTask;
        }

        public void SendDailyReport()
        {
            Console.WriteLine($"[Coravel Worker] Daily report sent at {DateTime.Now}");
            // Here we can add logic for sending emails or generating reports
        }

    }
}
