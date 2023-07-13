using System.Text;
using RabbitMQ.Client;

//Bağlantı oluşturma.
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("URI");

//Bağlantıyı aktifleştirme ve kanal açma.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.Write("Mesah: ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange: "direct-exchange-example", routingKey: "direct-queue-example", body: byteMessage);
}

Console.Read();