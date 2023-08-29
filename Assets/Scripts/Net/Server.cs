using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Server : MonoBehaviour
{
    #region SingleTon Implementation
    public static Server instance { set; get; }

    public void Awake()
    {
        instance = this;
    }
    #endregion

    public NetworkDriver driver;
    private NativeList<NetworkConnection> connections;

    private bool isActive = false;
    private const float keepAliveTickRate = 20.0f;
    private float lastKeepAlive;

    public Action connectionDropped;

    // methods

    public void Init(ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndpoint endpoint = NetworkEndpoint.AnyIpv4;
        endpoint.Port = port;

        if(driver.Bind(endpoint) != 0)
        {
            Debug.Log("Unable to bind on port " + endpoint.Port);
            return;
        }
        else
        {
            driver.Listen();
            Debug.Log("currently listening on port " + endpoint.Port);
        }

        connections = new NativeList<NetworkConnection>(2, Allocator.Persistent);
        isActive = true;
    }
    public void Shutdown()
    {
        if(isActive)
        {
            driver.Dispose();
            connections.Dispose();
            isActive = false;
        }
    }
    public void OnDestroy()
    {
        Shutdown();
    }
    
    public void Update()
    {
        if (!isActive)
            return;

        KeepAlive();

        driver.ScheduleUpdate().Complete();

        CleanupConnections();
        AcceptNewConnections();
        UpdateMessagePump();
    }

    //look at all connections
    private void CleanupConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i; // don't break loop
            }
        }
    }

    //someone knocking at the door?
    private void AcceptNewConnections()
    {
        // accept new connections
        NetworkConnection c;
        while ((c = driver.Accept()) != default(NetworkConnection)) //Anyone trying to connect?
        {
            connections.Add(c); //if someone is there add them to the list
        }
    }

    private void UpdateMessagePump()
    {
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if(cmd == NetworkEvent.Type.Data)
                {
                    NetUtility.OnData(stream, connections[i], this);
                }
                else if(cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    connections[i] = default(NetworkConnection);
                    connectionDropped?.Invoke();
                    Shutdown(); // this is just because it is a two player game, it shuts down the whole server instead of just disconnecting one
                }
            }
        }
    }

    //Server specific

    public void SendToClient(NetworkConnection connection, NetMessage msg)
    {
        DataStreamWriter writer; // get box
        driver.BeginSend(connection, out writer); // write address on box
        msg.Serialize(ref writer); // put stuff in box
        driver.EndSend(writer); // give back to post man to send out
    }
    public void Broadcast(NetMessage msg)
    {
        for (int i = 0; i < connections.Length; i++)
            if(connections[i].IsCreated)
            {
                Debug.Log($"Sending {msg.Code} to : {connections[i].InternalId}");
                SendToClient(connections[i], msg);
            }

        
    }


}
