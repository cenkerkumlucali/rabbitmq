//Bağlantı oluşturma.

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ochdqoxc:qm92Dtjw4giTO9AMCCIq830rpCk-bMlz@chimpanzee.rmq.cloudamqp.com/ochdqoxc");

//Bağlantıyı aktifleştirme ve kanal açma.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue oluşturma
channel.QueueDeclare(queue: "example-queue",
    exclusive: false,
    durable: true); //Consumer'da da kuyruk publisher'daki ile birebir aynı yapılandırmada tanımlanmalıdır!

//Queue'dan Mesaj okuma
EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queue: "example-queue", autoAck: false, consumer: consumer);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
consumer.Received += async (sender, eventArgs) =>
{
    //Kuyruğa gelen mesajın işlendiği yer.
    // eventArgs.Body : Kuyruktaki mesajın verisini bütünsel getiricektir.
    // eventArgs.Body.Span veya eventArgs.Body.toArray() : kuyruktaki mesajın byte verisini getiricektir.
    string body = Encoding.UTF8.GetString(eventArgs.Body.Span);
    Console.WriteLine(body);
    await Task.Delay(3000);
    channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
};
Console.Read();