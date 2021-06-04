using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAction : MonoBehaviour
{
    public GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))        // 스페이스바를 누르면
            manager.Action(manager.stageNum);   // 대화창 조작
    }
    private void FixedUpdate()
    {
        //Ray
        //Debug.DrawRay();
    }

}
