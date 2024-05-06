using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class InGame3_Game : State<InGame3>
{
    InGame3Canvas canvas;
    PhotonView pv;
    UIComponent[] playerUI = new UIComponent[2];
    public static InGame3UI_Step[] playerScore = new InGame3UI_Step[2];


    Timer timer;
    InGame3Player player;

    InGame3UI_Button btn;
    public InGame3_Game(InGame3 owner, PhotonView pv, InGame3State state, InGame3Player player) : base(owner)
    {
        this.pv = pv;
        this.player = player;
    }


    public override void Enter()
    {
        //동시에 실행시 어떻게 해야할지 고민해보기
        if (canvas == null)
        {
            canvas = UIMgr.Instance.GetBaseCanvasPrefab<InGame3Canvas>() as InGame3Canvas;    // 튜토리얼 캔버스 프리펩을 불러옵니다.
            canvas = MonoBehaviour.Instantiate<InGame3Canvas>(canvas); // 튜토리얼 캔버스를 생성합니다.
        }

        playerUI[(int)InGame3Player.UNCLE] = canvas.GetComponentInChildren<InGame3UI_UncleRun>();  //플레이어 UI가져오기
        playerUI[(int)InGame3Player.NEPHEW] = canvas.GetComponentInChildren<InGame3UI_NephewRun>();
        

        playerScore[(int)InGame3Player.UNCLE] = playerUI[(int)InGame3Player.UNCLE].GetComponentInChildren<InGame3UI_Step>();   //플레이어 스코어 가져오기
        playerScore[(int)InGame3Player.NEPHEW] = playerUI[(int)InGame3Player.NEPHEW].GetComponentInChildren<InGame3UI_Step>();


        btn = canvas.GetComponentInChildren<InGame3UI_Button>();
        btn.SetPlayer(player);

        timer = canvas.GetComponentInChildren<Timer>();
        timer.StartTimer(5.0f, showMS: true);    //타이머 5초로 시작


        InGame3.Instance.SetScore(player, 0);
    }


    public override void Execute()
    {
        if(!timer.TicTokTimer(showMS: true))    // 타이머 실행
            owner.ChangeStateWithEveryOne(InGame3State.END); // 타이머가 종료되면 END State로 넘어간다
        else if (!InGame3.Instance.endInGame3)  // 게임 종료 전이라면
        {
            playerScore[(int)InGame3Player.UNCLE].SetScore(InGame3.GetScore(InGame3Player.UNCLE));
            playerScore[(int)InGame3Player.NEPHEW].SetScore(InGame3.GetScore(InGame3Player.NEPHEW));
        }

    }

    public override void Exit()
    {

    }

}

