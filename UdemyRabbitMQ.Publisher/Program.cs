using RabbitMQ.Client;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace UdemyRabbitMQ.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://jwhtwypr:j6FED0tFdY5hSMni99ndYkNNwudznKmU@woodpecker.rmq.cloudamqp.com/jwhtwypr");

            using var connection = factory.CreateConnection();
            
            var channel = connection.CreateModel();

            //channel.QueueDeclare("hello-queue", true, false, false);
            channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);


            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"log {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs-fanout","", null, messageBody);

                Console.WriteLine($"Mesaj Gönderilmiştir. {message}");
            });
            
            Console.ReadLine();
        }
    }
}
