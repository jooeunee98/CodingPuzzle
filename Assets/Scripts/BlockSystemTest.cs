using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ����Ʈ�� ���õ� ��Ҵ� '���'�� Ī�ϰ�
 * ǥ�������� �巯���� �ڵ������� '����'���� ǥ��
 * 
 * �⺻������ ������ ���� root ��带 �������� right �����Ϳ� ������� ��Ģ���� ��
 * ���������� loop �� if ���� left �����Ϳ� subRoot ��带 �����ؼ� subRoot�� right �����Ϳ� ������
 * 
 * ���� ��Ģ�� ���� ������ �Ʒ��� ���·� �����
 * 
 * root - forward - left - loop(3) - forward - leaf
 *                           |-> subRoot - left - left - forward - subLeaf
 * 
 * root - leaf�� ����Ǵ� ����Ʈ�� main list��� �ϰ�
 * �Ʒ��� �б��Ͽ� ����Ǵ� �������� sub list��� ������
 * 
 * �ݺ����� ��� �ݺ�Ƚ��(loopNum)�� 1 �̻��� ���� �б��ؼ� ����
 * ���ǹ��� ��� �������� �־��� ���� ��/������ �Ǻ�, ���̸� �б��ؼ� ����
 * �̵��� ���� ������ ��ȿ���� �Ǵ��� �����ϴ� �޼ҵ带 Ȱ��
 * 
 * setCheckPoint() ���� ���
 * 
 * 
 */

public class BlockSystemTest : MonoBehaviour
{
    int listId = 0;             // ������ �����ϴ� id��
    //public int setLoopNum;
    public int initLoopNum;     // �ݺ����� loopNum �⺻��
    BStack blockStack;          // �б����� �����ϴ� ����
    Block checkPoint = null;    // ���õ� �б��� ����
    Block root, prev, leaf;
    // ht ���� ��ġ ������ ����
    bool moveX = false;
    public float posX = 0f;
    public float posY = 0f;
    public float posZ = 0f;

    public class BStack
    {
        Block[] array;
        int top;

        public BStack()
        {
            array = new Block[10];
            top = -1;
        }
        public bool isFull()
        {
            return top >= 9 ? true : false;
        }
        public bool isEmpty()
        {
            return top <= -1 ? true : false;
        }
        public void push(Block newBlock)
        {
            if (isFull())
            {
                Debug.Log("BStack is full");
            }
            else
            {
                array[++top] = newBlock;
            }
            //Debug.Log("Push : " + array[top].getInfo());

        }
        public Block pop()
        {
            if (isEmpty())
            {
                Debug.Log("BStack is Empty");
                return null;
            }
            //Debug.Log("Pop : " + array[top].getInfo());
            return array[top--];
        }
        public Block peek()
        {
            return array[top];
        }
    }

    public class Block
    {
        internal Block left;
        internal Block right;
        string info;

        public Block()
        {
            left = right = null;
            info = null;
        }
        public Block(string info)
        {
            left = right = null;
            this.info = info;
        }
        public Block(string info, int id)
        {
            left = right = null;
            this.info = info + ':' + id.ToString();
        }
        public string getInfo()
        {
            return info;
        }
        public bool isEmpty()
        {
            return (this.left == null && this.right == null) ? true : false;
        }
    }

    public class LoopBlock : Block
    {
        private int loopNum;

        public LoopBlock() : base()
        {
            this.loopNum = 0;
            Block subRoot = new Block("subRoot");
            Block subLeaf = new Block("subLeaf");
            base.left = subRoot;
            subRoot.right = subLeaf;
        }
        public LoopBlock(int loopNum, int num) : base("Button_loop", num)
        {
            this.loopNum = loopNum;
            Block subRoot = new Block("subRoot");
            Block subLeaf = new Block("subLeaf");
            base.left = subRoot;
            subRoot.right = subLeaf;
        }
        public int getLoopNum()
        {
            return this.loopNum;
        }
        public void setLoopNum(int num)
        {
            this.loopNum = num;
        }
        public void deductNum()
        {
            this.loopNum--;
        }
    }

    public class IfBlock : Block
    {
        private string condition;

        public IfBlock() : base()
        { }
        public IfBlock(int num) : base("ifBLock", num)
        { }
        public void setCondition(string condition)
        {
            this.condition = condition;
        }
        public string getCondition()
        {
            return condition;
        }
    }

