using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

public class InGame1_Turn_GameSet : State<InGame1>
{
    PhotonView pv;
    InGame1Canvas canvas;
    bool isClicked = false;
    public Sprite[] sprites = new Sprite[2];

    
    public InGame1_Turn_GameSet(InGame1 owner, PhotonView pv, Turn turn) : base(owner)
    {
        this.pv = pv;
    }

    public override void Enter() // ResultPopup을 띄웁니다.
    {
        canvas = UIMgr.Instance.GetCurrentCanvas<UIState_Wait>() as InGame1Canvas;

        if(InGame1.GetTotalScore(Turn.UNCLEWAIT)>InGame1.GetTotalScore(Turn.NEPHEWWAIT))
        {
            canvas.SetUIPanel<InGame1UI_ResultPopup>(new ResultUIParam("삼촌", sprites[0]));
        }
        else
        {
            canvas.SetUIPanel<InGame1UI_ResultPopup>(new ResultUIParam("조카", sprites[1]));
        }

        NetworkMgr.Instance.UnReady();
    }

    public override void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("경기 끝!"); // 사후 처리를 합니다.
            
            NetworkMgr.Instance.Ready();
            canvas.SetUIPanel<InGame1UI_ResultPopup>(new ResultUIParam("준비완료!", null));
        }

        if (!isClicked && NetworkMgr.Instance.IsAllReady())
        {
            isClicked = true;
            SceneMgr.Instance.LoadScene(SceneMgr.Scene.InGame2);    // 임시로 코드 작성
            pv.RPC("InGame2Start", RpcTarget.All);                  // 이게 작동을 안해서 
        }
    }

    public override void Exit()
    {

    }

    [PunRPC]
    public void InGame2Start()
    {
        canvas.SetUIPanel<InGame1UI_ResultPopup>(new ResultUIParam("InGame2Start!", null));
        NetworkMgr.Instance.UnReady();
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.InGame2);
    }
}

