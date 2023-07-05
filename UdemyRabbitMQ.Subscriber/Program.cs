using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace UdemyRabbitMQ.Subscriber
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
            //channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

            var randomQueueName = channel.QueueDeclare().QueueName;


            channel.QueueBind(randomQueueName,"logs-fanout","",null);



            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName,false,consumer);
            Console.WriteLine("Loglar dinleniyor. ");
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine(message);
                channel.BasicAck(e.DeliveryTag,false);
            };

            Console.ReadLine();
        }
    }
}
