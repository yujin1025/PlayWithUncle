using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomState_NephewReady : State<Room>
{
    UncleWaitUI waitUI;
    UncleStartUI startUI;
    UncleRoomNumUI roomUI;
    BaseCanvas uncleCanvas, nephewCanvas;

    public RoomState_NephewReady(Room owner, BaseCanvas uncleCanvas, BaseCanvas nephewCanvas, RoomState state) : base(owner)
    {
        this.uncleCanvas = uncleCanvas;
        this.nephewCanvas = nephewCanvas;
        startUI = uncleCanvas.GetUIPanel<UncleStartUI>() as UncleStartUI;
        waitUI = uncleCanvas.GetUIPanel<UncleWaitUI>() as UncleWaitUI;
        roomUI = uncleCanvas.GetUIPanel<UncleRoomNumUI>() as UncleRoomNumUI;
    }

    public override void Enter()
    {
        nephewCanvas.TurnOffCanvas();
        uncleCanvas.TurnOnCanvas();

        waitUI.UnsetUI();
        startUI.SetUI();
        startUI.UnsetStartBtn(); // 조카는 게임 시작 버튼을 비활성화합니다.

        roomUI.SetRoomName();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
