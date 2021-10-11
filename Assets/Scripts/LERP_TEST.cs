using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LERP_TEST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   public GameObject targetPosition;
 
    void Update()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, 1.0f);        
    }
}
