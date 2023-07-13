using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessage;

namespace RabbitMQ.ESB.MassTransit.RequestResponsePattern.Consumer.Consumers;

public class RequestMessageConsumer:IConsumer<RequestMessage>
{
    public Task Consume(ConsumeContext<RequestMessage> context)
    {
        Console.WriteLine(context.Message.Text);
        context.RespondAsync<ResponseMessage>(new{Text = $"{context.Message.MessageNo}. response to request"});
        return Task.CompletedTask;
    }
}