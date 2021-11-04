using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clear : MonoBehaviour
{
    public int success = 0;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            Destroy(gameObject.GetComponent<MeshRenderer>());

            success = 1;
        }
    }
}
