using Logger.Console;
using SocketDemo.Lib;

var server = new TcpSocket("127.0.0.1", 8005, new LogToConsole());
server.ServerStart();

while (true)
{
    var client = server.AcceptClient();
    Task.Run(() =>
    {
        while (true)
        {
            var message = client.ReceiveMessage();

            if (message == "exit")
            {
                client.Dispose();
                break;
            }

            client.SendMessage($"Ваше сообщение: {message} - получено");
        }
    });

}
