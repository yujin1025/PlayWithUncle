using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame3UI_Step : MonoBehaviour
{
    Text textStep;

    void Start()
    {
        textStep = GetComponentInChildren<Text>();
        textStep.text = "0";
    }

    // 플레이어 스코어를 표시합니다
    public void SetScore(int step)
    {
        textStep = GetComponentInChildren<Text>();
        textStep.text = step.ToString();
    }

}
