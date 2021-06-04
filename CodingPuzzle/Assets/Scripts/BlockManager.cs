using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    public GameObject selectBlock;
    public Text blockText;          // (tg) 블럭 오브젝트 내에 Text를 연결
    public int listId = 0;          // (tg) 블럭 id

    // ht 블럭 위치 조정용 변수
    public float posX = 0f;
    public float posY = 0f;

    public class Block
    {
        internal Block prev;
        internal Block next;
        internal string direction;
        public Block()                          // (tg) 기본생성자
        {
            direction = null;
            prev = null;
            next = null;
        }
        public Block(string state)              // (tg) head, tail 생성용
        {
            this.direction = state;
            prev = null;
            next = null;
        }
        public Block(string direction, int num) // (tg) 기본 블럭 생성용
        {
            this.direction = direction + ':' + num.ToString();
            prev = null;
            next = null;
        }
    }

    // (tg) 반복문 블럭(수정될 수 있음)
    public class ForBlock : Block
    {
        int turn;
    }
    // (tg) 조건문 블럭(수정될 수 있음)
    public class IfBlock : Block
    {
        string condition;
    }

    // (tg) 리스트 head 및 tail
    Block head, tail;

    // (tg) 리스트 초기화
    public void initlist()
    {
        //Debug.Log("Init list");
        head = new Block("head");
        tail = new Block("tail");
        head.next = tail;
        head.prev = head;
        //tail.next = tail;   head는 자기 참조가 되든데 tail은 왜?..
        tail.prev = head;
    }
    // (tg) 현재 블럭이 끝이면 true 아니면 false 반환
    public bool isTail(Block b)
    {
        return b.direction.Equals("tail");
    }
    // (tg) 넘겨받은 id값으로 블럭 탐색
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
                if (result[1].Equals(num))  // (tg) 블럭을 찾으면 반복 중지
                    break;
                search = search.next;
            }
        }
        return search;
    }
    // (tg) 클릭한 블럭을 생성해서 리스트 맨 앞에 삽입
    // ht 블럭 클릭하면 실행
    public int insertFront(string direction)
    {
        //direction = "test node";                   // (tg) 테스트용
        listId++;
        Block newBlock = new Block(direction, listId);
        newBlock.prev = head;
        newBlock.next = head.next;
        head.next.prev = newBlock;
        head.next = newBlock;
        return 1;
    }
    /* (tg) 중간삽입     드래그 앤 드랍했을 때 어떤 노드들 사이에 들어가는지 알 수 있어야 함
     * public int insert()
    {
        string direction = blockText.text;

        listId++;
        Block newBlock = new Block(direction, listId);

    }
    */
    // (tg) 클릭한 블럭을 생성해서 리스트 맨 뒤에 삽입
    public void insertLast(string direction)
    {
        //direction = "test node";       // (tg) 테스트용
        listId++;
        // ht Debug.Log("Here i am");
        Block newBlock = new Block(direction, listId);
        newBlock.prev = tail.prev;
        newBlock.next = tail;
        tail.prev.next = newBlock;
        tail.prev = newBlock;

        // ht 연결리스트에 노드가 추가되었으므로 이전에 있던 걸 지우고 새로 랜더링
        deleteBlocks("Block");
        showBlocks();
    }
    // (tg) 넘겨받은 id값으로 블럭 삭제 (수정 필요)
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
            //Debug.Log("Delete node's prev " + delete.prev.direction); // (tg) 테스트용
            //Debug.Log("Delete node's next " + delete.next.direction); // (tg) 테스트용
            delete.prev.next = delete.next;
            delete.next.prev = delete.prev;
            delete = null;
        }

        // ht 연결리스트에서 노드가 삭제되었으므로 이전에 있던 걸 지우고 새로 랜더링
        deleteBlocks("Block");
        showBlocks();
    }
    // (tg) 콘솔에 리스트 출력
    public void printList()
    {
        Block print = head.next;
        while (!isTail(print))
        {
            Debug.Log(print.direction);
            print = print.next;
        }
    }
    // (tg) 동작확인 함수
    public void verifiAlgorithm()
    {
        for (int i = 1; i < 9; i++)
        {
            Debug.Log(i + "노드 삽입");
            insertLast("test node");
        }
        printList();
        for (int i = 3; i < 5; i++)
            deleteNode(i.ToString());
        printList();
    }
    
    // ht 블럭 생성
    public void createForwardBlock()
    {
        GameObject newObj = new GameObject();
        newObj.name = "ForwardBlock";
        newObj.AddComponent<CanvasRenderer>();
        newObj.AddComponent<Button>();
        newObj.AddComponent<Image>();

        // Tag를 Block으로 변경
        newObj.gameObject.tag = "Block";

        // 이미지 변경
        newObj.GetComponent<Image>().sprite = Resources.Load("images/Forward", typeof(Sprite)) as Sprite;

        // Image가 보이도록 부모를 Panel로 변경 
        newObj.transform.SetParent(GameObject.Find("CodePanel").transform);

        // 생성위치를 좌상단으로 지정
        newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform의 PosX, PosY, PosZ 변경 방법
        newObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
        posY -= 100f; // 다음 블럭 y좌표 조정
        //Debug.Log("Created");

    }
    // ht 좌회전 블럭 생성
    public void createLeftBlock()
    {
        GameObject newObj = new GameObject();
        newObj.name = "LeftBlock";
        newObj.AddComponent<CanvasRenderer>();
        newObj.AddComponent<Button>();
        newObj.AddComponent<Image>();

        // Tag를 Block으로 변경
        newObj.gameObject.tag = "Block";

        // 이미지 변경
        newObj.GetComponent<Image>().sprite = Resources.Load("images/Left", typeof(Sprite)) as Sprite;

        // Image가 보이도록 부모를 Panel로 변경 
        newObj.transform.SetParent(GameObject.Find("CodePanel").transform);

        // 생성위치를 좌상단으로 지정
        newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform의 PosX, PosY, PosZ 변경 방법
        newObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
        posY -= 100f; // 다음 블럭 y좌표 조정
        //Debug.Log("Created");

    }
    // ht 우회전 블럭 생성
    public void createRightBlock()
    {
        GameObject newObj = new GameObject();
        newObj.name = "RightBlock";
        newObj.AddComponent<CanvasRenderer>();
        newObj.AddComponent<Button>();
        newObj.AddComponent<Image>();

        // Tag를 Block으로 변경
        newObj.gameObject.tag = "Block";

        // 이미지 변경
        newObj.GetComponent<Image>().sprite = Resources.Load("images/Right", typeof(Sprite)) as Sprite;

        // Image가 보이도록 부모를 Panel로 변경 
        newObj.transform.SetParent(GameObject.Find("CodePanel").transform);

        // 생성위치를 좌상단으로 지정
        newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform의 PosX, PosY, PosZ 변경 방법
        newObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
        posY -= 100f; // 다음 블럭 y좌표 조정
        //Debug.Log("Created");

    }
    // ht 만들었던 블럭들 삭제
    public void deleteBlocks(string target)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(target);
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
            //Debug.Log("Deleted");
        }
    }


    // ht 연결리스트 헤드를 받아서 화면에 출력
    public void showBlocks()
    {
        // head 부터 tail까지 돌면서 블럭생성
        Block LL = head.next;

        // 블럭 생성 시작위치 초기화
        posY = 0f;
        while (!isTail(LL))
        {
            // 각 노드가 어떤 블럭인지 판단후 해당 블럭 생성하는 함수 실행
            string[] direction_temp = LL.direction.Split(':');
            if (direction_temp[0].Equals("forward"))
            {
                createForwardBlock(); // 전진 블럭인 경우 (일단 모든 노드가 전진블럭이라고 가정)
            }
            else if (direction_temp[0].Equals("left"))
            {
                createLeftBlock(); // 좌회전 블럭인 경우

            }
            else if (direction_temp[0].Equals("right"))
            {
                createRightBlock(); // 우회전 블럭인 경우
            }
            LL = LL.next;
        }

        // 만들었던 블럭들 삭제
        //deleteBlocks("Block");
    }
    
    void Start()
    {
        initlist();
        // 동작 확인을 위해 스타트 함수에서 실행
        //verifiAlgorithm();

        // 연결리스트가 갱신될때마다 ( = 코딩블럭이 클릭될때 (insert), 코딩존의 블럭이 삭제될 때 (delete))
        // showBlocks() 실행


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
