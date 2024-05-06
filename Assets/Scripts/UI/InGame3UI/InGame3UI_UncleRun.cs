using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame3UI_UncleRun : UIComponent
{
    RectTransform UnclePos;
    public GameObject Uncle;
    void Update()
    {
        UnclePos = Uncle.GetComponent<RectTransform>();
        if (InGame3.GetPlayerPos() == 0)  //삼촌 > 조카
        {
            UnclePos.anchoredPosition = new Vector2(0, 0);
        }
        else if (InGame3.GetPlayerPos() == 1) //삼촌 < 조카
        {
            UnclePos.anchoredPosition = new Vector2(-160, 0);
        }
        else if (InGame3.GetPlayerPos() == 2)  //삼촌 = 조카
        {
            UnclePos.anchoredPosition = new Vector2(-80, 0);
        }

    }
}