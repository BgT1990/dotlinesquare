using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

//Carries values selected in the menu into the game
public class PersistObject : MonoBehaviour
{
    public int boardSize = 2;
    public int noOfPlayers;
    public int noOfAI;
    public string PlayerSlot1;
    public string PlayerSlot2;
    public string PlayerSlot3;
    public string PlayerSlot4;
    public string P1name;
    public string P2name;
    public string P3name;
    public string P4name;
    public List<Material> PlayerColours;
    public List<string> Slots;
    public bool PassedSlotsToBoard = false;
    public bool ReturnToSettings = false;
    public bool EnterGame = false;
    public bool AudioEnabled;
    public List<int> Wins;

    public GameObject Script = null;
    public GameObject Main = null;
    public GameObject Options = null;
    public GameObject Board = null;
    public Camera MainCamera = null;
    public GameObject AudioObject = null;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            Slots.Add("");
            Wins.Add(0);
        }
        PlayerSlot1 = "human";
        Slots[0] = "human";
        PlayerSlot2 = "empty";
        Slots[1] = "empty";
        PlayerSlot3 = "empty";
        Slots[2] = "empty";
        PlayerSlot4 = "empty";
        Slots[3] = "empty";
        noOfPlayers = 1;
        noOfAI = 0;

        if (FindObjectsOfType<PersistObject>().Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }

    }

    void Update()
    {
        //PlayerWrap();
        SizeWrap();
        //AiWrap();
        if (AudioObject == null)
        {
            GrabAudioObject();
        }

        if (PassedSlotsToBoard == false && !EnterGame)
        {
            //if(GameObject.Find("Board") != null)
            if(Board != null)
            {
                //GameObject Board = GameObject.Find("Board");
                BoardManager BoardScript = Board.GetComponent<BoardManager>();
                if (BoardScript.PlayingOnline == false)
                {
                    if (PlayerSlot1 == "empty")
                    {
                        BoardScript.isP1empty = true;
                    }
                    else
                    {
                        BoardScript.isP1empty = false;
                    }
                    if (PlayerSlot2 == "empty")
                    {
                        BoardScript.isP2empty = true;
                    }
                    else
                    {
                        BoardScript.isP2empty = false;
                    }
                    if (PlayerSlot3 == "empty")
                    {
                        BoardScript.isP3empty = true;
                    }
                    else
                    {
                        BoardScript.isP3empty = false;
                    }
                    if (PlayerSlot4 == "empty")
                    {
                        BoardScript.isP4empty = true;
                    }
                    else
                    {
                        BoardScript.isP4empty = false;
                    }
                }
                else if (BoardScript.PlayingOnline == true)
                {
                    noOfPlayers = 2;
                    noOfAI = 0;
                    PlayerSlot1 = "human";
                    Slots[0] = "human";
                    PlayerSlot2 = "human";
                    Slots[1] = "human";
                    BoardScript.isP1empty = false;
                    BoardScript.isP2empty = false;
                    BoardScript.isP3empty = true;
                    BoardScript.isP4empty = true;
                    BoardScript.noOfPlayers = 2;
                }
                MainCamera.GetComponent<HudElements>().P1name = P1name;
                MainCamera.GetComponent<HudElements>().PlayerNames[0] = P1name;
                MainCamera.GetComponent<HudElements>().P2name = P2name;
                MainCamera.GetComponent<HudElements>().PlayerNames[1] = P2name;
                MainCamera.GetComponent<HudElements>().P3name = P3name;
                MainCamera.GetComponent<HudElements>().PlayerNames[2] = P3name;
                MainCamera.GetComponent<HudElements>().P4name = P4name;
                MainCamera.GetComponent<HudElements>().PlayerNames[3] = P4name;
                for (int i = 0; i < 3; i++)
                {
                    BoardScript.PlayerColours[i] = PlayerColours[i];
                }
                MainCamera.GetComponent<HudElements>().DisableUnusedPlayerDisplaysNew();
                PassedSlotsToBoard = true;
            }
        }
        if (ReturnToSettings)
        {
            OnReturnToMenu();
        }
        if (EnterGame)
        {
            OnEnterOfflinePlay();
        }
        /*if (PlayerSlotsChanged)
        {
            ResetScores();
        }*/
    }

    private void PlayerWrap()
    {
        if (noOfPlayers > 4)
            {
            noOfPlayers = 1;
            }
        else if(noOfPlayers < 1)
            {
            noOfPlayers = 4;
            }
    }

    private void SizeWrap()
    {
        if (boardSize > 24)
        {
            boardSize = 2;
        }
        else if (boardSize < 2)
        {
            boardSize = 24;
        }
    }

    private void AiWrap()
    {
        if(noOfAI > noOfPlayers)
        {
            noOfAI = 0;
        }
        else if (noOfAI < 0)
        {
            noOfAI = noOfPlayers;
        }
    }
    public void OnEnterOfflinePlay()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SquareGameScene"))
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SquareGameScene"))
            {
                SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName("SquareGameScene"));
                foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
                {
                    if (go.name == "Board")
                    {
                        Board = go;
                    }
                }

                if (Board != null)
                {
                    Board.GetComponent<BoardManager>().PersistGameObject = this.gameObject;
                    MainCamera = Camera.main;
                    if (Wins[0] == 0 && Wins[1] == 0 && Wins[2] == 0 && Wins[3] == 0)
                    {
                        MainCamera.GetComponent<HudElements>().WinsDisplay.SetActive(false);
                    }
                    else
                    {
                        MainCamera.GetComponent<HudElements>().WinsDisplay.SetActive(true);
                        Board.GetComponent<BoardManager>().P1Wins = Wins[0];
                        Board.GetComponent<BoardManager>().P2Wins = Wins[1];
                        Board.GetComponent<BoardManager>().P3Wins = Wins[2];
                        Board.GetComponent<BoardManager>().P4Wins = Wins[3];
                    }
                    Board.GetComponent<BoardManager>().OnEnterGame();
                    Script = null;
                    EnterGame = false;
                    DontDestroyOnLoad(this.gameObject);
                }
            }
        }
    }

    public void OnReturnToMenu()
    {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName("Menu"));
            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {

                if (go.name == "ScriptHolder")
                {
                    Script = go;
                }
            }

            if (Main != null)
            { Main.SetActive(false);}

            if (Options != null)
            { Options.SetActive(true);}

            if (Script != null)
            {
                Script.GetComponent<MenuManager>().PersistGameObject = this.gameObject;
                Script.GetComponent<MenuManager>().LocalPlayButton();
                Script.GetComponent<MenuManager>().AudioObject = AudioObject;
                Script.GetComponent<MenuManager>().PasteExistingValues();
                PassedSlotsToBoard = false;
                Board = null;
                MainCamera = null;
                ReturnToSettings = false;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }

    public void GrabAudioObject()
    {
        AudioObject = GameObject.Find("AudioObject");
    }

    public void SlotCounter()
    {
        if (PlayerSlot1 != null)
        {
            //Slots.Insert(0, PlayerSlot1);
            //Slots.Add(PlayerSlot1);
            Slots[0] = PlayerSlot1;
            //else if (PlayerSlot1 == "easyai") { Slots[0] = PlayerSlot1; } //Easy AI
            //else if (PlayerSlot1 == "hardai") { Slots[0] = PlayerSlot1; } //Hard AI
        }
        if (PlayerSlot2 != "")
        {
            Slots[1] = PlayerSlot2;
            //if (PlayerSlot2 == "empty") { Slots[1] = PlayerSlot2; } //Here 0 is empty slot
            //else if (PlayerSlot2 == "human") { Slots[1] = PlayerSlot2; }//Human
            //else if (PlayerSlot2 == "easyai") { Slots[1] = PlayerSlot2; }//Easy AI
            //else if (PlayerSlot2 == "hardai") { Slots[1] = PlayerSlot2; }//Hard AI
        }
        if (PlayerSlot3 != null)
        {
            Slots[2] = PlayerSlot3;
            //if (PlayerSlot3 == "empty") { Slots[2] = PlayerSlot3; } //Here 0 is empty slot
            //else if (PlayerSlot3 == "human") { Slots[2] = PlayerSlot3; }//Human
            //else if (PlayerSlot3 == "easyai") { Slots[2] = PlayerSlot3; }//Easy AI
            //else if (PlayerSlot3 == "hardai") { Slots[2] = PlayerSlot3; }//Hard AI
        }
        if (PlayerSlot4 != null)
        {
            Slots[3] = PlayerSlot4;
            //if (PlayerSlot4 == "empty") { Slots[3] = PlayerSlot4; } //Here 0 is empty slot
            //else if (PlayerSlot4 == "human") { Slots[3] = PlayerSlot4; }//Human
            //else if (PlayerSlot4 == "easyai") { Slots[3] = PlayerSlot4; }//Easy AI
            //else if (PlayerSlot4 == "hardai") { Slots[3] = PlayerSlot4; }//Hard AI
        }
        //Slots.RemoveAt(4);
        noOfPlayers = 0;
        noOfAI = 0;

        foreach (string slot in Slots)
        {
            if (slot != null && slot != "")
            {
                if (slot == "empty") { } //nothing to do
                else if (slot == "human")
                { noOfPlayers += 1; }
                else if (slot == "easyai"|| slot == "medai"|| slot == "hardai")
                {
                    noOfPlayers += 1;
                    noOfAI += 1;
                }
            }
        }
    }
}
