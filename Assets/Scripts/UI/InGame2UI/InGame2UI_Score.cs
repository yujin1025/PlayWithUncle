using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame2UI_Score : MonoBehaviour
{
    Text textScore;

    private void Start()
    {
        SetScore(0);
    }

    // 플레이어 스코어를 표시합니다
    public void SetScore(int score)
    {
        textScore = GetComponentInChildren<Text>();
        textScore.text = "X " + score.ToString();
    }
}
