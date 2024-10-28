using NetworkCore;
using Server.Game.Room;
using Server.Session;
using System.Net;

class Program
{
    static Listener _listener = new Listener();
    static List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();

    static void TickRoom(int tick = 100)
    {
        var timer = new System.Timers.Timer();
        timer.Interval = tick;
        timer.Elapsed += ((s, e) => { RoomManager.Instance.Update(); });
        timer.AutoReset = true;
        timer.Enabled = true;

        _timers.Add(timer); 
    }

    static void Main(string[] args)
    {
        string ipString = "127.0.0.1";
        int port = 7777;

        IPAddress ipAddr = IPAddress.Parse(ipString);
        IPEndPoint endPoint = new IPEndPoint(ipAddr, port);

        TickRoom();

        _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
        Logger.InfoLog("Listening...");

        while (true)
            Thread.Sleep(100);

        FileLogger.Instance.Stop();
    }
}