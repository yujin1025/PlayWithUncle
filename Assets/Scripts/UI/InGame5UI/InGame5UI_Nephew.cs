using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame5UI_Nephew : UIComponent
{
    Text textScore;

    public void Start()
    {
        InGame5.Nephew_ClickPill += SetTextScore;  // Pill이 클릭 될 때마다 스코어 Text 갱신

        textScore = GetComponentInChildren<Text>();
        SetTextScore();      
    }

    // 스코어를 화면에 표시합니다
    public void SetTextScore(GameObject pill = null)
    {
        textScore.text = InGame5.GetScore(InGame5Player.NEPHEW).ToString() + "/20";
    }
}
