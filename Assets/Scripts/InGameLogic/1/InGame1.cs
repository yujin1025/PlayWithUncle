using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

// 인게임 1에 하나만 들어갈 매니저임, GameInstance에 포함되지 않기 위해 Mgr 이름 제거함

public enum Turn
{
    TUTORIAL,
    UNCLEANGLE,
    UNCLEPOWER,
    UNCLEWAIT,
    NEPHEWANGLE,
    NEPHEWPOWER,
    NEPHEWWAIT,
    GAMESET
}

public class InGame1Data // 인게임 1용 데이터입니다.
{
    public int[] scores = new int[8]; // 각 턴에 따른 데이터, 편히 사용하기 위해 턴의 갯수대로 더미 요소를 넣었음

    public bool IsSuccessShot(Turn turn) // Shot의 성공 여부를 판단합니다.
    {
        if (turn == Turn.UNCLEWAIT) return scores[(int)Turn.UNCLEANGLE] > 0 && scores[(int)Turn.UNCLEPOWER] > 0;
        else if (turn == Turn.NEPHEWWAIT) return scores[(int)Turn.NEPHEWANGLE] > 0 && scores[(int)Turn.NEPHEWPOWER] > 0;
        return false;
    }

    public int GetPowerScore(Turn turn) // 파워 점수 반환
    {
        if (turn == Turn.UNCLEPOWER)
        {
            return scores[(int)Turn.UNCLEPOWER];
        }
        else if (turn == Turn.NEPHEWPOWER)
        {
            return scores[(int)Turn.NEPHEWPOWER];
        }
        return 0;
    }

    public int GetTotalScore(Turn turn) // Shot에 성공했다면 파워 게이지를, 실패했다면 각도의 성공 여부를 리턴합니다.
    {
        if (IsSuccessShot(turn)) //성공했다면
        {
            if (turn == Turn.UNCLEWAIT)
            {
                return scores[(int)Turn.UNCLEPOWER]; //power은 항상 0이상
            }
            else if (turn == Turn.NEPHEWWAIT)
            {
                return scores[(int)Turn.NEPHEWPOWER];
            }
        }
        else if (turn == Turn.UNCLEWAIT) //실패했다면
        {
            return scores[(int)Turn.UNCLEANGLE]; //0또는 1
        }
        else if (turn == Turn.NEPHEWWAIT)
        {
            return scores[(int)Turn.NEPHEWANGLE]; //0또는 1
        }

        return 0;
    }

    public int GetTotalScore2(Turn turn) // 거리 결과
    {
        if (IsSuccessShot(turn)) //성공했다면
        {
            if (turn == Turn.UNCLEWAIT)
            {
                return scores[(int)Turn.UNCLEPOWER]; //power은 항상 0이상
            }
            else if (turn == Turn.NEPHEWWAIT)
            {
                return scores[(int)Turn.NEPHEWPOWER];
            }
        }
        else if (turn == Turn.UNCLEWAIT) //실패했다면
        {
            return 0; 
        }
        else if (turn == Turn.NEPHEWWAIT)
        {
            return 0; 
        }

        return 0;
    }

    public Winner CompareScore()
    {
        if (InGame1.GetTotalScore2(Turn.UNCLEWAIT) == InGame1.GetTotalScore2(Turn.NEPHEWWAIT))
            return Winner.NONE;
        else if (InGame1.GetTotalScore2(Turn.UNCLEWAIT) > InGame1.GetTotalScore2(Turn.NEPHEWWAIT))
            return Winner.UNCLE;
        else
            return Winner.NEPHEW;
    }
}

public class InGame1 : MonoBehaviourPunCallbacks
{
    #region InGame1Static

    static InGame1Data data = new InGame1Data();

    public static InGame1 Instance
    {
        get;
        private set;
    }

    public void SetScore(Turn turn, int score) => pv.RPC("SetScoreWithEveryOne", RpcTarget.All, turn, score);
    public static int GetPowerScore(Turn turn) => data.GetPowerScore(turn);
    public static int GetTotalScore(Turn turn) => data.GetTotalScore(turn);

