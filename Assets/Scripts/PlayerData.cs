using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int ChapterNumber, StageNumber;
    public int ClearThree, ClearTwo, ClearOne;
    // Start is called before the first frame update
    string stageName = "";
    public int clearResult;

    private void Start()
    {
        stageName = "stage" + ChapterNumber.ToString() + StageNumber.ToString();
    }

    public void saveClearData()
    {
        PlayerPrefs.SetInt(stageName, clearResult);
    }

    public void calculateResult()
    {
        int totalBlock = 4; //ÃÑ ºí·° °¹¼ö ºÒ·¯¿Í¾ßÇÔ

        if (totalBlock < ClearThree)
            clearResult = 3;
        else if (totalBlock < ClearTwo)
            clearResult = 2;
        else
            clearResult = 1;
    }
}
