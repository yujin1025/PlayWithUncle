using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomState_UncleReady : State<Room>
{
    UncleWaitUI waitUI;
    UncleStartUI startUI;

    public RoomState_UncleReady(Room owner, RoomState state) : base(owner)
    {
        UncleRoomCanvas canvas = UIMgr.Instance.GetCurrentCanvas<UIState_Wait>() as UncleRoomCanvas;
        startUI = canvas.GetUIPanel<UncleStartUI>() as UncleStartUI;
        waitUI = canvas.GetUIPanel<UncleWaitUI>() as UncleWaitUI;
    }

    public override void Enter()
    {
        waitUI.UnsetUI();
        startUI.SetUI();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
