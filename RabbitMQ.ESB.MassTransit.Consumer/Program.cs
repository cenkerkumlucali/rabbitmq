using MassTransit;
using RabbitMQ.ESB.MassTransit.Consumer.Consumers;

string rabbitMQUri = "URI";
string queueName = "example-queue";
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
    factory.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<ExampleMessageConsumer>();
    });
});
await bus.StartAsync();
Console.Read();