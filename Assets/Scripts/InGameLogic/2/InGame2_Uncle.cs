using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InGame2_Uncle : State<InGame2>
{
    PhotonView pv;

    InGame2UncleCanvas canvas;
    InGame2UI_Background background;
    InGame2UI_Cookies cookies;
    InGame2UI_HandPoint hand;
    StartPoint[] uncleTracks = new StartPoint[4];
    EndPoint endPoint;
    Timer timer;
    InGame2UI_Score[] uncleScores = new InGame2UI_Score[2];   // 0번째는 삼촌, 1번째는 조카

    Cookie newCookie;
    Vector2 mousePos;

    int uncleScore;

    public InGame2_Uncle(InGame2 owner, PhotonView pv, InGame2State state) : base(owner)
    {
        this.pv = pv;
    }

    public override void Enter()
    {
        // 변수 초기화
        uncleScore = 0;
        InGame2.Instance.SetScore(InGame2Player.UNCLE, 0); // 스코어 초기화

        canvas = UIMgr.Instance.GetBaseCanvasPrefab<InGame2UncleCanvas>() as InGame2UncleCanvas;
        canvas = MonoBehaviour.Instantiate<InGame2UncleCanvas>(canvas);

        background = canvas.GetComponentInChildren<InGame2UI_Background>();
        cookies = canvas.GetComponentInChildren<InGame2UI_Cookies>();
        hand = canvas.GetComponentInChildren<InGame2UI_HandPoint>();
        uncleTracks = canvas.GetComponentsInChildren<StartPoint>();
        endPoint = canvas.GetComponentInChildren<EndPoint>();
        timer = canvas.GetComponentInChildren<Timer>();
        uncleScores = canvas.GetComponentsInChildren<InGame2UI_Score>();          

        // 트랙 1, 2, 3에 쿠키 생성 후, 모든 쿠키의 부모를 background로 설정
        for(int i = 1; i < 4; i++)
        {
            background.SetUncleCookies(i - 1, uncleTracks[i].NewCookie(isUncleCookie: true));  
            background.SetUncleCookieParent(i - 1, background);
        }
        background.SetUncleCookieParent(2, cookies);    // 제일 아래 쿠키의 부모를 cookies로 세팅
        MonoBehaviour.Destroy(uncleTracks[3].gameObject);   // 제일 아래 트랙을 삭제

        background.SetCookies(cookies); // 쿠키즈 설정
        background.SetMoveDirDown();    // 배경 스크롤 아래로 설정
        SetCookieEventTrigger();    // 쿠키에 EventTrigger 설정
        timer.StartTimer(10.0f);            // 타이머 10초로 시작      
    }

    public override void Execute()
    {
        if (!timer.TicTokTimer())   // 타이머 실행
            InGame2.Instance.EndInGame2WithEveryOne();   // 타이머가 종료되면 END State로 넘어간다
        else if (!InGame2.Instance.endInGame2)  // 게임 종료 전이라면
        {
            uncleScores[(int)InGame2Player.UNCLE].SetScore(InGame2.GetScore(InGame2Player.UNCLE));
            uncleScores[(int)InGame2Player.NEPHEW].SetScore(InGame2.GetScore(InGame2Player.NEPHEW));
        }
    }

    public override void Exit()
    {

    }


    /* Cookie Event 설정 */
    // UncleCookie에서 PointerDown, Drag, PointerUp 이벤트가 발생했을 때 실행될 함수를 등록해준다
    void SetCookieEventTrigger()
    {
        owner.event_CookieDown += CookieDown;
        owner.event_CookieDrag += CookieDrag;
        owner.event_CookieUp += CookieUp;
    }

    // 포인터가 눌렸을 때
    void CookieDown(Cookie cookie)
    {
        newCookie = null;           // 쿠키 초기화
        cookie.SetFirstPos();       // 쿠키 지금 위치 저장      
        cookie.UnSetBlocksRaycast();     // 쿠키 Block 비활성화
        cookie.UpdatePos();    // 쿠키 위치 업데이트
        hand.UpdatePos();       // 손 위치 업데이트
    }

    // 드래그중일 때
    void CookieDrag(Cookie cookie)
    {
        cookie.UpdatePos();    // 쿠키 위치 업데이트
        hand.UpdatePos();       // 손 위치 업데이트
    }

    // 포인터가 떨어졌을 때
    void CookieUp(Cookie cookie)
    {
        cookie.SetBlocksRaycast();   // 쿠키 Block 활성화
        hand.ResetPos();       // 손 위치 초기화

        if (endPoint.cookieIN)    // 가방 위에 쿠키가 떨어지면
        {
            newCookie = uncleTracks[0].NewCookie(isUncleCookie: true);    // 랜덤 쿠키 생성
            newCookie.SetParent(background); // 새 쿠키 배경에 종속

            background.SetUncleCookiesOrder(newCookie); // 쿠키들 순서 재정렬
            
            MonoBehaviour.Destroy(cookie.gameObject);   // 쿠키 삭제
            background.StartScrollCoroutine();  // 배경 스트롤           

            InGame2.Instance.SetScore(InGame2Player.UNCLE, ++uncleScore);
        }
        else
        {
            cookie.ResetPos();  // 쿠키 원리 자리로 되돌리기 
        }
    }
}