    public static int GetTotalScore2(Turn turn) => data.GetTotalScore2(turn);

    public static Winner CompareScore() => data.CompareScore(); // Winner를 반환하는 함수
    public void LoadSceneWithEveryOne(SceneMgr.Scene ingame) => pv.RPC("LoadScene", RpcTarget.All, ingame); // 모든 player 씬을 로드 
    #endregion

    StateMachine<InGame1> stateMachine; // Turn을 매니징하기 위한 StateMachine
    EventListener el_OnUICompleted = new EventListener(); // UI가 모두 생성된 이후에 가져옵니다.
    PhotonView pv;
    InGame1Canvas canvas;

    void Start()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
        stateMachine = new StateMachine<InGame1>(new InGame1_Turn_Tutorial(this, pv, Turn.TUTORIAL)); // 튜토리얼로 시작합니다.
    }

    void Update()
    {
        stateMachine.Run(); // 스테이트 머신 수행
    }

    [PunRPC]
    void SetScoreWithEveryOne(Turn turn, int score) // 전체 클라이언트에게 스코어를 세팅하도록 합니다.
    {
        data.scores[(int)turn] = score;
    }


    public void ChangeStateWithEveryOne(Turn nextTurn) // 전체 클라이언트에게 스테이트를 변경하도록 합니다.
    {
        pv.RPC("ChangeState", RpcTarget.All, nextTurn);
    }

    [PunRPC]
    void LoadScene(SceneMgr.Scene ingame) // 전체 클라이언트에게 씬을 로드 합니다.
    {
        NetworkMgr.Instance.UnReady();
        SceneMgr.Instance.LoadScene(ingame);
    }

    [PunRPC]
    public void ChangeState(Turn nextTurn) // 턴 변경
    {
        switch (nextTurn)
        {
            case Turn.UNCLEANGLE: // 삼촌 각도 맞추기
                if(canvas == null)
                {
                    canvas = UIMgr.Instance.GetBaseCanvasPrefab<InGame1Canvas>() as InGame1Canvas;
                    canvas = MonoBehaviour.Instantiate<InGame1Canvas>(canvas);
                }
                stateMachine.ChangeState(new InGame1_Turn_Angle(this, pv, Turn.UNCLEANGLE, canvas), StateMachine<InGame1>.StateTransitionMethod.PopNPush);
                break;

            case Turn.UNCLEPOWER: // 삼촌 파워 채우기
                stateMachine.ChangeState(new InGame1_Turn_Power(this, pv, Turn.UNCLEPOWER), StateMachine<InGame1>.StateTransitionMethod.PopNPush);
                break;

            case Turn.NEPHEWANGLE: // 조카 각도 맞추기
                canvas = UIMgr.Instance.GetCurrentCanvas<UIState_Wait>() as InGame1Canvas;
                stateMachine.ChangeState(new InGame1_Turn_Angle(this, pv, Turn.NEPHEWANGLE, canvas), StateMachine<InGame1>.StateTransitionMethod.PopNPush);
                break;

            case Turn.NEPHEWPOWER: // 조카 파워 채우기
                stateMachine.ChangeState(new InGame1_Turn_Power(this, pv, Turn.NEPHEWPOWER), StateMachine<InGame1>.StateTransitionMethod.PopNPush);
                break;

            case Turn.UNCLEWAIT: // 삼촌 병뚜껑 발사 애니메이션
                stateMachine.ChangeState(new InGame1_Animating(this, Turn.UNCLEWAIT), StateMachine<InGame1>.StateTransitionMethod.PopNPush);
                break;

            case Turn.NEPHEWWAIT: // 조카 병뚜껑 발사 애니메이션
                stateMachine.ChangeState(new InGame1_Animating(this, Turn.NEPHEWWAIT), StateMachine<InGame1>.StateTransitionMethod.PopNPush);
                break;

            case Turn.GAMESET:
                stateMachine.ChangeState(new InGame1_Turn_GameSet(this, pv, Turn.GAMESET), StateMachine<InGame1>.StateTransitionMethod.PopNPush);
                break;
        }
    }

}
