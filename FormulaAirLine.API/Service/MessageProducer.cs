using System.Text;
using System.Text.Json;
using FormulaAirLine.API.Service.Interface;
using RabbitMQ.Client;

namespace FormulaAirLine.API.Service;

public class MessageProducer : IMessageProducer
{
      public async Task SendMessage<T>(T message)
      {
            var factory = new ConnectionFactory()
            {
                  HostName = "localhost",
                  UserName = "guest",
                  Password = "guest",
                  VirtualHost = "/"

            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "booking-queue",
                  durable: false,
                  exclusive: false,
                  autoDelete: false,
                  arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "booking-queue",
                body: body);
      }

}
