using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

// <Todo>
// 삼촌 쿠키 터치 시 => Track 터치 시로 변경하기
// PlayerLeftPopup 닫기 버튼이 동작을 안함 => 동작 할 때도 있고 안할때도 있음. POPUP들 확인
// ingame2부터 네트워크 끊겨도 인식 못함 

public class InGame2_Nephew : State<InGame2>
{
    PhotonView pv;

    InGame2NephewCanvas canvas;
    InGame2UI_Background background;
    StartPoint startPoint;
    EndPoint endPoint;
    Timer timer;
    InGame2UI_Score[] nephewScores = new InGame2UI_Score[2];   // 0번째는 삼촌, 1번째는 조카  

    Cookie newCookie;
    Vector2 mousePos;

    int nephewScore;

    /* State */
    public InGame2_Nephew(InGame2 owner, PhotonView pv, InGame2State state) : base(owner)
    {
        this.pv = pv;
    }

    public override void Enter()
    {
        // 변수 초기화
        nephewScore = 0;
        InGame2.Instance.SetScore(InGame2Player.NEPHEW, 0); // 스코어 초기화

        canvas = UIMgr.Instance.GetBaseCanvasPrefab<InGame2NephewCanvas>() as InGame2NephewCanvas;
        canvas = MonoBehaviour.Instantiate<InGame2NephewCanvas>(canvas);

        background = canvas.GetComponentInChildren<InGame2UI_Background>();
        startPoint = canvas.GetComponentInChildren<StartPoint>();
        endPoint = canvas.GetComponentInChildren<EndPoint>();
        timer = canvas.GetComponentInChildren<Timer>();
        nephewScores = canvas.GetComponentsInChildren<InGame2UI_Score>();

        SetBagEventTrigger();    // bag에 EventTrigger 설정
        timer.StartTimer(10.0f);            // 타이머 10초로 시작        
    }

    public override void Execute()
    {
        if (!timer.TicTokTimer())   // 타이머 실행
            InGame2.Instance.EndInGame2WithEveryOne();   // 타이머가 종료되면 END State로 넘어간다
        else if (!InGame2.Instance.endInGame2)  // 게임 종료 전이라면
        {
            nephewScores[(int)InGame2Player.UNCLE].SetScore(InGame2.GetScore(InGame2Player.UNCLE));
            nephewScores[(int)InGame2Player.NEPHEW].SetScore(InGame2.GetScore(InGame2Player.NEPHEW));
        }
        
    }

    public override void Exit()
    {

    }

    /* bag Event 설정 */
    // PointerDown, Drag, PointerUp 이벤트가 발생했을 때 실행될 함수를 등록해준다
    public void SetBagEventTrigger()
    {
        owner.event_CookieDown += CookieDown;
        owner.event_CookieDrag += CookieDrag;
        owner.event_CookieUp += CookieUp;
    }

    // 포인터가 눌렸을 때
    public void CookieDown(Cookie cookie)
    {
        newCookie = null;           // 쿠키 초기화

        newCookie = startPoint.NewCookie(isUncleCookie: false);    // 랜덤 쿠키 생성
        newCookie.UnSetBlocksRaycast();     // 쿠키 Block 비활성화
        newCookie.UpdatePos();    // 쿠키 위치 업데이트
    }

    // 드래그중일 때
    public void CookieDrag(Cookie cookie)
    {
        newCookie.UpdatePos();    // 쿠키 위치 업데이트
    }

    // 포인터가 떨어졌을 때
    public void CookieUp(Cookie cookie)
    {       
        newCookie.SetBlocksRaycast();   // 쿠키 Block 활성화

        if(endPoint.cookieIN)    // Track 위에 쿠키가 떨어지면
        {           
            newCookie.SetParent(background);    // 쿠키 부모 설정
            background.SetNephewCookies(newCookie);   // 쿠키 제거 후 첫번째 쿠키, 두번째 쿠키 설정
            newCookie.SetPos(endPoint.GetPos());  // Track 가운데로 쿠키 정렬
            background.StartScrollCoroutine();  // 배경 스트롤

            InGame2.Instance.SetScore(InGame2Player.NEPHEW, ++nephewScore);
        }
        else
        {
            newCookie.SetAlpha(.5f);    // 쿠키 알파값 흐리게 설정
            MonoBehaviour.Destroy(newCookie.gameObject, .4f); // 쿠키 소멸   
        }
    }
}
