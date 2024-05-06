using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public enum RoomState
{
    UNCLEWAIT, // 삼촌 대기 중
    UNCLEREADY, // 삼촌 시작 가능
    NEPHEWLOBBY, // 조카 방 찾는 중
    NEPHEWREADY, // 조카 들어 옴
    UNCLESTART // UNCLE 시작 누름
}

public class Room : MonoBehaviourPunCallbacks
{
    /* Room Canvas */
    [SerializeField] BaseCanvas uncleCanvas;
    [SerializeField] BaseCanvas nephewCanvas;

    public StateMachine<Room> stateMachine;
    EventListener el_OnUICompleted = new EventListener();
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        if (NetworkMgr.Instance.IsMasterClient()) // 방장이면
        {
            uncleCanvas = MonoBehaviour.Instantiate(uncleCanvas);
            stateMachine = new StateMachine<Room>(new RoomState_UncleWait(this, RoomState.UNCLEWAIT));
        }
        else
        {
            uncleCanvas = MonoBehaviour.Instantiate(uncleCanvas);
            nephewCanvas = MonoBehaviour.Instantiate(nephewCanvas);
            uncleCanvas.TurnOffCanvas();
            stateMachine = new StateMachine<Room>(new RoomState_NephewLobby(this, uncleCanvas, nephewCanvas, RoomState.NEPHEWLOBBY));
        }
    }

    void Update()
    {
        stateMachine.Run(); // 스테이트 머신 수행
    }

    public void ChangeState(RoomState nextState, bool ret = false)
    {
        if (ret)
        {
            stateMachine.ChangeState(null, StateMachine<Room>.StateTransitionMethod.ReturnToPrev);
        }

        switch (nextState)
        {
            //삼촌 대기 중
            case RoomState.UNCLEWAIT:
                stateMachine.ChangeState(new RoomState_UncleWait(this, RoomState.UNCLEWAIT), StateMachine<Room>.StateTransitionMethod.JustPush);
                break;

            //삼촌 게임 시작 가능
            case RoomState.UNCLEREADY:
                stateMachine.ChangeState(new RoomState_UncleReady(this, RoomState.UNCLEREADY), StateMachine<Room>.StateTransitionMethod.JustPush);
                break;

            // 조카 방 진입 전
            case RoomState.NEPHEWLOBBY:
                stateMachine.ChangeState(new RoomState_NephewLobby(this, uncleCanvas, nephewCanvas, RoomState.NEPHEWLOBBY), StateMachine<Room>.StateTransitionMethod.JustPush);
                break;

            //조카 방 진입 후
            case RoomState.NEPHEWREADY:
                stateMachine.ChangeState(new RoomState_NephewReady(this, uncleCanvas, nephewCanvas, RoomState.NEPHEWREADY), StateMachine<Room>.StateTransitionMethod.JustPush);
                break;

            // 삼촌이 게임 시작을 누름
            case RoomState.UNCLESTART:
                pv.RPC("GoToPrologue", RpcTarget.All);
                break;
        }
    }

    [PunRPC]
    public void GoToPrologue()
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.Prologue);
    }

}
