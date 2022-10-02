using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//Handles In-game scoreboards and buttons
public class HudElements : MonoBehaviour
{
    public GameObject P1PointsDisplay;
    public GameObject P2PointsDisplay;
    public GameObject P3PointsDisplay;
    public GameObject P4PointsDisplay;
    public GameObject WinsDisplay;
    public GameObject P1WinsDisplay;
    public GameObject P2WinsDisplay;
    public GameObject P3WinsDisplay;
    public GameObject P4WinsDisplay;
    public string P1name;
    public string P2name;
    public string P3name;
    public string P4name;
    public List<string> PlayerNames;
    public GameObject TurnDisplay;
    public GameObject ButtonPlayAgain;
    public GameObject ButtonPlayAgainText;
    public GameObject ButtonGoToMenu;
    public GameObject ButtonGoToMenuText;
    public GameObject PauseButton;
    public GameObject PauseButtonText;
    public GameObject Board;
    private GameObject MouseTarget;
    public GameObject PauseOptionsNew;
    public GameObject EndGameOptionsNew;
    public GameObject NewPauseButtonObject;
    public GameObject AudioObject;
    public GameObject ScoreboardParent;
    public GameObject PlayerScoreBoxPrefab;
    public Toggle AudioToggle;
    public List<GameObject> PlayerScoreBoxes;
    public List<int> PlayerNumbers;

    public GameObject Client;
    public bool RematchRequestSender = false;
    public bool RematchRequestReceived = false;

    void Start()
    {
        //DisableUnusedPlayerDisplays();
        //DisableUnusedPlayerDisplaysNew();
        ButtonPlayAgain.SetActive(false);
        ButtonGoToMenu.SetActive(false);
        PauseButton.SetActive(false); //Previously set to true, but now obsolete
        NewPauseButtonObject.SetActive(true);
        if (Board.GetComponent<BoardManager>().PlayingOnline == true)
        { NewPauseButtonObject.GetComponentInChildren<Text>().text = "Options"; }
            if (Board.GetComponent<BoardManager>().PlayingOnline == true)
        {
            PauseButtonText.GetComponent<TextMeshPro>().text = "Options";
        }
            WinsDisplay.SetActive(false);
    }

    void Update()
    {
        if (GameObject.Find("InfoPanelCanvas(Clone)") == null || GameObject.Find("InfoPanelCanvas(Clone)").GetComponent<ErrorPopupHandler>().Active == false)
        {
            UpdatePoints();
            UpdateWins();
            DisplayPlayerTurn();
            //UseButtons(); Obsolete
            //UsePauseButton(); Obsolete
        }
    }

    private void DisableUnusedPlayerDisplays()
    {//System from when players was done on an additive basis rather than discrete slots
        if (Board.GetComponent<BoardManager>().noOfPlayers >= 1)
        {
            P1PointsDisplay.SetActive(true);
            P2PointsDisplay.SetActive(false);
            P3PointsDisplay.SetActive(false);
            P4PointsDisplay.SetActive(false);
            P1WinsDisplay.SetActive(true);
            P2WinsDisplay.SetActive(false);
            P3WinsDisplay.SetActive(false);
            P4WinsDisplay.SetActive(false);

            if (Board.GetComponent<BoardManager>().noOfPlayers >= 2)
            {
                P2PointsDisplay.SetActive(true);
                P2WinsDisplay.SetActive(true);

                if (Board.GetComponent<BoardManager>().noOfPlayers >= 3)
                {
                    P3PointsDisplay.SetActive(true);
                    P3WinsDisplay.SetActive(true);

                    if (Board.GetComponent<BoardManager>().noOfPlayers >= 4)
                    {
                        P4PointsDisplay.SetActive(true);
                        P4WinsDisplay.SetActive(true);
                    }
                }
            }
        }
    }

