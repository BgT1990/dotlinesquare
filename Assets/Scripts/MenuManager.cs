using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//Handles the initial menus and passing the values to the game
public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }

    public GameObject MainButtons;
    public GameObject MainButtonsNew;
    public GameObject GameOptions;
    public GameObject GameOptionsNew;
    public GameObject PlayButton;
    public GameObject ExitButton;
    public GameObject GoButton;
    public GameObject IncreasePlayers;
    public GameObject DecreasePlayers;
    public GameObject IncreaseAi;
    public GameObject DecreaseAi;
    public GameObject IncreaseSize;
    public GameObject DecreaseSize;
    public GameObject BackButton;
    public GameObject Credits;
    public GameObject PlayerDisplay;
    public GameObject SizeDisplay;
    public Text SizeDisplayNew;
    public GameObject AiDisplay;
    private GameObject MouseTarget;
    public GameObject PersistGameObject;
    public Material highlightMat;
    public Material defaultMat;
    public GameObject SettingsMenu;
    public GameObject HowToPlay;
    public GameObject ExitAreYouSure;
    public GameObject AudioObject;
    public List<AudioClip> AudioEvents;

    public GameObject OnlineButton;
    public GameObject OnlineOptions;
    public GameObject HostOrConnect;
    public GameObject OnlineHostOrJoinNew;
    public GameObject HostOptionsNew;
    public GameObject ClientOptionsNew;
    public GameObject TemporaryHostWaiting;
    public GameObject HostButton;
    public GameObject ClientButton;
    public GameObject HostOptions;
    public GameObject BoardSizeSettingsOnline;
    public GameObject HostGoButton;
    public GameObject IncreaseSizeOnline;
    public GameObject DecreaseSizeOnline;
    public GameObject BoardSizeSlider;
    public GameObject BoardSizeSliderOnline;
    public GameObject SizeDisplayOnline;
    public GameObject SizeDisplayOnlineNew;
    public TextMeshPro WaitingForPlayer;
    public GameObject ClientOptions;
    public GameObject ClientGoButton;
    public Canvas NameInputCanvas;
    public InputField OnlineName;
    public bool connecting;

    public InputField P1nameinputfield;
    public InputField P2nameinputfield;
    public InputField P3nameinputfield;
    public InputField P4nameinputfield;
    public string P1name = "Player 1";
    public string P2name = "Player 2";
    public string P3name = "Player 3";
    public string P4name = "Player 4";
    public GameObject Slot1nametag;
    public GameObject Slot2nametag;
    public GameObject Slot3nametag;
    public GameObject Slot4nametag;
    public List<string> AiNames;
    public GameObject Slot1colourbutton;
    public GameObject Slot2colourbutton;
    public GameObject Slot3colourbutton;
    public GameObject Slot4colourbutton;
    public GameObject Slot1colourdisplay;
    public GameObject Slot2colourdisplay;
    public GameObject Slot3colourdisplay;
    public GameObject Slot4colourdisplay;
    public GameObject ColourSelectPopup;
    public int PlayerSelectingColour = -1;


    void Start()
    {
        Instance = this;
        connecting = false;
        MainButtons.SetActive(false);
        MainButtonsNew.SetActive(true);
        GameOptions.SetActive(false);
        GameOptionsNew.SetActive(false);
        ExitAreYouSure.SetActive(false);
        OnlineHostOrJoinNew.SetActive(false);
        HostOptionsNew.SetActive(false);
        ClientOptionsNew.SetActive(false);
        //OnlineHostOrJoinNew.SetActive(false);
        //HostOptionsNew.SetActive(false);
        //ClientOptionsNew.SetActive(false);
        HowToPlay.SetActive(false);
        SettingsMenu.SetActive(false);
        NameInputCanvas.gameObject.SetActive(false);
        WaitingForPlayer.text = "";
        BoardSizeSettingsOnline.SetActive(true);
        SetCamera();
        PersistGameObject = GameObject.Find("PersistGameObject");
        P1nameinputfield.gameObject.SetActive(true);
        Slot1colourbutton.SetActive(true);
        Slot1nametag.SetActive(false);
        P2nameinputfield.gameObject.SetActive(false);
        Slot2colourbutton.SetActive(false);
        Slot2nametag.SetActive(false);
        P3nameinputfield.gameObject.SetActive(false);
        Slot3colourbutton.SetActive(false);
        Slot3nametag.SetActive(false);
        P4nameinputfield.gameObject.SetActive(false);
        Slot4colourbutton.SetActive(false);
        Slot4nametag.SetActive(false);
        ColourSelectPopup.SetActive(false);
        UpdateColourTags();
    }

    void Update()
    {
        MouseSelect();

        if (GameOptions.activeSelf || HostOptions.activeSelf || GameOptionsNew.activeSelf || HostOptionsNew.activeSelf)
        {
            UpdateDisplays();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (MouseTarget != null)
            {
                ClickSound();
                if (MainButtonsNew.activeSelf)
                {
                    MenuButtonsNew();
                }
                else if (MainButtons.activeSelf)
                {
                    MenuButtons();
                }
                else if (GameOptions.activeSelf)
                {
                    OptionsButtons();
                }
                else if (GameOptionsNew.activeSelf)
                {
                    OptionsButtonsNew();
                }
                else if (OnlineOptions.activeSelf)
                {
                    if (HostOptions.activeSelf)
                    {

                        HostButtons();
                    }
                    else if (ClientOptions.activeSelf)
                    {
                        ClientButtons();
                    }
                    else
                    {
                        OnlineButtons();
                    }
                }
            }
            //check there is a target and if the target is available
        }
        //For initial client connecting check + error message popup
        if (!connecting && (ClientOptions.activeSelf || ClientOptionsNew.activeSelf))
        {
            NameInputCanvas.gameObject.SetActive(true);
            ClientOptions.SetActive(true);
            GameManager.Instance.BackButton();

        }
        else if (connecting && (ClientOptions.activeSelf || ClientOptionsNew.activeSelf))
        {
            ClientOptions.SetActive(false);
        }
        if (AudioObject == null) { AudioObject = PersistGameObject.GetComponent<PersistObject>().AudioObject; }
    }

    private void SetCamera()
    {
        Vector3 CameraPos = new Vector3(0, 1, -10);
        GameObject.FindObjectOfType<Camera>().GetComponent<Transform>().position = CameraPos;
    }

    private void MouseSelect()
    {
        if (!Camera.main)
            return;

        if (MouseTarget != null)
        {
            MouseTarget.GetComponent<MeshRenderer>().material = defaultMat;
            MouseTarget = null;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 80.0f, LayerMask.GetMask("Button")))
        {
            MouseTarget = hit.transform.gameObject;
            MouseTarget.GetComponent<MeshRenderer>().material = highlightMat;

        }
        //else
        //{
        //    if (MouseTarget != null)
        //    {
        //        MouseTarget.GetComponent<MeshRenderer>().material = defaultMat;
        //        MouseTarget = null;
        //    }
        //    else
        //        MouseTarget = null;
        //}

    }

    private void MenuButtons()
    {
        if (MouseTarget == PlayButton)
        {
            MainButtons.SetActive(false);
            //GameOptions.SetActive(true); //Old Options
            GameOptionsNew.SetActive(true);
        }
        if (MouseTarget == OnlineButton)
        {
            MainButtons.SetActive(false);
            OnlineOptions.SetActive(true);
            HostOrConnect.SetActive(true);
        }
        else if (MouseTarget == ExitButton)
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }

    public void ClickSound()
    {
        if (AudioObject != null)
        {
            AudioObject.GetComponent<AudioSource>().PlayOneShot(AudioEvents[5]);
        }
    }

    private void MenuButtonsNew()
    {
        //nothing really needed here... switched it to public methods run on button clicks.
    }

    public void LocalPlayButton()
    {
        ClickSound();
        MainButtonsNew.SetActive(false);
        GameOptionsNew.SetActive(true);
    }
    public void OnlinePlayButton()
    {
        ClickSound();
        MainButtonsNew.SetActive(false);
        //OnlineOptions.SetActive(true);
        //HostOrConnect.SetActive(true);
        OnlineHostOrJoinNew.SetActive(true);
    }
    public void HowtoPlayButton()
    {
        ClickSound();
        MainButtonsNew.SetActive(false);
        HowToPlay.SetActive(true);
    }

    public void CreditsButton()
    {
        ClickSound();
        MainButtonsNew.SetActive(false);
        Credits.SetActive(true);
    }

    public void MainOptionsButton()
    {
        ClickSound();
        MainButtonsNew.SetActive(false);
        SettingsMenu.SetActive(true);
    }
    public void HostButtonNew()
    {
        ClickSound();
        OnlineHostOrJoinNew.SetActive(false);
        HostOptionsNew.SetActive(true);
    }
    public void ClientButtonNew()
    {
        ClickSound();
        OnlineHostOrJoinNew.SetActive(false);
        ClientOptionsNew.SetActive(true);
    }
    public void TempLinkToOldOnline()
    {
        OnlineOptions.SetActive(true);
        HostOrConnect.SetActive(true);
        OnlineHostOrJoinNew.SetActive(false);
    }

    public void MainExitButton()
    {
        ClickSound();
        Debug.Log("Quitting");
        Application.Quit();
    }
    public void FirstExitButton()
    {
        ClickSound();
        MainButtonsNew.SetActive(false);
        ExitAreYouSure.SetActive(true);
    }
    public void CancelExitButton()
    {
        ClickSound();
        MainButtonsNew.SetActive(true);
        ExitAreYouSure.SetActive(false);
    }

    private void OptionsButtons()
    {
        if (MouseTarget == GoButton)
        {
            //Application.LoadLevel(1);
            SceneManager.LoadScene(1);
        }
        else if (MouseTarget == IncreasePlayers)
        {
            PersistGameObject.GetComponent<PersistObject>().noOfPlayers += 1;            
        }
        else if (MouseTarget == DecreasePlayers)
        {
            PersistGameObject.GetComponent<PersistObject>().noOfPlayers -= 1;            
        }
        else if (MouseTarget == IncreaseSize)
        {
            PersistGameObject.GetComponent<PersistObject>().boardSize += 1;      
        }
        else if (MouseTarget == DecreaseSize)
        {
            PersistGameObject.GetComponent<PersistObject>().boardSize -= 1;
        }
        else if (MouseTarget == IncreaseAi)
        {
            PersistGameObject.GetComponent<PersistObject>().noOfAI += 1;
        }
        else if (MouseTarget == DecreaseAi)
        {
            PersistGameObject.GetComponent<PersistObject>().noOfAI -= 1;
        }
        else if (MouseTarget.tag == "BackButton")
        {
            MainButtons.SetActive(true);
            GameOptions.SetActive(false);
        }

    }

    private void OptionsButtonsNew()
    {
        if (MouseTarget == IncreasePlayers)
        {
            PersistGameObject.GetComponent<PersistObject>().noOfPlayers += 1;
        }
        else if (MouseTarget == DecreasePlayers)
        {
            PersistGameObject.GetComponent<PersistObject>().noOfPlayers -= 1;
        }
        else if (MouseTarget == IncreaseAi)
        {
            PersistGameObject.GetComponent<PersistObject>().noOfAI += 1;
        }
        else if (MouseTarget == DecreaseAi)
        {
            PersistGameObject.GetComponent<PersistObject>().noOfAI -= 1;
        }

        //PersistGameObject.GetComponent<PersistObject>().boardSize = (int) BoardSizeSlider.GetComponent<Slider>().value;
    }

    public void GoButtonNew()
    {
        ClickSound();
        if (P1name == null || P1name == "")
        {
            PersistGameObject.GetComponent<PersistObject>().P1name = "Player 1";
        }
        else
        {
            PersistGameObject.GetComponent<PersistObject>().P1name = P1name;
        }
        if (P2name == null || P2name == "")
        {
            PersistGameObject.GetComponent<PersistObject>().P2name = "Player 2";
        }
        else
        {
            PersistGameObject.GetComponent<PersistObject>().P2name = P2name;
        }
        if (P3name == null || P3name == "")
        {
            PersistGameObject.GetComponent<PersistObject>().P3name = "Player 3";
        }
        else
        {
            PersistGameObject.GetComponent<PersistObject>().P3name = P3name;
        }
        if (P4name == null || P4name == "")
        {
            PersistGameObject.GetComponent<PersistObject>().P4name = "Player 4";
        }
        else
        {
            PersistGameObject.GetComponent<PersistObject>().P4name = P4name;
        }
        PersistGameObject.GetComponent<PersistObject>().EnterGame = true;

        SceneManager.LoadScene(1);
    }

    public void PasteExistingValues()
    {
        UpdateColourTags();
        BoardSizeSlider.GetComponent<Slider>().value = PersistGameObject.GetComponent<PersistObject>().boardSize;
        GameObject DropDown1 = GameObject.Find("Slot 1 Dropdown");
        P1name = PersistGameObject.GetComponent<PersistObject>().P1name;
        P2name = PersistGameObject.GetComponent<PersistObject>().P2name;
        P3name = PersistGameObject.GetComponent<PersistObject>().P3name;
        P4name = PersistGameObject.GetComponent<PersistObject>().P4name;

        {
            {//Slot 1
                if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 == "human")
                {
                    DropDown1.GetComponent<Dropdown>().value = 0;
                    P1nameinputfield.gameObject.SetActive(true);
                    Slot1nametag.SetActive(false);
                    Slot1colourbutton.SetActive(true);
                    P1nameinputfield.text = PersistGameObject.GetComponent<PersistObject>().P1name;
                    
                }
                else
                {
                    if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 == "easyai")
                    {
                        DropDown1.GetComponent<Dropdown>().value = 1;
                    }
                    else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 == "medai")
                    {
                        DropDown1.GetComponent<Dropdown>().value = 2;
                    }
                    else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 == "hardai")
                    {
                        DropDown1.GetComponent<Dropdown>().value = 3;
                    }
                    P1nameinputfield.gameObject.SetActive(false);
                    Slot1nametag.SetActive(true);
                    Slot1colourbutton.SetActive(true);
                    Slot1nametag.GetComponent<Text>().text = PersistGameObject.GetComponent<PersistObject>().P1name;
                }
            }
            {//Slot 2
                GameObject DropDown2 = GameObject.Find("Slot 2 Dropdown");
                if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 == "empty")
                {
                    DropDown2.GetComponent<Dropdown>().value = 0;
                    P2nameinputfield.gameObject.SetActive(false);
                    Slot2nametag.SetActive(false);
                }
                else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 == "human")
                {
                    DropDown2.GetComponent<Dropdown>().value = 1;
                    P2nameinputfield.gameObject.SetActive(true);
                    Slot2nametag.SetActive(false);
                    Slot2colourbutton.SetActive(true);
                    P2nameinputfield.text = PersistGameObject.GetComponent<PersistObject>().P2name;
                }
                else { 

                    if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 == "easyai")
                    {
                        DropDown2.GetComponent<Dropdown>().value = 2;
                    }
                    else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 == "medai")
                    {
                        DropDown2.GetComponent<Dropdown>().value = 3;
                    }
                    else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 == "hardai")
                    {
                        DropDown2.GetComponent<Dropdown>().value = 4;
                    }
                    P2nameinputfield.gameObject.SetActive(false);
                    Slot2nametag.SetActive(true);
                    Slot2colourbutton.SetActive(true);
                    Slot2nametag.GetComponent<Text>().text = PersistGameObject.GetComponent<PersistObject>().P2name;
                }
            }
            {//Slot 3
                GameObject DropDown3 = GameObject.Find("Slot 3 Dropdown");
                if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 == "empty")
                {
                    DropDown3.GetComponent<Dropdown>().value = 0;
                    P3nameinputfield.gameObject.SetActive(false);
                    Slot3nametag.SetActive(false);
                }
                else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 == "human")
                {
                    DropDown3.GetComponent<Dropdown>().value = 1;
                    P3nameinputfield.gameObject.SetActive(true);
                    Slot3nametag.SetActive(false);
                    Slot3colourbutton.SetActive(true);
                    P3nameinputfield.text = PersistGameObject.GetComponent<PersistObject>().P3name;
                }
                else
                {
                     if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 == "easyai")
                    {
                        DropDown3.GetComponent<Dropdown>().value = 2;
                    }
                    else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 == "medai")
                    {
                        DropDown3.GetComponent<Dropdown>().value = 3;
                    }
                    else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 == "hardai")
                    {
                        DropDown3.GetComponent<Dropdown>().value = 4;
                    }
                    P3nameinputfield.gameObject.SetActive(false);
                    Slot3nametag.SetActive(true);
                    Slot3colourbutton.SetActive(true);
                    Slot3nametag.GetComponent<Text>().text = PersistGameObject.GetComponent<PersistObject>().P3name;
                }
            }
            {//Slot 4
                GameObject DropDown4 = GameObject.Find("Slot 4 Dropdown");
                if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 == "empty")
                {
                    DropDown4.GetComponent<Dropdown>().value = 0;
                    P4nameinputfield.gameObject.SetActive(false);
                    Slot4nametag.SetActive(false);
                }
                else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 == "human")
                {
                    DropDown4.GetComponent<Dropdown>().value = 1;
                    P4nameinputfield.gameObject.SetActive(true);
                    Slot4nametag.SetActive(false);
                    Slot4colourbutton.SetActive(true);
                    P4nameinputfield.text = PersistGameObject.GetComponent<PersistObject>().P4name;
                }
                else
                {
                    if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 == "easyai")
                    {
                        DropDown4.GetComponent<Dropdown>().value = 2;
                    }
                    else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 == "medai")
                    {
                        DropDown4.GetComponent<Dropdown>().value = 3;
                    }
                    else if (PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 == "hardai")
                    {
                        DropDown4.GetComponent<Dropdown>().value = 4;
                    }
                    P4nameinputfield.gameObject.SetActive(false);
                    Slot4nametag.SetActive(true);
                    Slot4colourbutton.SetActive(true);
                    Slot4nametag.GetComponent<Text>().text = PersistGameObject.GetComponent<PersistObject>().P4name;
                }
            }
        }
    }

    public void BackButtonNew()
    {
        ClickSound();
        if (GameOptionsNew.activeSelf)
        {
            MainButtonsNew.SetActive(true);
            GameOptionsNew.SetActive(false);
        }
        else if (HowToPlay.activeSelf)
        {
            MainButtonsNew.SetActive(true);
            HowToPlay.SetActive(false);
        }
        else if (Credits.activeSelf)
        {
            MainButtonsNew.SetActive(true);
            Credits.SetActive(false);
        }
        else if (SettingsMenu.activeSelf)
        {
            MainButtonsNew.SetActive(true);
            SettingsMenu.SetActive(false);
        }
        else if (OnlineHostOrJoinNew.activeSelf)
        {
            MainButtonsNew.SetActive(true);
            OnlineHostOrJoinNew.SetActive(false);
        }
        else if (HostOptionsNew.activeSelf)
        {
            OnlineHostOrJoinNew.SetActive(true);
            HostOptionsNew.SetActive(false);
        }
        else if (ClientOptionsNew.activeSelf)
        {
            OnlineHostOrJoinNew.SetActive(true);
            ClientOptionsNew.SetActive(false);
            GameManager.Instance.BackButton();
        }
        else if (TemporaryHostWaiting.activeSelf)
        {
            HostOptionsNew.SetActive(true);
            TemporaryHostWaiting.SetActive(false);
            GameManager.Instance.BackButton();
        }
    }

    public void OpenPopUpButton(GameObject ObjectToOpen)
    {
        ObjectToOpen.SetActive(true);
    }

    public void ClosePopUpButton(GameObject ObjectToClose)
    {
        ObjectToClose.SetActive(false);
    }

    public void Player1NameChanged()
    {
        P1name = P1nameinputfield.text;
    }
    public void Player2NameChanged()
    {
        P2name = P2nameinputfield.text;
    }
    public void Player3NameChanged()
    {
        P3name = P3nameinputfield.text;
    }
    public void Player4NameChanged()
    {
        P4name = P4nameinputfield.text;
    }

    public void PlayerSlotDropDown1()
    {
        ClickSound();
        if (!PersistGameObject.GetComponent<PersistObject>().ReturnToSettings)
        {
            GameObject ThisDropDown = GameObject.Find("Slot 1 Dropdown");

            Debug.Log("Slot 1 changed");
            PersistGameObject.GetComponent<PersistObject>().Wins[0] = 0;
            if (ThisDropDown.GetComponent<Dropdown>().value == 0)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 = "human";
                P1nameinputfield.gameObject.SetActive(true);
                P1nameinputfield.transform.Find("Placeholder").GetComponent<Text>().text = "Player 1";
                Slot1colourbutton.SetActive(true);
                Slot1nametag.SetActive(false);
                P1name = "";
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 1)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 = "easyai";
                P1nameinputfield.gameObject.SetActive(false);
                Slot1colourbutton.SetActive(true);
                Slot1nametag.SetActive(true);
                Slot1nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot1nametag.GetComponent<Text>().text == P2name || Slot1nametag.GetComponent<Text>().text == P3name || Slot1nametag.GetComponent<Text>().text == P4name)
                {
                    Slot1nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P1name = Slot1nametag.GetComponent<Text>().text;
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 2)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 = "medai";
                P1nameinputfield.gameObject.SetActive(false);
                Slot1colourbutton.SetActive(true);
                Slot1nametag.SetActive(true);
                Slot1nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot1nametag.GetComponent<Text>().text == P2name || Slot1nametag.GetComponent<Text>().text == P3name || Slot1nametag.GetComponent<Text>().text == P4name)
                {
                    Slot1nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P1name = Slot1nametag.GetComponent<Text>().text;

            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 3)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 = "hardai";
                P1nameinputfield.gameObject.SetActive(false);
                Slot1colourbutton.SetActive(true);
                Slot1nametag.SetActive(true);
                Slot1nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot1nametag.GetComponent<Text>().text == P2name || Slot1nametag.GetComponent<Text>().text == P3name || Slot1nametag.GetComponent<Text>().text == P4name)
                {
                    Slot1nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P1name = Slot1nametag.GetComponent<Text>().text;
            }
            PersistGameObject.GetComponent<PersistObject>().SlotCounter();
        }
    }
    public void PlayerSlotDropDown2()
    {
        ClickSound();
        if (!PersistGameObject.GetComponent<PersistObject>().ReturnToSettings)
        {
            GameObject ThisDropDown = GameObject.Find("Slot 2 Dropdown");

            Debug.Log("Slot 2 changed");
            PersistGameObject.GetComponent<PersistObject>().Wins[1] = 0;
            if (ThisDropDown.GetComponent<Dropdown>().value == 0)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 = "empty";
                P2nameinputfield.gameObject.SetActive(false);
                Slot2colourbutton.SetActive(false);
                Slot2nametag.SetActive(false);
                P2name = "";
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 1)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 = "human";
                P2nameinputfield.gameObject.SetActive(true);
                P2nameinputfield.transform.Find("Placeholder").GetComponent<Text>().text = "Player 2";
                Slot2colourbutton.SetActive(true);
                Slot2nametag.SetActive(false);
                P2name = "";
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 2)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 = "easyai";
                P2nameinputfield.gameObject.SetActive(false);
                Slot2colourbutton.SetActive(true);
                Slot2nametag.SetActive(true);
                Slot2nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot2nametag.GetComponent<Text>().text == P1name || Slot2nametag.GetComponent<Text>().text == P3name || Slot2nametag.GetComponent<Text>().text == P4name)
                {
                    Slot2nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P2name = Slot2nametag.GetComponent<Text>().text;
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 3)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 = "medai";
                P2nameinputfield.gameObject.SetActive(false);
                Slot2colourbutton.SetActive(true);
                Slot2nametag.SetActive(true);
                Slot2nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot2nametag.GetComponent<Text>().text == P1name || Slot2nametag.GetComponent<Text>().text == P3name || Slot2nametag.GetComponent<Text>().text == P4name)
                {
                    Slot2nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P2name = Slot2nametag.GetComponent<Text>().text;
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 4)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 = "hardai";
                P2nameinputfield.gameObject.SetActive(false);
                Slot2colourbutton.SetActive(true);
                Slot2nametag.SetActive(true);
                Slot2nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot2nametag.GetComponent<Text>().text == P1name || Slot2nametag.GetComponent<Text>().text == P3name || Slot2nametag.GetComponent<Text>().text == P4name)
                {
                    Slot2nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P2name = Slot2nametag.GetComponent<Text>().text;
            }
            PersistGameObject.GetComponent<PersistObject>().SlotCounter();
        }
    }
    public void PlayerSlotDropDown3()
    {
        ClickSound();
        if (!PersistGameObject.GetComponent<PersistObject>().ReturnToSettings)
        {
            GameObject ThisDropDown = GameObject.Find("Slot 3 Dropdown");

            Debug.Log("Slot 3 changed");
            PersistGameObject.GetComponent<PersistObject>().Wins[2] = 0;
            if (ThisDropDown.GetComponent<Dropdown>().value == 0)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 = "empty";
                P3nameinputfield.gameObject.SetActive(false);
                Slot3colourbutton.SetActive(false);
                Slot3nametag.SetActive(false);
                P3name = "";
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 1)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 = "human";
                P3nameinputfield.gameObject.SetActive(true);
                P3nameinputfield.transform.Find("Placeholder").GetComponent<Text>().text = "Player 3";
                Slot3colourbutton.SetActive(true);
                Slot3nametag.SetActive(false);
                P3name = "";
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 2)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 = "easyai";
                P3nameinputfield.gameObject.SetActive(false);
                Slot3colourbutton.SetActive(true);
                Slot3nametag.SetActive(true);
                Slot3nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot3nametag.GetComponent<Text>().text == P1name || Slot3nametag.GetComponent<Text>().text == P2name || Slot3nametag.GetComponent<Text>().text == P4name)
                {
                    Slot3nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P3name = Slot3nametag.GetComponent<Text>().text;
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 3)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 = "medai";
                P3nameinputfield.gameObject.SetActive(false);
                Slot3colourbutton.SetActive(true);
                Slot3nametag.SetActive(true);
                Slot3nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot3nametag.GetComponent<Text>().text == P1name || Slot3nametag.GetComponent<Text>().text == P2name || Slot3nametag.GetComponent<Text>().text == P4name)
                {
                    Slot3nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P3name = Slot3nametag.GetComponent<Text>().text;
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 4)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 = "hardai";
                P3nameinputfield.gameObject.SetActive(false);
                Slot3colourbutton.SetActive(true);
                Slot3nametag.SetActive(true);
                Slot3nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot3nametag.GetComponent<Text>().text == P1name || Slot3nametag.GetComponent<Text>().text == P2name || Slot3nametag.GetComponent<Text>().text == P4name)
                {
                    Slot3nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P3name = Slot3nametag.GetComponent<Text>().text;
            }
            PersistGameObject.GetComponent<PersistObject>().SlotCounter();
        }
    }
    public void PlayerSlotDropDown4()
    {
        ClickSound();
        if (!PersistGameObject.GetComponent<PersistObject>().ReturnToSettings)
        {
            GameObject ThisDropDown = GameObject.Find("Slot 4 Dropdown");

            Debug.Log("Slot 4 changed");
            PersistGameObject.GetComponent<PersistObject>().Wins[3] = 0;
            if (ThisDropDown.GetComponent<Dropdown>().value == 0)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 = "empty";
                P4nameinputfield.gameObject.SetActive(false);
                Slot4colourbutton.SetActive(false);
                Slot4nametag.SetActive(false);
                P4name = "";
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 1)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 = "human";
                P4nameinputfield.gameObject.SetActive(true);
                P4nameinputfield.placeholder.GetComponent<Text>().text = "Player 4";
                Slot4colourbutton.SetActive(true);
                Slot4nametag.SetActive(false);
                P4name = "";
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 2)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 = "easyai";
                P4nameinputfield.gameObject.SetActive(false);
                Slot4colourbutton.SetActive(true);
                Slot4nametag.SetActive(true);
                Slot4nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot4nametag.GetComponent<Text>().text == P1name || Slot4nametag.GetComponent<Text>().text == P2name || Slot4nametag.GetComponent<Text>().text == P3name)
                {
                    Slot4nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P4name = Slot4nametag.GetComponent<Text>().text;
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 3)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 = "medai";
                P4nameinputfield.gameObject.SetActive(false);
                Slot4colourbutton.SetActive(true);
                Slot4nametag.SetActive(true);
                Slot4nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot4nametag.GetComponent<Text>().text == P1name || Slot4nametag.GetComponent<Text>().text == P2name || Slot4nametag.GetComponent<Text>().text == P3name)
                {
                    Slot4nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P4name = Slot4nametag.GetComponent<Text>().text;
            }
            else if (ThisDropDown.GetComponent<Dropdown>().value == 4)
            {
                PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 = "hardai";
                P4nameinputfield.gameObject.SetActive(false);
                Slot4colourbutton.SetActive(true);
                Slot4nametag.SetActive(true);
                Slot4nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                while (Slot4nametag.GetComponent<Text>().text == P1name || Slot4nametag.GetComponent<Text>().text == P2name || Slot4nametag.GetComponent<Text>().text == P3name)
                {
                    Slot4nametag.GetComponent<Text>().text = AiNames[Random.Range(0, AiNames.Count)];
                }
                P4name = Slot4nametag.GetComponent<Text>().text;
            }
            PersistGameObject.GetComponent<PersistObject>().SlotCounter();
        }
    }

    public void ColourSelectButton(int ButtonID)
    {
        ClickSound();
        PlayerSelectingColour = ButtonID;
        ColourSelectPopup.SetActive(true);
    }
    public void CloseColourSelectPopup()
    {
        ClickSound();
        PlayerSelectingColour = -1;
        ColourSelectPopup.SetActive(false);
    }
    public void ColourButton(GameObject myobject)
    {
        ClickSound();
        Material MyMaterial = myobject.GetComponent<CanvasRenderer>().GetMaterial();
        if (MyMaterial != Slot1colourdisplay.GetComponent<Image>().material && MyMaterial != Slot2colourdisplay.GetComponent<Image>().material && MyMaterial != Slot3colourdisplay.GetComponent<Image>().material && MyMaterial != Slot4colourdisplay.GetComponent<Image>().material)
        {
            PersistGameObject.GetComponent<PersistObject>().PlayerColours[PlayerSelectingColour] = myobject.GetComponent<CanvasRenderer>().GetMaterial();
            PlayerSelectingColour = -1;
            ColourSelectPopup.SetActive(false);
            UpdateColourTags();
        }
    }
    public void UpdateColourTags()
    {
        Slot1colourdisplay.GetComponent<Image>().material = PersistGameObject.GetComponent<PersistObject>().PlayerColours[0];
        Slot2colourdisplay.GetComponent<Image>().material = PersistGameObject.GetComponent<PersistObject>().PlayerColours[1];
        Slot3colourdisplay.GetComponent<Image>().material = PersistGameObject.GetComponent<PersistObject>().PlayerColours[2];
        Slot4colourdisplay.GetComponent<Image>().material = PersistGameObject.GetComponent<PersistObject>().PlayerColours[3];       
    }

    public void BoardSizeSliderFunc()
    {
        ClickSound();
        PersistGameObject.GetComponent<PersistObject>().boardSize = (int)BoardSizeSlider.GetComponent<Slider>().value;
    }

    private void OnlineButtons()
    {
        if (MouseTarget == HostButton)
        {
            HostOptions.SetActive(true);
            HostOrConnect.SetActive(false);
            NameInputCanvas.gameObject.SetActive(true);
        }
        else if (MouseTarget == ClientButton)
        {
            ClientOptions.SetActive(true);
            HostOrConnect.SetActive(false);
            NameInputCanvas.gameObject.SetActive(true);
        }
        else if (MouseTarget.tag == "BackButton")
        {
            //MainButtons.SetActive(true);
            OnlineHostOrJoinNew.SetActive(true);
            GameOptions.SetActive(false);
            OnlineOptions.SetActive(false);

        }
    }

    private void HostButtons()
    {
        if (MouseTarget == HostGoButton)
        {
            GameManager.Instance.HostButton();
            NameInputCanvas.gameObject.SetActive(false);
            WaitingForPlayer.text = "Waiting for an Opponent...";
            BoardSizeSettingsOnline.SetActive(false);
            HostGoButton.SetActive(false);
        }

        else if (MouseTarget.tag == "BackButton")
        {
            GameManager.Instance.BackButton();
            WaitingForPlayer.text = "";
            BoardSizeSettingsOnline.SetActive(true);
            HostGoButton.SetActive(true);
            HostOptions.SetActive(false);
            NameInputCanvas.gameObject.SetActive(false);
            HostOrConnect.SetActive(true);
        }

        else if (MouseTarget == IncreaseSizeOnline)
        {
            PersistGameObject.GetComponent<PersistObject>().boardSize += 1;
        }
        else if (MouseTarget == DecreaseSizeOnline)
        {
            PersistGameObject.GetComponent<PersistObject>().boardSize -= 1;
        }
    }

    private void ClientButtons()
    {
        if (MouseTarget == ClientGoButton)
        {
            NameInputCanvas.gameObject.SetActive(false);
            GameManager.Instance.ConnectToServerButton();
        }
        else if (MouseTarget.tag == "BackButton")
        {
            GameManager.Instance.BackButton();
            ClientOptions.SetActive(false);
            NameInputCanvas.gameObject.SetActive(false);
            HostOrConnect.SetActive(true);
        }
    }

    public void ClientGoButtonNew()
    {
        ClickSound();
        ClientOptionsNew.SetActive(false);
        GameManager.Instance.ConnectToServerButton();
    }
    public void HostGoButtonNew()
    {
        ClickSound();
        HostOptionsNew.SetActive(false);
        TemporaryHostWaiting.SetActive(true);
        GameManager.Instance.HostButton();
    }
    public void OnlineBoardSizeSliderNew()
    {
        ClickSound();
        PersistGameObject.GetComponent<PersistObject>().boardSize = (int)BoardSizeSliderOnline.GetComponent<Slider>().value;
    }
    private void UpdateDisplays()
    {
        PlayerDisplay.GetComponent<TextMeshPro>().text = PersistGameObject.GetComponent<PersistObject>().noOfPlayers.ToString();
        SizeDisplay.GetComponent<TextMeshPro>().text = PersistGameObject.GetComponent<PersistObject>().boardSize.ToString();
        SizeDisplayOnline.GetComponent<TextMeshPro>().text = PersistGameObject.GetComponent<PersistObject>().boardSize.ToString();
        SizeDisplayOnlineNew.GetComponent<Text>().text = PersistGameObject.GetComponent<PersistObject>().boardSize.ToString();
        SizeDisplayNew.text = PersistGameObject.GetComponent<PersistObject>().boardSize.ToString();
        AiDisplay.GetComponent<TextMeshPro>().text = PersistGameObject.GetComponent<PersistObject>().noOfAI.ToString();
    }
}
