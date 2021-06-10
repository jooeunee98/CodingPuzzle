using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotion : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private IEnumerator coroutine;
    bool go_forward = false;
    bool turn_right = false;
    bool turn_left = false;
    float stRotationY;
    int turnDegree = 0;
    void Start()
    {
        
    }
    public void Coru()
    {
        animator = GameObject.Find("Character").GetComponent<Animator>();
        coroutine = readCommand(1.0f);
        StartCoroutine(coroutine);

    }

    public IEnumerator readCommand(float waitTime)
    {
        BlockManager call = GameObject.Find("BlockManager").GetComponent<BlockManager>();

        BlockManager.Block LL = call.head.next;

        while (!call.isTail(LL))
        {
            //Debug.Log(LL.direction.Split(':')[0]);

            if (LL.direction.Split(':')[0].Equals("Button_left"))
            {
                Debug.Log("좌회전!");
                go_forward = false;
                turn_left = true;
                turnDegree = 0;
                // 도는건 얼마 안걸리니까 waitTime -> 1 로 변경
                waitTime = 2f;
            } else if (LL.direction.Split(':')[0].Equals("Button_right"))
            {
                Debug.Log("우회전!");
                go_forward = false;
                turn_right = true;
                turnDegree = 0;
                // 도는건 얼마 안걸리니까 waitTime -> 1 로 변경
                waitTime = 2f;
            }
            else
            {
                Debug.Log("직진!");
                waitTime = 1.0f;
                go_forward = true;
            }

            LL = LL.next;

            yield return new WaitForSeconds(waitTime);
        }
        animator.SetBool("theEnd", false);

        go_forward = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (go_forward == true)
        {
            GameObject.Find("Character").transform.Translate(GameObject.Find("Character").transform.localRotation * Vector3.forward * Time.deltaTime * 5.75f, Space.World);
        }
        else if (turn_right == true)
        {
            if (turnDegree < 90)
            {
                GameObject.Find("Character").transform.Rotate(0, 2, 0);
                turnDegree += 2;
            }
        }
        else if (turn_left == true)
        {
            if (turnDegree < 90)
            {
                GameObject.Find("Character").gameObject.transform.Rotate(0, -2, 0);
                turnDegree += 2;
            }
        }
    }
}
