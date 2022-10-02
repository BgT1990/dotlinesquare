using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the screensaver function of the main menu
public class MainMenuDemoScreenSaver : MonoBehaviour
{
    public GameObject BehindMenuPanel;
    public bool ScreenSaverActive;
    public bool TimerActive;
    public Camera MainMenuCamera;
    public Camera GameCamera;
    public Camera ActionCamera;
    private string Coroutine;

    //Could do these as a list
    public GameObject MainMenu; //0
    public GameObject LocalOptions; //1
    public GameObject OnlineOptions; //2
    public GameObject HowToPlay; //3
    public GameObject Settings; //4
    public GameObject HostOptions; //5
    public GameObject ClientOptions;//6
    public GameObject ClientOptionsNew;//7
    public GameObject HostOptionsNew;//8
    public GameObject OnlineOptionsNew;//9
    public int ScreenSaverMenuIndex;
    public GameObject Title;
    public Vector3 MainStartPos;
    public Quaternion MainStartRot;
    public Vector3 GameStartPos;
    public Quaternion GameStartRot;
    public bool Lerp1 = false;
    public bool Lerp2 = false;
    public float TimeStartedLerping;
    float TimeSinceStarted = 0.0f;
    float t = 0.0f;
    public float TimeTakenDuringLerp = 3; //Time in seconds for camera transition to take.
    public float TimebeforeScreenSaver = 30;

    private void Start()
    {
        GameCamera.enabled = false;
        ActionCamera.enabled = false;
        MainMenuCamera.enabled = true;
        TimerActive = false;
        ScreenSaverActive = false;
        Coroutine = "ScreenSaverTimer"; //coroutine will reset when called as a string, but only pause when called as a method
        ScreenSaverMenuIndex = -1;
    }

    //if no inputs, count to 30... if 30 is reached, disable current buttons and grey panel and move/switch the camera to look a the board
    //if any inputs reset the timer and re-enable buttons/grey panel
    private void Update()
    {
        if (InputCheck()) //check for mouse clicks, mouse axis and keypresses
        {

            //Debug.Log("Input was detected");
            StopCoroutine(Coroutine);
            TimerActive = false;
            
            if (ScreenSaverActive)
            {
                ScreenSaverEnd();
            }
        }
        else if(!ScreenSaverActive && !TimerActive)
        {
            Timer();
        }
        if (Lerp1)
        {
            CamTransitionMainToGame();
        }
        else if (Lerp2)
        {
            CamTransitionGameToMain();
        }
        /*
        if (ScreenSaverActive && ActionCamera.enabled == true)
        {
            CamTransitionMainToGame();
        }
        else if(!ScreenSaverActive && ActionCamera.enabled == true)
        {
            CamTransitionGameToMain();
        }
        */
    }

    private void FixedUpdate()
    {

    }

    private bool InputCheck()
    {
        if (Input.GetAxis("Mouse X") !=0.0f || Input.GetAxis("Mouse Y") != 0.0f || Input.anyKeyDown) //input.anykey seems to just always return true, so holding a key will not stop the timer
        {
            return true;
        }
        
        return false;
    }

    public IEnumerator ScreenSaverTimer()
    {
        //Debug.Log("0 seconds");
        //yield return new WaitForSecondsRealtime(5);
        //Debug.Log("5 seconds");
        //yield return new WaitForSecondsRealtime(5);
        //Debug.Log("10 seconds");
        //yield return new WaitForSecondsRealtime(10);
        //Debug.Log("20 seconds");
        //yield return new WaitForSecondsRealtime(10);
        //Debug.Log("30 seconds");
        yield return new WaitForSecondsRealtime(TimebeforeScreenSaver);
        ScreenSaver();
    }

    private void Timer()
    {
        TimerActive = true;
        StartCoroutine(Coroutine);
        //Debug.Log("Starting Screensaver Timer");
    }

    private void ScreenSaver()
    {
        TimerActive = false;
        StopCoroutine(Coroutine);
        ScreenSaverActive = true;
        Debug.Log("Starting Screensaver");      
        BehindMenuPanel.SetActive(false);
        //ActionCamera.enabled = true;
        MainStartPos = MainMenuCamera.GetComponent<Transform>().position;
        MainStartRot = MainMenuCamera.GetComponent<Transform>().rotation;
        Lerp1 = true;
        TimeStartedLerping = Time.time;
        //GameCamera.enabled = true;
        //MainMenuCamera.enabled = false;

        Title.SetActive(false);
        if (MainMenu.activeSelf)
        {
            ScreenSaverMenuIndex = 0;
            MainMenu.SetActive(false);
        }
        else if (LocalOptions.activeSelf)
        {
            ScreenSaverMenuIndex = 1;
            LocalOptions.SetActive(false);
        }
        else if (OnlineOptions.activeSelf)
        {
            ScreenSaverMenuIndex = 2;
            OnlineOptions.SetActive(false);
        }
        else if (HowToPlay.activeSelf)
        {
            ScreenSaverMenuIndex = 3;
            HowToPlay.SetActive(false);
        }
        else if (Settings.activeSelf)
        {
            ScreenSaverMenuIndex = 4;
            Settings.SetActive(false);
        }
        else if (HostOptions.activeSelf)
        {
            ScreenSaverMenuIndex = 5;
            HostOptions.SetActive(false);
        }
        else if (ClientOptions.activeSelf)
        {
            ScreenSaverMenuIndex = 6;
            ClientOptions.SetActive(false);
        }
        else if (ClientOptionsNew.activeSelf)
        {
            ScreenSaverMenuIndex = 7;
            ClientOptionsNew.SetActive(false);
        }
        else if (HostOptionsNew.activeSelf)
        {
            ScreenSaverMenuIndex = 8;
            HostOptionsNew.SetActive(false);
        }
        else if (OnlineOptionsNew.activeSelf)
        {
            ScreenSaverMenuIndex = 9;
            OnlineOptionsNew.SetActive(false);
        }
    }

