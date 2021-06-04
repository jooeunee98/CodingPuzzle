using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    public GameObject selectBlock;
    public Text blockText;          // �� ������Ʈ ���� Text�� ����
    public int listId = 0;          // �� id

    // ht �� ��ġ ������ ����
    public float posX = 0f;
    public float posY = 0f;

    public class Block
    {
        internal Block prev;
        internal Block next;
        internal string direction;
        public Block()                          // �⺻������
        {
            direction = null;
            prev = null;
            next = null;
        }
        public Block(string state)              // head, tail ������
        {
            this.direction = state;
            prev = null;
            next = null;
        }
        public Block(string direction, int num) // �⺻ �� ������
        {
            this.direction = direction + ':' + num.ToString();
            prev = null;
            next = null;
        }
    }
    
    // �ݺ��� ��(������ �� ����)
    public class ForBlock : Block
    {
        int turn;
    }
    // ���ǹ� ��(������ �� ����)
    public class IfBlock : Block
    {
        string condition;
    }

    // ����Ʈ head �� tail
    Block head, tail;
    // ����Ʈ �ʱ�ȭ
    public void initlist()
    {
        Debug.Log("Init list");
        head = new Block("head");
        tail = new Block("tail");
        head.next = tail;
        head.prev = head;
        //tail.next = tail;   head�� �ڱ� ������ �ǵ絥 tail�� ��?..
        tail.prev = head;
    }
    // ���� ���� ���̸� true �ƴϸ� false ��ȯ
    public bool isTail(Block b)
    {
        return b.direction.Equals("tail");
    }
    // �Ѱܹ��� id������ �� Ž��
    public Block getBlock(string num)
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
                if (result[1].Equals(num))  // ���� ã���� �ݺ� ����
                    break;
                search = search.next;
            }
        }
        return search;
    }
    // ���ο� ���� �����ؼ� ����Ʈ �� �տ� ����
    // ht �� Ŭ���ϸ� ����
    public int insertFront()
    {
        string direction = blockText.text;
        //string direction = "test node";                   // �׽�Ʈ��
        listId++;
        Block newBlock = new Block(direction, listId);
        newBlock.prev = head;
        newBlock.next = head.next;
        head.next.prev = newBlock;
        head.next = newBlock;
        return 1;
    }
    /* �߰�����     �巡�� �� ������� �� � ���� ���̿� ������ �� �� �־�� ��
     * public int insert()
    {
        string direction = blockText.text;

        listId++;
        Block newBlock = new Block(direction, listId);

    }
    */
    // ���ο� ���� �����ؼ� ����Ʈ �� �ڿ� ����
    public int insertLast()
    {
        string direction = blockText.text;
        //string direction = "test node";       // �׽�Ʈ��
        listId++;
        Block newBlock = new Block(direction, listId);
        newBlock.prev = tail.prev;
        newBlock.next = tail;
        tail.prev.next = newBlock;
        tail.prev = newBlock;

        // ht ���Ḯ��Ʈ�� ��尡 �߰��Ǿ����Ƿ� ������ �ִ� �� ����� ���� ������
        deleteBlocks("Block");
        showBlocks();

        return 1;
    }
    // �Ѱܹ��� id������ �� ����
    public void deleteBlock(string num)
    {
        // ht �����ϱ����� ������ ���� ���° num ���� �˾Ƴ�����
        //string[] result = blockText.text.Split(':');
        //string deleteNum = result[1];
        //Block delete = getBlock(deleteNum); 
        Block delete = getBlock(num);
        // ht �׳� ���⼭ deleteNum ��ſ� num �ѱ�� �ȵǳ�?
        if (delete == null)
            Debug.Log("Can't find that block");
        else
        {
            Debug.Log("Delete node");
            //Debug.Log("Delete node's prev " + delete.prev.direction); // �׽�Ʈ��
            //Debug.Log("Delete node's next " + delete.next.direction); // �׽�Ʈ��
            delete.prev.next = delete.next;
            delete.next.prev = delete.prev;
            delete = null;
        }

        // ht ���Ḯ��Ʈ���� ��尡 �����Ǿ����Ƿ� ������ �ִ� �� ����� ���� ������
        deleteBlocks("Block");
        showBlocks();
    }
    // �ֿܼ� ����Ʈ ���
    public void printBlock()
    {
        Block print = head.next;
        while (!isTail(print))
        {
            Debug.Log(print.direction);
            print = print.next;
        }

        
    }
    // ����Ȯ�� �Լ�
    public void verifiAlgorithm()
    {
        for (int i = 1; i < 9; i++)
        {
            Debug.Log(i + "��� ����");
            insertLast();
        }
        printBlock();
        for (int i = 3; i < 5; i++)
            deleteBlock(i.ToString());
        printBlock();
    }

    // ht �� ����
    public void createBlock()
    {
        GameObject newObj = new GameObject();
        newObj.name = "Block";
        newObj.AddComponent<CanvasRenderer>();
        newObj.AddComponent<Button>();
        newObj.AddComponent<Image>();

        // Tag�� Block���� ����
        newObj.gameObject.tag = "Block";

        // �̹��� ����
        newObj.GetComponent<Image>().sprite = Resources.Load("images/forward", typeof(Sprite)) as Sprite;

        // Image�� ���̵��� �θ� Panel�� ���� 
        newObj.transform.SetParent(GameObject.Find("Panel").transform);

        // ������ġ�� �»������ ����
        newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform�� PosX, PosY, PosZ ���� ���
        newObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
        posY -= 100f; // ���� �� y��ǥ ����
        Debug.Log("Created");

    }

    // ht ������� ���� ����
    public void deleteBlocks(string target)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(target);
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
            Debug.Log("Deleted");
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
            createBlock();
            LL = LL.next;
        }
        
        // ������� ���� ����
        //deleteBlocks("Block");
    }

    void Start()
    {
        initlist();
        // ���� Ȯ���� ���� ��ŸƮ �Լ����� ����
        verifiAlgorithm();

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
