using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSfx : MonoBehaviour
{
    public AudioClip ButtonClick;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
    }
}
