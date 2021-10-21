using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + "감지!");
        string detectObject = null;
        
        switch (other.tag)
        {
            case "SnowBall":
                detectObject = "SnowBall";
                break;
            default:
                detectObject = "theOther";
                break;
        }
        CharacterMotion.hitTag = detectObject;
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + "감지 종료");
    }
}
