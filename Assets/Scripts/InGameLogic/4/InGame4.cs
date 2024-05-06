using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public enum InGame4State
{
    INTRO,
    ONGAME,
    OUTRO
}

public enum InGame4Player
{
    UNCLE,
    NEPHEW
}

public class InGame4Data
{
    public float[] playerTimer = new float[2];

    public bool[] isTimerTouched = new bool[2];

    public bool GetTouch(InGame4Player player)
    {
        return isTimerTouched[(int)player];
    }

    public float GetPlayerTimer(InGame4Player player)
    {
        return playerTimer[(int)player];
    }

    public Winner CompareTimer()
    {
        if ((GetTouch(InGame4Player.UNCLE) == false) && (GetTouch(InGame4Player.NEPHEW) == false))
            return Winner.NONE;
        else if ((GetTouch(InGame4Player.UNCLE) == false) && (GetTouch(InGame4Player.NEPHEW) == true))              //한명이라도 누르면
            return Winner.NEPHEW;
        else if ((GetTouch(InGame4Player.UNCLE) ==true) && (GetTouch(InGame4Player.NEPHEW) == false))              //한명이라도 누르면
            return Winner.UNCLE;                                                                                                        //그놈이 승자
        else
        {
            if (Mathf.Abs(playerTimer[(int)InGame4Player.UNCLE] - 10f) < Mathf.Abs(playerTimer[(int)InGame4Player.NEPHEW] - 10f))
                return Winner.UNCLE;
            else if (Mathf.Abs(playerTimer[(int)InGame4Player.UNCLE] - 10f) > Mathf.Abs(playerTimer[(int)InGame4Player.NEPHEW] - 10f))
                return Winner.NEPHEW;
            else return Winner.NONE;
        }
    }
};

public class InGame4 : MonoBehaviourPunCallbacks
{
    #region InGame4Static

    static InGame4Data data = new InGame4Data();

    public static InGame4 Instance
    {
        get;
        private set;
    }

    public void SetTouch(InGame4Player player, bool isTouched) => pv.RPC("SetTouchWithEveryOne", RpcTarget.All, player, isTouched);
    public bool GetTouch(InGame4Player player) => data.GetTouch(player);
    public void SetTimer(InGame4Player player, float setTimer) => pv.RPC("SetTimerWithEveryOne", RpcTarget.All, player, setTimer);
    public float GetTimer(InGame4Player player) => data.GetPlayerTimer(player);
    public void LoadSceneWithEveryOne(SceneMgr.Scene ingame) => pv.RPC("LoadScene", RpcTarget.All, ingame);
    public static Winner CompareTimer() => data.CompareTimer();
    #endregion

    StateMachine<InGame4> stateMachine;
    EventListener el_OnUICompleted = new EventListener();
    PhotonView pv;

    public InGame4Player _player;

    void Start()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
        stateMachine = new StateMachine<InGame4>(new InGame4_Intro(this, pv, InGame4State.INTRO));

        if (NetworkMgr.Instance.IsMasterClient())   // 플레이어 세팅
            _player = InGame4Player.UNCLE;
        else
            _player = InGame4Player.NEPHEW;

        Instance.SetTouch(InGame4Player.UNCLE, false);
        Instance.SetTouch(InGame4Player.NEPHEW, false);
    }

    void Update()
    {
        stateMachine.Run();
    }

    [PunRPC]
    void SetTimerWithEveryOne(InGame4Player player, float setTimer) 
    {
        data.playerTimer[(int)player] = setTimer;
        InGame4_Ongame.childTimers[(int)player].SetPlayerTimer(setTimer.ToString("F2"));
    }

    [PunRPC]
    void SetTouchWithEveryOne(InGame4Player player, bool isTouched)
    {
        data.isTimerTouched[(int)player] = isTouched;
    }

    public void ChangeStateWithEveryOne(InGame4State nextState) // 전체 클라이언트에게 스테이트를 변경하도록 합니다.
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
    public void ChangeState(InGame4State nextGame4State)
    {
        switch (nextGame4State)
        {
            //게임 시작 후 끝날 때까지
            case InGame4State.ONGAME:
                stateMachine.ChangeState(new InGame4_Ongame(this, pv, InGame4State.ONGAME, _player), StateMachine<InGame4>.StateTransitionMethod.PopNPush);
                break;
            //게임 끝나고 난 후
            case InGame4State.OUTRO:
                stateMachine.ChangeState(new InGame4_Outro(this, pv, InGame4State.OUTRO, _player), StateMachine<InGame4>.StateTransitionMethod.PopNPush);
                break;
        }
    }
}
