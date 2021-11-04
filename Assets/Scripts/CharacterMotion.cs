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
        ((BlockSystem.LoopBlock)blockStack.peek()).setLoopNum(2);
    }
    print = blockStack.pop().right; 
*/

public class CharacterMotion : MonoBehaviour
{
    string beforeAction;
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
    bool roll_snow = false;
    public static string hitTag;
    float stRotationY;
    int turnDegree = 0;

    void Start()
    {
        hitTag = "Noting";
    }

    public void Coru()
    {
        animator = GameObject.Find("Character").GetComponent<Animator>();
        coroutine = readCommand(1.0f);
        StartCoroutine(coroutine);
    }

    public void setAnimator(string beforeAction, string kindOf)
    {
        if (beforeAction == "Button_left" || beforeAction == "Button_right" || beforeAction == "Button_forward")
        {
            if (kindOf == "Button_left" || kindOf == "Button_right" || kindOf == "Button_forward")
            {

            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Push", false);
                animator.SetBool("Plant Tree", false);
                animator.SetBool("Picking up", false);
                animator.SetBool("Success", false);
                animator.SetBool("Fail", false);
            }
        } else if (beforeAction == "Button_push")
        {
            if (kindOf == "Button_push")
            {

            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Push", false);
                animator.SetBool("Plant Tree", false);
                animator.SetBool("Picking up", false);
                animator.SetBool("Success", false);
                animator.SetBool("Fail", false);
            }
        }
        /*else if (beforeAction == "Button_push")
        {
            if (kindOf == "Button_left" || kindOf == "Button_right" || kindOf == "Button_forward")
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", true);
                animator.SetBool("Push", false);
                animator.SetBool("Plant Tree", false);
                animator.SetBool("Picking up", false);
                animator.SetBool("Success", false);
                animator.SetBool("Fail", false);
            }
            else
            {

            }
        }*/
        else
        {

        }
    }

    public IEnumerator readCommand(float waitTime)
    {
        // loopBlock�� ������ ���ÿ� Ǫ�� & �ݺ� Ƚ�� Ȯ��
        // �ݺ� Ƚ����ŭ �ش� ����Ʈ ���
        // �ݺ� Ƚ���� 0�� �����ϸ�, ���ÿ��� ��
        string kindOf = null;
        string numResource = "Images/Ŭ����ѹ�Ÿ��Ʋ";
        int num = 0;
        BlockSystem call = GameObject.Find("BlockSystem").GetComponent<BlockSystem>();
        BlockSystem.Block print = call.getRoot();
        BlockSystem.BStack blockStack = new BlockSystem.BStack();

        blockStack.push(print);
        while (!blockStack.isEmpty())
        {
            beforeAction = kindOf;
            kindOf = print.getInfo().Split(':')[0];
            // Animator ����
            setAnimator(beforeAction, kindOf);

            if (kindOf.Equals("Button_loop"))
            {
                blockStack.push(print); // loop or if���� ��� ���ÿ� �����ؼ� üũ����Ʈ ����
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
                        ((BlockSystem.LoopBlock)blockStack.peek()).setLoopNum(2);
                    }
                    print = blockStack.pop().right;
                    num = 0;
                }
            }
            else if (kindOf.Equals("Button_if"))
            {
                blockStack.push(print); // loop or if���� ��� ���ÿ� �����ؼ� üũ����Ʈ ����

                if (((BlockSystem.IfBlock)print).Validation(hitTag))
                    print = blockStack.peek().left;
                else
                {
                    print = blockStack.pop();
                    ((BlockSystem.IfBlock)print).setWorkOut();
                    print = print.right;
                }
            }
            // ���� ��尡 leaf or subLeaf�̸� �ܸ������� ������ ���̹Ƿ� ���ÿ� ����� üũ����Ʈ�� ���ư�
            else if (call.isLeaf(print))
            {
                // subLeaf�� ���� �ݺ����� 1ȸ ������ ���̹Ƿ�, �ݺ��� Ƚ�� ����
                if (blockStack.peek().getInfo().Split(':')[0].Equals("Button_loop"))
                {
                    BlockSystem.Block temp = blockStack.peek();
                    //GameObject parent = GameObject.Find(temp.getInfo()).gameObject;
                    GameObject child = GameObject.Find(temp.getInfo()).gameObject.transform.Find("Image_loopNum").gameObject;
                    child.GetComponent<Image>().sprite = Resources.Load(numResource + ((BlockSystem.LoopBlock)temp).getLoopNum(), typeof(Sprite)) as Sprite;
                }
                print = blockStack.pop();

                //Debug.Log(blockStack.peek().getInfo());
            }
            // �� �ܴ� �⺻ ���̹Ƿ� ����� ���� ��� ����
            else
            {
                print = print.right;
            }
            Debug.Log("print info : " + print.getInfo());
            switch (kindOf)
            {
                case "Button_left":
                    {
                        Debug.Log("��ȸ��!");
                        go_forward = false;
                        turn_left = true;
                        turn_right = false;
                        roll_snow = false;
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
                        roll_snow = false;
                        turnDegree = 0;
                        // ���°� �� �Ȱɸ��ϱ� waitTime -> 1 �� ����
                        waitTime = 1f;
                        break;
                    }
                case "Button_forward":
                    {
                        Debug.Log("����!");
                        animator.SetBool("Walking", true); 
                        waitTime = 1f;
                        go_forward = true;
                        break;
                    }
                case "Button_push":
                    {
                        Debug.Log("������!");
                        animator.SetBool("Push", true);
                        waitTime = 1f;
                        go_forward = false;
                        if (hitTag == "SnowBall")
                        {
                            roll_snow = true;
                        }
                        //GameObject.Find("Character").transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                        break;
                    }
                case "Button_fruit":
                    {
                        Debug.Log("�����ݱ�");
                        waitTime = 1;
                        go_forward = false;
                        CollisionDetector.pickUp = true;
                        break;
                    }
                case "Button_plant":
                    {
                        Debug.Log("�����ɱ�");
                        waitTime = 1f;
                        go_forward = false;
                        CollisionDetector.delCuttedTree = true;
                        GameObject prefab = Resources.Load("Prefabs/Pref_Tree_02") as GameObject;
                        GameObject tree = Instantiate(prefab) as GameObject;
                        tree.gameObject.name = "PlantedTree";
                        tree.transform.SetParent(GameObject.Find("testmap").transform);
                        tree.transform.localPosition = GameObject.Find("Character").transform.localPosition;
                        tree.GetComponent<BoxCollider>().isTrigger = true;
                        tree.GetComponent<Transform>().localScale = new Vector3(3, 3, 3);
                        break;
                    }
                default:
                    {
                        waitTime = 0f;
                        go_forward = false;
                        turn_right = false;
                        turn_left = false;
                        roll_snow = false;
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
        else if (roll_snow == true)
        {
            animator.SetBool("Push", true);
            GameObject.Find("Character").transform.Translate(GameObject.Find("Character").transform.localRotation * Vector3.forward * Time.deltaTime * 5.75f, Space.World);
            GameObject.Find("Pref_SnowBall").transform.Translate(GameObject.Find("Character").transform.localRotation * Vector3.forward * Time.deltaTime * 5.75f, Space.World);
            GameObject.Find("Pref_SnowBall").transform.transform.Rotate(Vector3.right * 130.0f * Time.deltaTime);

        }
    }
}
