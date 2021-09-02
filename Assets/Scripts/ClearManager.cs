using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = GameObject.FindWithTag("ClearCanvas");
        GameObject Home = Instantiate(Resources.Load("prefabs/Home")) as GameObject;
        Home.transform.SetParent(parent.transform, false);

        GameObject Menu = Instantiate(Resources.Load("prefabs/Menu")) as GameObject;
        Menu.transform.SetParent(parent.transform, false);

        GameObject Settings = Instantiate(Resources.Load("prefabs/Settings")) as GameObject;
        Settings.transform.SetParent(parent.transform, false);

        GameObject ClearImage = Instantiate(Resources.Load("prefabs/ClearImage")) as GameObject;
        ClearImage.transform.SetParent(parent.transform, false);

        GameObject StageImage = Instantiate(Resources.Load("prefabs/StageImage")) as GameObject;
        StageImage.transform.SetParent(parent.transform, false);

        GameObject Retry = Instantiate(Resources.Load("prefabs/Retry")) as GameObject;
        Retry.transform.SetParent(parent.transform, false);

        GameObject Next = Instantiate(Resources.Load("prefabs/Next")) as GameObject;
        Next.transform.SetParent(parent.transform, false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
