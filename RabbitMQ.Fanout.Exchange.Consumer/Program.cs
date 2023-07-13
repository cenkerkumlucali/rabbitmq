
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı oluşturma.
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ochdqoxc:qm92Dtjw4giTO9AMCCIq830rpCk-bMlz@chimpanzee.rmq.cloudamqp.com/ochdqoxc");

//Bağlantıyı aktifleştirme ve kanal açma.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

Console.Write("Kuyruk adını giriniz: ");
string queueName = Console.ReadLine();

channel.QueueDeclare(queue: queueName, exclusive: false);

channel.QueueBind(queue: queueName, exchange: "fanout-exchange-example", routingKey: String.Empty);

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(
    queue: queueName, autoAck: true, consumer: consumer);
consumer.Received += (sender, eventArgs) =>
{
    string message = Encoding.UTF8.GetString(eventArgs.Body.Span);
    Console.WriteLine(message);
};
Console.Read();