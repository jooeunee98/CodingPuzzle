using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlay : MonoBehaviour
{
    public AudioSource sound;
    private Rect audiorect;
    public GameObject switchOn, switchOff;

    // Start is called before the first frame update
    void Start()
    {
        switchOn.SetActive(false);
    }

    public void BGMOnOff(bool onOff)
    {
        bool onoffSwitch = gameObject.GetComponent<Toggle>().isOn;
        if (onoffSwitch)
        {
            switchOn.SetActive(true);
            switchOff.SetActive(false);
        }
        if (!onoffSwitch)
        {
            switchOn.SetActive(false);
            switchOff.SetActive(true);
        }
    }
}