    public void DisableUnusedPlayerDisplaysNew()
    {
        //Run from PersistObject After Boardmanager is told slots
        if(Board.GetComponent<BoardManager>().isP1empty == true)
        {
            P1PointsDisplay.SetActive(false);
            P1WinsDisplay.SetActive(false);
        }
        else
        {
            P1PointsDisplay.SetActive(true);
            P1WinsDisplay.SetActive(true);
            PlayerNumbers.Add(1);
        }
        if (Board.GetComponent<BoardManager>().isP2empty == true)
        {
            P2PointsDisplay.SetActive(false);
            P2WinsDisplay.SetActive(false);
        }
        else
        {
            P2PointsDisplay.SetActive(true);
            P2WinsDisplay.SetActive(true);
            PlayerNumbers.Add(2);
        }
        if (Board.GetComponent<BoardManager>().isP3empty == true)
        {
            P3PointsDisplay.SetActive(false);
            P3WinsDisplay.SetActive(false);
        }
        else
        {
            P3PointsDisplay.SetActive(true);
            P3WinsDisplay.SetActive(true);
            PlayerNumbers.Add(3);
        }
        if (Board.GetComponent<BoardManager>().isP4empty == true)
        {
            P4PointsDisplay.SetActive(false);
            P4WinsDisplay.SetActive(false);
        }
        else
        {
            P4PointsDisplay.SetActive(true);
            P4WinsDisplay.SetActive(true);
            PlayerNumbers.Add(4);
        }

        for (int i = 0; i < (Board.GetComponent<BoardManager>().noOfPlayers); i++)
        {
            GameObject go = Instantiate(PlayerScoreBoxPrefab,ScoreboardParent.transform);
            go.transform.SetParent(ScoreboardParent.transform);
            go.transform.localPosition = new Vector3(-41, (-90 * i)+15, 0);
            go.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            go.GetComponent<PlayerScoreUiScript>().MyPlayerNumber = PlayerNumbers[i];
            go.GetComponent<PlayerScoreUiScript>().FillInInfo();
            PlayerScoreBoxes.Add(go);
        }
    }

    private void UpdatePoints()
    {
        if (Board.GetComponent<BoardManager>().PlayingOnline == false)
        {
            /* Old Scoreboard
            if (P1PointsDisplay.activeSelf)
            {
                P1PointsDisplay.GetComponent<TextMeshPro>().text = (P1name + ": " + Board.GetComponent<BoardManager>().P1Score.ToString());
            }
            if (P2PointsDisplay.activeSelf)
            {
                P2PointsDisplay.GetComponent<TextMeshPro>().text = (P2name + ": " + Board.GetComponent<BoardManager>().P2Score.ToString());
            }
            if (P3PointsDisplay.activeSelf)
            {
                P3PointsDisplay.GetComponent<TextMeshPro>().text = (P3name + ": " + Board.GetComponent<BoardManager>().P3Score.ToString());
            }
            if (P4PointsDisplay.activeSelf)
            {
                P4PointsDisplay.GetComponent<TextMeshPro>().text = (P4name + ": " + Board.GetComponent<BoardManager>().P4Score.ToString());
            }*/
            foreach (GameObject PlayerDisplay in PlayerScoreBoxes)
            {
                int PlayerNumber = PlayerDisplay.GetComponent<PlayerScoreUiScript>().MyPlayerNumber;
                if (PlayerNumber == 1) { PlayerDisplay.GetComponent<PlayerScoreUiScript>().Scoretext = Board.GetComponent<BoardManager>().P1Score.ToString(); }
                else if (PlayerNumber == 2) { PlayerDisplay.GetComponent<PlayerScoreUiScript>().Scoretext = Board.GetComponent<BoardManager>().P2Score.ToString(); }
                else if(PlayerNumber == 3) { PlayerDisplay.GetComponent<PlayerScoreUiScript>().Scoretext = Board.GetComponent<BoardManager>().P3Score.ToString(); }
                else if(PlayerNumber == 4) { PlayerDisplay.GetComponent<PlayerScoreUiScript>().Scoretext = Board.GetComponent<BoardManager>().P4Score.ToString(); }
            }            
        }
        else if (Board.GetComponent<BoardManager>().PlayingOnline == true && Client != null && Client.GetComponent<Client>().players[0].name != null)
        {
            if (P1PointsDisplay.activeSelf)
            {
                P1PointsDisplay.GetComponent<TextMeshPro>().text = (Client.GetComponent<Client>().players[0].name.ToString() +": " + Board.GetComponent<BoardManager>().P1Score.ToString());
            }
            if (P2PointsDisplay.activeSelf)
            {
                P2PointsDisplay.GetComponent<TextMeshPro>().text = (Client.GetComponent<Client>().players[1].name.ToString() + ": " + Board.GetComponent<BoardManager>().P2Score.ToString());
            }
            if (P3PointsDisplay.activeSelf)
            {
                P3PointsDisplay.GetComponent<TextMeshPro>().text = (Client.GetComponent<Client>().players[2].name.ToString() + ": " + Board.GetComponent<BoardManager>().P3Score.ToString());
            }
            if (P4PointsDisplay.activeSelf)
            {
                P4PointsDisplay.GetComponent<TextMeshPro>().text = (Client.GetComponent<Client>().players[3].name.ToString() + ": " + Board.GetComponent<BoardManager>().P4Score.ToString());
            }
        }

    }

