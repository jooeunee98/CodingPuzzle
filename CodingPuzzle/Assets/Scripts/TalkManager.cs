using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;                 // < 스테이지 넘버, 대화목록 인덱스 >
    Dictionary<int, Sprite> portraitData;               // 초상화 종류

    public Sprite[] portraitArr;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[] { "안녕?:0", "이 곳에 처음 왔구나?:1", "도비는 자유야!:2", "오류도 해결했어:3" }); // "대사:표정인덱스"
        talkData.Add(100, new string[] { "평범한 나무상자다." });
        talkData.Add(200, new string[] { "누군가 사용했던 흔적이 있는 책상이다." });

        // 캐릭터의 4가지 표정
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);
    }

    public string GetTalk(int stageNum, int talkIndex)      // 대화 내용을 넘겨주는 함수
    {
        if (talkIndex == talkData[stageNum].Length)
            return null;
        else
            return talkData[stageNum][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
