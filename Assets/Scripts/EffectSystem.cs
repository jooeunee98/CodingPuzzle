using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSystem : MonoBehaviour
{
    double timer;
    double waitingTime;
    int stageNo = 5;
    int check = 0;

    // Start is called before the first frame update
    void Start()
    {
        StageClearEffect();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitingTime && check == 0)
        {
            //Action
            GameObject stageNoObj = GameObject.Find("Stage" + stageNo); // �ش� �������� ������ ������Ʈ �˻�

            stageNoObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Ǫ������");
            check = 1;
            timer = 0;

        }

    }

    void StageClearEffect()
    {
        // stageNo = ~~ // �������� ��ȣ �޾ƿ���
        GameObject stageNoObj = GameObject.Find("Stage" + stageNo); // �ش� �������� ������ ������Ʈ �˻�

        GameObject prefab = Resources.Load("Prefabs/crtn_poof") as GameObject;
        GameObject crtn_poof = Instantiate(prefab) as GameObject;
        crtn_poof.gameObject.name = "crtn_poof";
        crtn_poof.transform.SetParent(GameObject.Find("Canvas").transform);
        crtn_poof.transform.localPosition = stageNoObj.transform.localPosition;
        crtn_poof.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);

        timer = 0.0f;
        waitingTime = 1.5f;


    }
}
