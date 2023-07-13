using System.Text;
using RabbitMQ.Client;

//Bağlantı oluşturma.
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("URI");

//Bağlantıyı aktifleştirme ve kanal açma.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

//Queue'ya mesaj gönderme
//RabbitMQ kuyruğa atacağı mesajları byte türünden kabul etmektedir. Haliyle mesajları bizim byte dönüştürmemiz gerekecektir.
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

for (int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties);
}

Console.Read();