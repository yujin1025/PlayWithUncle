using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InGame1_Turn_Power : State<InGame1>
{
    InGame1UI_PlayingPopup popup;
    PopupPower power;
    Timer timer;

    PhotonView pv;
    Turn currTurn;
    bool isClicked;

    public InGame1_Turn_Power(InGame1 owner, PhotonView pv, Turn turn) : base(owner)
    {
        currTurn = turn;
        this.pv = pv;
    }

    public override void Enter()
    {
        isClicked = false;

        InGame1Canvas canvas = MonoBehaviour.FindObjectOfType<InGame1Canvas>();
        popup = canvas.GetUIPanel<InGame1UI_PlayingPopup>() as InGame1UI_PlayingPopup;
        power = popup.GetPopupPower();
        power.StartPower();

        timer = canvas.GetComponentInChildren<Timer>();
        timer.StartTimer(10.0f);    //타이머 10초로 시작

        InGame1.Instance.SetScore(currTurn, 0); // 스코어 초기화
    }

    public override void Execute()
    {
        
        if (!isPlayer()) { 
            timer.TicTokTimer(); 
            if(InGame1.GetPowerScore(currTurn) != 0) power.EndPower(); // 앵글 코루틴을 멈춥니다.
            return; 
        }

        if (!isClicked && Input.GetMouseButtonDown(0)) // 제한 시간 내 클릭한 경우
        {
            isClicked = true;
            PowerOver(power.GetScore());
        }

        if (!isClicked && !timer.TicTokTimer()) // 클릭하지 못 했음
        {
            isClicked = true;
            PowerOver(0);
        }
    }

    public override void Exit()
    {
        popup.UnsetUI(); // 턴이 끝나면 팝업을 닫습니다.
        
    }

    bool isPlayer()  // 삼촌 턴의 삼촌이거나, 조카 턴의 조카인 경우 player임
    {
        return (NetworkMgr.Instance.IsMasterClient() && currTurn == Turn.UNCLEPOWER)
        || (!NetworkMgr.Instance.IsMasterClient() && currTurn == Turn.NEPHEWPOWER);
    }

    void PowerOver(int score)
    {
        InGame1.Instance.SetScore(currTurn, score); // 클릭한 순간에 스코어를 기록합니다.
        power.EndPower(); // 앵글 코루틴을 멈춥니다.
        UIMgr.Instance.StartCoroutine(PowerCoroutine());// 0.5초 딜레이 코루틴 시작
    }

    public IEnumerator PowerCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (currTurn == Turn.UNCLEPOWER)
        {
            owner.ChangeStateWithEveryOne(Turn.UNCLEWAIT);  // 애니메이션을 수행합니다.
        }
        else if (currTurn == Turn.NEPHEWPOWER)
        {
            owner.ChangeStateWithEveryOne(Turn.NEPHEWWAIT);
        }
    }
}
