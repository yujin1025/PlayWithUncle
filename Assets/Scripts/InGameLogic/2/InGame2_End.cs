using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InGame2_End : State<InGame2>
{
    PhotonView pv;

    BaseCanvas canvas;
    EndingPopup popup;

    float popupDelay = 4f, currDelay = 0.0f;
    bool isLoading = false;


    public InGame2_End(InGame2 owner, PhotonView pv, InGame2State state) : base(owner)
    {
        this.pv = pv;
    }

    public override void Enter()
    {
        canvas = UIMgr.Instance.GetCurrentCanvas<UIState_Wait>();

        if (InGame2.CompareScore() != Winner.NONE)  // 무승부가 아닐 경우
        {
            popup = UIMgr.Instance.UI_Instantiate("Popup/EndingPopup", canvas.transform).GetComponent<EndingPopup>();   // 엔딩팝업 띄우기
            popup.SetWinner(InGame2.CompareScore());    // InGame2의 Winner를 엔딩팝업에 표시하고, Winner의 이미지로 교체            
        }
        else // 무승부일 경우 
        {
            UIMgr.Instance.UI_Instantiate("Popup/TiePopup", canvas.transform); // 무승부 팝업 띄우기
        }

        NetworkMgr.Instance.UnReady();
    }

    public override void Execute()
    {
        currDelay += Time.deltaTime;
        
        if (!isLoading && currDelay > popupDelay) // 딜레이 후
        {            
            NetworkMgr.Instance.Ready();

            if (NetworkMgr.Instance.IsMasterClient() && NetworkMgr.Instance.IsAllReady())
            {
                isLoading = true;
                NextInGame();
            }
        }      
    }

    public override void Exit()
    {

    }

    void NextInGame()
    {
        NetworkMgr.Instance.UnReady();
        if (InGame2.CompareScore() == Winner.NONE)
            InGame2.Instance.LoadSceneWithEveryOne(SceneMgr.Scene.InGame2);
        else
            InGame2.Instance.LoadSceneWithEveryOne(SceneMgr.Scene.InGame3);
    }
}
