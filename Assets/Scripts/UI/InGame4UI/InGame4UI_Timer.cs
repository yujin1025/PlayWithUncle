using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame4UI_Timer : UIComponent
{
    Text textTime;
    float time;     
    float timer;    // 현재 카운트 업된 타이머 시간 (0초부터 카운트 업)
    float unitTime = 0f;
    float fadeTime = 2f;

    public string GetTimerText() { return textTime.text; }   // 현재 카운트 업된 타이머 텍스트를 가져옴

    public float GetTime() { return timer; } // 현재 카운트 업된 타이머 float를 가져옴

    public void ResetTimer() { timer = time; }  // 타이머 초기화

    public void TicTokTimer() // 타이머 시간 카운트업. Execute()함수 마지막 줄에서 실행시켜줘야함. 완료.
    {
        if (timer > 12f) return;        
        textTime.text = string.Format("{0:0.00}", timer);   // ms를 두자리 까지 표기 ex)29:99
        timer += Time.deltaTime;
    }

    public void StartTimer(float t = 0f) // 타이머 시작. Enter()함수에서 실행. 완료.
    {
        time = t;
        timer = time;
        textTime = this.GetComponent<Text>();

        textTime.text = string.Format("{0:0.00}", timer);   // ms를 두자리 까지 표기
    }

    public void FadeOutTimer()      //main타이머 2초동안 흐려짐. 완료.
    {                 
        unitTime += Time.deltaTime / fadeTime;
        Color fadeColor = textTime.color;
        //게임 재시작 대비해서 이렇게 해놓음.
        if (unitTime > 0f && unitTime < 1f)
        {
            fadeColor.a = Mathf.Lerp(1, 0, unitTime);
            textTime.color = fadeColor;
        }
        else return;
    }

    public void CopyTimer()         //터치 감지 시 삼촌/조카의 타이머 각 화면에 동기화. 완료.
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (InGame4.Instance._player == InGame4Player.UNCLE)
            {
                if (InGame4.Instance.GetTouch(InGame4Player.UNCLE) == true)
                    return;
                InGame4.Instance.SetTimer(InGame4Player.UNCLE, GetTime());               
                InGame4.Instance.SetTouch(InGame4Player.UNCLE, true);
            }
            else
            {
                if (InGame4.Instance.GetTouch(InGame4Player.NEPHEW) == true)
                    return;
                InGame4.Instance.SetTimer(InGame4Player.NEPHEW, GetTime());                
                InGame4.Instance.SetTouch(InGame4Player.NEPHEW, true);
            }               
        }
    }   

}