    public Block getRoot()
    {
        return root;
    }

    // Tree �ʱ�ȭ
    public void initTree()
    {
        root = new Block("root");
        leaf = new Block("leaf");
        root.right = leaf;
        checkPoint = root;
        Debug.Log("Tree initialize");
    }

    // �÷��̾ ������ ������ �����ϰ� ��ȯ
    public Block createNode(string bInfo)
    {
        listId++;
        Block newBlock = null;
        switch (bInfo)
        {
            case "Button_loop":
                // loopNum �ʱ�ȭ�ϴ� ��� �����ϱ�
                newBlock = new LoopBlock(initLoopNum, listId);
                Debug.Log("initLoopNum : " + ((LoopBlock)newBlock).getLoopNum().ToString());
                break;
            case "Button_if":
                newBlock = new IfBlock();
                break;
            default:
                newBlock = new Block(bInfo, listId);
                break;
        }
        Debug.Log("Create Node : " + newBlock.getInfo());
        return newBlock;
    }

    // ���� ��尡 leaf���� subLeaf ���� �Ǵ�
    // mode == true : ȭ�� �� ������ ����ϴ� showBlocks()���� ���
    // mode == false : ����Ʈ�� ��带 �����ϴ� insertNode()���� ���
    public bool isLeaf(Block bInfo, bool mode = false)
    {
        bool itis = false;
        if (mode)
        {
            if (bInfo == leaf || bInfo == null)
            {
                itis = true;
            }
        }
        else
        {
            if (bInfo == leaf || bInfo.getInfo().Equals("subLeaf"))
            {
                itis = true;
            }
        }
            return itis;
    }

    // ��带 ������ ��ġ�� Ž��
    public Block getInsertPos(Block block)
    {
        while (!isLeaf(block.right))
            block = block.right;
        return block;
    }

    // mode false : ���� ���õ� ��� Ž���� ��ȯ
    // mode true : ���� ���õ� ����� ���� ��� Ž���� ��ȯ(���� ���Ḯ��Ʈ�̱� ������ ���� ��尪 �ʿ�)
    public Block findNode(string bInfo, bool mode = false)
    {
        Debug.Log("Find some node about : " + bInfo);
        Block search = null;
        Block searchPrev = null;
        BStack s1 = new BStack();
        BStack s2 = new BStack();

        s1.push(root);
        s2.push(root);

        while (!s1.isEmpty())
        {
            search = s1.pop();
            searchPrev = s2.pop();
            //Debug.Log(search.getInfo() + " befor entering if statment");
            if (search.getInfo().Equals(bInfo))
            {
                break;
            }
            if (!isLeaf(search.right))
            {
                s1.push(search.right);
                s2.push(search);
            }
            if (search.left != null)
            {
                s1.push(search.left);
                s2.push(search);
            }
            //Debug.Log("turn : " + ++count);
        }
        if (mode == false)
        {
            Debug.Log("Found node : " + search.getInfo());
            return search;
        }
        else
        {
            Debug.Log("Found previous node: " + searchPrev.getInfo());
            return searchPrev;
        }
    }

    // ��带 �߰��� �����ϱ� ���� ���� ����� ��ġ�� �˾Ƴ�
    public void setPrevNode(string bInfo)
    {
        Debug.Log("Set previous node");
        prev = findNode(bInfo);
    }

    // �б����� �������� ������ �ݺ���/���ǹ��� �������� ��
    // ���� ���� ������ ���ؼ� üũ ����Ʈ�� ������
    public void setCheckPoint(string bInfo)
    {
        string kindOf = checkPoint.getInfo();
        Debug.Log("Set check point at " + bInfo);
        checkPoint = findNode(bInfo);

        // �б��� ����
        if (kindOf.Equals("root"))
        {
            Debug.Log("bInfo's left : " + checkPoint.left.getInfo());
            checkPoint = checkPoint.left;
        }
        // �б��� �ʱ�ȭ
        else
            checkPoint = root;
        //if (checkPoint == null)
        //    checkPoint = findNode(bInfo);
        //else
        //    checkPoint = null;
    }

