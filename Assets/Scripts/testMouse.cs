using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMouse : MonoBehaviour
{
    RaycastHit hit;
    float MaxDistance = 15000000000f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
        else if(Input.GetKey(KeyCode.LeftArrow))
            transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 0.3f);
            if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance))
            {
                hit.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
