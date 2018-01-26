using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Kfzteile24.Interfaces.StockUpdate.Producer
{

    public class ApiTimer
    {
        private readonly int intervall;
        private Timer timer = null;
        private JsonHandler jsonHandler;

        public ApiTimer()
        {
            intervall = Convert.ToInt32((ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("TimerSettings")["Intervall"]));
        }

        public void Start()
        {
            var autoEvent = new AutoResetEvent(false);
            timer = new Timer(Callback, autoEvent, intervall, intervall);
            autoEvent.WaitOne();
            timer.Change(intervall, intervall);
        }

        public void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();
            Environment.Exit(1);
        }

        private void Callback(object stateInfo)
        {
            var autoReset = (AutoResetEvent)stateInfo;
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            Console.WriteLine("Callback");

            try
            {
                var rabbitHandler = new RabbitHandler();
                if (!rabbitHandler.IsOpen)
                {
                    throw new Exception();
                }

                var apiHandler = new ApiHandler();
                Stream dataStream = apiHandler.GetDataFromApi();

                using (StreamReader reader = new StreamReader(dataStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        jsonHandler = new JsonHandler(line);
                        if (!jsonHandler.IsValid)
                        {
                            break;
                        }
                        rabbitHandler.Send(jsonHandler.StockUpdateJson);
                        //apiHandler.CommitPut(jsonHandler.StockUpdateJson.GetValue("sequence_no").ToString());
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            timer.Change(intervall, intervall);
        }
    }
}