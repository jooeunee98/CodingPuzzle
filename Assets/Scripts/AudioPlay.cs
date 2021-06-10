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
        sound = gameObject.AddComponent<AudioSource>();
        switchOn.SetActive(false);
    }

    public void BGMOnOff(bool onOff)
    {
        sound.mute = !onOff;
    }
    // 사운드 버튼 이미지 변경
    public void OnChangeValue()
    {
        bool onoffSwitch = gameObject.GetComponent<Toggle>().isOn;
        if (onoffSwitch)
        {
            Debug.Log("off");
            switchOn.SetActive(true);
            switchOff.SetActive(false);
        }
        if (!onoffSwitch)
        {
            Debug.Log("on");
            switchOn.SetActive(false);
            switchOff.SetActive(true);
        }

    }
}