    // ��� ����
    public void insertNode(string bInfo)
    {
        Block newBlock = createNode(bInfo);
        Block insertPos = null;

        // ������ �߰������� ���Ѵٸ�
        if (prev != null)
        {
            newBlock.right = prev.right;
            prev.right = newBlock;
            prev = null;
        }
        // ������ ������ ���� ���� �����ϱ� ���Ѵٸ�
        else
        {
            // checkPoint ���� root�̸� main list ���� ����
            if (checkPoint.getInfo().Equals("root"))
            {
                insertPos = getInsertPos(root);
            }
            // checkPoint ���� root�� �ƴϸ� sub list ���� ����
            else
            {
                insertPos = getInsertPos(checkPoint);
            }
            newBlock.right = insertPos.right;
            insertPos.right = newBlock;
        }

        deleteBlocks("Block");
        showBlocks();
    }

    // ���õ� ��� ����
    public void deleteNode(string bInfo)
    {
        Block delPrev = findNode(bInfo, true);
        Debug.Log("Previous node of delete : " + delPrev.getInfo());
        Block delete = findNode(bInfo);
        Debug.Log("Delete node : " + delete.getInfo());

        if (isLeaf(delete))
            Debug.Log("A block that does not exist.");
        else
        {
            Debug.Log("Delete node");
            delPrev.right = delete.right;
        }

        // ht ����Ʈ���� ��尡 �����Ǿ����Ƿ� ������ �ִ� �������� ����� ���� ������
        deleteBlocks("Block");
        showBlocks();
    }

    // ��� ��� ����
    public void deleteAll()
    {
        listId = 0;
        deleteBlocks("Block");
        root.right = leaf;
        checkPoint = root;
        Debug.Log("Reset tree");
    }

    // �ݺ����� loopNum ���� Ȯ���ؼ� �ݺ��� �� �ִ��� �ƴ��� Ȯ��
    public bool loopValidate(Block bInfo, int num)
    {
        LoopBlock loopBlock = (LoopBlock)bInfo;
        int loopNum = loopBlock.getLoopNum();
        if (loopNum - num> 0)
        {
            Debug.Log(loopNum - num);
            //loopBlock.setLoopNum(--loopNum);
            return true;
        }
        return false;
    }

