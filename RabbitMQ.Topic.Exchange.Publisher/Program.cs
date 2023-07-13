using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı oluşturma.
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("URI");

//Bağlantıyı aktifleştirme ve kanal açma.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel(); 

channel.ExchangeDeclare(exchange:"topic-exchange-example",type:ExchangeType.Topic);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write("Mesajın gönderileceği topic formatını belirtiniz :");
    string? topic = Console.ReadLine();
    channel.BasicPublish(
        exchange:"topic-exchange-example",routingKey:topic,body:message);
}

Console.Read();