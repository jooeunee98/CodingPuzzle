using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //BlockManager blockManager;              // 블록 매니저
    BlockSystem blockManager;
    public TalkManager talkManager;         // 대화 매니저
    public GameObject talkPanel;            // 대화창 판넬
    public Image portraitImg;               // 대화창 초상화
    public Text talkText;                   // 대화창에 띄울 텍스트
    public GameObject scanObject;           // 대화를 위해 호출된 게임 오브젝트
    public int stageNum;                    // 현재 스테이지 번호
    public bool isAction;                   // 대화창을 띄웠는지 판단
    public int talkIndex;
    static private string prev_scene;       // 이전 씬의 정보

    private void Start()
    {
        if (talkManager.TY_StageNum == 0)
        {
            talkIndex = 0;
        }
        else
        {
            if (talkManager.TY_StageNum == 11)
            {
                talkIndex = 8;
            }
            else if (talkManager.TY_StageNum == 14)
            {
                talkIndex = talkManager.TY_DialogNum[talkManager.TY_StageNum - 2];
            }
            else if (talkManager.TY_StageNum == 21)
            {
                talkIndex = talkManager.TY_DialogNum[15];
            }
            else if (talkManager.TY_StageNum == 25)
            {
                talkIndex = talkManager.TY_DialogNum[22];

            }
            else if (talkManager.TY_StageNum == 31)
            {
                talkIndex = talkManager.TY_DialogNum[25];

            }
            else if (talkManager.TY_StageNum == 34)
            {
                talkIndex = talkManager.TY_DialogNum[31];

            }
            else
            {
                talkIndex = talkManager.TY_DialogNum[talkManager.TY_StageNum - 1];
            }
        }
        Debug.Log("System start");
        //blockManager = BlockManager.FindObjectOfType<BlockManager>();
        blockManager = BlockSystem.FindObjectOfType<BlockSystem>();
    }

    public void Action(int sNumber)  // 대화 시작
    {
        //ObjData objData = scanObj.GetComponent<ObjData>();

        isAction = true;                    // 대화 액션 시작
        if (sNumber > 0)                // 만약 대화를 위해 호출된 오브젝트가 있다면
        {
            stageNum = sNumber;
            //Talk(objData.id, objData.isNpc);
            Talk(stageNum, true);           // 임의로 모든 대화상대는 NPC로 설정
        }
        else
        {
            //scanObject.name = "Coding Puzzle";
            talkText.text = "테스트 중입니다.";
        }
        //talkText.text = "이것의 이름은" + scanObject.name + "이라고 한다.";    // 대화창에 출력할 텍스트
        talkPanel.SetActive(isAction);     // isAction의 상태에 따라 대화창 띄움/내림

    }

    void Talk(int sNumber, bool isNpc)       // (tg) 대화를 시도하는 오브젝트에 따라 대사가 나옴
    {
        string talkData = talkManager.GetTalk(sNumber, talkIndex);
        Debug.Log("isNpc" + isNpc);

        if (talkIndex == talkManager.TY_DialogNum[talkManager.TY_StageNum])       // 현재 스테이지의 대화를 모두 출력했다면
        {
            isAction = false;       // 대화 패널을 내리고

            Debug.Log(talkManager.TY_StageNum + "번 스테이지 대화가 종료되었습니다.");
            //  talkManager.TY_StageNum 번째 스테이지 씬으로 전환
            return;                 // 종료
        }
        /*if (talkData == null)       // 더 이상 대사가 없다면
        {
            isAction = false;       // 대화 패널을 내리고
            talkIndex = 0;
            return;                 // 종료
        }*/
        if (isNpc)
        {
            Debug.Log("to delete :");
            string[] result = talkData.Split(':');
            talkText.text = result[0];         // NPC의 대화는 구분자로 대사와 표정으로 구성되어 있으므로 이를 나눔
            Debug.Log(result[0]);
            Debug.Log(result[1]);
            portraitImg.sprite = talkManager.GetPortrait(stageNum, int.Parse(result[1]));
            portraitImg.color = new Color(1, 1, 1, 1);      // 대화 상대가 NPC면 초상화 보이고
        }
        else
        {
            talkText.text = talkData;
            portraitImg.color = new Color(1, 1, 1, 0);      // 대화 상대가 NPC가 아니면 초상화가 안 보임
        }
        isAction = true;
        talkIndex++;                                        // 다음 대사 호출을 위한 인덱스 증가
    }

    // (tg) 레드존(유저에게 주어지는 블럭들이 표시되는 곳)에서 버튼 클릭 시 코드블럭을 블루존(코딩존)에 생성
    public void ClickDirect()
    {
        //Debug.Log("Some button was clicked");
        string clicked = EventSystem.current.currentSelectedGameObject.name;    // 클릭된 버튼이 어느 건지알아옴
        Debug.Log("button : " + clicked);
        //blockManager.insertLast(clicked);                                       // 클릭된 버튼 노드 생성
        blockManager.insertNode(clicked);
    }
    // (tg) 기존의 블럭들 사이에 새로운 블럭 삽입
    // 임의의 블럭(A)을 선택하고 레드존의 블럭(B)을 누르면, A 뒤에 B를 삽입
    public void MidInsert()
    {
        string prevBlockName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Insert new block at next : " + prevBlockName);
        //blockManager.setMiddle(prevBlockName);
        string kindOf = prevBlockName.Split(':')[0];
        if (kindOf.Equals("Button_loop") || kindOf.Equals("Button_if"))
        {
            blockManager.setCheckPoint(prevBlockName);
        }
        else
        {
            blockManager.setPrevNode(prevBlockName);
        }
    }
    // (tg) 블루존에 있는 코드블럭을 제거
    public void ClickDelete()
    {
        // 코드블럭-삭제 버튼이 부모 자식관계
        // 삭제 버튼의 부모를 알아내서 몇 번째 블럭을 삭제하는지 알아냄
        GameObject deleteButton = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        string delButtonName = deleteButton.name;               // 삭제하는 블럭의 이름
        Debug.Log("Delete this button : " + delButtonName);     // 동작 확인
        //blockManager.deleteNode(delButtonName);                 // 블루존에서 블럭 삭제
        blockManager.deleteNode(delButtonName);
    }

    // (tg) Scene 변경
    public void SceneChange()
    {
        // 버튼을 눌러서 이동할 씬 이름 받아옴
        string called = EventSystem.current.currentSelectedGameObject.name;
        //Debug.Log("SceneChange() called");

        // 만약 close 버튼이 눌러지면 이전 씬으로 이동
        // 메인 -> 스테이지 -> 설정
        // 위와 같이 가정할 때 설정 씬에서 close 버튼을 누르면 스테이지로 이동
        if (called.Equals("Button_Close"))
        {
            //Debug.Log("Button_close was pressed");
            //Debug.Log(prev_scene);
            SceneManager.LoadScene(prev_scene);
            prev_scene = null;
        }
        // 그 외의 버튼이 눌러졌을 경우
        else
        {
            // 돌아갈 씬의 정보를 갱신
            prev_scene = SceneManager.GetActiveScene().name;
            Debug.Log(prev_scene);
            called = called.Split('_')[1];
            Debug.Log("++++++++++++" + called + "++++++++++++++");
            // 버튼에 따라 씬 이동
            SceneManager.LoadScene(called);
        }
    }
}