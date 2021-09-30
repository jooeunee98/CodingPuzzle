using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PushEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeImageStart()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/눌린시작버튼");

    }

    public void returnImageStart()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/시작버튼");

    }
    public void changeImageSettings()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/눌린시작버튼");

    }

    public void returnImageSettings()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/설정버튼 1");

    }

}
