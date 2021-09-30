using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour
{
    public GameObject blockStart;
    public GameObject groundBlock;
    public int speed = 1;

    float creatX, creatY, creatZ;
    int count = 1, total = 0, line = 0;

    GameObject child;
    float timespan = 0f;

    // Start is called before the first frame update
    void Start()
    {
        creatX = blockStart.transform.position.x;
        creatY = blockStart.transform.position.y;
        creatZ = blockStart.transform.position.z;

        
        child = Instantiate(groundBlock, new Vector3(creatX, creatY, creatZ), Quaternion.identity);
        child.transform.localScale = new Vector3(0, 0, 0);
        child.transform.parent = blockStart.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //단일 블럭 생성 애니메이션
        if (timespan < 1)
        {
            child.transform.localScale += new Vector3(Time.deltaTime * 10, Time.deltaTime * 10, Time.deltaTime * 10);
        }
        else
        {
            child.transform.localScale = new Vector3(10, 10, 10);
            count = 2;
            total++;
        }
        timespan += Time.deltaTime;

        //한줄 생성
        if (count != 1 && total < 5)
        {
            creatZ += (float)5.75;
            count = 1;

            child = Instantiate(groundBlock, new Vector3(creatX, creatY, creatZ), Quaternion.identity);
            child.transform.localScale = new Vector3(0, 0, 0);
            child.transform.parent = blockStart.GetComponent<Transform>();
            timespan = 0;
        }
    }
}
