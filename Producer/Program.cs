using System;
using System.Threading;

namespace Kfzteile24.Interfaces.StockUpdate.Producer
{
    class Program
    {

        static void Main(string[] args)
        {
            ApplicationConfiguration.Configure();
            var timer = new ApiTimer();
            var timerThread = new Thread(timer.Start);
            timerThread.Name = "StockUpdateProducer";
            timerThread.Start();
        }
    }
}