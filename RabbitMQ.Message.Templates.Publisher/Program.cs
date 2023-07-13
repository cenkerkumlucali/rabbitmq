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

//
// string queueName = "example-p2p-queue";
// channel.QueueDeclare(
//     queue: queueName,
//     durable: false,
//     exclusive: false,
//     autoDelete: false
// );
// byte[] message = Encoding.UTF8.GetBytes("Merhaba");
// channel.BasicPublish(exchange: "", routingKey: queueName, body: message);

#endregion

#region Publish/Subsribe (Pub/Sub) Tasarımı

// string exchangeName = "example-pub-sub-exchange";
// channel.ExchangeDeclare(
//     exchange: exchangeName,
//     type: ExchangeType.Fanout);
// for (int i = 0; i < 100; i++)
// {
//     byte[] message = Encoding.UTF8.GetBytes("Merhaba "+i);
//     channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, body: message);
// }

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

string replyQueueName = channel.QueueDeclare().QueueName;
string correlationId = Guid.NewGuid().ToString();

#region Request Mesajını Oluşturma ve Gönderme

IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;
for (int i = 0; i < 100; i++)
{

    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);

    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        body: message,
        basicProperties: properties
    );
}

#endregion

#region Response Kuyruğu Dinleme

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer);
consumer.Received += async (sender, eventArgs) =>
{
    if (eventArgs.BasicProperties.CorrelationId == correlationId)
    {
        string message = Encoding.UTF8.GetString(eventArgs.Body.Span);
        Console.WriteLine($"Response :{message}");
    }
};

#endregion

#endregion


Console.Read();