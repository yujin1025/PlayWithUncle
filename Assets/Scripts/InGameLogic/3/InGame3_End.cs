using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InGame3_End : State<InGame3>
{
    PhotonView pv;

    BaseCanvas canvas;  
    EndingPopup popup;

    float popupDelay = 4f, currDelay = 0.0f;    // 팝업이 떠있는 시간, 경과한 시간
    bool isLoading = false;     // 씬 로드가 한번만 실행되게 제어하는 변수

    Winner winner;

    public InGame3_End(InGame3 owner, PhotonView pv, InGame3State state, InGame3Player player) : base(owner)
    {
        this.pv = pv;
    }

    public override void Enter()
    {
        canvas = UIMgr.Instance.GetCurrentCanvas<UIState_Wait>();   // 현재 캔버스 불러오기

        winner = InGame3.CompareScore();

        if (winner != Winner.NONE)   // 무승부가 아닐 경우
        {           
            popup = UIMgr.Instance.UI_Instantiate("Popup/EndingPopup", canvas.transform).GetComponent<EndingPopup>();   // 엔딩팝업 띄우기
            popup.SetWinner(winner);    // InGame3의 Winner를 엔딩팝업에 표시하고, Winner의 이미지로 교체
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
        if (winner == Winner.NONE)
            InGame3.Instance.LoadSceneWithEveryOne(SceneMgr.Scene.InGame3);
        else
            InGame3.Instance.LoadSceneWithEveryOne(SceneMgr.Scene.InGame4);
    }
}
