using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState_Normal : State<UIMgr>
{
    BaseCanvas _canvasPrefab;
    public BaseCanvas _canvas;

    public UIState_Normal(UIMgr owner, BaseCanvas baseCanvasPrefab) : base(owner)
    {
        _canvasPrefab = baseCanvasPrefab;
    }

    public override void Enter()
    {
        _canvas = MonoBehaviour.Instantiate(_canvasPrefab); // Normal State에선 캔버스가 하나만 있음을 보장합니다.
    }

    public override void Execute()
    {

    }
    public override void Exit()
    {
        MonoBehaviour.Destroy(_canvas);
    }
}
