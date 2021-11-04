using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioSource sound;
    private Rect audiorect;
    private static BackgroundAudio audioPlay;

    // Start is called before the first frame update
    void Start()
    {
        sound = gameObject.AddComponent<AudioSource>();
        
    }

    public void BGMOnOff(bool onOff)
    {
        sound.mute = !onOff;
    }

    private void Awake()
    {
        if (audioPlay == null)
        {
            audioPlay = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}