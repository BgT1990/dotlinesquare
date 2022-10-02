using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SettingsScript : MonoBehaviour
/*
 * https://docs.unity3d.com/ScriptReference/QualitySettings.html
 * https://docs.unity3d.com/ScriptReference/Screen.html
 */
{
    public Toggle FullScreenToggle;
    public static Resolution[] resolutions;
    public List<Resolution> resolutionlist;
    public List<string> resolutionstringlist;
    public Dropdown ResolutionDropdown;
    public bool FullScreenBool;
    public GameObject AudioObject;
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider EffectsVolume;
    public Toggle AudioEnabled;

    private void Start()
    {
        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            FullScreenToggle.GetComponent<Toggle>().isOn = true;
            FullScreenBool = true;
        }
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            FullScreenToggle.GetComponent<Toggle>().isOn = false;
            FullScreenBool = false;
        }
        ResolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions;
        resolutionlist = resolutions.OfType<Resolution>().ToList();
        foreach (Resolution R in resolutionlist)
        {
            resolutionstringlist.Add(R.ToString());
        }
        ResolutionDropdown.AddOptions(resolutionstringlist);

    }

    public void FullScreen(GameObject ThisToggle)
    {
        if (ThisToggle.GetComponent<Toggle>().isOn) { Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; Debug.Log("FullScreen"); FullScreenBool = true; }
        else { Screen.fullScreenMode = FullScreenMode.Windowed; Debug.Log("Windowed"); FullScreenBool = false; }
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenBool);
    }

    public void ResolutionChange()
    {
        Screen.SetResolution(resolutionlist[ResolutionDropdown.GetComponent<Dropdown>().value].width, resolutionlist[ResolutionDropdown.GetComponent<Dropdown>().value].height, FullScreenBool);
    }

    private void OnEnable()
    {
        if (AudioObject == null)
        {
            AudioObject = GameObject.Find("PersistGameObject").GetComponent<PersistObject>().AudioObject;
        }

        if (AudioObject.GetComponent<AudioSource>().mute == true)
        {
            AudioEnabled.isOn = false;
        }
        else if (AudioObject.GetComponent<AudioSource>().mute == false)
        {
            AudioEnabled.isOn = true;
        }
    }

    public void AudioEnabler()
    {
        if (AudioEnabled.isOn) { AudioObject.GetComponent<AudioSource>().mute = false; }
        else { AudioObject.GetComponent<AudioSource>().mute = true; }
    }
}
