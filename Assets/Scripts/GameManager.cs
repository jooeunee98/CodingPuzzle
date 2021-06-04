using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public BlockManager blockManager;       // 블록 매니저
    public TalkManager talkManager;         // 대화 매니저
    public GameObject talkPanel;            // 대화창 판넬
    public Image portraitImg;               // 대화창 초상화
    public Text talkText;                   // 대화창에 띄울 텍스트
    public GameObject scanObject;           // 대화를 위해 호출된 게임 오브젝트
    public int stageNum;                    // 현재 스테이지 번호
    public bool isAction;                   // 대화창을 띄웠는지 판단
    public int talkIndex;                   

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

    void Talk(int sNumber, bool isNpc)       // 대화를 시도하는 오브젝트에 따라 대사가 나옴
    {
        string talkData = talkManager.GetTalk(sNumber, talkIndex);
        Debug.Log("isNpc" + isNpc);
        if (talkData == null)       // 더 이상 대사가 없다면
        {
            isAction = false;       // 대화 패널을 내리고
            talkIndex = 0;
            return;                 // 종료
        }
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

    public void ClickDirect()
    {
        //Debug.Log("Some button was clicked");
        string clicked = EventSystem.current.currentSelectedGameObject.name;    // 클릭된 버튼이 어느 건지알아옴
        //Debug.Log("button : " + clicked);
        blockManager.insertLast(clicked);                                       // 클릭된 버튼 노드 생성
    }
    public void ClickDelete() // 개발 중..
    {
        blockManager.deleteNode("3");
    }
}