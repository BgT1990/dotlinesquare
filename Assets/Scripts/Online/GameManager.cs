using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Handles Online functionality
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    public GameObject mainMenu;
    public GameObject serverMenu;
    public GameObject connectMenu;
    
    public GameObject serverPrefab;
    public GameObject clientPrefab;
    public Canvas InfoPanelPopupPrefab;

    public InputField nameInput;
    public InputField nameInputnew1;
    public InputField nameInputnew2;
    public InputField hostInput;
    public InputField hostInputnew;
    public bool theOne;

    private void Start()
    {
        theOne = false;

        if (!theOne)
        {
            if (FindObjectsOfType<GameManager>().Length > 1)
            {

                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
                theOne = true;

                //handled in MenuManager
                //serverMenu.SetActive(false);
                //connectMenu.SetActive(false);

                //obsolete
                //DontDestroyOnLoad(gameObject);
            }
        }
    }

    ////handled in MenuManager
    //public void ConnectButton()
    //{
    //    //Debug.Log("Connect");
    //    mainMenu.SetActive(false);
    //    connectMenu.SetActive(true);
    //}

    public void HostButton()
    {
        //Debug.Log("Host");
        try
        {
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.Init();

            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            //c.clientName = GameObject.Find("NameInput").GetComponent<InputField>().text;
            //c.clientName = nameInput.text;
            //c.clientName = nameInputnew1.text;

            c.clientName = nameInputnew1.text != "" ? nameInputnew1.text : nameInputnew2.text;

            c.isHost = true;
            if (c.clientName == "")
            {
                c.clientName = "Host"; Debug.Log("Using default name");
            }
            c.ConnectToServer("127.0.0.1", 6321);
            c.GameSizeOnline = GameObject.Find("PersistGameObject").GetComponent<PersistObject>().boardSize;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        //handled in MenuManager
        //mainMenu.SetActive(false);
        //serverMenu.SetActive(true);
    }

    public void ConnectToServerButton()
    {
        //string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;
        //string hostAddress = hostInput.text;
        string hostAddress = hostInputnew.text != "" ? hostInputnew.text : hostInput.text;
        if (hostAddress == "")
        {
            hostAddress = "127.0.0.1";
            Debug.Log("Trying default IP");
        }

        try
        {
            MenuManager.Instance.connecting = true;
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            //c.clientName = GameObject.Find("NameInput").GetComponent<InputField>().text;
            //c.clientName = nameInput.text;
            //c.clientName = nameInputnew1.text;
            c.clientName = nameInputnew1.text != "" ? nameInputnew1.text : nameInputnew2.text;
            if (c.clientName == "")
            {
                c.clientName = "Client"; Debug.Log("Using default name");
            }
            c.ConnectToServer(hostAddress, 6321);
            //connectMenu.SetActive(false);

            
        }
        catch (Exception e)
        {
            //MenuManager.Instance.ClientOptions.GetComponent<GameObject>().SetActive(true);
            MenuManager.Instance.connecting = false;
            Debug.Log(e.Message);
            
        }
    }

    public void BackButton()
    {
        //handled in MenuManager
        //mainMenu.SetActive(true);
        //serverMenu.SetActive(false);
        //connectMenu.SetActive(false);

        Server s = FindObjectOfType<Server>();
        if (s != null)
        {
            Destroy(s.gameObject);
            Debug.Log("Servers removed");
        }

        foreach(Client c in FindObjectsOfType<Client>())
        if (c != null)
        {
            Destroy(c.gameObject);
                Debug.Log("Client " + c.clientName + c.GetInstanceID().ToString() + " removed");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SquareGameScene");
    }

}
