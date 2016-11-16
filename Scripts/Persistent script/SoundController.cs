using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{

    private static SoundController instance = null;

    void Start()
    {
        if (instance == null)
        {   //making sure we only initialize one instance.
            instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        else {                  //Destroying unused instances.
            GameObject.Destroy(this.gameObject);
        }
    }

    public void PlayMusic(AudioClip music, bool loop)
    {
        GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
        GameObject.Find("Main Camera").GetComponent<AudioSource>().clip = music;
        GameObject.Find("Main Camera").GetComponent<AudioSource>().loop = loop;
        GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
    }

    public void PlaySFX(AudioClip sound, GameObject go) {
        go.GetComponent<AudioSource>().clip = sound;
        go.GetComponent<AudioSource>().Play();
    }
}
