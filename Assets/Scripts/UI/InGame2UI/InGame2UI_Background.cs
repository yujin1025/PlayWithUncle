using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame2UI_Background : UIComponent
{
    // 배경을 위한 변수들
    float TRACK_LEN = 360f; // 트랙 한칸 거리
    float speed = 9000f;    // 트랙이 이동하는 속도
    Vector2 moveDir = Vector2.up;   // 트랙 이동 방향 - up: 조카, down: 삼촌
    int trackNum = 0;       // 트랙이 이동한 횟수. 12회가 한바퀴    

    // 삼촌을 위한 변수들
    public Cookie[] uncleCookies = new Cookie[3];
    public StartPoint[] uncleTrack = new StartPoint[3];

    InGame2UI_Cookies cookies;

    // 조카를 위한 변수들
    public Cookie firstCookie;
    public Cookie secondCookie;
    EndPoint endPoint;

    
    // 배경 설정 함수
    public override Vector2 GetPos()
    {
        return GetComponent<RectTransform>().anchoredPosition;
    }

    public override void SetPos(Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void SetCookies(InGame2UI_Cookies cookies)
    {
        this.cookies = cookies;
    }

    // 삼촌의 경우 배경 스크롤이 down
    public void SetMoveDirDown()
    {
        moveDir = Vector2.down;
    }

    // 코루틴을 시작하는 함수. 매개변수로 moveDir를 받을 예정
    public void StartScrollCoroutine()
    {
        StartCoroutine(ScrollBG());
    }

    // 배경 스크롤 코루틴
    IEnumerator ScrollBG()
    {       
        Vector2 startPos = GetPos();
        Vector2 endPos = GetPos() + moveDir * TRACK_LEN;
        float countTime = 0f;
        float newPos = 0f;
        trackNum++;

        if (trackNum > 12)  // 배경 위치 초기화 ( 무한 반복 위함 )
        {           
            SetPos(Vector2.zero);
            startPos = GetPos();
            endPos = GetPos() + moveDir * TRACK_LEN;
            trackNum = 1;

            SetCookiePos(); // 쿠키 위치 초기화
        }        

        while (newPos < TRACK_LEN)   // 배경 한칸 이동
        {         
            newPos = countTime * speed;
            SetPos(startPos + moveDir * newPos);
            countTime += Time.deltaTime;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        SetPos(endPos); // 배경 위치 재조정     

        if (NetworkMgr.Instance.IsMasterClient())
            SetUncleCookieParent(2, cookies);   // 삼촌이 터치 할 쿠키의 부모를 cookies로 설정
    }


    // 쿠키 위치 재조정
    void SetCookiePos()
    {
        if (NetworkMgr.Instance.IsMasterClient())
            SetUncleCookiePos();
        else
            SetNephewCookiePos();
    }


    // 조카 쿠키 설정
    public void SetNephewCookies(Cookie newCookie)
    {
        if (firstCookie != null)
            MonoBehaviour.Destroy(firstCookie.gameObject);
        firstCookie = secondCookie;
        secondCookie = newCookie;
    }

    void SetNephewCookiePos()
    {
        endPoint = FindObjectOfType<EndPoint>();
        firstCookie.SetPos(endPoint.GetPos() - moveDir * TRACK_LEN);
        secondCookie.SetPos(endPoint.GetPos());
    }


    // 삼촌 쿠키 설정
    public void SetUncleCookies(int i, Cookie cookie)
    {
        uncleCookies[i] = cookie;
    }

    void SetUncleCookiePos()
    {
        uncleTrack = FindObjectsOfType<StartPoint>();
        uncleCookies[0].SetPos(uncleTrack[2].GetPos());
        uncleCookies[1].SetPos(uncleTrack[1].GetPos());
        uncleCookies[2].SetPos(uncleTrack[0].GetPos());
    }  
 
    public void SetUncleCookiesOrder(Cookie newCookie)
    {
        for (int i = 2; i > 0; i--)
        {
            uncleCookies[i] = uncleCookies[i - 1];
        }
        uncleCookies[0] = newCookie;
    }

    public void SetUncleCookieParent(int i, UIComponent parent)
    {
        uncleCookies[i].SetParent(parent);
    }

    public void DestroyUncleCookie(int i)
    {
        MonoBehaviour.Destroy(uncleCookies[i].gameObject);
    }
}
