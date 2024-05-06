using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

// 인게임 1에 하나만 들어갈 매니저임, GameInstance에 포함되지 않기 위해 Mgr 이름 제거함

public enum InGame5State
{
    INTRO,     // 인트로
    GAME,       // 게임 시작
    END        // 게임 종료
}

public enum InGame5Player
{
    UNCLE,
    NEPHEW
}

public class InGame5Data // 인게임 5용 데이터입니다.
{
    public int[] scores = new int[2]; // scores[0]에는 삼촌 데이터, scores[1]에는 조카 데이터


    public int GetScore(InGame5Player player) // player의 현재 스코어를 반환
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

public class InGame5 : MonoBehaviourPunCallbacks
{
    
    #region InGame5Static

    static InGame5Data data = new InGame5Data();    

    public static InGame5 Instance
    {
        get;
        private set;
    }

    public void SetScore(InGame5Player player, int score) => pv.RPC("SetScoreWithEveryOne", RpcTarget.All, player, score); //SetScoreWithEveryOne(player, score);
    public static int GetScore(InGame5Player player) => data.GetScore(player);
    public static Winner CompareScore() => data.CompareScore();
    public void LoadSceneWithEveryOne(SceneMgr.Scene ingame) => pv.RPC("LoadScene", RpcTarget.All, ingame);
    public void DestroyPillWithEveryOne(InGame5Player player, int pillindex) => /*DestroyPill(player, pillindex);*/pv.RPC("DestroyPill", RpcTarget.All, player, pillindex);
    #endregion

    public delegate void UIEvent(GameObject pill);     // 클릭 Event를 제어하는 delegate 
    public static UIEvent Uncle_ClickPill;   // 삼촌 약 클릭 할 때 실행 
    public static UIEvent Nephew_ClickPill;   // 조카 약 클릭 할 때 실행 

    StateMachine<InGame5> stateMachine; // Turn을 매니징하기 위한 StateMachine
    EventListener el_OnUICompleted = new EventListener(); // UI가 모두 생성된 이후에 가져옵니다.
    PhotonView pv;

    public InGame5Player _player;   // 플레이어 구별. 삼촌 or 조카
    
    void Start()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
        stateMachine = new StateMachine<InGame5>(new InGame5_Intro(this, pv, InGame5State.INTRO));  // 인트로

        Uncle_ClickPill = null;
        Nephew_ClickPill = null;

        if (NetworkMgr.Instance.IsMasterClient())   // 플레이어 세팅
            _player = InGame5Player.UNCLE;
        else
            _player = InGame5Player.NEPHEW;
    }

    void Update()
    {
        stateMachine.Run(); // 스테이트 머신 수행
    }

    [PunRPC]
    void SetScoreWithEveryOne(InGame5Player player, int score) // 전체 클라이언트에게 스코어를 세팅하도록 합니다.
    {
        data.scores[(int)player] = score;
        InGame5_StartGame.playerScore[(int)player].SetTextScore(player); // 화면에 보이는 스코어 text를 세팅합니다

        if (score == 10 || score == 20)
            PillBottle.Instance.SetPillBottle(player);
    }

    [PunRPC]
    void LoadScene(SceneMgr.Scene ingame) // 전체 클라이언트에게 씬을 로드 합니다.
    {
        NetworkMgr.Instance.UnReady();
        SceneMgr.Instance.LoadScene(ingame);
    }


    [PunRPC]
    void DestroyPill(InGame5Player player, int pillindex) // 전체 클라이언트에게 이벤트를 발생시킵니다
    {
        InGame5_StartGame.playerPills[(int)player].DestroyPill(pillindex);
    }

    public void ChangeStateWithEveryOne(InGame5State nextState) // 전체 클라이언트에게 스테이트를 변경하도록 합니다.
    {
        pv.RPC("ChangeState", RpcTarget.All, nextState);
    }

    [PunRPC]
    public void ChangeState(InGame5State nextState) // 스테이트 변경
    {
        switch (nextState)
        {
            case InGame5State.GAME: // 게임 시작
                stateMachine.ChangeState(new InGame5_StartGame(this, pv, InGame5State.GAME, _player), StateMachine<InGame5>.StateTransitionMethod.PopNPush);
                break;

            case InGame5State.END:  // 게임 종료
                stateMachine.ChangeState(new InGame5_End(this, pv, InGame5State.END, _player), StateMachine<InGame5>.StateTransitionMethod.PopNPush);
                break;
        }
    }

}
