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
        if (Input.GetButtonDown("Jump"))        // �����̽��ٸ� ������
            manager.Action(manager.stageNum);   // ��ȭâ ����
    }
    private void FixedUpdate()
    {
        //Ray
        //Debug.DrawRay();
    }

}
