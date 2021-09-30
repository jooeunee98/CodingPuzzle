using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class UpAndDown : MonoBehaviour
{
    [Header("속도, 길이")]

    [SerializeField] [Range(0f, 10f)] private float speed = 1f;
    [SerializeField] [Range(0f, 10f)] private float length = 1f;

    private float runningTime = 0f;
    private float yPos = 0f;

    // Use this for initialization
    void Start()
    {

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                runningTime += Time.deltaTime * speed;
                yPos = Mathf.Sin(runningTime) * length;
                Debug.Log(yPos);
                this.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yPos, gameObject.transform.position.z);
            });
    }
}