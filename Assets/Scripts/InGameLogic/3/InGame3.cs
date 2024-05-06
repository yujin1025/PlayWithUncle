using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public enum InGame3State
{
    INTRO,
    GAME,
    END
}

public enum InGame3Player
{
    UNCLE,
    NEPHEW
}


public class InGame3Data // 인게임 3용 데이터
{
    public int[] scores = new int[2]; // scores[0]에는 삼촌 데이터, scores[1]에는 조카 데이터

    
    public int GetScore(InGame3Player player) // player의 현재 스코어를 반환
    {
        return scores[(int)player];
    }

    public int GetPlayerPos()
    {
        if(scores[0]>scores[1]) //삼촌 > 조카
        {
            return 0;
        }
        else if(scores[0]<scores[1]) //삼촌 < 조카
        {
            return 1;
        }
        else //삼촌 = 조카
        {
            return 2;
        }
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

public class InGame3 : MonoBehaviourPunCallbacks
{
    #region InGame3Static

    static InGame3Data data = new InGame3Data();
    static float timer = 30.0f;
    public static InGame3 Instance
    {
        get;
        private set;
    }
    public bool endInGame3;

    public void SetScore(InGame3Player player, int score) => pv.RPC("SetScoreWithEveryOne", RpcTarget.All, player, score);
    public static int GetScore(InGame3Player player) => data.GetScore(player);
    public static Winner CompareScore() => data.CompareScore(); // Winner를 반환하는 함수
    public static int GetPlayerPos() => data.GetPlayerPos();
    public void LoadSceneWithEveryOne(SceneMgr.Scene ingame) => pv.RPC("LoadScene", RpcTarget.All, ingame); // 모든 player 씬을 로드 
    #endregion

    StateMachine<InGame3> stateMachine;
    EventListener el_OnUICompleted = new EventListener();
    PhotonView pv;

    public InGame3Player _player;   // 플레이어 구별. 삼촌 or 조카

    void Start()
    {
        endInGame3 = false;
        Instance = this;
        pv = GetComponent<PhotonView>();
        stateMachine = new StateMachine<InGame3>(new InGame3_Intro(this, pv, InGame3State.INTRO));  // 인트로
        
        if (NetworkMgr.Instance.IsMasterClient())   // 플레이어 세팅
            _player = InGame3Player.UNCLE;
        else
            _player = InGame3Player.NEPHEW;
    }

    void Update()
    {
        stateMachine.Run();
    }

    [PunRPC]
    void SetScoreWithEveryOne(InGame3Player player, int score) // 전체 클라이언트에게 스코어를 세팅하도록 합니다.
    {
        data.scores[(int)player] = score;
        InGame3_Game.playerScore[(int)player].SetScore(score); // 화면에 보이는 스코어 text를 세팅합니다
    }

    public void ChangeStateWithEveryOne(InGame3State nextState) // 전체 클라이언트에게 스테이트를 변경하도록 합니다.
    {
        pv.RPC("ChangeState", RpcTarget.All, nextState);
    }

    [PunRPC]
    void LoadScene(SceneMgr.Scene ingame) // 전체 클라이언트에게 씬을 로드 합니다.
    {
        NetworkMgr.Instance.UnReady();
        SceneMgr.Instance.LoadScene(ingame);
    }

    [PunRPC]
    void EndInGame3() // 전체 클라이언트에게 스코어를 세팅하도록 합니다.
    {
        endInGame3 = true;
        ChangeState(InGame3State.END);
    }

    [PunRPC]
    public void ChangeState(InGame3State nextState) // 스테이트 변경
    {
        switch (nextState)
        {
            case InGame3State.GAME: // 게임 중
                stateMachine.ChangeState(new InGame3_Game(this, pv, InGame3State.GAME, _player), StateMachine<InGame3>.StateTransitionMethod.PopNPush);
                break;

            case InGame3State.END:  // 게임 종료
                stateMachine.ChangeState(new InGame3_End(this, pv, InGame3State.END, _player), StateMachine<InGame3>.StateTransitionMethod.PopNPush);
                break;
        }
    }

}
