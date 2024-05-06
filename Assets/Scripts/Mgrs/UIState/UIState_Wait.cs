using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState_Wait : State<UIMgr>
{
    public List<BaseCanvas> _baseCanvasPrefabs = new List<BaseCanvas>(); // 캔버스 리스트를 가지고 대기함

    public UIState_Wait(UIMgr owner, List<BaseCanvas> baseCanvasPrefabs) : base(owner)
    {
        _baseCanvasPrefabs = baseCanvasPrefabs;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
