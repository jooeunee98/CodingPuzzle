using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    (tg)
    연결리스트와 관련된 요소는 '노드'로 칭하고
    표면적으로 드러나는 코딩블럭은 '블럭'으로 표기
*/

public class BlockManager : MonoBehaviour
{
    public GameObject selectBlock;
    public Text blockText;          // (tg) 블럭 오브젝트 내에 Text를 연결
    public int listId = 0;          // (tg) 블럭 id

    // ht 블럭 위치 조정용 변수
    public float posX = 0f;
    public float posY = 0f;
    public float posZ = 0f;

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

    // (tg) 리스트 head 및 tail, middle(리스트 중간에 삽입할 위치를 가리키는 포인터)
    Block head, tail, middle;

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
        middle = null;
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
    // (tg) 중간삽입     개발 중.. 드래그 앤 드랍했을 때 어떤 노드들 사이에 들어가는지 알 수 있어야 함
    public void setMiddle(string prevNode)
    {
        string idValue = prevNode.Split(':')[1];
        middle = getNode(idValue);
    }
    // (tg) 클릭한 블럭을 생성해서 리스트 맨 뒤에 삽입
    public void insertLast(string direction)
    {
        //direction = "test node";       // (tg) 테스트용
        // ht Debug.Log("Here i am");
        listId++;                                       // id값 증가
        Block newBlock = new Block(direction, listId);  // 노드 생성
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
            newBlock.prev = tail.prev;                      // 연결
            newBlock.next = tail;
            tail.prev.next = newBlock;
            tail.prev = newBlock;
        }
        // ht 연결리스트에 노드가 추가되었으므로 이전에 있던 걸 지우고 새로 랜더링
        deleteBlocks("Block");
        showBlocks();
    }
    
    // (tg) 넘겨받은 id값으로 블럭 삭제 (수정 필요)
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
    // (tg) 연결리스트 동작확인 함수
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

    /* (tg) 아래에 블럭생성 통합코드를 작성했으므로 삭제 요청합니당! ㅎㅅㅎ
    // ht 블럭 생성
    public void createForwardBlock(string id)
    {
        GameObject newObj = new GameObject();
        GameObject layerObj = new GameObject();
        newObj.name = "ForwardBlock";
        layerObj.name = "BlockDelete";
        newObj.AddComponent<CanvasRenderer>();
        newObj.AddComponent<Button>();
        newObj.AddComponent<Image>();
        layerObj.AddComponent<CanvasRenderer>();
        layerObj.AddComponent<Button>();
        layerObj.AddComponent<Image>();

        // Tag를 Block으로 변경
        newObj.gameObject.tag = "Block";
        
        // 이미지 변경
        newObj.GetComponent<Image>().sprite = Resources.Load("images/Forward", typeof(Sprite)) as Sprite;
        layerObj.GetComponent<Image>().sprite = Resources.Load("images/Button_blockDelete", typeof(Sprite)) as Sprite;

        // Image가 보이도록 부모를 Panel로 변경 
        newObj.transform.SetParent(GameObject.Find("CodePanel").transform);
        layerObj.transform.SetParent(GameObject.Find("CodePanel").transform);

        // 생성위치를 좌상단으로 지정
        newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);

        layerObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        layerObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        layerObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform의 PosX, PosY, PosZ 변경 방법
        newObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
        layerObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
        posY -= 100f; // 다음 블럭 y좌표 조정

        layerObj.GetComponent<RectTransform>().localScale = new Vector2(1,1);
        //Debug.Log("Created");

    }
    // ht 좌회전 블럭 생성
    public void createLeftBlock(string id)
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
    public void createRightBlock(string id)
    {
        GameObject newObj = new GameObject();
        newObj.name = "RightBlock:"+id;
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
    -여기까지-
    */
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

    // (tg) 블록생성 함수 코드 중복을 막기 위해서 하나로 통합
    public void createBlock(string block)
    {
        string whatis = block.Split(':')[0];            // 클릭된 블럭이 무엇인지 판단
        string listId = block.Split(':')[1];            // 클릭된 불럭의 id값 저장 (삭제 등을 할 때 무슨 블럭인지 알아야 해서)
        string prefResource;                            // 생성할 프리펩 리소스
        string designObjName;                           // 클릭된 블럭의 이름
        string imgResource;                             // 프리펩에 씌울 이미지 이름
                
        // 이름 및 이미지 지정
        if (whatis.Equals("Button_left"))       // 좌회전 버튼이면
        {
            designObjName = "LeftBlock:" + listId;
            imgResource = "images/Left";
            prefResource = "Prefabs/Button_left";
        }
        else if (whatis.Equals("Button_forward"))       // 직진 버튼이면
        {
            designObjName = "ForwardBlock:" + listId;
            imgResource = "images/forward";
            prefResource = "Prefabs/Button_forward";
        }
        else
        {   // 둘 다 아니면 우회전 버튼이므로
            designObjName = "RightBlock:" + listId;
            imgResource = "images/Right";
            prefResource = "Prefabs/Button_right";
        }

        // 프리펩 호출
        GameObject prefab = Resources.Load(prefResource) as GameObject;
        // 블루존(코딩존)에 생성할 프리펩 인스턴스화
        GameObject newObj = Instantiate(prefab) as GameObject;
        newObj.name = designObjName;
        newObj.GetComponent<Image>().sprite = Resources.Load(imgResource, typeof(Sprite)) as Sprite;

        // Tag를 Block으로 변경
        newObj.gameObject.tag = "Block";

        // Image가 보이도록 부모를 Panel로 변경 
        newObj.transform.SetParent(GameObject.Find("codeCanvas").transform);

        // 생성위치를 좌상단으로 지정
        //newObj.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        //newObj.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        //newObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        // RectTransform의 PosX, PosY, PosZ 변경 방법
        posY += 438f;
        newObj.GetComponent<RectTransform>().position = new Vector3(posX, posY, posZ);
        posY -= 438f;

        Debug.Log(posX + " " + posY + " " + posZ);
        posY -= 65f; // 다음 블럭 y좌표 조정
                      // ht 연결리스트 헤드를 받아서 화면에 출력
    }
        public void showBlocks()
    {
        // head 부터 tail까지 돌면서 블럭생성
        Block LL = head.next;
        // 블럭 생성 시작위치 초기화
        posY = 0f;
        while (!isTail(LL))
        {
            // (tg) 코드 중복을 최소화 하기 위해서 아래의 코드를 함수 하나로 처리
            // 각 노드가 어떤 블럭인지 판단후 해당 블럭 생성하는 함수 실행
            /*string[] direction_temp = LL.direction.Split(':');
            if (direction_temp[0].Equals("Button_forward"))
            {
                createForwardBlock(direction_temp[1]); // 전진 블럭인 경우 (일단 모든 노드가 전진블럭이라고 가정)
            }
            else if (direction_temp[0].Equals("Button_left"))
            {
                createLeftBlock(direction_temp[1]); // 좌회전 블럭인 경우

            }
            else if (direction_temp[0].Equals("Button_right"))
            {
                createRightBlock(direction_temp[1]); // 우회전 블럭인 경우
            }
            */
            createBlock(LL.direction);
            LL = LL.next;
        }

        // (tg) 만들었던 블럭들 삭제
        //deleteBlocks("Block");
    }

    void Start()
    {
        initlist();
        posX += 389.5f;
        //posZ -= 379f;
        // (tg) 동작 확인을 위해 스타트 함수에서 실행
        //verifiAlgorithm();
    }

    void Update()
    {

    }

    /*  (tg) 단일 연결리스트 코드
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
