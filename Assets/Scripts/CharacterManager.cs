using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {

        // ������ ȣ��
        GameObject prefab = Resources.Load("Prefabs/Walking") as GameObject;
        // �����(�ڵ���)�� ������ ������ �ν��Ͻ�ȭ
        GameObject character = Instantiate(prefab) as GameObject;
        // mapBlocks �ؿ��ٰ� ����
        character.transform.SetParent(GameObject.Find("mapBlocks").transform);
        // ù��° ��� ������ ����
        character.transform.localPosition = new Vector3(0, 8, 0);
        character.name = "Character";


       
       
    }
    
// Update is called once per frame
void Update()
    {
        
    }
}
