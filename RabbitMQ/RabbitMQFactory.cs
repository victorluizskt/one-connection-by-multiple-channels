using RabbitMQ.Client;

namespace Produtor.RabbitMQ
{
    public class RabbitMQFactory
    {
        public static IConnection CreateConnection(IConnection? connection) 
        {
            if(connection != null)
            {
                return connection;
            }

            var factory = new ConnectionFactory() { HostName = "localhost" };
            return factory.CreateConnection();
        }

        public static IDictionary<string, IModel> CreateModels(IEnumerable<string> channels, IConnection connection)
        {
            var modelsDict = new Dictionary<string, IModel>();

            foreach (var channel in channels)
            {
                var model = connection.CreateModel(); 
                modelsDict[channel] = model;
            }

            return modelsDict;
        }
    }
}