    private void UpdateWins()
    {
        if (WinsDisplay.activeSelf)
        {
            if (Board.GetComponent<BoardManager>().PlayingOnline == false)
            {
                if (P1WinsDisplay.activeSelf)
                {
                    P1WinsDisplay.GetComponent<TextMeshPro>().text = (P1name + ": " + Board.GetComponent<BoardManager>().P1Wins.ToString());
                }
                if (P2WinsDisplay.activeSelf)
                {
                    P2WinsDisplay.GetComponent<TextMeshPro>().text = (P2name + ": " + Board.GetComponent<BoardManager>().P2Wins.ToString());
                }
                if (P3PointsDisplay.activeSelf)
                {
                    P3WinsDisplay.GetComponent<TextMeshPro>().text = (P3name + ": " + Board.GetComponent<BoardManager>().P3Wins.ToString());
                }
                if (P4WinsDisplay.activeSelf)
                {
                    P4WinsDisplay.GetComponent<TextMeshPro>().text = (P4name + ": " + Board.GetComponent<BoardManager>().P4Wins.ToString());
                }
            }
            else if (Board.GetComponent<BoardManager>().PlayingOnline == true && Client != null && Client.GetComponent<Client>().players[0].name != null)
            {
                if (P1WinsDisplay.activeSelf)
                {
                    P1WinsDisplay.GetComponent<TextMeshPro>().text = (Client.GetComponent<Client>().players[0].name.ToString() + ": " + Board.GetComponent<BoardManager>().P1Wins.ToString());
                }
                if (P2WinsDisplay.activeSelf)
                {
                    P2WinsDisplay.GetComponent<TextMeshPro>().text = (Client.GetComponent<Client>().players[1].name.ToString() + ": " + Board.GetComponent<BoardManager>().P2Wins.ToString());
                }
                if (P3PointsDisplay.activeSelf)
                {
                    P3WinsDisplay.GetComponent<TextMeshPro>().text = (Client.GetComponent<Client>().players[2].name.ToString() + ": " + Board.GetComponent<BoardManager>().P3Wins.ToString());
                }
                if (P4WinsDisplay.activeSelf)
                {
                    P4WinsDisplay.GetComponent<TextMeshPro>().text = (Client.GetComponent<Client>().players[3].name.ToString() + ": " + Board.GetComponent<BoardManager>().P4Wins.ToString());
                }
            }
        }
    }

