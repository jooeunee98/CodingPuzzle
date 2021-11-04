using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{

    private bool[] isPushed = new bool[4];
    GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        pushUpdate(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pushUpdate(int i)
    {
        isPushed[i] = true;

        for(int j=0; j<4; j++)
        {
            if(j != i)
            {
                isPushed[j] = false;
            }
        }

        for(int j=0; j<4; j++)
        {
            obj = GameObject.FindWithTag("ChapterButton" + (j + 1));
            if(isPushed[j] == true)
            {
                obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/클리어스테이지"+(j+1));

            }
            else
            {
                obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/언클리어스테이지" + (j + 1));

            }
        }
    }
}
