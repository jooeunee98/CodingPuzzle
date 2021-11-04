using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMaterialSetting : MonoBehaviour
{
    Renderer skyRender;
    public float ChapterNumber;
    // Start is called before the first frame update
    void Start()
    {
        skyRender = GetComponent<Renderer>();
        if (ChapterNumber == 1)
        {
            skyRender.material.mainTextureScale = new Vector2(1.23f, 1);
        }
        else if (ChapterNumber == 2)
        {
            skyRender.material.mainTextureScale = new Vector2(-0.42f, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ChapterNumber == 1)
        {

        }
    }
}
