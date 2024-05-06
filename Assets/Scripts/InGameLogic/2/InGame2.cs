using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public enum InGame2State
{
    INTRO,     // 인트로
    UNCLE,     // 삼촌 화면
    NEPHEW,    // 조카 화면
    END        // 게임 종료
}

public enum InGame2Player
{
    UNCLE,
    NEPHEW
}

public class InGame2Data // 인게임 5용 데이터입니다.
{
    public int[] scores = new int[2]; // scores[0]에는 삼촌 데이터, scores[1]에는 조카 데이터


    public int GetScore(InGame2Player player) // player의 현재 스코어를 반환
    {
        return scores[(int)player];
    }

    public Winner CompareScore()  // player의 점수 비교
    {
        if (scores[0] == scores[1])     // 동점이면
            return Winner.NONE;
        else if (scores[0] > scores[1]) // 삼촌이 이기면
            return Winner.UNCLE;
        else                            // 조카가 이기면
            return Winner.NEPHEW;
    }
}
public class InGame2 : MonoBehaviourPunCallbacks
{
    #region InGame2Static
    static InGame2Data data = new InGame2Data();
    public static InGame2 Instance{ get; private set; }
    public bool endInGame2;

    public void SetScore(InGame2Player player, int score) => pv.RPC("SetScoreWithEveryOne", RpcTarget.All, player, score);
    public static int GetScore(InGame2Player player) => data.GetScore(player);
    public static Winner CompareScore() => data.CompareScore();
    public void LoadSceneWithEveryOne(SceneMgr.Scene ingame) => pv.RPC("LoadScene", RpcTarget.All, ingame);
    public void EndInGame2WithEveryOne() => pv.RPC("EndInGame2", RpcTarget.All);
    #endregion

    StateMachine<InGame2> stateMachine;
    EventListener el_OnUICompleted = new EventListener();
    PhotonView pv;

    // PointerDown, Drag, Up 이벤트를 관리하는 delegate
    #region InGame2Event
   
    public delegate void UIEvent();     // 배경 Event를 제어하는 delegate 
    public UIEvent event_PointerDown;   // 배경 클릭 할 때 실행 
    public UIEvent event_PointerDrag;   // 배경 드래그 할 때 실행
    public UIEvent event_PointerUp;     // 배경 클릭 끝났을 때 실행

    public delegate void CookieEvent(Cookie cookie);    // 쿠키 Event를 제어하는 delegate
    public CookieEvent event_CookieDown;    // 삼촌: 쿠키 클릭할때. 조카: 가방 클릭할때
    public CookieEvent event_CookieDrag;    
    public CookieEvent event_CookieUp;
    #endregion

    void Start()
    {
        endInGame2 = false;
        Instance = this;
        pv = GetComponent<PhotonView>();
        stateMachine = new StateMachine<InGame2>(new InGame2_Intro(this, pv, InGame2State.INTRO));        
    }

    void Update()
    {
        stateMachine.Run(); // 스테이트 머신 수행
    }

    [PunRPC]
    void SetScoreWithEveryOne(InGame2Player player, int score) // 전체 클라이언트에게 스코어를 세팅하도록 합니다.
    {
        data.scores[(int)player] = score;             
    }

    [PunRPC]
    void LoadScene(SceneMgr.Scene ingame) // 전체 클라이언트에게 씬을 로드 합니다.
    {
        NetworkMgr.Instance.UnReady();
        SceneMgr.Instance.LoadScene(ingame);
    }

    [PunRPC]
    void EndInGame2() // 전체 클라이언트에게 스코어를 세팅하도록 합니다.
    {
        endInGame2 = true;
        ChangeState(InGame2State.END);
    }

    [PunRPC]
    public void ChangeState(InGame2State nextState)
    {
        switch (nextState)
        {
            // 인트로
            case InGame2State.INTRO:
                stateMachine.ChangeState(new InGame2_Intro(this, pv, InGame2State.INTRO), StateMachine<InGame2>.StateTransitionMethod.JustPush);
                break;

            // 삼촌 게임 시작
            case InGame2State.UNCLE:
                stateMachine.ChangeState(new InGame2_Uncle(this, pv, InGame2State.UNCLE), StateMachine<InGame2>.StateTransitionMethod.PopNPush);
                break;

            // 조카 게임 시작
            case InGame2State.NEPHEW:
                stateMachine.ChangeState(new InGame2_Nephew(this, pv, InGame2State.NEPHEW), StateMachine<InGame2>.StateTransitionMethod.PopNPush);
                break;

            // 게임 끝
            case InGame2State.END:
                stateMachine.ChangeState(new InGame2_End(this, pv, InGame2State.END), StateMachine<InGame2>.StateTransitionMethod.PopNPush);
                break;
        }
    }
}
