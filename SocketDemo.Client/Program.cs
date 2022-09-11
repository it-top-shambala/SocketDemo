using Logger.Console;
using SocketDemo.Lib;

var server = new TcpSocket("127.0.0.1", 8005, new LogToConsole());
server.ConnectToServer();

while (true)
{
    Console.Write("Введите текст мообщения (введите exit для выхода): ");
    var message = Console.ReadLine();

    if (message == "exit")
    {
        server.SendMessage(message);
        server.Dispose();
        break;
    }

    server.SendMessage(message);

    message = server.ReceiveMessage();
    Console.WriteLine($"Ответ сервера: {message}");
}

Console.WriteLine("Выход...");
