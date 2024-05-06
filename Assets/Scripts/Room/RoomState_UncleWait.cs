using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomState_UncleWait : State<Room>
{
    UncleWaitUI waitUI;
    UncleStartUI startUI;

    public RoomState_UncleWait(Room owner, RoomState state) : base(owner)
    {
        UncleRoomCanvas canvas = UIMgr.Instance.GetCurrentCanvas<UIState_Wait>() as UncleRoomCanvas;
        startUI = canvas.GetUIPanel<UncleStartUI>() as UncleStartUI;
        waitUI = canvas.GetUIPanel<UncleWaitUI>() as UncleWaitUI;
    }

    public override void Enter()
    {
        waitUI.SetUI();
        startUI.UnsetUI();
    }

    public override void Execute()
    {
        if (NetworkMgr.Instance.IsFullRoom())
        {
            owner.ChangeState(RoomState.UNCLEREADY);
        }
    }

    public override void Exit()
    {

    }
}
