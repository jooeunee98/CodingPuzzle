using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Nickname : MonoBehaviour
{
    //public GameObject inputField;
    //public GameObject changButton;
    public Text inputText;
    public Text nickName; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(inputText.text);
            Destroy(this.gameObject);
            nickName.text = inputText.text;
        }
    }
    void Quit()
    {
        Application.Quit();
    }
}

