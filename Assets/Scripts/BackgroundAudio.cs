using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    private static BackgroundAudio backgroundAudio;

    void Awake()
    {
        if (backgroundAudio == null)
        {
            backgroundAudio = this;
            DontDestroyOnLoad(backgroundAudio);
        }
        else
        {
            Debug.Log("Destroy : " + gameObject);
            Destroy(gameObject);
        }
    }
}