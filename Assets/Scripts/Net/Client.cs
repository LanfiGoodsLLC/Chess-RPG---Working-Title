using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Client : MonoBehaviour
{
    #region SingleTon Implementation
    public static Client instance { set; get; }

    public void Awake()
    {
        instance = this;
    }
    #endregion

    public NetworkDriver driver;
    private NetworkConnection connection;

    private bool isActive = false;

    public Action connectionDropped;

    //methods
    public void Init(string ip, ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndpoint endpoint = NetworkEndpoint.Parse(ip, port);

        connection = driver.Connect(endpoint); //localhost 127.0.0.1

        Debug.Log("Attempting to connect to server on " + endpoint.Address);

        isActive = true;

        RegisterToEvent();
    }
    public void Shutdown()
    {
        if (isActive)
        {
            UnregisterToEvent();
            driver.Dispose();           
            isActive = false;
            connection = default(NetworkConnection);
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

        driver.ScheduleUpdate().Complete();
        CheckAlive();

        UpdateMessagePump();
    }
    private void CheckAlive()
    {
        if(!connection.IsCreated && isActive)
        {
            Debug.Log("Something went wrong, lost connection to server");
            connectionDropped?.Invoke();
            Shutdown();
        }
    }
    private void UpdateMessagePump()
    {
        DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
            {
                if(cmd == NetworkEvent.Type.Connect)
                {
                SendToServer(new NetWelcome());
                Debug.Log("We're connected");
                }

                else if (cmd == NetworkEvent.Type.Data)
                {
                NetUtility.OnData(stream, default(NetworkConnection));
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                Debug.Log("Client got disconnected from Server");
                connection = default(NetworkConnection);
                connectionDropped?.Invoke(); // question mark makes sure somebody is listening, if not it would crash
                Shutdown();
                }
            }
        
    }

    public void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer; // get box
        driver.BeginSend(connection, out writer); // write address on box
        msg.Serialize(ref writer); // put stuff in box
        driver.EndSend(writer); // give back to post man to send out
    }

    //event parsing

    private void RegisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
    }
    private void UnregisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
    }
    private void OnKeepAlive(NetMessage nm)
    {
        //send it back to keep bhoth side alive
        SendToServer(nm);
    }

}
