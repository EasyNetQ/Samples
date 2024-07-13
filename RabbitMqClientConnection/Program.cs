using RabbitMQ.Client;

var timeout = TimeSpan.FromSeconds(72);
// var connectionFactory = new ConnectionFactory
// {
//     HostName = "rabbitmq",
//     ContinuationTimeout = timeout,
//     HandshakeContinuationTimeout = timeout,
//     RequestedConnectionTimeout = timeout,
//     SocketReadTimeout = timeout,
//     SocketWriteTimeout = timeout
// };
var connectionFactory = new ConnectionFactory
{
    Uri = new Uri(@"amqp://rabbitmq/?connection_timeout=72000")
};

using var connection = connectionFactory.CreateConnection();

Console.WriteLine("connected! Hit return to end.");
Console.ReadLine();
