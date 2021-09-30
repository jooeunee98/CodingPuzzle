using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    (tg)
    ���Ḯ��Ʈ�� ���õ� ��Ҵ� '���'�� Ī�ϰ�
    ǥ�������� �巯���� �ڵ������� '����'���� ǥ��
*/

public class BlockManager : MonoBehaviour
{
    public int listId = 0;          // (tg) ���� id
    public int loopNum = 0;            // (tg) �ݺ��� �ݺ� Ƚ��
    // ht ���� ��ġ ������ ����
    public float posX = 0f;
    public float posY = 0f;
    public float posZ = 0f;
    
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
        public Block(string direction, int num) // (tg) �⺻ ���� ������
        {
            this.direction = direction + ':' + num.ToString();
            prev = null;
            next = null;
        }
    }

    // (tg) �ݺ��� ����(������ �� ����)
    public class ForBlock : Block
    {
        internal int turn;
        internal Block loop;

        public ForBlock() : base()
        {
            turn = 0;
            loop = null;
        }
        public ForBlock(int turn) : base("ForBlock")
        {
            this.turn = turn;
            loop = null;
        }
    }
    // (tg) ���ǹ� ����(������ �� ����)
    public class IfBlock : Block
    {
        string condition;
    }
    
    // (tg) ����Ʈ head �� tail, middle(����Ʈ �߰��� ������ ��ġ�� ����Ű�� ������)
    public Block head, tail, middle;

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
        middle = null;
    }
    
    // (tg) ���� ������ ���̸� true �ƴϸ� false ��ȯ
    public bool isTail(Block b)
    {
        return b.direction.Equals("tail");
    }
    // (tg) �Ѱܹ��� id������ ���� Ž��
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
                if (result[1].Equals(num))  // (tg) ������ ã���� �ݺ� ����
                    break;
                search = search.next;
            }
        }
        return search;
    }
    // (tg) Ŭ���� ������ �����ؼ� ����Ʈ �� �տ� ����
    // ht ���� Ŭ���ϸ� ����
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
    // (tg) �߰�����
    public void setMiddle(string prevNode)
    {
        string idValue = prevNode.Split(':')[1];
        // middle�� ����Ʈ �߰� ��ġ ����
        middle = getNode(idValue);
    }
    // (tg) Ŭ���� ������ �����ؼ� ����Ʈ �� �ڿ� ����
    public void insertLast(string direction)
    {
        //direction = "test node";       // (tg) �׽�Ʈ��
        // ht Debug.Log("Here i am");
        listId++;                                       // id�� ����
        Block newBlock = null;

        if (direction.Equals("ForBlock"))
            newBlock = new ForBlock(loopNum);
        else if (direction.Equals("IfBlock"))
            newBlock = new IfBlock();
        else
            newBlock = new Block(direction, listId);  // ��� ����
        // middle���� null �ƴ϶��, ����Ʈ �߰��� insert�� ����
        if (middle != null)
        {
            newBlock.prev = middle;
            newBlock.next = middle.next;
            middle.next.prev = newBlock;
            middle.next = newBlock;
            middle = null;
        }
        else
        {
            newBlock.prev = tail.prev;                      // ����
            newBlock.next = tail;
            tail.prev.next = newBlock;
            tail.prev = newBlock;
        }
        // ht ���Ḯ��Ʈ�� ��尡 �߰��Ǿ����Ƿ� ������ �ִ� �� ����� ���� ������
        deleteBlocks("Block");
        showBlocks();
    }
    
    // (tg) �Ѱܹ��� id������ ���� ���� (���� �ʿ�)
    public void deleteNode(string blockName)
    {
        string[] result = blockName.Split(':'); //(tg)
        string deleteNum = result[1]; //(tg)
        Block delete = getNode(result[1]);
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
    // (tg) ���Ḯ��Ʈ ����Ȯ�� �Լ�
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

    // ht ������� ������ ����
    public void deleteBlocks(string target)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(target);
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
            //Debug.Log("Deleted");
        }
    }

    // (tg) ���ϻ��� �Լ� �ڵ� �ߺ��� ���� ���ؼ� �ϳ��� ����
    public void createBlock(string block)
    {
        string whatis = block.Split(':')[0];            // Ŭ���� ������ �������� �Ǵ�
        string listId = block.Split(':')[1];            // Ŭ���� �ҷ��� id�� ���� (���� ���� �� �� ���� �������� �˾ƾ� �ؼ�)
        string prefResource;                            // ������ ������ ���ҽ�
        string designObjName;                           // Ŭ���� ������ �̸�
        string imgResource;                             // �����鿡 ���� �̹��� �̸�
                
        // �̸� �� �̹��� ����
        if (whatis.Equals("Button_left"))       // ��ȸ�� ��ư�̸�
        {
            designObjName = "LeftBlock:" + listId;
            imgResource = "images/Left";
            prefResource = "Prefabs/Button_left";
        }
        else if (whatis.Equals("Button_forward"))       // ���� ��ư�̸�
        {
            designObjName = "ForwardBlock:" + listId;
            imgResource = "images/forward";
            prefResource = "Prefabs/Button_forward";
        }
        else
        {   // �� �� �ƴϸ� ��ȸ�� ��ư�̹Ƿ�
            designObjName = "RightBlock:" + listId;
            imgResource = "images/Right";
            prefResource = "Prefabs/Button_right";
        }

        // ������ ȣ��
        GameObject prefab = Resources.Load(prefResource) as GameObject;
        // ������(�ڵ���)�� ������ ������ �ν��Ͻ�ȭ
        GameObject newObj = Instantiate(prefab) as GameObject;
        newObj.name = designObjName;
        newObj.GetComponent<Image>().sprite = Resources.Load(imgResource, typeof(Sprite)) as Sprite;

        // Tag�� Block���� ����
        newObj.gameObject.tag = "Block";

        // Image�� ���̵��� �θ� Panel�� ���� 
        newObj.transform.SetParent(GameObject.Find("codeCanvas").transform);

        // ������ġ�� �»������ ����
        //newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        //newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        //newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform�� PosX, PosY, PosZ ���� ���
        //posY += 438f;
        newObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(posX, posY, posZ);
        //posY -= 438f;

        //Debug.Log(posX + " " + posY + " " + posZ);
        posY -= 100f; // ���� ���� y��ǥ ����
                      // ht ���Ḯ��Ʈ ��带 �޾Ƽ� ȭ�鿡 ���
    }
    public void showBlocks()
    {
        // head ���� tail���� ���鼭 ��������
        Block LL = head.next;
        // ���� ���� ������ġ �ʱ�ȭ
        posY = 0f;
        while (!isTail(LL))
        {
            // (tg) �ڵ� �ߺ��� �ּ�ȭ �ϱ� ���ؼ� �Ʒ��� �ڵ带 �Լ� �ϳ��� ó��
            // �� ��尡 � �������� �Ǵ��� �ش� ���� �����ϴ� �Լ� ����
            createBlock(LL.direction);
            LL = LL.next;
        }
        // (tg) ������� ������ ����
        //deleteBlocks("Block");
    }

    public void characterMove()
    {
        Block LL = head.next;
        while (!isTail(LL))
        {
            string whatis = LL.direction.Split(':')[0]; // Ŭ���� ������ �������� �Ǵ�
            if (whatis.Equals("Button_left"))       // ��ȸ�� ��ư�̸�
            {
                GameObject Character = GameObject.FindWithTag("Character");
            }
            else if (whatis.Equals("Button_forward"))       // ���� ��ư�̸�
            {
                
            }
            else
            {   // �� �� �ƴϸ� ��ȸ�� ��ư�̹Ƿ�
                
            }
            LL = LL.next;
        }
    }

    void Start()
    {
        int turn = 0;
        initlist();
        //posX += 389.5f;
        //posZ -= 379f;
        // (tg) ���� Ȯ���� ���� ��ŸƮ �Լ����� ����
        //verifiAlgorithm();
    }

    void Update()
    {
    }

    /*  (tg) ���� ���Ḯ��Ʈ �ڵ�
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