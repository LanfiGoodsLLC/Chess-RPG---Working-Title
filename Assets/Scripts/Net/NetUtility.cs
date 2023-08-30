using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;


public enum OpCode
{ // try to not go above 255 messages
    KEEP_ALIVE = 1,
    WELCOME = 2,
    START_GAME = 3,
    MAKE_MOVE = 4,
    REMATCH = 5,

}

public static class NetUtility 
{

    public static void OnData(DataStreamReader stream, NetworkConnection cnn, Server server = null)
    {
        NetMessage msg = null; // when we get data make a message
        var opCode = (OpCode)stream.ReadByte(); //open box and see what's inside
        switch (opCode) // tells you what type of message with data stream
        {
            case OpCode.KEEP_ALIVE: msg = new NetKeepAlive(stream); break;
            case OpCode.WELCOME: msg = new NetWelcome(stream); break;
            case OpCode.START_GAME: msg = new NetStartGame(stream); break;
            case OpCode.MAKE_MOVE: msg = new NetMakeMove(stream); break;
            case OpCode.REMATCH: msg = new NetRematch(stream); break;
            default:
                Debug.LogError("Message received had no OpCode"); // means code is going to explode
                break;
        }
        
        if(server != null)
        
       msg.ReceivedOnServer(cnn);
         else
       msg.ReceivedOnClient();
        
    }

    //C Client, S Server
    //Net Messages
    public static Action<NetMessage> C_KEEP_ALIVE;
    public static Action<NetMessage> C_WELCOME;
    public static Action<NetMessage> C_START_GAME;
    public static Action<NetMessage> C_MAKE_MOVE;
    public static Action<NetMessage> C_REMATCH;
    public static Action<NetMessage, NetworkConnection> S_KEEP_ALIVE;
    public static Action<NetMessage, NetworkConnection> S_WELCOME;
    public static Action<NetMessage, NetworkConnection> S_START_GAME;
    public static Action<NetMessage, NetworkConnection> S_MAKE_MOVE;
    public static Action<NetMessage, NetworkConnection> S_REMATCH;

}
