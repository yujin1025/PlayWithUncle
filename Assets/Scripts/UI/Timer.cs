using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//타이머 스크립트는 Text 컴퍼넌트가 있는 오브젝트에 붙여야함.
public class Timer : UIComponent
{ 
    Text textTime;
    float time;     // 몇 초짜리 타이머인지
    float timer;    // 현재 카운트 다운된 타이머 시간 (time초부터 0초까지 카운트 다운)

    public float GetTimer() { return timer; }   // 현재 카운트 다운된 타이머 시간을 가져옴

    public void ResetTimer() { textTime.color = Color.black; timer = time + 1; }  // 타이머 초기화

    public void StartTimer(float t, bool showMS = false) // 타이머 시작. Enter()함수에서 실행
    {
        time = t;
        timer = time;
        textTime = this.GetComponent<Text>();
        textTime.color = Color.black;
        ShowTime(showMS, timer);
    }

    public bool TicTokTimer(bool showMS = false) // 타이머 시간 카운트 다운. Execute()함수 마지막 줄에서 실행시켜줘야함.
    {
        if (timer < 0.01f) ShowTime(showMS, 0f);
        if (timer < 0.0f) { return false; }
        if (timer < 4.0f) textTime.color = Color.red;

        ShowTime(showMS, timer);
        
        timer -= Time.deltaTime;
        return true;
    }

    public void ShowTime(bool showMS, float timer)
    {
        if (showMS)
            textTime.text = string.Format("{0:0.00}", timer);   // ms를 두자리 까지 표기 ex)29:99
        else textTime.text = ((int)timer).ToString();                // s만 표기 ex)29
    }   
}
