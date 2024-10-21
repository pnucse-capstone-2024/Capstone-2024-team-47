using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public class PopupQueue
{
    static PopupQueue _instance = new PopupQueue();
    public static PopupQueue Instance { get { return _instance; } }

    Queue<Tuple<string, Action>> _popupQueue = new Queue<Tuple<string, Action>>();
    object _lock = new object();

    public void Push(string message)
    {
        lock (_lock)
        {
            _popupQueue.Enqueue(Tuple.Create<string, Action>(message, null));
        }
    }

    public void Push(string message, Action action)
    {
        lock (_lock)
        {
            _popupQueue.Enqueue(Tuple.Create(message, action));
        }
    }
    public void Push(Tuple<string, Action> item)
    {
        lock (_lock)
        {
            _popupQueue.Enqueue(item);
        }
    }

    public Tuple<string, Action> Pop()
    {
        lock (_lock)
        {
            if (_popupQueue.Count == 0)
                return null;

            return _popupQueue.Dequeue();
        }
    }

    public List<Tuple<string, Action>> PopAll()
    {
        List<Tuple<string, Action>> list = new List<Tuple<string, Action>>();
        lock (_lock) 
        {
            while (_popupQueue.Count > 0) 
                list.Add(_popupQueue.Dequeue());
        }

        return list;
    }
}