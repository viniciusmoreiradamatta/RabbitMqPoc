using System.Text;
using RabbitMQ.Client;

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

model.ExchangeDeclare($"pocExchange", "topic", true, false, null);

int i = 0;

while (true)
{
    model.BasicPublish("pocExchange", "");
    model.ConfirmSelect();

    var basicProperties = model.CreateBasicProperties();

    basicProperties.MessageId = Guid.NewGuid().ToString("D");

    basicProperties.DeliveryMode = 2;

    basicProperties.Headers = new Dictionary<string, object>()
            {
                {"content-type","application/json" }
            };

    var data = new { Ok = true, Value = i };

    var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(data));

    model.BasicPublish(exchange: "pocExchange",
                        routingKey: "test.app.add",
                        basicProperties: basicProperties,
                        body: body);

    i++;

    await Task.Delay(1000);
}