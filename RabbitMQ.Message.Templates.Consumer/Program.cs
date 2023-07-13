using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı oluşturma.
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("URI");

//Bağlantıyı aktifleştirme ve kanal açma.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point-to-Poing) Tasarımı

// string queueName = "example-p2p-queue";
// channel.QueueDeclare(
//     queue: queueName,
//     durable: false,
//     exclusive: false,
//     autoDelete: false
// );
// EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
// channel.BasicConsume(queue: queueName, autoAck: false,consumer:consumer);
//
// consumer.Received += (sender, eventArgs) =>
// {
//     string message = Encoding.UTF8.GetString(eventArgs.Body.Span);
//     Console.WriteLine(message);
// };

#endregion

#region Publish/Subsribe (Pub/Sub) Tasarımı

// string exchangeName = "example-pub-sub-exchange";
// channel.ExchangeDeclare(
//     exchange: exchangeName,
//     type: ExchangeType.Fanout);
//
// string queueName = channel.QueueDeclare().QueueName;
//
// channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty);
//
// EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
//
// channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
//
// channel.BasicQos(
//     prefetchSize: 0,
//     prefetchCount: 1,
//     global: false);
//
// consumer.Received += (sender, eventArgs) =>
// {
//     string message = Encoding.UTF8.GetString(eventArgs.Body.Span);
//     Console.WriteLine(message);
// };

#endregion

#region Work Queue(İş Kuyruğu) Tasarımı

#endregion

#region Request/Response Tasarımı
string requestQueueName = "example-request-response-queue";
channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(
    queue: requestQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, eventArgs) =>
{
    string message = Encoding.UTF8.GetString(eventArgs.Body.Span);
    Console.WriteLine($"{message}");
    byte[] responseMessage = Encoding.UTF8.GetBytes($"İşlem tamamlandı :{message}");
    IBasicProperties properties = channel.CreateBasicProperties();
    properties.CorrelationId = eventArgs.BasicProperties.CorrelationId;
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: eventArgs.BasicProperties.ReplyTo,
        body: responseMessage,
        basicProperties: properties
    );
};

#endregion

Console.Read();