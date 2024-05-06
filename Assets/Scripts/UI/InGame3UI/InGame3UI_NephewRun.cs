using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame3UI_NephewRun : UIComponent
{

    RectTransform NephewPos;
    public GameObject Nephew;
    void Update()
    {
        NephewPos = Nephew.GetComponent<RectTransform>();
        if(InGame3.GetPlayerPos()==0)  //삼촌 > 조카
        {
            NephewPos.anchoredPosition = new Vector2(0, 0);
        }    
        else if(InGame3.GetPlayerPos()==1) //삼촌 < 조카
        {
            NephewPos.anchoredPosition = new Vector2(160, 0);
        }
        else if(InGame3.GetPlayerPos()==2)  //삼촌 = 조카
        {
            NephewPos.anchoredPosition = new Vector2(80, 0);
        }

    }

}

