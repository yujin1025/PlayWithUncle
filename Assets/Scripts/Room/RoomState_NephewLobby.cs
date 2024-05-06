using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// State 넘어가는 조건이 EnterUI에 있음

public class RoomState_NephewLobby : State<Room>
{
    BaseCanvas uncleCanvas, nephewCanvas;
    EnterUI nephewEnterUI;

    public RoomState_NephewLobby(Room owner, BaseCanvas uncleCanvas, BaseCanvas nephewCanvas, RoomState state) : base(owner)
    {
        this.uncleCanvas = uncleCanvas;
        this.nephewCanvas = nephewCanvas;
        //nephewEnterUI = nephewCanvas.GetUIPanel<EnterUI>() as EnterUI;
    }

    public override void Enter()
    {
        nephewCanvas.TurnOnCanvas();
        uncleCanvas.TurnOffCanvas();
        //nephewEnterUI.SetUI();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
