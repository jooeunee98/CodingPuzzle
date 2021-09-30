using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Raycast 활용해서 코딩블럭 삭제하는 기능을 만드려고 했었는데 실패..
 * 드래그 앤 드랍 개발할 때 Raycast 기능을 활용한다고 알아서 일단 남겨둠
*/

public class MouseManager : MonoBehaviour, IPointerClickHandler
{
    float MaxDistance = 1500000000000000000f;
    Vector3 MousePosition;
    Camera Cam;

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Mouse Click Button : Left");
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Mouse Click Button : Middle");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Mouse Click Button : Right");
        }
        Debug.Log("Mouse Position : " + eventData.position);
        Debug.Log("Mouse Click Count : " + eventData.clickCount);
        //throw new System.NotImplementedException();
    }

    private void Start()
    {
        Cam = GetComponent<Camera>();
    }
    void Update()
    {
        //modifyRay();
    }

    void modifyRay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MousePosition = Input.mousePosition;
            MousePosition = Cam.ScreenToWorldPoint(MousePosition);

            RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, MaxDistance);
            Debug.DrawRay(MousePosition, transform.forward * 10, Color.red, 0.3f);
            if (hit)
            {
                Debug.Log("hit");
            }
        }
    }

        void nedModify()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray Ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(Ray, out hit))
            {
                //hit.transform.GetComponent<SpriteRenderer>().color = Color.red; // 젤랑이 색 변경
                //hit.transform.GetComponent<MeshRenderer>().material.color = Color.red; // 큐브 색 변경
                string block = hit.transform.gameObject.name;
                Debug.Log("block name : " + block);
                // 여러분들 중 예제를 따라하시고 계시는 분은 큐브색 변경 코드를 사용하세요.
            }
            Debug.DrawRay(Ray.origin, Ray.direction * 50, Color.red, 0.3f);

        }
    }
    }
