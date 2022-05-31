using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, World!");

ConnectionFactory _connectionFactory = new()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "usuarioMaster",
    Password = "s3nh40k",
    VirtualHost = "main",
    DispatchConsumersAsync = true
};

using var connection = _connectionFactory.CreateConnection();
var model = connection.CreateModel();

model.QueueDeclare("pocExchange_consumer2_queue", true, false, false);
model.QueueBind("pocExchange_consumer2_queue", "pocExchange", "#.add", null);

var consumer = new AsyncEventingBasicConsumer(model);

model.BasicQos(0, 1, false);

consumer.Received += async (_, eventArgs) =>
 {
     await Task.Delay(2000);
 };

model.BasicConsume(queue: "pocExchange_consumer2_queue", autoAck: false, consumer: consumer);

while (true)
{
    await Task.Delay(1000);
    Console.WriteLine("runnign");
}