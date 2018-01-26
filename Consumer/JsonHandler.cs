using System.Text;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Kfzteile24.Interfaces.StockUpdate.Consumer
{

    internal class JsonHandler
    {
        private JObject _stockMovementJson;
        private JSchema _schemaJson;

        public bool IsValid { get { return _stockMovementJson != null && _stockMovementJson.IsValid(_schemaJson); } }

        public JsonHandler(StockMovement stockMovement)
        {
            _schemaJson = JSchema.Parse(File.ReadAllText(@"C:\Users\user\Documents\Visual Studio 2017\Projects\RabbitMQ\Schemas\Product.json"));

            _stockMovementJson = JObject.Parse(JsonConvert.SerializeObject(stockMovement));
        }
    }


}