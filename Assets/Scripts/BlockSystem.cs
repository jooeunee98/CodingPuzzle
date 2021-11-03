using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 리스트와 관련된 요소는 '노드'로 칭하고
 * 표면적으로 드러나는 코딩블럭은 '블럭'으로 표기
 * 
 * 기본적으로 생성된 노드는 root 노드를 시작으로 right 포인터에 연결됨을 원칙으로 함
 * 예외적으로 loop 및 if 노드는 left 포인터에 subRoot 노드를 생성해서 subRoot의 right 포인터에 연결함
 * 
 * 위의 규칙에 따라 블럭은 아래의 형태로 연결됨
 * 
 * root - forward - left - loop(3) - forward - leaf
 *                           |-> subRoot - left - left - forward - subLeaf
 * 
 * root - leaf로 연결되는 리스트를 main list라고 하고
 * 아래로 분기하여 연결되는 리스르를 sub list라고 정의함
 * 
 * 반복문의 경우 반복횟수(loopNum)이 1 이상일 때만 분기해서 동작
 * 조건문의 경우 조건절로 주어진 식의 참/거짓을 판별, 참이면 분기해서 동작
 * 이들은 각각 동작이 유효한지 판단을 수행하는 메소드를 활용
 * 
 * setCheckPoint() 가독성이 좋게 수정 요망
 * 
 * 
 */

public class BlockSystem : MonoBehaviour
{
    int listId = 0;             // 블럭을 구별하는 id값
    //public int setLoopNum;
    public int initLoopNum;     // 반복문의 loopNum 기본값
    BStack blockStack;          // 분기점을 저장하는 스택
    Block checkPoint = null;    // 선택된 분기점 저장
    Block root, prev, leaf;
    FuncLists funcLists = new FuncLists();   // 함수 리스트 저장
    RaycastHit hit;

