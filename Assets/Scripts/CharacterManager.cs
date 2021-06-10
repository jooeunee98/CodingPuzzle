using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {

        // 프리펩 호출
        GameObject prefab = Resources.Load("Prefabs/Walking") as GameObject;
        // 블루존(코딩존)에 생성할 프리펩 인스턴스화
        GameObject character = Instantiate(prefab) as GameObject;
        // mapBlocks 밑에다가 생성
        character.transform.SetParent(GameObject.Find("mapBlocks").transform);
        // 첫번째 블록 위에서 생성
        character.transform.localPosition = new Vector3(0, 8, 0);
        character.name = "Character";


       
       
    }
    
// Update is called once per frame
void Update()
    {
        
    }
}