    public void CamInterruptTransition()
    {
        //If input is recieved when transitioning from main to game, this will fix that
        //Just do the same as game to main, but using the actioncamera position as the starting location
    }

    public void CamTransitionMainToGame()
    {
        MainMenuCamera.enabled = false;
        ActionCamera.enabled = true;

        if (t < 1.0f)
        {
            TimeSinceStarted = Time.time - TimeStartedLerping;
            t = TimeSinceStarted / TimeTakenDuringLerp;
            //t += Time.deltaTime/10.0f;
            Vector3 V = Vector3.Lerp(MainStartPos, GameCamera.GetComponent<Transform>().position, t);
            Quaternion Q = Quaternion.Lerp(MainStartRot, GameCamera.GetComponent<Transform>().rotation, t);
            ActionCamera.GetComponent<Transform>().SetPositionAndRotation(V, Q);
            //Debug.Log("CamMtoG t=" + t + " V=" + V + " Q=" + Q);
        }
        else
        {
            ActionCamera.enabled = false;
            GameCamera.enabled = true;
            TimeSinceStarted = 0.0f;
            t = 0.0f;
            Lerp1 = false;
        }
        //Fixed thanks to http://www.blueraja.com/blog/404/how-to-use-unity-3ds-linear-interpolation-vector3-lerp-correctly
    }

    public void CamTransitionGameToMain()
    {
        GameCamera.enabled = false;
        ActionCamera.enabled = true;

        if (t < 1.0f)
        {
            TimeSinceStarted = Time.time - TimeStartedLerping;
            t = TimeSinceStarted / TimeTakenDuringLerp;
            //t += Time.deltaTime/10.0f;
            Vector3 V = Vector3.Lerp(GameStartPos, MainMenuCamera.GetComponent<Transform>().position, t);
            Quaternion Q = Quaternion.Lerp(GameStartRot, MainMenuCamera.GetComponent<Transform>().rotation, t);
            ActionCamera.GetComponent<Transform>().SetPositionAndRotation(V, Q);
            //Debug.Log("CamGtoM t=" + t + " V=" + V + " Q=" + Q);
        }
        else
        {
            ActionCamera.enabled = false;
            MainMenuCamera.enabled = true;
            TimeSinceStarted = 0.0f;
            t = 0.0f;
            Lerp2 = false;
        }
    }

    private void ScreenSaverEnd()
    {
        ScreenSaverActive = false;
        Debug.Log("Stopping Screensaver");
        BehindMenuPanel.SetActive(true);
        //ActionCamera.enabled = true;
        GameStartPos = GameCamera.GetComponent<Transform>().position;
        GameStartRot = GameCamera.GetComponent<Transform>().rotation;
        Lerp2 = true;
        TimeStartedLerping = Time.time;
        //GameCamera.enabled = false;
        //MainMenuCamera.enabled = true;

        Title.SetActive(true);
        if (ScreenSaverMenuIndex == 0)
        {
            MainMenu.SetActive(true);
        }
        else if (ScreenSaverMenuIndex == 1)
        {
            LocalOptions.SetActive(true);
        }
        else if (ScreenSaverMenuIndex == 2)
        {
            OnlineOptions.SetActive(true);
        }
        else if (ScreenSaverMenuIndex == 3)
        {
            HowToPlay.SetActive(true);
        }
        else if (ScreenSaverMenuIndex == 4)
        {
            Settings.SetActive(true);
        }
        else if (ScreenSaverMenuIndex == 5)
        {
            HostOptions.SetActive(true);
        }
        else if (ScreenSaverMenuIndex == 6)
        {
            ClientOptions.SetActive(true);
        }
        else if (ScreenSaverMenuIndex == 7)
        {
            ClientOptionsNew.SetActive(true);
        }
        else if (ScreenSaverMenuIndex == 8)
        {
            HostOptionsNew.SetActive(true);
        }
        else if (ScreenSaverMenuIndex == 9)
        {
            OnlineOptionsNew.SetActive(true);
        }

    }
}
