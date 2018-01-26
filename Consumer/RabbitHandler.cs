using RabbitMQ.Client;
using System;
using Microsoft.Extensions.Configuration;


namespace Kfzteile24.Interfaces.StockUpdate.Consumer
{

    internal class RabbitHandler
    {
        public RabbitHandler()
        {
            this.EnsureOpenConnection();
        }
        private IConnection connection;

        private void Connect()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = ApplicationConfiguration.Configuration.GetSection("RabbitSettings")["HostName"],
                UserName = ApplicationConfiguration.Configuration.GetSection("RabbitSettings")["UserName"],
                Password = ApplicationConfiguration.Configuration.GetSection("RabbitSettings")["Password"],
                RequestedHeartbeat = Convert.ToUInt16(ApplicationConfiguration.Configuration.GetSection("RabbitSettings")["Hearbeat"])
            };

            connection = connectionFactory.CreateConnection();
        }

        private void EnsureOpenConnection()
        {
            if (!connection.IsOpen)
            {
                this.Connect();
            }
        }
    }
}