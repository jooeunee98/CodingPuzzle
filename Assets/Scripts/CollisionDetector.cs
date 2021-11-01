using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    static public bool pickUp = false;
    static public bool delCuttedTree = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " ����!");
    }

    private void OnTriggerStay(Collider other)
    {
        string detectObject = null;

        switch (other.name)
        {
            case "SnowBall":
                detectObject = "SnowBall"; break;
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
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + "���� ����");
        CharacterMotion.hitTag = "theOther";
    }
}
