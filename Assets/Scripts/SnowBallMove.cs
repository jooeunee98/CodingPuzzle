using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// (중요!!) 눈굴리기 명령어가 스택에서 나올차례일때마다 눈덩이에 이 스크립트 컴포넌트 넣기 << 완성 후 CharaterManager.cs 에 22, 23번 라인 삭제 (테스트 용임)

public class SnowBallMove : MonoBehaviour
{
    bool CAN_MOVE = false;
    private IEnumerator coroutine;
    int BALL_X, BALL_Y, BALL_Z;
    bool GO = false;
    // Start is called before the first frame update
    void Start()
    {

        GameObject Boy = GameObject.Find("Character");
        if (gameObject.transform.position.x - Boy.transform.position.x > 1 || gameObject.transform.position.x - Boy.transform.position.x <= -1)
        {
            BALL_X = 1; BALL_Y = 0; BALL_Z = 0;
        }
        else if (gameObject.transform.position.z - Boy.transform.position.z > 1 || gameObject.transform.position.z - Boy.transform.position.z <= -1)
        {
            BALL_X = 0; BALL_Y = 0; BALL_Z = 1;
        }

        Coru();

    }
    public void Coru()
    {
        
        coroutine = MOVE_BALL(1.0f);
        
        StartCoroutine(coroutine);

    }
    // Update is called once per frame


    public IEnumerator MOVE_BALL(float waitTime)
    {

        {
            waitTime = 1f;
            GO = true;


            yield return new WaitForSeconds(waitTime);

            Destroy(gameObject.GetComponent<SnowBallMove>());

        }
        GO = false;

    }

    void Update()
    {

        if (GO == true)
        {
            Debug.Log(BALL_X + " " + BALL_Y + " " + BALL_Z);
            gameObject.transform.Translate(new Vector3(BALL_X, BALL_Y, BALL_Z) * Time.deltaTime * 5.75f, Space.World);
            gameObject.transform.Rotate(0.6f, 0, 0);
        }
    }
}
