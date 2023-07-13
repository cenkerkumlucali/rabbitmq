using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı oluşturma.
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ochdqoxc:qm92Dtjw4giTO9AMCCIq830rpCk-bMlz@chimpanzee.rmq.cloudamqp.com/ochdqoxc");

//Bağlantıyı aktifleştirme ve kanal açma.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();
channel.ExchangeDeclare(exchange: "header-exchange-example", type: ExchangeType.Headers);
for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write("Lütfen header value değerini giriniz :");
    string? value = Console.ReadLine();
    IBasicProperties properties = channel.CreateBasicProperties();
    properties.Headers = new Dictionary<string, object>
    {
        ["no"] = value
    };
    channel.BasicPublish(exchange:"header-exchange-example",routingKey:string.Empty,body:message,basicProperties:properties);
}

Console.Read();