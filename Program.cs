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
            QueueList.QUEUE_CALCULO_ALUGUEL,
            QueueList.QUEUE_KM_EXCEDENTE,
            QueueList.QUEUE_MULTA,
            QueueList.QUEUE_PNEU,
            QueueList.QUEUE_REEMBOLSO
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
                case QueueList.QUEUE_CALCULO_ALUGUEL:
                case QueueList.QUEUE_KM_EXCEDENTE:
                case QueueList.QUEUE_MULTA:
                case QueueList.QUEUE_PNEU:
                case QueueList.QUEUE_REEMBOLSO:
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
