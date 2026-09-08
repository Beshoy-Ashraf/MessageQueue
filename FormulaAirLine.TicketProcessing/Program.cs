using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Wellcome to Formula AirLine Ticket Processing System!");
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
var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, EventArgs) =>
{
      var body = EventArgs.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      Console.WriteLine($"Received booking request: {message}");

      await Task.Delay(1000);

      Console.WriteLine($"Processed booking request: {message}");
};

await channel.BasicConsumeAsync(queue: "booking-queue",
     autoAck: true,
     consumer: consumer);