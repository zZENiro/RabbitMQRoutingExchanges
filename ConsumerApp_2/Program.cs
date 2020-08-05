using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerApp_2
{
    class Program
    {
        const string queue_2 = "q_2";
        const string exchange_1 = "exchnage_1"; 

        static string consumerName;

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            consumerName = args[0];

            using (var connection = factory.CreateConnection())
            using (var chanel = connection.CreateModel())
            {
                chanel.QueueDeclare(queue_2, false, false, true);
                chanel.QueueBind(queue_2, exchange_1, "black");
                chanel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(chanel);
                consumer.Received += (sender, ea) =>
                {
                    var message = Encoding.ASCII.GetString(ea.Body.ToArray());
                    ProcessMessage(message);
                    chanel.BasicAck(ea.DeliveryTag, false);
                };

                chanel.BasicConsume(queue_2, false, consumer);

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
