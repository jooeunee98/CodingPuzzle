using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    public AudioSource sound;
    private Rect audiorect;

    // Start is called before the first frame update
    void Start()
    {
        sound = gameObject.AddComponent<AudioSource>();
    }

    public void BGMOnOff(bool onOff)
    {
        sound.mute = !onOff;
    }
}