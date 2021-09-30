using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Raycast Ȱ���ؼ� �ڵ��� �����ϴ� ����� ������� �߾��µ� ����..
 * �巡�� �� ��� ������ �� Raycast ����� Ȱ���Ѵٰ� �˾Ƽ� �ϴ� ���ܵ�
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
                //hit.transform.GetComponent<SpriteRenderer>().color = Color.red; // ������ �� ����
                //hit.transform.GetComponent<MeshRenderer>().material.color = Color.red; // ť�� �� ����
                string block = hit.transform.gameObject.name;
                Debug.Log("block name : " + block);
                // �����е� �� ������ �����Ͻð� ��ô� ���� ť��� ���� �ڵ带 ����ϼ���.
            }
            Debug.DrawRay(Ray.origin, Ray.direction * 50, Color.red, 0.3f);

        }
    }
    }
