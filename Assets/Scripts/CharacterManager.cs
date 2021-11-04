using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float spawnPosX, spawnPosY, spawnPosZ;
    public float spawnRotX, spawnRotY, spawnRotZ;

    void Start()
    {
        spawnPosY = 8;
        CharacterSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CharacterSpawn ()
    {
        // ������ ȣ��
        GameObject prefab = Resources.Load("Prefabs/Walking") as GameObject;
        // �����(�ڵ���)�� ������ ������ �ν��Ͻ�ȭ
        GameObject character = Instantiate(prefab) as GameObject;
        // mapBlocks �ؿ��ٰ� ����
        character.transform.SetParent(GameObject.Find("mapBlocks").transform);
        // ù��° ��� ������ ����
        character.transform.localPosition = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
        character.transform.localEulerAngles = new Vector3(spawnRotX, spawnRotY, spawnRotZ);
        character.name = "Character";
    }
}
