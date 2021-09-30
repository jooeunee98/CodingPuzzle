using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clear : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            Destroy(gameObject);
            Debug.Log(111111);
        }
    }
}
