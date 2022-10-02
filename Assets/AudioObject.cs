using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    public AudioClip ClickSoundClip;
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<AudioObject>().Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void ClickSound()
    {
        this.GetComponent<AudioSource>().PlayOneShot(ClickSoundClip);
    }
}
