using Logger.Console;
using SocketDemo.Lib;

var server = new TcpSocket("127.0.0.1", 8005, new LogToConsole());
server.ConnectToServer();

Console.Write("Введите текст мообщения: ");
var message = Console.ReadLine();

server.SendMessage(message);

message = server.ReceiveMessage();
Console.WriteLine($"Ответ сервера: {message}");

server.Dispose();
