using Google.Protobuf;
using NetworkCore;
using NetworkCore.Packet;
using Session;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager
{
    ServerSession _session = new ServerSession();

    public void Init()
    {
        string ipString = "127.0.0.1";
        int port = 7777;
        IPAddress ipAddr = IPAddress.Parse(ipString);
        IPEndPoint endPoint = new IPEndPoint(ipAddr, port);

        Connector connector = new Connector();

        connector.Connect(endPoint, () => { return _session; }, 1);
    }

    public void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = _session.serverPacketHandler.GetPacketHandler(packet.Id);
            if (handler != null)
                handler.Invoke(_session, packet.Message);
        }
    }

    public void Send(IMessage packet)
    {
        _session.Send(packet);
    }


}
