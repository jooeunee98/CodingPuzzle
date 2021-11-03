using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ����Ʈ�� ���õ� ��Ҵ� '���'�� Ī�ϰ�
 * ǥ�������� �巯���� �ڵ����� '��'���� ǥ��
 * 
 * �⺻������ ������ ���� root ��带 �������� right �����Ϳ� ������� ��Ģ���� ��
 * ���������� loop �� if ���� left �����Ϳ� subRoot ��带 �����ؼ� subRoot�� right �����Ϳ� ������
 * 
 * ���� ��Ģ�� ���� ���� �Ʒ��� ���·� �����
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
 * setCheckPoint() �������� ���� ���� ���
 * 
 * 
 */

public class BlockSystem : MonoBehaviour
{
    int listId = 0;             // ���� �����ϴ� id��
    //public int setLoopNum;
    public int initLoopNum;     // �ݺ����� loopNum �⺻��
    BStack blockStack;          // �б����� �����ϴ� ����
    Block checkPoint = null;    // ���õ� �б��� ����
    Block root, prev, leaf;
    FuncLists funcLists = new FuncLists();   // �Լ� ����Ʈ ����
    RaycastHit hit;

    // ht �� ��ġ ������ ����
    //bool moveY = false;
    bool flagPos = false;
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
        public int countElement()
        {
            return top + 1;
        }
    }

    // ============================================================================================
    // �Լ� ���
    // �ԷµǴ� ���� ���� �Լ� root�� ����Ʈȭ �ϰ� �̸� �����ϱ� ����.
    public class InfoComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var xb = (Block)x;
            var yb = (Block)y;
            return xb.info.CompareTo(yb.info);
        }
    }
    // Custom ArrayList�� FuncLists
    public class FuncLists
    {
        ArrayList AllFuncs;
        IComparer infoComparer = new InfoComparer();

        public FuncLists()
        {
            AllFuncs = new ArrayList();
        }
        // ��� �߰�
        public void Add(Block bInfo)
        {
            AllFuncs.Add(bInfo);
        }
        // AllFuncs���� bInfo�� ��ġ�ϴ� func list Ž�� �� ��ȯ
        public Block GetFuncList(string bInfo)
        {
            // Binary Search�� �����ϱ� ���� ����
            AllFuncs.Sort(infoComparer);
            int idx = AllFuncs.BinarySearch(new Block { info = bInfo }, infoComparer);
            return (Block)(AllFuncs[idx]);
        }
    }
    // ============================================================================================

    public class Block
    {
        internal Block left;
        internal Block right;
        internal string info { get; set; }

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
        private bool workOut;

        public IfBlock() : base()
        {
            this.condition = "NULL";
            Block subRoot = new Block("subRoot");
            Block subLeaf = new Block("subLeaf");
            base.left = subRoot;
            subRoot.right = subLeaf;
            this.workOut = true;
        }
        public IfBlock(string condition, int num) : base(condition, num)
        {
            this.condition = condition;
            Block subRoot = new Block("subRoot");
            Block subLeaf = new Block("subLeaf");
            base.left = subRoot;
            subRoot.right = subLeaf;
            this.workOut = true;
        }
        public bool getWorkOut()
        {
            return workOut;
        }
        public void setWorkOut()
        {
            workOut = true;
        }
        public string getCondition()
        {
            return condition;
        }
        public bool Validation(string tagInfo)
        {
            if (workOut)
            {
                switch (tagInfo)
                {
                    case "SnowBall":
                        Debug.Log("There is " + tagInfo + " in front of the Baby");
                        break;
                    case "TakeFruits":
                        Debug.Log("Character can pick up a fruit");
                        break;
                    case "PlantTrees":
                        Debug.Log("He can plant a tree here");
                        break;
                    default:
                        Debug.Log("There is no any object");
                        break;
                }
                workOut = false;
                return true;
            }
            else
            {
                workOut = true;
                return false;
            }
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
        //Debug.Log("Tree initialize");
    }

    // �÷��̾ ������ ���� �����ϰ� ��ȯ
    public Block createNode(string bInfo)
    {
        listId++;
        Block newBlock = null;
        switch (bInfo)
        {
            case "Button_loop":
                // loopNum �ʱ�ȭ�ϴ� ��� �����ϱ�
                newBlock = new LoopBlock(initLoopNum, listId);

                //Debug.Log("initLoopNum : " + ((LoopBlock)newBlock).getLoopNum().ToString());
                break;
            case "Button_if":
            case "Button_else":
                newBlock = new IfBlock(bInfo, listId);
                break;
            case "Button_func":
                newBlock = new Block(bInfo, listId);    // main list�� �����ϴ� �Լ� ��
                funcLists.Add(new Block(bInfo, listId)); // ���� ���� ������ funcList�� ���� ��, �Լ� ���� 2���� node�� ���
                break;
            default:
                newBlock = new Block(bInfo, listId);
                break;
        }
        //Debug.Log("Create Node : " + newBlock.getInfo());
        return newBlock;
    }

    // ���� ��尡 leaf���� subLeaf ���� �Ǵ�
    // mode == true : ȭ�� �� ���� ����ϴ� showBlocks()���� ���
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

    // ���ο� ��带 ������ ��ġ�� Ž��
    public Block getInsertPos(Block block)
    {
        while (!isLeaf(block.right))
            block = block.right;
        return block;
    }

    // mode false : ���� ���õ� ��� Ž���� ��ȯ
    // mode true : ���� ���õ� ����� ���� ��� ��ȯ(���� ���Ḯ��Ʈ�̱� ������ ���� ��尪 �ʿ�)
    public Block findNode(string bInfo, bool mode = false)
    {
        //Debug.Log("Find some node about : " + bInfo);
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
            // �ݺ���, ���ǹ� ���� �б��� �߰� ��
            if (search.left != null)
            {
                s1.push(search.left);
                s2.push(search);
            }
            //Debug.Log("turn : " + ++count);
        }
        if (mode == false)
        {
            //Debug.Log("Found node : " + search.getInfo());
            return search;
        }
        else
        {
            //Debug.Log("Found previous node: " + searchPrev.getInfo());
            return searchPrev;
        }
    }

    // ��带 �߰��� �����ϱ� ���� ���� ����� ��ġ�� �˾Ƴ�
    public void setPrevNode(string bInfo)
    {
        //Debug.Log("Set previous node");
        prev = findNode(bInfo);
    }

    // �б����� �������� ������ �ݺ���/���ǹ��� �������� ��
    // ���� ���� ������ ���ؼ� üũ ����Ʈ�� ������
    public void setCheckPoint(string bInfo)
    {
        string prev = checkPoint.getInfo().Split(':')[0];    // ���� checkPoint
        string chosen = null;                                   // ������ checkPoint
        //Debug.Log("Set check point at " + prev);
        checkPoint = findNode(bInfo);

        // �б��� ����
        if (prev.Equals("root"))
        {
            chosen = checkPoint.getInfo().Split(':')[0];
            switch (chosen)
            {
                // �б��� ����
                case "Button_loop":
                case "Button_if":
                    //Debug.Log("binfo's left : " + checkPoint.left.getInfo());
                    checkPoint = checkPoint.left;
                    break;
                case "Button_func":

                    break;
            }
        }
        // �б��� �ʱ�ȭ
        else
            checkPoint = root;
    }

    // ��� ����
    public void insertNode(string bInfo)
    {
        Block newBlock = createNode(bInfo);
        Block insertPos = null;

        // ������ �߰������� ���Ѵٸ�
        if (prev != null)
        {
            if (bInfo.Equals("Button_push"))
            {
                Block dummy1 = new Block("dummy", listId++);
                Block dummy2 = new Block("dummy", listId++);
                dummy1.right = prev.right;
                prev.right = dummy1;
                dummy2.right = prev.right;
                prev.right = dummy2;
            }
            newBlock.right = prev.right;
            prev.right = newBlock;
            prev = null;
        }
        // ������ ������ ����(main, loop ��) ���� �����ϱ� ���Ѵٸ�
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
            if (bInfo.Equals("Button_else") && (insertPos.getInfo().Split(':')[0] != "Button_if"))
            {
                Debug.Log("��ȿ���� ���� ����");
                return;
            }
            // collider�� SnowBall�� �����ϱ� ���� push ���� �о���̴� ����
            // �ӽ÷� push �� �տ� ���̺� 2���� �����ؼ� push ���� �д� �ð��� ����
            if (bInfo.Equals("Button_push"))
            {
                Block dummy1 = new Block("dummy", listId++);
                Block dummy2 = new Block("dummy", listId++);
                dummy1.right = insertPos.right;
                insertPos.right = dummy1;
                insertPos = dummy1;
                dummy2.right = dummy1.right;
                dummy1.right = dummy2;
                insertPos = dummy2;
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
        //Debug.Log("Previous node of delete : " + delPrev.getInfo());
        Block delete = findNode(bInfo);
        string kindOf;
        //Debug.Log("Delete node : " + delete.getInfo());

        if (isLeaf(delete))
            Debug.Log("A block that does not exist.");
        else
        {
            //Debug.Log("Delete node");
            delPrev.right = delete.right;
        }
        kindOf = bInfo.Split(':')[0];
        if (kindOf.Equals("Button_loop") || kindOf.Equals("Button_if"))
            checkPoint = root;
        // ht ����Ʈ���� ��尡 �����Ǿ����Ƿ� ������ �ִ� ������ ����� ���� ������
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
    public bool loopValidate(Block binfo)
    {
        LoopBlock loopBlock = (LoopBlock)binfo;
        int loopNum = loopBlock.getLoopNum();
        bool vali = false;
        if (loopNum > 0)
        {
            //Debug.Log(loopNum);
            loopBlock.setLoopNum(--loopNum);
            vali = true;
        }
        return vali;
    }
    public void createFunc()
    {
        Block f1 = new Block("f1");
        Block f2 = new Block("f2");
        Block f3 = new Block("f3");

        funcLists.Add(f3);
        funcLists.Add(f2);
        funcLists.Add(f1);
        Block obj = new Block();
        //funcLists.Sort(obj);
        Debug.Log(funcLists.GetFuncList("f1").info);
        /*foreach (Block temp in funcLists)
        {
            Debug.Log(temp.getInfo());
        }*/
    }

    public void printList()
    {
        // loopBlock�� ������ ���ÿ� Ǫ�� & �ݺ� Ƚ�� Ȯ��
        // �ݺ� Ƚ����ŭ �ش� ����Ʈ ���
        // �ݺ� Ƚ���� 0�� �����ϸ�, �ش� loopBlock�� ���ÿ��� ��
        string kindOf = null;
        Block print = root;
        Debug.Log("Print list");

        blockStack.push(root);
        while (!blockStack.isEmpty())
        {
            kindOf = print.getInfo().Split(':')[0];
            switch (kindOf)
            {
                // loop or if���� ��� ���ÿ� �����ؼ� üũ����Ʈ ����
                case "Button_loop":
                case "Button_if":
                    blockStack.push(print);
                    // �ݺ��� ��ȿ�� ��
                    if (loopValidate(print))
                    {
                        // true�̸� �ݺ��� ���� �� ����
                        print = print.left;
                    }
                    else
                    {
                        // �ƴϸ� �ݺ����� �ǳʶ�
                        print = blockStack.pop().right;
                    }
                    break;
                case "Button_func":     // main list���� func list�� �Ѿ
                    // main list�� ���ƿ��� ���� check point
                    blockStack.push(print);
                    // func block�� ������ ������ ���ϴ� ��带 funcList���� ������
                    print = funcLists.GetFuncList(print.info);
                    break;
            }
            // ���� ��尡 leaf or subLeaf�̸� �ܸ������� ������ ���̹Ƿ� ���ÿ� ����� üũ����Ʈ�� ���ư�
            if (isLeaf(print))
            {
                print = blockStack.pop();
            }
            // �� �ܴ� �⺻ ���̹Ƿ� ����� ���� ��� ����
            else
            {
                print = print.right;
            }
            Debug.Log(print.getInfo());
        }
    }

    public void displayBlock()
    {
        Vector3 local = GameObject.Find(blockStack.peek().getInfo()).GetComponent<RectTransform>().anchoredPosition;
        //Debug.Log("Top of Stack : " + blockStack.peek().getInfo());
        Block search;
        BStack s1 = new BStack();
        search = blockStack.peek().left;

        s1.push(search);
        while (!s1.isEmpty())
        {
            search = s1.pop();
            string kindOf = search.getInfo().Split(':')[0];
            if (kindOf.Equals("Button_loop") || kindOf.Equals("Button_if"))
            {
                if (search.right.getInfo().Equals("subLeaf"))
                {
                    //Debug.Log("case 1");
                    notEnter = true;
                }
                posX = local.x + 50f;
                posY = local.y - 50f;

                break;
            }
            if (!isLeaf(search.right))
            {
                s1.push(search.right);
            }

        }
    }
    int loopCount = 0;
    bool notEnter = false;
    // �� object�� �������� ���ؼ� ����
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
                    imgResource = "images/Left";
                    prefResource = "Prefabs/Button_left";
                    break;
                }
            case "Button_forward":
                {
                    imgResource = "images/Forward";
                    prefResource = "Prefabs/Button_forward";
                    break;
                }
            case "Button_right":
                {
                    imgResource = "images/Right";
                    prefResource = "Prefabs/Button_right";
                    break;
                }
            case "Button_loop":
                {
                    imgResource = "images/For";
                    prefResource = "Prefabs/Button_loop";
                    numResource += "images/Ŭ����ѹ�Ÿ��Ʋ" + initLoopNum.ToString();
                    if (blockStack.isEmpty())
                    {
                        //Debug.Log("stack is empty");
                        blockStack.push(binfo);
                        posX = -50f;
                        posY -= 50f;
                    }
                    else
                    {
                        //Debug.Log("stack is not empty");
                        loopCount++;
                        flagPos = true;
                    }
                    break;
                }
            case "Button_if":
                {
                    imgResource = "images/image_renewal/����";
                    prefResource = "Prefabs/Button_if";
                    if (blockStack.isEmpty())
                    {
                        //Debug.Log("stack is empty");
                        blockStack.push(binfo);
                        posX = -50f;
                        posY -= 50f;
                    }
                    else
                    {
                        //Debug.Log("stack is not empty");
                        loopCount++;
                        flagPos = true;
                    }
                    break;
                }
            case "Button_else":
                {
                    imgResource = "images/image_renewal/����";
                    prefResource = "Prefabs/Button_if";
                    flagPos = true;
                    break;
                }
            case "Button_push":
                {
                    imgResource = "images/image_renewal/���б�";
                    prefResource = "Prefabs/Button_push";
                    break;
                }
            case "Button_plant":
                {
                    imgResource = "images/image_renewal/�����ɱ�";
                    prefResource = "Prefabs/Button_push";
                    break;
                }
            case "Button_fruit":
                {
                    imgResource = "images/image_renewal/�����ݱ�";
                    prefResource = "Prefabs/Button_push";
                    break;
                }
            case "subRoot":
                {
                    //moveY = true;
                    return;
                }
            case "subLeaf":
                {
                    posX = GameObject.Find(blockStack.pop().getInfo()).GetComponent<RectTransform>().anchoredPosition.x - 50f;
                    if (loopCount > 0 && notEnter)
                    {
                        notEnter = false;
                        posY += 50f;
                    }
                    posY -= 50f;
                    return;
                }
            default:
                {
                    return;
                }
        }

        // ���� �� y��ǥ ����
        
        if (flagPos)
        {
            displayBlock();
            blockStack.push(binfo);
            flagPos = false;
        }
        else
        {
            posX += 50f;
        }
        //Debug.Log("posX : " + posX + " posY : " + posY);
        // ������ ȣ��
        GameObject prefab = Resources.Load(prefResource) as GameObject;
        // �����(�ڵ���)�� ������ ������ �ν��Ͻ�ȭ
        GameObject newObj = Instantiate(prefab) as GameObject;
        newObj.name = binfo.getInfo();
        newObj.GetComponent<Image>().sprite = Resources.Load(imgResource, typeof(Sprite)) as Sprite;
        if (kindOf.Equals("Button_loop"))
        {
            GameObject child = newObj.transform.Find("Image_loopNum").gameObject;
            child.GetComponent<Image>().sprite = Resources.Load(numResource, typeof(Sprite)) as Sprite;
        }
        // Tag�� Block���� ����
        newObj.gameObject.tag = "Block";
        // mage�� ���̵��� �θ� Panel�� ����
        newObj.transform.SetParent(GameObject.Find("codeCanvas").transform);
        // ������ġ�� �»������ ����
        newObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(posX, posY, posZ);
    }

    // ������ ������ ���� ȭ��� ���
    public void showBlocks()
    {
        // root, subroot, leaf, subleaf�� �ƴ� �� ���
        // �⺻ ���� ����ϰ� ���� �� ������ �������⸸ �ϸ� ��
        // loop ���� �� ��ü�� ������ְ�, scope ���� �ִ� ���� ������ Ƚ����ŭ ���
        // if ���� ~ ~ ~
        // ���ÿ��� root�� loop ��, if ���� ���� check point�� Ȱ��
        // loop�� if�� ������ ��ȿ���� �ʴٸ�, ���ÿ��� check point�� ���� ���� �� ������ ��������
        Block search = null;
        BStack s1 = new BStack();
        posY = 0f;
        posX = -50f;

        s1.push(root);
        while (!s1.isEmpty())
        {
            search = s1.pop();
            //Debug.Log("============" + search.getInfo());
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
    // ht ������� ���� ����
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
