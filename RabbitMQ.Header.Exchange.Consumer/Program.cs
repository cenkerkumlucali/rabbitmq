using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı oluşturma.
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("URI");

//Bağlantıyı aktifleştirme ve kanal açma.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "header-exchange-example",
    type: ExchangeType.Headers);


Console.Write("Lütfen header value'sunu giriniz :");
string? value = Console.ReadLine();
string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(exchange: "header-exchange-example",
    queue: queueName,
    routingKey: string.Empty,
    arguments:new Dictionary<string, object>
    {
        ["x-match"] = "all",
        ["no"] = value
    }
);
EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
consumer.Received += (sender, eventArgs) =>
{
    string message = Encoding.UTF8.GetString(eventArgs.Body.Span);
    Console.WriteLine(message);
};

Console.Read();