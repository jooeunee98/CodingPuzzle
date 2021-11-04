using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearbgm : MonoBehaviour
{

    AudioSource audiocom;
    float time = 10;
    // Use this for initialization 
    void Start()
    {
        audiocom = gameObject.GetComponent<AudioSource>();
        audiocom.volume = 0;
    }

    // Update is called once per frame 
    void Update()
    {
        if (audiocom.volume <= 1)
        {
            audiocom.volume += Time.deltaTime/time;

        }
    }
}
