using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // Start is called before the first frame update
    // ĳ���� �� ��(������) �ʱ�ȭ ��ġ
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
        // ������ ĳ���� ����
        Destroy(GameObject.Find("Character"));

        // ������ ȣ��
        GameObject prefab = Resources.Load("Prefabs/Walking") as GameObject;    // ĳ����
        // �����(�ڵ���)�� ������ ������ �ν��Ͻ�ȭ
        GameObject character = Instantiate(prefab) as GameObject;
        // mapBlocks �ؿ��ٰ� ����
        character.transform.SetParent(GameObject.Find("mapBlocks").transform);
        // ù��° ��� ������ ����
        character.transform.localPosition = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
        character.transform.localEulerAngles = new Vector3(spawnRotX, spawnRotY, spawnRotZ);
        character.name = "Character";
        blockManager.deleteAll();
    }
}