    private void DisplayPlayerTurn()
    {
        if (!Board.GetComponent<BoardManager>().Endgame)
        {
            if (Board.GetComponent<BoardManager>().PlayingOnline == true && !Board.GetComponent<BoardManager>().IsEmptyTurn && Board.GetComponent<BoardManager>().currentPlayer <= 2)
            {
                TurnDisplay.GetComponent<TextMeshPro>().text = (Client.GetComponent<Client>().players[Board.GetComponent<BoardManager>().currentPlayer - 1].name.ToString() + "'s turn");
            }
            else if(!Board.GetComponent<BoardManager>().IsEmptyTurn)
            {
                TurnDisplay.GetComponent<TextMeshPro>().text = (PlayerNames[Board.GetComponent<BoardManager>().currentPlayer -1] + "'s turn");
            }
            /*else //What to do instead of showing default player names for empty turns
            {
                TurnDisplay.GetComponent<TextMeshPro>().text = TurnDisplay.GetComponent<TextMeshPro>().text;
            }*/
        }
        else if (!RematchRequestSender && !RematchRequestReceived)
        {
            //EndGameButtonDisplay();
            EndGameOptionsNew.SetActive(true);
            NewPauseButtonObject.SetActive(false);
            WinsDisplay.SetActive(true);

            if (Board.GetComponent<BoardManager>().Winner != 0)
            {
                if (Board.GetComponent<BoardManager>().PlayingOnline == true)
                {
                    TurnDisplay.GetComponent<TextMeshPro>().text = ("Game Over, " + Client.GetComponent<Client>().players[Board.GetComponent<BoardManager>().Winner - 1].name.ToString() + " wins!");
                }
                else
                {
                    //TurnDisplay.GetComponent<TextMeshPro>().text = ("Game Over, Player " + Board.GetComponent<BoardManager>().Winner.ToString() + " wins!");
                    TurnDisplay.GetComponent<TextMeshPro>().text = ("Game Over, " + PlayerNames[Board.GetComponent<BoardManager>().Winner -1] + " wins!");
                }
            }
            else if (Board.GetComponent<BoardManager>().Winner == 0)
            {
                TurnDisplay.GetComponent<TextMeshPro>().text = ("Game Over, it's a tie!");
            }
        }
        else if (RematchRequestSender)
        {
            //wait for other player to accept rematch request...
        }
    }

