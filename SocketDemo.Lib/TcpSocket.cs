using System.Net;
using System.Net.Sockets;
using System.Text;
using Logger;

namespace SocketDemo.Lib;

public class TcpSocket : IDisposable
{
    private ILogger? _log;
    private Socket _socket;
    private IPEndPoint? _endPoint;

    public TcpSocket(string ip, int port, ILogger? log = null)
    {
        _log = log;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
    }

    public TcpSocket(Socket socket, ILogger? log = null)
    {
        _log = log;
        _socket = socket;
        _endPoint = null;
    }

    public void ServerStart()
    {
        if (_endPoint == null)
        {
            return;
        }

        _socket.Bind(_endPoint);
        _socket.Listen(10);
        _log?.Success("Сервер запущен.");
        _log?.Info("Ожидание подключения клиентов...");
    }

    public TcpSocket AcceptClient()
    {
        var client = new TcpSocket(_socket.Accept(), _log);
        _log?.Success("Клиент подключен.");
        _log?.Info("Ожидание сообщения от клиента");
        return client;
    }

    public void ConnectToServer()
    {
        if (_endPoint == null)
        {
            return;
        }

        _socket.Connect(_endPoint);
        _log?.Success("Подключение к серверу прошло успешно.");
    }

    public string ReceiveMessage()
    {
        var message = new StringBuilder();
        var data = new byte[128];

        do
        {
            var bytes = _socket.Receive(data);
            message.Append(Encoding.Unicode.GetString(data, 0, bytes));
        } while (_socket.Available > 0);

        _log?.Success("Приём данных от клиента закончен");
        _log?.Info(message.ToString());

        return message.ToString();
    }

    public void SendMessage(string message)
    {
        var data = Encoding.Unicode.GetBytes(message);
        _socket.Send(data);
        _log?.Success("Отправка данных выполнена");
    }

    public void Dispose()
    {
        _socket.Dispose();
    }
}
