using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessage;

string rabbitMQUri = "URI";

string requestQueue = "request-queue";

string queueName = "example-queue";
IBusControl bus =
    Bus.Factory.CreateUsingRabbitMq(factory => { factory.Host(rabbitMQUri); });
await bus.StartAsync();

IRequestClient<RequestMessage> request =
    bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{requestQueue}"));
int i = 1;
while (true)
{
    await Task.Delay(200);
    Response<ResponseMessage> response = await request.GetResponse<ResponseMessage>(new {MessageNo = i,Text = $"{i}. request"});
    Console.WriteLine($"Response received :{response.Message.Text}");
    i++;
}
Console.Read();