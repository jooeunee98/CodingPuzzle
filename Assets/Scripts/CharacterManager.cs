using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 캐릭터 및 별(골지점) 초기화 위치
    public float spawnPosX, spawnPosY, spawnPosZ;
    public float spawnRotX, spawnRotY, spawnRotZ;
    BlockSystem blockManager;
    void Start()
    {
        spawnPosY = 8;
        blockManager = BlockSystem.FindObjectOfType<BlockSystem>();
        CharacterSpawn();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CharacterSpawn ()
    {
        // 기존의 캐릭터 삭제
        Destroy(GameObject.Find("Character"));

        // 프리펩 호출
        GameObject prefab = Resources.Load("Prefabs/Walking") as GameObject;    // 캐릭터
        // 블루존(코딩존)에 생성할 프리펩 인스턴스화
        GameObject character = Instantiate(prefab) as GameObject;
        // mapBlocks 밑에다가 생성
        character.transform.SetParent(GameObject.Find("mapBlocks").transform);
        // 첫번째 블록 위에서 생성
        character.transform.localPosition = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
        character.transform.localEulerAngles = new Vector3(spawnRotX, spawnRotY, spawnRotZ);
        character.name = "Character";
        blockManager.deleteAll();
    }
}
