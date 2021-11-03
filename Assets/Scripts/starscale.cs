using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starscale : MonoBehaviour
{

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    AudioSource audio1;
    AudioSource audio2;
    AudioSource audio3;
    public AudioClip music;
    int i = 0;
    float timespan = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        star1 = GameObject.Find("star1");
        star2 = GameObject.Find("star2");
        star3 = GameObject.Find("star3");
        audio1 = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timespan >= 0)
        {
            timespan -= Time.deltaTime;
            return;
        }
        else
        {
            if (0 < i && i < 25)
            {
                star1.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f); // 짠나타나여
            }
            if (12 < i && i <= 37)
            {
                star2.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f); // 짠나타나여
            }
            if (25 < i && i <= 50)
            {
                star3.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f); // 짠나타나여
            }
           /* if (i == 1)
                audio1.Play();
            if (i == 12)
            {
                audio2 = gameObject.AddComponent<AudioSource>();
                audio2.clip = music;
                audio2.Play();
            }
            if (i == 25)
            {
                audio3 = gameObject.AddComponent<AudioSource>();
                audio3.clip = music;
                audio3.Play();
            }
            i++;*/
        }
    }
}
