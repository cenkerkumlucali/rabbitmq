namespace RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessage;

public record RequestMessage
{
    public int MessageNo { get; set; }
    public string Text { get; set; }
}