using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    public GameObject selectBlock;
    public Text blockText;          // (tg) �� ������Ʈ ���� Text�� ����
    public int listId = 0;          // (tg) �� id

    // ht �� ��ġ ������ ����
    public float posX = 0f;
    public float posY = 0f;

    public class Block
    {
        internal Block prev;
        internal Block next;
        internal string direction;
        public Block()                          // (tg) �⺻������
        {
            direction = null;
            prev = null;
            next = null;
        }
        public Block(string state)              // (tg) head, tail ������
        {
            this.direction = state;
            prev = null;
            next = null;
        }
        public Block(string direction, int num) // (tg) �⺻ �� ������
        {
            this.direction = direction + ':' + num.ToString();
            prev = null;
            next = null;
        }
    }

    // (tg) �ݺ��� ��(������ �� ����)
    public class ForBlock : Block
    {
        int turn;
    }
    // (tg) ���ǹ� ��(������ �� ����)
    public class IfBlock : Block
    {
        string condition;
    }

    // (tg) ����Ʈ head �� tail
    Block head, tail;

    // (tg) ����Ʈ �ʱ�ȭ
    public void initlist()
    {
        //Debug.Log("Init list");
        head = new Block("head");
        tail = new Block("tail");
        head.next = tail;
        head.prev = head;
        //tail.next = tail;   head�� �ڱ� ������ �ǵ絥 tail�� ��?..
        tail.prev = head;
    }
    // (tg) ���� ���� ���̸� true �ƴϸ� false ��ȯ
    public bool isTail(Block b)
    {
        return b.direction.Equals("tail");
    }
    // (tg) �Ѱܹ��� id������ �� Ž��
    public Block getNode(string num)
    {
        Block search = head.next;
        if (isTail(search))
        {
            Debug.Log("List is empty");
        }
        else
        {
            while (!isTail(search))
            {
                string[] result = search.direction.Split(':');
                if (result[1].Equals(num))  // (tg) ���� ã���� �ݺ� ����
                    break;
                search = search.next;
            }
        }
        return search;
    }
    // (tg) Ŭ���� ���� �����ؼ� ����Ʈ �� �տ� ����
    // ht �� Ŭ���ϸ� ����
    public int insertFront(string direction)
    {
        //direction = "test node";                   // (tg) �׽�Ʈ��
        listId++;
        Block newBlock = new Block(direction, listId);
        newBlock.prev = head;
        newBlock.next = head.next;
        head.next.prev = newBlock;
        head.next = newBlock;
        return 1;
    }
    /* (tg) �߰�����     �巡�� �� ������� �� � ���� ���̿� ������ �� �� �־�� ��
     * public int insert()
    {
        string direction = blockText.text;

        listId++;
        Block newBlock = new Block(direction, listId);

    }
    */
    // (tg) Ŭ���� ���� �����ؼ� ����Ʈ �� �ڿ� ����
    public void insertLast(string direction)
    {
        //direction = "test node";       // (tg) �׽�Ʈ��
        listId++;
        // ht Debug.Log("Here i am");
        Block newBlock = new Block(direction, listId);
        newBlock.prev = tail.prev;
        newBlock.next = tail;
        tail.prev.next = newBlock;
        tail.prev = newBlock;

        // ht ���Ḯ��Ʈ�� ��尡 �߰��Ǿ����Ƿ� ������ �ִ� �� ����� ���� ������
        deleteBlocks("Block");
        showBlocks();
    }
    // (tg) �Ѱܹ��� id������ �� ���� (���� �ʿ�)
    public void deleteNode(string num)
    {
        //string[] result = blockText.text.Split(':'); (tg)
        //string deleteNum = result[1]; (tg)
        Block delete = getNode(num);
        if (delete == null)
            Debug.Log("Can't find that block");
        else
        {
            Debug.Log("Delete node");
            //Debug.Log("Delete node's prev " + delete.prev.direction); // (tg) �׽�Ʈ��
            //Debug.Log("Delete node's next " + delete.next.direction); // (tg) �׽�Ʈ��
            delete.prev.next = delete.next;
            delete.next.prev = delete.prev;
            delete = null;
        }

        // ht ���Ḯ��Ʈ���� ��尡 �����Ǿ����Ƿ� ������ �ִ� �� ����� ���� ������
        deleteBlocks("Block");
        showBlocks();
    }
    // (tg) �ֿܼ� ����Ʈ ���
    public void printList()
    {
        Block print = head.next;
        while (!isTail(print))
        {
            Debug.Log(print.direction);
            print = print.next;
        }
    }
    // (tg) ����Ȯ�� �Լ�
    public void verifiAlgorithm()
    {
        for (int i = 1; i < 9; i++)
        {
            Debug.Log(i + "��� ����");
            insertLast("test node");
        }
        printList();
        for (int i = 3; i < 5; i++)
            deleteNode(i.ToString());
        printList();
    }
    
    // ht �� ����
    public void createForwardBlock()
    {
        GameObject newObj = new GameObject();
        newObj.name = "ForwardBlock";
        newObj.AddComponent<CanvasRenderer>();
        newObj.AddComponent<Button>();
        newObj.AddComponent<Image>();

        // Tag�� Block���� ����
        newObj.gameObject.tag = "Block";

        // �̹��� ����
        newObj.GetComponent<Image>().sprite = Resources.Load("images/Forward", typeof(Sprite)) as Sprite;

        // Image�� ���̵��� �θ� Panel�� ���� 
        newObj.transform.SetParent(GameObject.Find("CodePanel").transform);

        // ������ġ�� �»������ ����
        newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform�� PosX, PosY, PosZ ���� ���
        newObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
        posY -= 100f; // ���� �� y��ǥ ����
        //Debug.Log("Created");

    }
    // ht ��ȸ�� �� ����
    public void createLeftBlock()
    {
        GameObject newObj = new GameObject();
        newObj.name = "LeftBlock";
        newObj.AddComponent<CanvasRenderer>();
        newObj.AddComponent<Button>();
        newObj.AddComponent<Image>();

        // Tag�� Block���� ����
        newObj.gameObject.tag = "Block";

        // �̹��� ����
        newObj.GetComponent<Image>().sprite = Resources.Load("images/Left", typeof(Sprite)) as Sprite;

        // Image�� ���̵��� �θ� Panel�� ���� 
        newObj.transform.SetParent(GameObject.Find("CodePanel").transform);

        // ������ġ�� �»������ ����
        newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform�� PosX, PosY, PosZ ���� ���
        newObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
        posY -= 100f; // ���� �� y��ǥ ����
        //Debug.Log("Created");

    }
    // ht ��ȸ�� �� ����
    public void createRightBlock()
    {
        GameObject newObj = new GameObject();
        newObj.name = "RightBlock";
        newObj.AddComponent<CanvasRenderer>();
        newObj.AddComponent<Button>();
        newObj.AddComponent<Image>();

        // Tag�� Block���� ����
        newObj.gameObject.tag = "Block";

        // �̹��� ����
        newObj.GetComponent<Image>().sprite = Resources.Load("images/Right", typeof(Sprite)) as Sprite;

        // Image�� ���̵��� �θ� Panel�� ���� 
        newObj.transform.SetParent(GameObject.Find("CodePanel").transform);

        // ������ġ�� �»������ ����
        newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform�� PosX, PosY, PosZ ���� ���
        newObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
        posY -= 100f; // ���� �� y��ǥ ����
        //Debug.Log("Created");

    }
    // ht ������� ���� ����
    public void deleteBlocks(string target)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(target);
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
            //Debug.Log("Deleted");
        }
    }


    // ht ���Ḯ��Ʈ ��带 �޾Ƽ� ȭ�鿡 ���
    public void showBlocks()
    {
        // head ���� tail���� ���鼭 ������
        Block LL = head.next;

        // �� ���� ������ġ �ʱ�ȭ
        posY = 0f;
        while (!isTail(LL))
        {
            // �� ��尡 � ������ �Ǵ��� �ش� �� �����ϴ� �Լ� ����
            string[] direction_temp = LL.direction.Split(':');
            if (direction_temp[0].Equals("forward"))
            {
                createForwardBlock(); // ���� ���� ��� (�ϴ� ��� ��尡 �������̶�� ����)
            }
            else if (direction_temp[0].Equals("left"))
            {
                createLeftBlock(); // ��ȸ�� ���� ���

            }
            else if (direction_temp[0].Equals("right"))
            {
                createRightBlock(); // ��ȸ�� ���� ���
            }
            LL = LL.next;
        }

        // ������� ���� ����
        //deleteBlocks("Block");
    }
    
    void Start()
    {
        initlist();
        // ���� Ȯ���� ���� ��ŸƮ �Լ����� ����
        //verifiAlgorithm();

        // ���Ḯ��Ʈ�� ���ŵɶ����� ( = �ڵ����� Ŭ���ɶ� (insert), �ڵ����� ���� ������ �� (delete))
        // showBlocks() ����


    }

    void Update()
    {
    }

    /*
        public class BlockList
        {
            Block head;

            public void CreateList()
            {
                head = new Block();
            }
            /*
            public Block getBlock(int id)
            {
                Block search = head;
                while (search != null && search.id != id)
                {
                    search = search.next;
                }
                if (search != null)
                    return search;
                else
                    return null;
            }

            public int insertFront(string direction)
            {
                Block newBlock = new Block(direction);
                head = newBlock;
                return 1;
            }
            public int insert(string direction)
            {
                Block search = head;
                Block newBlock = new Block(direction);

                while (search.next != null)
                {
                    search = search.next;
                }
                search.next = newBlock;
                return 1;
            }
            public int insertAfter(int prevId, string direction)
            {
                int success = 0;
                if (head.next == null)
                    insertFront(direction);
                else
                {
                    Block newBlock = new Block(direction);
                    Block prev = getBlock(prevId);
                    if (prev != null)
                    {
                        newBlock.next = prev.next;
                        prev.next = newBlock;
                        success = 1;
                    }
                }
                return success;
            }
            public int deleteBlock(int id)
            {
                int success = 0;
                Block prev = null;
                return success;
            }


        }
    */
}
