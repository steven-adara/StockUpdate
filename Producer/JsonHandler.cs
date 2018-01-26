using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

using System.Collections.Generic;

namespace Kfzteile24.Interfaces.StockUpdate.Producer
{
    internal class JsonHandler
    {
        public JObject StockUpdateJson { get; }
        public JObject StockUpdateJsonEnvelope { get; set; }
        private JSchema schema;
        private JSchema envelope;
        public bool IsValid { get { return StockUpdateJson != null && StockUpdateJson.IsValid(schema); } }
        public JsonHandler(string stockUpdate)
        {
            CreateEnvelopeObject();

            schema = JSchema.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + @"/StockUpdate.json"));
            envelope = JSchema.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + @"/Envelope.json"));
            StockUpdateJson = JObject.Parse(stockUpdate);

            bool t = StockUpdateJsonEnvelope.IsValid(envelope);

            StockUpdateJsonEnvelope["message"] = StockUpdateJson;
            StockUpdateJsonEnvelope["created_at"] = DateTime.Now;
        }

        private void CreateEnvelopeObject()
        {
            if (this.StockUpdateJsonEnvelope == null)
            {
                StockUpdateJsonEnvelope = new JObject();
                StockUpdateJsonEnvelope.Add("version", 0);
                StockUpdateJsonEnvelope.Add("type", "application/json");
                StockUpdateJsonEnvelope.Add("message", null);
                StockUpdateJsonEnvelope.Add("created_at", null);
            }
        }
    }
}