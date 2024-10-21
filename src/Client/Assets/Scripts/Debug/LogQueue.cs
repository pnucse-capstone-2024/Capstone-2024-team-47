using Google.Protobuf;
using System.Collections.Generic;

public class LogQueue
{
    static LogQueue _instance = new LogQueue();
    public static LogQueue Instance { get { return _instance; } }

    Queue<string> _logQueue = new Queue<string>();
    object _lock = new object();

    public void Push(string logMessage)
    {
        lock (_lock)
        {
            _logQueue.Enqueue(logMessage);
        }
    }

    public string Pop()
    {
        lock (_lock)
        {
            if (_logQueue.Count == 0)
                return null;

            return _logQueue.Dequeue();
        }
    }

    public List<string> PopAll()
    {
        List<string> list = new List<string>();

        lock (_lock)
        {
            while (_logQueue.Count > 0)
                list.Add(_logQueue.Dequeue());
        }

        return list;
    }
}