using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerApp
{
    class Program
    {
        const string queue_1 = "q_1";
        const string exchange_1 = "exchnage_1";
        const string exchange_2 = "exchnage_2";
        static string consumerName;

        static void Main(string[] args)
        {
            consumerName = args[0];

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var chanel = connection.CreateModel())
            {
                chanel.QueueDeclare(queue_1, false, false, true);
                
                chanel.QueueBind(queue_1, exchange_2, "red");
                chanel.QueueBind(queue_1, exchange_2, "black");
                chanel.QueueBind(queue_1, exchange_1, "black");

                chanel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(chanel);
                consumer.Received += (sender, ea) =>
                {
                    var message = Encoding.ASCII.GetString(ea.Body.ToArray());
                    chanel.BasicAck(ea.DeliveryTag, false);
                };

                chanel.BasicConsume(queue_1, false, consumer);

                System.Console.WriteLine($"{consumerName} is started!");
                Console.ReadKey();
            }
        }

        static void ProcessMessage(string message)
        {
            System.Console.WriteLine($"{consumerName} processing new message...");
            Thread.Sleep(3000);
            System.Console.WriteLine($"{consumerName} done message: {message}.");
        }
    }
}
