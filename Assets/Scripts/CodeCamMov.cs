using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeCamMov : MonoBehaviour
{

    public float moveSpeed;
    GameObject tr; // 비행기의 현재 x,y 좌표점
    private Vector3 initMousePos;



    // Start is called before the first frame update
    void Start()
    {
        tr = gameObject;
    }




    // Update is called once per frame
    void Update()
    {
        // 마우스 클릭 및 터치했을때
        if (Input.GetMouseButtonDown(0))
        {
            initMousePos = Input.mousePosition;
            initMousePos.z = 10;
            initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);

        }

        // 마우스 드래그시
        if (Input.GetMouseButton(0))
        {
            Vector3 movedPoint = Input.mousePosition;
            movedPoint.z = 10;
            movedPoint = Camera.main.ScreenToWorldPoint(movedPoint);

            Vector3 differencePos = movedPoint - initMousePos;
            differencePos.z = 0;

            initMousePos = Input.mousePosition;
            initMousePos.z = 10;
            initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);

            tr.transform.localPosition = new Vector3(Mathf.Clamp(tr.transform.localPosition.x + differencePos.x, -3.5f, 3.5f),
                tr.transform.localPosition.y,
                Mathf.Clamp(tr.transform.localPosition.z + differencePos.y, -4.5f, 4.5f));
        }

        //마우스 줌인 아웃
        if (Input.GetMouseButton(2)) 
                {
                    transform.Translate(Vector3.right * -Input.GetAxis("Mouse X") * moveSpeed);
                    transform.Translate(transform.up * -Input.GetAxis("Mouse Y") * moveSpeed, Space.World);
                }

                gameObject.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * 10);
    }
    /*
    public void OnDrag()
    {
        int touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            if (prevPos == Vector2.zero)
            {
                prevPos = Input.GetTouch(0).position;
                return;
            }

            Vector2 dir = (Input.GetTouch(0).position - prevPos).normalized;
            Vector3 vec = new Vector3(-(dir.x), -(dir.y), 0);

            cam.position += vec * moveSpeed * Time.deltaTime;
            prevPos = Input.GetTouch(0).position;
        }

        else if (touchCount == 2)
        {
            if (prevDistance == 0)
            {
                prevDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                return;
            }

            float curDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            float move = prevDistance - curDistance;

            Vector3 pos = cam.position;

            if (move < 0) pos.z += moveSpeed * Time.deltaTime * 1;
            else if (move > 0) pos.z -= moveSpeed * Time.deltaTime * 1;

            cam.position = pos;
            prevDistance = curDistance;
        }
    }

    public void exitDrag()
    {

    }
    */
}
