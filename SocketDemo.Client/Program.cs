using System.Net;
using System.Net.Sockets;
using System.Text;
using Logger.Console;

var log = new LogToConsole();

log.Info("Запуск клиента:");

var ip = IPAddress.Parse("127.0.0.1");
var port = 8005;
var endPoint = new IPEndPoint(ip, port);
log.Info("- Определение IP-адреса сервера");

var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
server.Connect(endPoint);
log.Success("Подключение к серверу прошло успешно.");

Console.Write("Введите текст мообщения: ");
var messageSend = Console.ReadLine();
var data = Encoding.Unicode.GetBytes(messageSend);
server.Send(data);
log.Info("Отправка сообщения на сервер");

var message = new StringBuilder();
var bytes = 0;
data = new byte[128];

do
{
    bytes = server.Receive(data);
    message.Append(Encoding.Unicode.GetString(data, 0, bytes));
} while (server.Available > 0);

log.Success($"Ответ от сервера: {message.ToString()}");

server.Shutdown(SocketShutdown.Both);
server.Close();
