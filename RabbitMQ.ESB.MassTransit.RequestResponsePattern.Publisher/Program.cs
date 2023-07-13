using MassTransit;
using RabbitMQ.ESB.MassTransit.RequestResponsePattern.Consumer.Consumers;

string rabbitMQUri = "URI";

string requestQueue = "request-queue";

string queueName = "example-queue";
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
    factory.ReceiveEndpoint(requestQueue, endpoint =>
    {
        endpoint.Consumer<RequestMessageConsumer>();
    });
});

await bus.StartAsync();

Console.Read();