using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    static public bool pickUp = false;
    static public bool delCuttedTree = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " 감지!");
    }

    private void OnTriggerStay(Collider other)
    {
        string detectObject = null;
        string detectName = null;

        switch (other.tag)
        {
            case "SnowBall":
                detectObject = "SnowBall";
                detectName = other.name;
                break;
            case "PlantTrees":
                Debug.Log("PlantTrees");
                detectObject = "PlantTrees"; break;
            case "TakeFruits":
                Debug.Log("TakeFruits");
                detectObject = "TakeFruits"; break;
            default:
                detectObject = "theOther";
                break;
        }

        if (other.tag.Equals("Character") && (pickUp == true))
        {
            Destroy(gameObject);
            Debug.Log("Pick up a fruit");
            pickUp = false;
        }
        if (other.tag.Equals("Character") && (delCuttedTree == true))
        {
            Destroy(gameObject);
            Debug.Log("Delete cutted tree");
            delCuttedTree = false;
        }
        CharacterMotion.hitTag = detectObject;
        CharacterMotion.hitName = detectName;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + "감지 종료");
        CharacterMotion.hitTag = "theOther";
        CharacterMotion.hitName = "theOther";
    }
}
