using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı oluşturma.
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ochdqoxc:qm92Dtjw4giTO9AMCCIq830rpCk-bMlz@chimpanzee.rmq.cloudamqp.com/ochdqoxc");

//Bağlantıyı aktifleştirme ve kanal açma.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// 1. adım
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);
// 2. adım
string queueName = channel.QueueDeclare().QueueName;
// 3. adım
channel.QueueBind(queue: queueName, exchange: "direct-exchange-example", routingKey: "direct-queue-example");

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
consumer.Received += (sender, eventArgs) =>
{
    string message = Encoding.UTF8.GetString(eventArgs.Body.Span);
    Console.WriteLine(message);
};
Console.Read();

//1. adım: Publisherdaki exchange ile birebir aynı isim ve type a sahip bir exchange tanımlanmalıdır.
//2. adım: Publisher tarafından routing key de bulunan değerdeki kuyruğa gönderilen mesajalrı kendi oluşturduğumuz kuyruğa yönlendirerek tüketmememiz gerekmektedir. Bunun için öncelikle bir kuyruk oluşturmalıyız.