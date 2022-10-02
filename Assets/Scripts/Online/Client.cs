using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles messages between the server and the game
public class Client : MonoBehaviour
{
    public string clientName;
    public bool isHost;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private int Skip = 0;
    public int GameSizeOnline;
    public bool readyToStart = false;

    public List<GameClient> players = new List<GameClient>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool ConnectToServer(string host, int port)
    {
        if (socketReady)
            return false;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            socketReady = true;
        }
        catch (Exception e)
        {
            MenuManager.Instance.connecting = false;
            Debug.Log("Socket error " + e.Message);
            Canvas go = Instantiate(GameManager.Instance.InfoPanelPopupPrefab);
                go.GetComponent<ErrorPopupHandler>().ErrorCode = 1;
            
        }

        return socketReady;
    }

    private void Update()
    {
        
        if (Skip == 0 && SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            Camera.main.GetComponent<HudElements>().Client = this.gameObject;
            Skip = 1;
        }

        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
        else
        {
            Canvas go = Instantiate(GameManager.Instance.InfoPanelPopupPrefab);
            go.GetComponent<ErrorPopupHandler>().ErrorCode = 2;
        }

        if(readyToStart == true)
        {
            GameManager.Instance.StartGame();
            readyToStart = false;
        }
    }

    //Sending messages to the server
    public void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
    }

    //Read messages from the server
    private void OnIncomingData(string data)
    {
        //Debug.Log(data);
        Debug.Log("Client:" + data);
        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "SWHO":
                for (int i = 1; i < aData.Length - 1; i++)
                {
                    UserConnected(aData[i], false);
                }
                Send("CWHO|" + clientName + "|" + ((isHost) ? 1 : 0).ToString());
                break;

            case "SCNN":
                UserConnected(aData[1], false);
                if (isHost)
                {
                    Send("CGBS|" + GameSizeOnline.ToString());
                }
                break;

            case "SGBS":
                if (!isHost)
                {
                    GameObject.Find("PersistGameObject").GetComponent<PersistObject>().boardSize = int.Parse(aData[1]);
                    readyToStart = true;
                }
                break;

            case "SMOV":

                //Here adapt for linedot
                BoardManager.Instance.ReceivedTurnCount = int.Parse(aData[4]);
                if (BoardManager.Instance.ReceivedTurnCount <= BoardManager.Instance.TurnCounter)
                { }
                else
                {
                    BoardManager.Instance.ReceivedX = int.Parse(aData[1]);
                    BoardManager.Instance.ReceivedY = int.Parse(aData[2]);
                    BoardManager.Instance.ReceivedHorizontal = (int.Parse(aData[3]) == 1 ? true : false);

                    Array.Clear(aData, 0, aData.Length);
                }

                //CheckersBoard.Instance.TryMove(int.Parse(aData[1]), int.Parse(aData[2]), int.Parse(aData[3]), int.Parse(aData[4]));
                
                break;

            case "SDCN":
                //Server Disconnection message
                Canvas go = Instantiate(GameManager.Instance.InfoPanelPopupPrefab);
                go.GetComponent<ErrorPopupHandler>().ErrorCode = 2;
                break;

            case "SCCK":
                //Server connection check
                //Currently not needed
                break;

            case "SPWR":
                //Player wants rematch
                if (Camera.main.GetComponent<HudElements>().RematchRequestSender != true)
                {
                    Canvas go1 = Instantiate(GameManager.Instance.InfoPanelPopupPrefab);
                    go1.GetComponent<ErrorPopupHandler>().ErrorCode = 3;
                    Camera.main.GetComponent<HudElements>().RematchRequestReceived = true;
                }
                break;

            case "SPAR":
                //Player accepts rematch
                if (Camera.main.GetComponent<HudElements>().RematchRequestSender == true)
                {
                    //GameObject.Find("InfoPanelCanvas").GetComponent<ErrorPopupHandler>().PopUpFinished();
                    Canvas.FindObjectOfType<ErrorPopupHandler>().PopUpFinished();
                    Camera.main.GetComponent<HudElements>().PauseButton.SetActive(true);
                    Camera.main.GetComponent<HudElements>().RematchRequestSender = false;
                    GameObject.Find("Board").GetComponent<BoardManager>().ResetCheck = true;
                    //GameObject.Find("Board").GetComponent<BoardManager>().currentPlayer += 1; //Doesnt currently work

                }
                break;
        }
        
    }

    private void UserConnected(string name, bool host)
    {
        GameClient c = new GameClient();
        c.name = name;

        players.Add(c);

        //This currently starts the game, later make it so the host can start when there are at least 2 players
        if (players.Count == 2)
        {
            GameObject.Find("PersistGameObject").GetComponent<PersistObject>().PlayerSlot3 = "empty";
            GameObject.Find("PersistGameObject").GetComponent<PersistObject>().PlayerSlot4 = "empty";
            if (isHost)
            {
                readyToStart = true;
            }
        }
            
        // GameManager.Instance.StartGame();
    }

    private void OnDestroy()
    {
        string msg = "CDCN|";
        Send(msg);
        CloseSocket("Destroy");
    }

    private void OnApplicationQuit()
    {
        string msg = "CDCN|";
        Send(msg);
        CloseSocket("Quit");
    }

    private void OnDisable()
    {
        string msg = "CDCN|";
        Send(msg);
        CloseSocket("Disable");
    }

    private void CloseSocket(string why)
    {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
        Debug.Log("Client Stopped" + why);
    }
}

public class GameClient
{
    public string name;
    public bool isHost;
}
