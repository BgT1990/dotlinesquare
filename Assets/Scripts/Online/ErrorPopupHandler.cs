using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Handles informing user of exceptions and disconnects
public class ErrorPopupHandler : MonoBehaviour
{
    public Canvas Mycanvas;
    public GameObject TextObject;
    public Text TextComponent;
    public Button Button1;
    public Button Button2;
    public Text Button1Text;
    public Text Button2Text;
    public int ErrorCode = 0;
    public bool Active = false;

    void Start()
    {
        Mycanvas = this.gameObject.GetComponent<Canvas>();
        TextObject = gameObject.transform.Find("ErrorTitle").gameObject;
        TextComponent = TextObject.GetComponent<Text>();
        Button1 = gameObject.transform.Find("ErrorButton1").GetComponent<Button>();
        Button2 = gameObject.transform.Find("ErrorButton2").GetComponent<Button>();
        TextComponent.text = "Error Message";
        Button1Text = Button1.transform.Find("ErrorButton1Text").GetComponent<Text>();
        Button2Text = Button2.transform.Find("ErrorButton2Text").gameObject.GetComponent<Text>();
        Button1Text.text =  "Option 1";
        Button2Text.text =  "Option 2";
    }

    void Update()
    {
        if (!Active)
        {
            if (ErrorCode == 0)
            {
                //nothing is here
            }
            else if (ErrorCode == 1)
            {
                CouldNotConnect();
            }
            else if (ErrorCode == 2)
            {
                GameObject PreviousPopUp = GameObject.Find("InfoPanelCanvas(Clone)");
                if (PreviousPopUp != this.gameObject)
                {
                    Destroy(PreviousPopUp);
                    OtherPlayerDisconnected();

                }
                else
                {
                    OtherPlayerDisconnected();
                }
            }
            else if (ErrorCode == 3)
            {
                OtherPlayerWantstoPlayAgain();
            }
            else if (ErrorCode == 4)
            {
                WaitingForOtherPlayer();
            }
        }
        
    }

    private void CouldNotConnect()
    {
        //Error code 1
        Active = true;
        TextComponent.text = "Could not connect";
        Button1Text.text = "Try Again";
        Button2Text.text = "Back to Menu";

        Button1.onClick.AddListener(PopUpFinished);
        Button2.onClick.AddListener(BacktoMenu);

        MenuManager.Instance.ClientOptions.SetActive(false);
        MenuManager.Instance.NameInputCanvas.gameObject.SetActive(false);
    }

    private void OtherPlayerDisconnected()
    {
        //Error code 2
        Active = true;
        TextComponent.text = "Lost connection to other player";
        Button2Text.text = "Back to menu";

        Button1.gameObject.SetActive(false);
        Button2.onClick.AddListener(BacktoMenu);
        
        GameObject.Find("Board").GetComponent<BoardManager>().GamePaused = true;

        Camera.main.GetComponent<HudElements>().PauseButton.SetActive(false);
        Camera.main.GetComponent<HudElements>().ButtonPlayAgain.SetActive(false);
        Camera.main.GetComponent<HudElements>().ButtonGoToMenu.SetActive(false);

        GameManager.Instance.BackButton();

        Destroy(GameObject.Find("PersistGameObject"));
        Destroy(GameObject.Find("OnlineGameManager"));
    }

    private void OtherPlayerWantstoPlayAgain()
    {
        //Error code 3
        Active = true;
        TextComponent.text = "Other player wants a rematch";
        Button1Text.text = "Play again";
        Button2Text.text = "Back to Menu";

        Button1.onClick.AddListener(PlayAgain);
        Button2.onClick.AddListener(BacktoMenu);

        Camera.main.GetComponent<HudElements>().PauseButton.SetActive(false);
        Camera.main.GetComponent<HudElements>().ButtonPlayAgain.SetActive(false);
        Camera.main.GetComponent<HudElements>().ButtonGoToMenu.SetActive(false);

    }

    private void PlayAgain()
    {
        FindObjectOfType<Client>().GetComponent<Client>().Send("CPAR|");
        Camera.main.GetComponent<HudElements>().PauseButton.SetActive(true);
        GameObject.Find("Board").GetComponent<BoardManager>().ResetCheck = true;
        Camera.main.GetComponent<HudElements>().RematchRequestReceived = false;
        //GameObject.Find("Board").GetComponent<BoardManager>().currentPlayer += 1; //Doesnt currently work
        PopUpFinished();
    }

    private void WaitingForOtherPlayer()
    {
        //Error code 4
        Active = true;
        TextComponent.text = "Waiting for other player...";
        
        Button2Text.text = "Back to Menu";

        Button1.gameObject.SetActive(false);
        Button2.onClick.AddListener(BacktoMenu);
    }

    private void AreYouSure()
    {
        //nothing here yet
        //could be called upon quitting from online games
    }

    private void BacktoMenu()
    {
        GameManager.Instance.BackButton();
        if (ErrorCode == 1)
        {
            MenuManager.Instance.ClientOptions.SetActive(false);
            MenuManager.Instance.ClientOptionsNew.SetActive(false);
            MenuManager.Instance.NameInputCanvas.gameObject.SetActive(false);
            //MenuManager.Instance.HostOrConnect.SetActive(true);
            MenuManager.Instance.OnlineHostOrJoinNew.SetActive(true);
        }

        else if (ErrorCode == 2)
        {
            SceneManager.LoadScene(0);
        }

        else if (ErrorCode == 3)
        {
            SceneManager.LoadScene(0);
        }
        
        else if (ErrorCode == 4)
        {
            SceneManager.LoadScene(0);
        }

        ErrorCode = 0;
        PopUpFinished();
        
    }

    public void PopUpFinished()
    {
        if (ErrorCode == 1)
        {
            MenuManager.Instance.ClientOptionsNew.SetActive(true);
            MenuManager.Instance.ClientOptions.SetActive(true);
            MenuManager.Instance.NameInputCanvas.gameObject.SetActive(true);
        }

        //Mycanvas.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
