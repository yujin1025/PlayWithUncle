using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class InGame5_StartGame : State<InGame5>
{
    PhotonView pv;
    InGame5Canvas canvas;

    UIComponent[] playerUI = new UIComponent[2];

    InGame5UI_Arrow arrow;

    public static InGame5UI_Score[] playerScore = new InGame5UI_Score[2];
    public static InGame5UI_Pills[] playerPills = new InGame5UI_Pills[2];

    Timer timer;
    InGame5Player player;

    bool endGame;
    

    public InGame5_StartGame(InGame5 owner, PhotonView pv, InGame5State state, InGame5Player player) : base(owner)
    {
        this.pv = pv;
        this.player = player;
    }

    public override void Enter()
    {
        

        if (canvas == null)
        {
            canvas = UIMgr.Instance.GetBaseCanvasPrefab<InGame5Canvas>() as InGame5Canvas;    // 튜토리얼 캔버스 프리펩을 불러옵니다.
            canvas = MonoBehaviour.Instantiate<InGame5Canvas>(canvas); // 튜토리얼 캔버스를 생성합니다.
        }        

        playerUI[(int)InGame5Player.UNCLE] = canvas.GetComponentInChildren<InGame5UI_Uncle>();  //플레이어 UI가져오기
        playerUI[(int)InGame5Player.NEPHEW] = canvas.GetComponentInChildren<InGame5UI_Nephew>();

        playerScore[(int)InGame5Player.UNCLE] = playerUI[(int)InGame5Player.UNCLE].GetComponentInChildren<InGame5UI_Score>();   //플레이어 스코어 가져오기
        playerScore[(int)InGame5Player.NEPHEW] = playerUI[(int)InGame5Player.NEPHEW].GetComponentInChildren<InGame5UI_Score>();

        playerPills = canvas.GetComponentsInChildren<InGame5UI_Pills>();
        
        arrow = canvas.GetComponentInChildren<InGame5UI_Arrow>();   
        SetArrow();           // 화살표 위치 설정
      
        timer = canvas.GetComponentInChildren<Timer>();
        timer.StartTimer(20.0f, showMS: true);    //타이머 10초로 시작       

        // 변수 초기화
        endGame = false;
        InGame5.Instance.SetScore(player, 0);
    }

    public override void Execute()
    {
        // 20점에 도달 하거나 타임 오버시 게임종료!
        if (!endGame && !timer.TicTokTimer(showMS: true)) // 타이머 가동 중
            EndInGame5();
        else if(!endGame && InGame5.GetScore(player) >= 20)
            EndInGame5();
    }

    public override void Exit()
    {

    }


    public void SetArrow()  // 플레이어 위치를 가르키는 화살표를 플레이어와 연결
    {
        arrow.SetPos(playerUI[(int)player].GetPos());
    }

    public void EndInGame5()
    {
        endGame = true;
        owner.ChangeStateWithEveryOne(InGame5State.END);
    }
}
