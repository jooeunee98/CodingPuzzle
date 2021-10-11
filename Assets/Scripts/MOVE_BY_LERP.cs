using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_BY_LERP : MonoBehaviour
{
    float CHARACTER_POS_X = 0;
    float CHARACTER_POS_Y = 0;
    int CHARACTER_DIRECTION = 0; // # 0 : µ¿ # 1 : ¼­ # 2 : ³² # 3 : ºÏ
    bool CAN_MOVE = false;
    public GameObject targetPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MOVE()
    {
        switch (CHARACTER_DIRECTION)
        {
            case 0:
                CHARACTER_POS_Y++;
                break;
            case 1:
                CHARACTER_POS_Y--;
                break;
            case 2:
                CHARACTER_POS_X++;
                break;
            case 3:
                CHARACTER_POS_X--;
                break;
        }
        CAN_MOVE = true;
    }

    public void LEFT_TURN()
    {
        switch (CHARACTER_DIRECTION)
        {
            case 0:
                CHARACTER_DIRECTION = 3;
                break;
            case 1:
                CHARACTER_DIRECTION = 2;
                break;
            case 2:
                CHARACTER_DIRECTION = 0;
                break;
            case 3:
                CHARACTER_DIRECTION = 1;
                break;
        }
        CAN_MOVE = true;
    }

    public void RIGHT_TURN()
    {
        switch (CHARACTER_DIRECTION)
        {
            case 0:
                CHARACTER_DIRECTION = 2;
                break;
            case 1:
                CHARACTER_DIRECTION = 3;
                break;
            case 2:
                CHARACTER_DIRECTION = 1;
                break;
            case 3:
                CHARACTER_DIRECTION = 0;
                break;
        }
        CAN_MOVE = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CAN_MOVE == true)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(CHARACTER_POS_X,CHARACTER_POS_Y,0), 0.05f);
            if(gameObject.transform.position.x == CHARACTER_POS_X && gameObject.transform.position.y == CHARACTER_POS_Y)
            {
                CAN_MOVE = false;
            }
        }


    }
}
