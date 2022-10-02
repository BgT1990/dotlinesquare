using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UnityEngine;

//Handles messages between the client and the internet
public class Server : MonoBehaviour //If want server on Linux, remove monobehaviour and use a run on update loop at all times. (see @11:00 mins on Server tutorial video)//actually it works anyway
{
    public int port = 6321;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;
    private bool serverStarted;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void Update()
    {
        if (!serverStarted)
            return;

        foreach (ServerClient c in clients)
        {
            //Is the client still connected?
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            //Tell our player somebody has disconnected

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void StopListening(string why)
    {
        try
        {
            foreach (ServerClient c in clients)
            {
                c.tcp.Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        // You must close the tcp listener
        try
        {
            server.Stop();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        Debug.Log("Server Stopped" + why);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        string allUsers = "";
        foreach (ServerClient i in clients)
        {
            allUsers += i.clientName + '|';
        }

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);

        StartListening();

        //Debug.Log("Somebody has connected!");

        BroadCast("SWHO|" + allUsers, clients[clients.Count - 1]);
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    //Server Send
    private void BroadCast(string data, List<ServerClient> cl)
    {
        foreach (ServerClient sc in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("Write error : " + e.Message);
            }
        }
    }
    private void BroadCast(string data, ServerClient c)
    {
        List<ServerClient> sc = new List<ServerClient> { c };
        BroadCast(data, sc);
    }
    //Server Read
    private void OnIncomingData(ServerClient c, string data)
    {
        //Debug.Log(c.clientName + " : " + data);
        //Debug.Log(data);
        Debug.Log("Server:" + data);
        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "CWHO":
                //Client connected and self identifying
                c.clientName = aData[1];
                c.isHost = (aData[2] == "0") ? false : true;
                BroadCast("SCNN|" + c.clientName, clients);
                break;

            case "CGBS":
                //Game Board Size
                BroadCast("SGBS|" + aData[1], clients);

                break;

            case "CMOV":
                //Client sends a move
               // if (BoardManager.Instance.isMyTurn == true)
                //{
                    BroadCast("SMOV|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4], clients);
                    //data.Replace('C', 'S');
                    BroadCast(data, clients);
                    Array.Clear(aData, 0, aData.Length);
                //}
                break;

            case "CDCN":
                //Client sends disconnection notification
                BroadCast("SDCN|", clients);
                break;

            case "CCCK":
                //Client connection check
                //Currently not needed
                break;

                case "CPWR":
                //Client wants to play again
                BroadCast("SPWR|", clients);
                break;
                
                case "CPAR":
                //Client player accepts rematch
                BroadCast("SPAR|", clients);
                break;

        }
        
    }

    private void OnDestroy()
    {
        BroadCast("SDCN|", clients);
        StopListening("Destroy");
    }

    private void OnApplicationQuit()
    {
        BroadCast("SDCN|", clients);
        StopListening("Quit");
    }

    private void OnDisable()
    {
        BroadCast("SDCN|", clients);
        StopListening("Disable");
    }

}

public class ServerClient
{
    public string clientName;
    public TcpClient tcp;
    public bool isHost;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }
}