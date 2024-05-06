using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame5UI_Score : MonoBehaviour
{
    Text textScore;

    // 스코어를 화면에 표시합니다
    public void SetTextScore(InGame5Player player)
    {
        textScore = GetComponentInChildren<Text>();
        textScore.text = InGame5.GetScore(player).ToString() + "/20";
    }
}
