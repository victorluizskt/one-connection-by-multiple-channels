using Produtor.RabbitMQ;
using RabbitMQ.Client;
using System.Text;

namespace Produtor
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            var queues = new List<string>
            {
                QueueList.QUEUE_FACEBOOK,
                QueueList.QUEUE_GOOGLE,
                QueueList.QUEUE_ORKUT,
                QueueList.QUEUE_TWITTER,
                QueueList.QUEUE_INSTAGRAM
            };

            RunQueue(queues);
        }

        [Obsolete]
        private static void RunQueue(List<string> queues)
        {
            var connection = RabbitMQFactory.CreateConnection(null);

            var cont = 1000;
            while (cont > 0)
            {
                foreach (var queue in queues)
                {
                    GetChannelFactoryToSendFactory(queue, connection, "Victor L.");
                }

                cont--;
            }
        }

        [Obsolete]
        private static void GetChannelFactoryToSendFactory(string queueToPublish, IConnection connection, string publisher)
        {
            switch (queueToPublish)
            {
                case QueueList.QUEUE_FACEBOOK:
                case QueueList.QUEUE_GOOGLE:
                case QueueList.QUEUE_ORKUT:
                case QueueList.QUEUE_TWITTER:
                case QueueList.QUEUE_INSTAGRAM:
                    BuildPublishers(connection, queueToPublish, publisher);
                    break;
                default:
                    BuildPublishers(connection, queueToPublish, publisher);
                    break;
            }
        }

        [Obsolete]
        private static void BuildPublishers(IConnection connection, string queueName, string publisherName)
        {
            Task.Run(() =>
            {
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var basicPublishBatch = channel.CreateBasicPublishBatch();

                for (int i = 0; i < 1000000; i++)
                {
                    var body = Encoding.UTF8.GetBytes("Vamos codificar.");
                    basicPublishBatch.Add(exchange: "", routingKey: queueName, mandatory: false, properties: null, body: body);
                }

                basicPublishBatch.Publish();
                Console.WriteLine($"{publisherName} sent 1000 messages to queue: {queueName}.");
                Thread.Sleep(1000);
            });
        }
    }
}
