using System.Net;
using System.Net.Sockets;
using System.Text;
using Logger.Console;

var log = new LogToConsole();

log.Info("Запуск сервера:");

var ip = IPAddress.Parse("127.0.0.1");
var port = 8005;
var endPoint = new IPEndPoint(ip, port);
log.Info("- Определение IP-адреса сервера");

var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
log.Info("- Создание сокета для подключений клиентов");

server.Bind(endPoint);
server.Listen(10);
log.Success("Сервер запущен. Ожидание подключения клиентов...");

while (true)
{
    var client = server.Accept();
    log.Success("Клиент подключен.");
    log.Info("Ожидание сообщения от клиента");

    var message = new StringBuilder();
    var bytes = 0;
    var data = new byte[128];

    do
    {
        bytes = client.Receive(data);
        message.Append(Encoding.Unicode.GetString(data, 0, bytes));
    } while (client.Available > 0);

    log.Success("Приём данных от клиента закончен");
    log.Info(message.ToString());

    data = Encoding.Unicode.GetBytes("Ваши данные успешно получены");
    client.Send(data);

    client.Shutdown(SocketShutdown.Both);
    client.Close();
}
