using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SenderApp
{
    class Program
    {
        const string exchange_1 = "exchnage_1";
        const string exchange_2 = "exchnage_2";
        static string message;

        static void Main(string[] args)
        {
            message = args[0];
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var chanel = connection.CreateModel())
            {
                chanel.ExchangeDeclare(exchange_1, ExchangeType.Direct);
                chanel.ExchangeDeclare(exchange_2, ExchangeType.Direct);

                var props = chanel.CreateBasicProperties();
                props.Persistent = true;

                var body = Encoding.ASCII.GetBytes(message);

                // there i want to send a message to c2
                chanel.BasicPublish(exchange_1, "black", props, body);
                
                // chanel.BasicPublish(exchange_2, "red", props, body);
                // chanel.BasicPublish(exchange_2, "black", props, body);
                
                System.Console.WriteLine("Message sended.");
            }
        }
    }
}
