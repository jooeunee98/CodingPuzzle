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
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/�������۹�ư");

    }

    public void returnImageStart()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/���۹�ư");

    }
    public void changeImageSettings()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/�������۹�ư");

    }

    public void returnImageSettings()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/������ư 1");

    }

}
