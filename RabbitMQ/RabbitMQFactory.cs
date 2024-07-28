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
    }
}
