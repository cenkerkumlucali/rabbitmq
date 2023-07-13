using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

string rabbitMQUri = "amqps://ochdqoxc:qm92Dtjw4giTO9AMCCIq830rpCk-bMlz@chimpanzee.rmq.cloudamqp.com/ochdqoxc";

string queueName = "example-queue";
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});
ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new Uri($"{rabbitMQUri}/{queueName}"));

Console.Write("Gönderilecek mesaj :");
string? message = Console.ReadLine();
await sendEndpoint.Send<IMessage>(new ExampleMessage() { Text = message });

Console.Read();