    private void UseButtons()
    {
        if (!Camera.main) //|| !ButtonPlayAgain.activeSelf || !ButtonGoToMenu.activeSelf)
            return;

        if (MouseTarget != null)
        {
            MouseTarget.GetComponent<MeshRenderer>().material = Board.GetComponent<BoardManager>().defaultMat;
            MouseTarget = null;
            
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 90.0f, LayerMask.GetMask("Button")))
        {
            MouseTarget = hit.transform.gameObject;

            MouseTarget.GetComponent<MeshRenderer>().material = Board.GetComponent<BoardManager>().highlightMat;
            
        }
    }

    private void UsePauseButton()
    {
        if (Board.GetComponent<BoardManager>().GamePaused == false)
        {
            if (Input.GetMouseButtonDown(0) && MouseTarget == PauseButton)
            {
                Board.GetComponent<BoardManager>().GamePaused = true;
                if (Board.GetComponent<BoardManager>().PlayingOnline == false)
                {
                    ButtonPlayAgain.SetActive(true);
                }
                ButtonGoToMenu.SetActive(true);
                ButtonPlayAgainText.GetComponent<TextMeshPro>().text = ("Restart");
                PauseButtonText.GetComponent<TextMeshPro>().text = ("Continue");
            }
        }
        else if (Board.GetComponent<BoardManager>().GamePaused == true)
        {

            if (Input.GetMouseButtonDown(0))
            {
                if (MouseTarget == ButtonPlayAgain)
                {
                    MouseTarget.GetComponent<MeshRenderer>().material = Board.GetComponent<BoardManager>().defaultMat;
                    MouseTarget = null;
                    Board.GetComponent<BoardManager>().ResetCheck = true;
                    if (Board.GetComponent<BoardManager>().PlayingOnline == true)
                    {
                        PauseButtonText.GetComponent<TextMeshPro>().text = "Options";
                    }
                    else
                    {
                        PauseButtonText.GetComponent<TextMeshPro>().text = "Pause";
                    }
                    ButtonPlayAgainText.GetComponent<TextMeshPro>().text = "Play again";
                    ButtonPlayAgain.SetActive(false);
                    ButtonGoToMenu.SetActive(false);
                    Board.GetComponent<BoardManager>().GamePaused = false;
                }
                else if (MouseTarget == ButtonGoToMenu)
                {
                    MouseTarget.GetComponent<MeshRenderer>().material = Board.GetComponent<BoardManager>().defaultMat;
                    MouseTarget = null;
                    //Destroy(GameObject.Find("PersistGameObject").GetComponent<GameObject>());
                    GameManager.Instance.BackButton();
                    PersistObject p = GameObject.Find("PersistGameObject").GetComponent<PersistObject>();
                    GameManager g = GameObject.Find("OnlineGameManager").GetComponent<GameManager>();
                    Destroy(p.gameObject);
                    Destroy(g.gameObject);
                    //Application.LoadLevel(0);
                    SceneManager.LoadScene(0);
                }
                else if (MouseTarget == PauseButton)
                {
                    MouseTarget.GetComponent<MeshRenderer>().material = Board.GetComponent<BoardManager>().defaultMat;
                    MouseTarget = null;
                    ButtonPlayAgainText.GetComponent<TextMeshPro>().text = "Play again";
                    if (Board.GetComponent<BoardManager>().PlayingOnline == true)
                    {
                        PauseButtonText.GetComponent<TextMeshPro>().text = "Options";
                    }
                    else
                    {
                        PauseButtonText.GetComponent<TextMeshPro>().text = "Pause";
                    }
                    ButtonPlayAgain.SetActive(false);
                    ButtonGoToMenu.SetActive(false);
                    Board.GetComponent<BoardManager>().GamePaused = false;
                }
            }
        }
    }

    private void EndGameButtonDisplay()
    {
        ButtonPlayAgain.SetActive(true);
        ButtonGoToMenu.SetActive(true);
        PauseButton.SetActive(false);
        WinsDisplay.SetActive(true);

        if (Input.GetMouseButtonDown(0))
        {
            if (MouseTarget == ButtonPlayAgain)
            {
                if (Board.GetComponent<BoardManager>().PlayingOnline == false)
                {
                    MouseTarget.GetComponent<MeshRenderer>().material = Board.GetComponent<BoardManager>().defaultMat;
                    MouseTarget = null;
                    Board.GetComponent<BoardManager>().ResetCheck = true;
                    ButtonPlayAgain.SetActive(false);
                    ButtonGoToMenu.SetActive(false);
                    PauseButton.SetActive(true);
                }
                else if (Board.GetComponent<BoardManager>().PlayingOnline == true)
                {
                    MouseTarget.GetComponent<MeshRenderer>().material = Board.GetComponent<BoardManager>().defaultMat;
                    MouseTarget = null;
                    ButtonPlayAgain.SetActive(false);
                    ButtonGoToMenu.SetActive(false);
                    Canvas go = Instantiate(GameManager.Instance.InfoPanelPopupPrefab);
                    go.GetComponent<ErrorPopupHandler>().ErrorCode = 4;
                    Client.GetComponent<Client>().Send("CPWR|");
                    RematchRequestSender = true;
                }
            }
            else if (MouseTarget == ButtonGoToMenu)
            {
                MouseTarget.GetComponent<MeshRenderer>().material = Board.GetComponent<BoardManager>().defaultMat;
                MouseTarget = null;

                GameManager.Instance.BackButton();
                PersistObject p = GameObject.Find("PersistGameObject").GetComponent<PersistObject>();
                GameManager g = GameObject.Find("OnlineGameManager").GetComponent<GameManager>();
                Destroy(p.gameObject);
                Destroy(g.gameObject);

                //Application.LoadLevel(0);
                SceneManager.LoadScene(0);
            }
        }
    }

    //New UI Button Functionality

    public void NewPauseButton()
    {
        if (Board.GetComponent<BoardManager>().PlayingOnline == false)
        {
            TurnDisplay.GetComponent<TextMeshPro>().text = "Game Paused";
            Board.GetComponent<BoardManager>().GamePaused = true;
            PauseOptionsNew.SetActive(true);
            NewPauseButtonObject.SetActive(false);
        }
    }

    public void NewRestartButton()
    {
        PauseOptionsNew.SetActive(false);
        Board.GetComponent<BoardManager>().ResetCheck = true;
        Board.GetComponent<BoardManager>().GamePaused = false;
        NewPauseButtonObject.SetActive(true);
    }

    public void NewPlayAgainButton()
    {
        PauseOptionsNew.SetActive(false);
        Board.GetComponent<BoardManager>().ResetCheck = true;
        Board.GetComponent<BoardManager>().GamePaused = false;
        EndGameOptionsNew.SetActive(false);
        NewPauseButtonObject.SetActive(true);
    }

    public void NewBackToOptionsButton()
    {
        GameObject PersistObject;
        //Coming soon! -need to make persist object more persistant :)
        //GameObject.Find("PersistGameObject").GetComponent<PersistObject>().ReturnToSettings = true;
        PersistObject = Board.GetComponent<BoardManager>().PersistGameObject;
        PersistObject.GetComponent<PersistObject>().ReturnToSettings = true;
        PersistObject.GetComponent<PersistObject>().Wins[0] = Board.GetComponent<BoardManager>().P1Wins;
        PersistObject.GetComponent<PersistObject>().Wins[1] = Board.GetComponent<BoardManager>().P2Wins;
        PersistObject.GetComponent<PersistObject>().Wins[2] = Board.GetComponent<BoardManager>().P3Wins;
        PersistObject.GetComponent<PersistObject>().Wins[3] = Board.GetComponent<BoardManager>().P4Wins;
        GameManager.Instance.BackButton();
        SceneManager.LoadScene(0);

    }

    public void NewBacktoMainMenuButton()
    {
        GameManager.Instance.BackButton();
        PersistObject p = GameObject.Find("PersistGameObject").GetComponent<PersistObject>();
        GameManager g = GameObject.Find("OnlineGameManager").GetComponent<GameManager>();
        Destroy(p.gameObject);
        Destroy(g.gameObject);
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        if (AudioObject == null)
        {
            AudioObject = GameObject.Find("PersistGameObject").GetComponent<PersistObject>().AudioObject;
        }

        if (AudioObject.GetComponent<AudioSource>().mute == true)
        {
            AudioToggle.isOn = false;
        }
        else if (AudioObject.GetComponent<AudioSource>().mute == false)
        {
            AudioToggle.isOn = true;
        }
    }

    public void NewAudioToggleButton(GameObject ThisToggle)
    {
        if (ThisToggle.GetComponent<Toggle>().isOn) { AudioObject.GetComponent<AudioSource>().mute = false; }
        else { AudioObject.GetComponent<AudioSource>().mute = true; }
    }

    public void NewResumeButton()
    {
        Board.GetComponent<BoardManager>().GamePaused = false;
        PauseOptionsNew.SetActive(false);
        NewPauseButtonObject.SetActive(true);
        TurnDisplay.GetComponent<TextMeshPro>().text = (PlayerNames[Board.GetComponent<BoardManager>().currentPlayer - 1] + "'s turn");
    }
}