    public void printList()
    {
        // loopBlock�� ������ ���ÿ� Ǫ�� & �ݺ� Ƚ�� Ȯ��
        // �ݺ� Ƚ����ŭ �ش� ����Ʈ ���
        // �ݺ� Ƚ���� 0�� �����ϸ�, ���ÿ��� ��
        string kindOf = null;
        int num = 0;
        Block print = root;
        Debug.Log("Print list");

        blockStack.push(root);
        while (!blockStack.isEmpty())
        {
            kindOf = print.getInfo().Split(':')[0];
            // loop or if���� ��� ���ÿ� �����ؼ� üũ����Ʈ ����
            if (kindOf.Equals("Button_loop") || kindOf.Equals("Button_if"))
            {
                blockStack.push(print);
                // �ݺ��� ��ȿ�� ��
                if (loopValidate(print, num))
                {
                    // true�̸� �ݺ��� ���� ���� ���
                    print = blockStack.peek().left;
                    Debug.Log("Loop is validated");
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
            else if (isLeaf(print))
            {
                print = blockStack.pop();
            }
            // �� �ܴ� �⺻ �����̹Ƿ� ����� ���� ��� ����
            else
            {
                print = print.right;
            }
            Debug.Log(print.getInfo());
        }
    }

    // ���� object�� �������� ���ؼ� ����
    public void createBlock(Block binfo)
    {
        string prefResource = null;
        string imgResource = null;
        string numResource = null;
        string kindOf = binfo.getInfo().Split(':')[0];

        // ������ �ϳ��� ���ս�Ű��, ���۳�Ʈ�� ���� �����ϴ°ŷ� ��ġ��
        // �̸� �� �̹��� ����
        switch (kindOf)
        {
            case "Button_left":
                {
                    imgResource = "images_renewal/Button_left";
                    prefResource = "Prefabs/Button_left";
                    break;
                }
            case "Button_forward":
                {
                    imgResource = "images_renewal/Button_forward";
                    prefResource = "Prefabs/Button_forward";
                    break;
                }
            case "Button_right":
                {
                    imgResource = "images_renewal/Button_right";
                    prefResource = "Prefabs/Button_right";
                    break;
                }
            case "Button_loop":
                {
                    imgResource = "images_renewal/Button_for";
                    prefResource = "Prefabs/Button_loop";
                    numResource += "Images/Image_stage_" + initLoopNum.ToString();
                    Debug.Log("numResource : " + numResource);
                    break;
                }
            case "subRoot":
                {
                    moveX = true;
                    return;
                }
            case "subLeaf":
                {
                    moveX = false;
                    posX = 0f;
                    return;
                }
            default:
                {
                    return;
                }
        }
        
        // ���� ���� y��ǥ ����
        if (moveX)
        {
            posX += 50f;
        }
        else
        {
            posY -= 50f;
        }
        Debug.Log("Create block at blue zone : " + binfo.getInfo());
        Debug.Log("posX : " + posX + " posY : " + posY);
        // ������ ȣ��
        GameObject prefab = Resources.Load(prefResource) as GameObject;
        // ������(�ڵ���)�� ������ ������ �ν��Ͻ�ȭ
        GameObject newObj = Instantiate(prefab) as GameObject;
        newObj.name = binfo.getInfo();
        newObj.GetComponent<Image>().sprite = Resources.Load(imgResource, typeof(Sprite)) as Sprite;
        if (kindOf.Equals("Button_loop"))
        {
            GameObject child = newObj.transform.Find("Image_loopNum").gameObject;
            Debug.Log(child);
            child.GetComponent<Image>().sprite = Resources.Load(numResource, typeof(Sprite)) as Sprite;
        }
        // Tag�� Block���� ����
        newObj.gameObject.tag = "Block";
        // mage�� ���̵��� �θ� Panel�� ����
        newObj.transform.SetParent(GameObject.Find("codeCanvas").transform);
        // ������ġ�� �»������ ����
        newObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(posX, posY, posZ);
    }

    // ������ ������ ������ ȭ��� ���
    public void showBlocks()
    {
        // root, subroot, leaf, subleaf�� �ƴ� �� ���
        // �⺻ ������ ����ϰ� ���� ���� ������ �������⸸ �ϸ� ��
        // loop ������ ���� ��ü�� ������ְ�, scope ���� �ִ� ������ ������ Ƚ����ŭ ���
        // if ������ ~ ~ ~
        // ���ÿ��� root�� loop ����, if ������ ���� check point�� Ȱ��
        // loop�� if�� ������ ��ȿ���� �ʴٸ�, ���ÿ��� check point�� ���� ���� ���� ������ ��������
        Block search = null;
        BStack s1 = new BStack();
        posY = 0f;
        posX = 0f;

        s1.push(root);
        while (!s1.isEmpty())
        {
            search = s1.pop();
            Debug.Log("============" + search.getInfo());
            createBlock(search);
            if (!isLeaf(search.right, true))
            {
                s1.push(search.right);
            }
            if (search.left != null)
            {
                s1.push(search.left);
            }
            //Debug.Log("turn : " + ++count);
        }
    }
    // ht ������� ������ ����
    public void deleteBlocks(string target)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(target);
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }

    public void treeTest()
    {
        Block[] arr = new Block[10];

        insertNode("Button_forward");   //1
        insertNode("Button_right");     //2
        insertNode("Button_right");     //3
        insertNode("Button_loop");      //4
        insertNode("Button_forward");   //5
        insertNode("Button_left");      //6
        setCheckPoint("Button_loop:4");
        insertNode("Button_forward");   //7
        insertNode("Button_left");      //8
        insertNode("Button_right");     //9
        setCheckPoint("root");
        insertNode("Button_left");      //10
        printList();

        LoopBlock tmp = (LoopBlock)findNode("Button_loop:4");
        tmp.setLoopNum(1);

        Debug.Log("========================Delete function test=====================");
        deleteNode("Button_right:2");
        deleteNode("Button_forward:7");
        deleteNode("Button_left:8");
        printList();

        Debug.Log("========================Insert in the middle test=====================");
        setPrevNode("Button_right:3");
        insertNode("Button_right");     //11
        setCheckPoint("Button_loop:4");
        insertNode("Button_right");     //12
        tmp.setLoopNum(1);

        printList();

        //deleteNode("Button_forward:0");
        //Inorder(root);
        //Debug.Log("find node : " + findNode("Button_forward:0").getInfo());
        //Block temp = l1;
        //Debug.Log(temp.GetType());
    }

    // Start is called before the first frame update
    void Start()
    {
        blockStack = new BStack();
        initTree();
        //initLoopNum = setLoopNum;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}