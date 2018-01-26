using RabbitMQ.Client;
using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;


namespace Kfzteile24.Interfaces.StockUpdate.Producer
{

    internal class RabbitHandler
    {
        public bool IsOpen;
        private IConnection connection;
        private IModel channel;
        private IBasicProperties properties;
        public RabbitHandler()
        {
            this.EnsureOpenConnection();
            this.CreateChannel();
        }


        private void Connect()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("RabbitSettings")["HostName"],
                UserName = ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("RabbitSettings")["UserName"],
                Password = ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("RabbitSettings")["Password"],
                RequestedHeartbeat = Convert.ToUInt16(ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("RabbitSettings")["Hearbeat"])
            };

            connection = connectionFactory.CreateConnection();
        }

        private void CreateChannel()
        {
            this.channel = this.connection.CreateModel();
            this.properties = this.channel.CreateBasicProperties();
            this.properties.DeliveryMode = 2;
            this.properties.Persistent = true;
            this.properties.ContentType = "application/json";
        }

        private void EnsureOpenConnection()
        {
            if (connection == null || !connection.IsOpen)
            {
                this.Connect();
            }

            IsOpen = connection.IsOpen;
        }

        public void Send(JObject message)
        {
            try
            {
                this.channel.BasicPublish(exchange: "StockUpdate-ExC",
                                        routingKey: "",
                                        basicProperties: this.properties,
                                        body: Encoding.UTF8.GetBytes(message.ToString()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}