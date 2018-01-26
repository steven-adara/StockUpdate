using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web;

using Newtonsoft.Json;

namespace Kfzteile24.Interfaces.StockUpdate.Producer
{
    public class ApiHandler
    {
        private Stream streamData;
        private string query;

        public Stream GetDataFromApi()
        {
            var uriBuilder = new UriBuilder()
            {
                Host = ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["HostName"],
                Port = Convert.ToInt32(ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["Port"]),
                Path = ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["Path"],
                Query = "locationCode=LUD&limit=500"
            };

            var client = new HttpClient();

            //var client = GetClient();

            client.DefaultRequestHeaders.Add("UserToken", ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["Token"]);

            streamData = client.GetStreamAsync(uriBuilder.ToString()).Result;
            return streamData;
        }

        public void CommitPut(string sequenceId)
        {

            var uriBuilder = new UriBuilder()
            {
                Host = ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["HostName"],
                Port = Convert.ToInt32(ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["Port"]),
                Path = ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["Path"],
                Query = String.Format("sequence_no={0}", sequenceId)
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("UserToken", ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["Token"]);
            client.PutAsync(uriBuilder.ToString(), null);
        }

        private HttpClient GetHttplClient()
        {
            var uriBuilder = new UriBuilder()
            {
                Host = ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["HostName"],
                Port = Convert.ToInt32(ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["Port"]),
                Path = ApplicationConfiguration.Configuration.GetSection("Connection").GetSection("ApiSettings")["Path"],
                Query = "locationCode=LUD&limit=500"
            };


            return new HttpClient();
        }
    }
}