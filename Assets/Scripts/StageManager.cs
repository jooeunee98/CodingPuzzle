using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void stage1()
    {
        SceneManager.LoadScene(8);
    }
    public void stage2()
    {
        SceneManager.LoadScene(9);
    }
    public void stage3()
    {
        SceneManager.LoadScene(10);
    }
    public void stage4()
    {
        SceneManager.LoadScene(11);
    }
    public void stage5()
    {
        SceneManager.LoadScene(12);
    }
}
