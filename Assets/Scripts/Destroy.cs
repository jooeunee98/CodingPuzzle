using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    GameObject characterManager;
    // Start is called before the first frame update
    void Start()
    {
        characterManager = GameObject.Find("CharacterManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            Destroy(other.gameObject);
            characterManager.GetComponent<CharacterManager>().CharacterSpawn();
        }
    }
}
