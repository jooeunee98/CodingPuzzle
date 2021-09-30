using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        // loopBlock�� ������ ���ÿ� Ǫ�� & �ݺ� Ƚ�� Ȯ��
        // �ݺ� Ƚ����ŭ �ش� ����Ʈ ���
        // �ݺ� Ƚ���� 0�� �����ϸ�, ���ÿ��� ��
        string kindOf = null;
        string numResource = "Images/Image_stage_";
        int num = 0;
        BlockSystemTest call = GameObject.Find("BlockSystemTest").GetComponent<BlockSystemTest>();
        BlockSystemTest.Block print = call.getRoot();
        BlockSystemTest.BStack blockStack = new BlockSystemTest.BStack();

        blockStack.push(print);
        while (!blockStack.isEmpty())
        {
            kindOf = print.getInfo().Split(':')[0];
            // loop or if���� ��� ���ÿ� �����ؼ� üũ����Ʈ ����
            if (kindOf.Equals("Button_loop") || kindOf.Equals("Button_if"))
            {
                blockStack.push(print);
                // �ݺ��� ��ȿ�� ��
                if (call.testValidate(print))
                {
                    // true�̸� �ݺ��� ���� �� ���
                    print = blockStack.peek().left;
                    num++;
                }
                else
                {
                    // �ƴϸ� �ݺ����� �ǳʶ�
                    print = blockStack.pop().right;
                    num = 0;
                }
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
                    Debug.Log("**numResource** : " + numResource);
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
    }
}
