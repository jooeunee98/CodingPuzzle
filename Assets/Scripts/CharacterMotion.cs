using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 28 ~ 136�� ��
/*  
    60 ~ 65��
    // �ƴϸ� �ݺ����� �ǳʶ�
    if (!blockStack.isEmpty())
    {
        ((BlockSystemTest.LoopBlock)blockStack.peek()).setLoopNum(2);
    }
    print = blockStack.pop().right; 
*/

public class CharacterMotion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float forceMagnitude;   // ĳ���Ͱ� Object�� ���ϴ� ��
    private float MaxDistance = 7f;
    RaycastHit hit;

    private Animator animator;
    private IEnumerator coroutine;
    bool go_forward = false;
    bool turn_right = false;
    bool turn_left = false;
    string hitTag;
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
    /*
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null)
        {
            Vector3 ForceDirection = hit.gameObject.transform.position - transform.position;
            ForceDirection.y = 0;
            ForceDirection.Normalize();

            rigidbody.AddForceAtPosition(ForceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
        gameObject.GetComponent<CharacterMotion>().forceMagnitude
    }
    */
    public IEnumerator readCommand(float waitTime)
    {
        // loopBlock�� ������ ���ÿ� Ǫ�� & �ݺ� Ƚ�� Ȯ��
        // �ݺ� Ƚ����ŭ �ش� ����Ʈ ���
        // �ݺ� Ƚ���� 0�� �����ϸ�, ���ÿ��� ��
        string kindOf = null;
        string numResource = "Images/Ŭ����ѹ�Ÿ��Ʋ";
        int num = 0;
        BlockSystemTest call = GameObject.Find("BlockSystemTest").GetComponent<BlockSystemTest>();
        BlockSystemTest.Block print = call.getRoot();
        BlockSystemTest.BStack blockStack = new BlockSystemTest.BStack();

        blockStack.push(print);
        while (!blockStack.isEmpty())
        {
            kindOf = print.getInfo().Split(':')[0];
            // loop or if���� ��� ���ÿ� �����ؼ� üũ����Ʈ ����
            if (kindOf.Equals("Button_loop"))
            {
                blockStack.push(print);
                // �ݺ��� ��ȿ�� ��
                if (call.loopValidate(print))
                {
                    // true�̸� �ݺ��� ���� �� ���
                    print = blockStack.peek().left;
                    num++;
                }
                else
                {
                    // �ƴϸ� �ݺ����� �ǳʶ�
                    if (!blockStack.isEmpty())
                    {
                        ((BlockSystemTest.LoopBlock)blockStack.peek()).setLoopNum(2);
                    }
                    print = blockStack.pop().right;
                    num = 0;
                }
            }
            else if (kindOf.Equals("Button_if"))
            {
                Debug.Log("Shot ray");
                switch (hitTag) {
                    case "SnowBall":
                        Debug.Log("There is SnowBall in front of the object");
                        break;
                    default:
                        Debug.Log("There is no any object");
                        break;
                }
                print = print.right;
            }
            // ���� ��尡 leaf or subLeaf�̸� �ܸ������� ������ ���̹Ƿ� ���ÿ� ����� üũ����Ʈ�� ���ư�
            else if (call.isLeaf(print))
            {
                // subLeaf�� ���� �ݺ����� 1ȸ ������ ���̹Ƿ�, �ݺ��� Ƚ�� ����
                if (GameObject.Find(blockStack.peek().getInfo()) != null)
                {
                    BlockSystemTest.Block temp = blockStack.peek();
                    //GameObject parent = GameObject.Find(temp.getInfo()).gameObject;
                    GameObject child = GameObject.Find(temp.getInfo()).gameObject.transform.Find("Image_loopNum").gameObject;
                    //Debug.Log("**numResource** : " + numResource);
                    child.GetComponent<Image>().sprite = Resources.Load(numResource + ((BlockSystemTest.LoopBlock)temp).getLoopNum(), typeof(Sprite)) as Sprite;
                }
                print = blockStack.pop();

                //Debug.Log(blockStack.peek().getInfo());
            }
            // �� �ܴ� �⺻ ���̹Ƿ� ����� ���� ��� ����
            else
            {
                print = print.right;
            }
            Debug.Log(print.getInfo());
            switch (kindOf)
            {
                case "Button_left":
                    {
                        Debug.Log("��ȸ��!");
                        go_forward = false;
                        turn_left = true;
                        turn_right = false;
                        turnDegree = 0;
                        // ���°� �� �Ȱɸ��ϱ� waitTime -> 1 �� ����
                        waitTime = 1f;
                        break;
                    }
                case "Button_right":
                    {
                        Debug.Log("��ȸ��!");
                        go_forward = false;
                        turn_right = true;
                        turn_left = false;
                        turnDegree = 0;
                        // ���°� �� �Ȱɸ��ϱ� waitTime -> 1 �� ����
                        waitTime = 1f;
                        break;
                    }
                case "Button_forward":
                    {
                        Debug.Log("����!");
                        waitTime = 1f;
                        go_forward = true;
                        break;
                    }
                default:
                    {
                        waitTime = 0f;
                        go_forward = false;
                        turn_right = false;
                        turn_left = false;
                        turnDegree = 0;
                        break;
                    }
            }
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
                GameObject.Find("Character").transform.Rotate(0, -2, 0);
                turnDegree += 2;
            }
        }

        /* ĳ���� �տ� ������ �ִ��� Ȯ���ϴ°� ����
        directionToTarget = transform.position - GameObject.Find("Pref_TreeCutted (2)").transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        float distance = directionToTarget.magnitude;

        if (distance < 10)
            Debug.Log("target is in front of me");
        */
    }

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, MaxDistance))
        {
            //Debug.Log("There is SnowBall in front of the object");
            //if (hit.transform.tag.Equals("SnowBall"))
            hit.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            hitTag = hit.transform.name;
        }
        else
        Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 10000f);
    }
}