    // ht 블럭 위치 조정용 변수
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
    // 함수 기능
    // 입력되는 여러 개의 함수 root를 리스트화 하고 이를 관리하기 위함.
    public class InfoComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var xb = (Block)x;
            var yb = (Block)y;
            return xb.info.CompareTo(yb.info);
        }
    }
    // Custom ArrayList인 FuncLists
    public class FuncLists
    {
        ArrayList AllFuncs;
        IComparer infoComparer = new InfoComparer();

        public FuncLists()
        {
            AllFuncs = new ArrayList();
        }
        // 요소 추가
        public void Add(Block bInfo)
        {
            AllFuncs.Add(bInfo);
        }
        // AllFuncs에서 bInfo와 일치하는 func list 탐색 및 반환
        public Block GetFuncList(string bInfo)
        {
            // Binary Search를 실행하기 위해 정렬
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

    // Tree 초기화
    public void initTree()
    {
        root = new Block("root");
        leaf = new Block("leaf");
        root.right = leaf;
        checkPoint = root;
        //Debug.Log("Tree initialize");
    }

    // 플레이어가 선택한 블럭을 생성하고 반환
    public Block createNode(string bInfo)
    {
        listId++;
        Block newBlock = null;
        switch (bInfo)
        {
            case "Button_loop":
                // loopNum 초기화하는 방법 생각하기
                newBlock = new LoopBlock(initLoopNum, listId);

                //Debug.Log("initLoopNum : " + ((LoopBlock)newBlock).getLoopNum().ToString());
                break;
            case "Button_if":
            case "Button_else":
                newBlock = new IfBlock(bInfo, listId);
                break;
            case "Button_func":
                newBlock = new Block(bInfo, listId);    // main list에 연결하는 함수 블럭
                funcLists.Add(new Block(bInfo, listId)); // 위와 동일 정보로 funcList에 저장 즉, 함수 블럭은 2개의 node로 운용
                break;
            default:
                newBlock = new Block(bInfo, listId);
                break;
        }
        //Debug.Log("Create Node : " + newBlock.getInfo());
        return newBlock;
    }

    // 현재 노드가 leaf인지 subLeaf 인지 판단
    // mode == true : 화면 상에 블럭을 출력하는 showBlocks()에서 사용
    // mode == false : 리스트에 노드를 삽입하는 insertNode()에서 사용
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

    // 새로운 노드를 삽입할 위치를 탐색
    public Block getInsertPos(Block block)
    {
        while (!isLeaf(block.right))
            block = block.right;
        return block;
    }

    // mode false : 임의 선택된 노드 탐색값 반환
    // mode true : 임의 선택된 노드의 이전 노드 반환(단일 연결리스트이기 때문에 이전 노드값 필요)
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
            // 반복문, 조건문 등의 분기점 발견 시
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

    // 노드를 중간에 삽입하기 위해 이전 노드의 위치를 알아냄
    public void setPrevNode(string bInfo)
    {
        //Debug.Log("Set previous node");
        prev = findNode(bInfo);
    }

    // 분기점을 기준으로 나뉘는 반복문/조건문을 선택했을 때
    // 삽입 등의 동작을 위해서 체크 포인트를 지정함
    public void setCheckPoint(string bInfo)
    {
        string prev = checkPoint.getInfo().Split(':')[0];    // 현재 checkPoint
        string chosen = null;                                   // 지정한 checkPoint
        //Debug.Log("Set check point at " + prev);
        checkPoint = findNode(bInfo);

        // 분기점 설정
        if (prev.Equals("root"))
        {
            chosen = checkPoint.getInfo().Split(':')[0];
            switch (chosen)
            {
                // 분기점 설정
                case "Button_loop":
                case "Button_if":
                    //Debug.Log("binfo's left : " + checkPoint.left.getInfo());
                    checkPoint = checkPoint.left;
                    break;
                case "Button_func":

                    break;
            }
        }
        // 분기점 초기화
        else
            checkPoint = root;
    }

    // 노드 삽입
    public void insertNode(string bInfo)
    {
        Block newBlock = createNode(bInfo);
        Block insertPos = null;

        // 유저가 중간삽입을 원한다면
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
        // 유저가 선택한 라인(main, loop 등) 끝에 삽입하길 원한다면
        else
        {
            // checkPoint 값이 root이면 main list 끝에 삽입
            if (checkPoint.getInfo().Equals("root"))
            {
                insertPos = getInsertPos(root);
            }
            // checkPoint 값이 root가 아니면 sub list 끝에 삽입
            else
            {
                insertPos = getInsertPos(checkPoint);
            }
            if (bInfo.Equals("Button_else") && (insertPos.getInfo().Split(':')[0] != "Button_if"))
            {
                Debug.Log("유효하지 않은 동작");
                return;
            }
            // collider가 SnowBall을 감지하기 전에 push 블럭을 읽어들이는 문제
            // 임시로 push 블럭 앞에 더미블럭 2개를 생성해서 push 블럭을 읽는 시간을 늦춤
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

    // 선택된 노드 삭제
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
        // ht 리스트에서 노드가 삭제되었으므로 이전에 있던 블럭들을 지우고 새로 랜더링
        deleteBlocks("Block");
        showBlocks();
    }

    // 모든 노드 삭제
    public void deleteAll()
    {
        listId = 0;
        deleteBlocks("Block");
        root.right = leaf;
        checkPoint = root;
        Debug.Log("Reset tree");
    }

    // 반복문의 loopNum 값을 확인해서 반복할 수 있는지 아닌지 확인
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
        // loopBlock을 만나면 스택에 푸쉬 & 반복 횟수 확인
        // 반복 횟수만큼 해당 리스트 출력
        // 반복 횟수가 0에 도달하면, 해당 loopBlock을 스택에서 팝
        string kindOf = null;
        Block print = root;
        Debug.Log("Print list");

        blockStack.push(root);
        while (!blockStack.isEmpty())
        {
            kindOf = print.getInfo().Split(':')[0];
            switch (kindOf)
            {
                // loop or if문일 경우 스택에 저장해서 체크포인트 생성
                case "Button_loop":
                case "Button_if":
                    blockStack.push(print);
                    // 반복문 유효성 평가
                    if (loopValidate(print))
                    {
                        // true이면 반복문 내부 블럭 저장
                        print = print.left;
                    }
                    else
                    {
                        // 아니면 반복문을 건너뜀
                        print = blockStack.pop().right;
                    }
                    break;
                case "Button_func":     // main list에서 func list로 넘어감
                    // main list로 돌아오기 위한 check point
                    blockStack.push(print);
                    // func block과 동일한 정보를 지니는 노드를 funcList에서 가져옴
                    print = funcLists.GetFuncList(print.info);
                    break;
            }
            // 현재 노드가 leaf or subLeaf이면 단말노드까지 도달한 것이므로 스택에 저장된 체크포인트로 돌아감
            if (isLeaf(print))
            {
                print = blockStack.pop();
            }
            // 그 외는 기본 블럭이므로 출력을 위해 노드 변경
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
    // 블럭 object를 프리펩을 통해서 생성
    public void createBlock(Block binfo)
    {
        string prefResource = null;
        string imgResource = null;
        string numResource = null;
        string kindOf = binfo.getInfo().Split(':')[0];

        // 프리펩 하나로 통합시키고, 컴퍼넌트를 직접 수정하는거로 고치기
        // 이름 및 이미지 지정
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
                    numResource += "images/클리어넘버타이틀" + initLoopNum.ToString();
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
                    imgResource = "images/image_renewal/조건";
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
                    imgResource = "images/image_renewal/예외";
                    prefResource = "Prefabs/Button_if";
                    flagPos = true;
                    break;
                }
            case "Button_push":
                {
                    imgResource = "images/image_renewal/눈밀기";
                    prefResource = "Prefabs/Button_push";
                    break;
                }
            case "Button_plant":
                {
                    imgResource = "images/image_renewal/나무심기";
                    prefResource = "Prefabs/Button_push";
                    break;
                }
            case "Button_fruit":
                {
                    imgResource = "images/image_renewal/과일줍기";
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

        // 다음 블럭 y좌표 조정
        
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
        // 프리펩 호출
        GameObject prefab = Resources.Load(prefResource) as GameObject;
        // 블루존(코딩존)에 생성할 프리펩 인스턴스화
        GameObject newObj = Instantiate(prefab) as GameObject;
        newObj.name = binfo.getInfo();
        newObj.GetComponent<Image>().sprite = Resources.Load(imgResource, typeof(Sprite)) as Sprite;
        if (kindOf.Equals("Button_loop"))
        {
            GameObject child = newObj.transform.Find("Image_loopNum").gameObject;
            child.GetComponent<Image>().sprite = Resources.Load(numResource, typeof(Sprite)) as Sprite;
        }
        // Tag를 Block으로 변경
        newObj.gameObject.tag = "Block";
        // mage가 보이도록 부모를 Panel로 변경
        newObj.transform.SetParent(GameObject.Find("codeCanvas").transform);
        // 생성위치를 좌상단으로 지정
        newObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(posX, posY, posZ);
    }

    // 유저가 선택한 블럭을 화면상에 출력
    public void showBlocks()
    {
        // root, subroot, leaf, subleaf가 아닐 때 출력
        // 기본 블럭은 출력하고 다음 블럭 정보를 가져오기만 하면 끝
        // loop 블럭은 블럭 자체를 출력해주고, scope 내에 있는 블럭을 정해진 횟수만큼 출력
        // if 블럭은 ~ ~ ~
        // 스택에는 root와 loop 블럭, if 블럭을 삽입 check point로 활용
        // loop와 if의 조건이 유효하지 않다면, 스택에서 check point를 빼서 다음 블럭 정보를 가져오기
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
    // ht 만들었던 블럭들 삭제
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
