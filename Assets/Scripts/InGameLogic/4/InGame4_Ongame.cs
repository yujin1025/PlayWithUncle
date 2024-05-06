using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class InGame4_Ongame : State<InGame4>
{
    PhotonView pv;
    InGame4Canvas canvas;

    InGame4Player player;
    
    InGame4UI_Microwave microwave;
    InGame4UI_Timer mainTimer;                                                          
    public static InGame4UI_ChildTimer[] childTimers = new InGame4UI_ChildTimer[2];

    bool endGame = false;
   
    public InGame4_Ongame(InGame4 owner, PhotonView pv, InGame4State state, InGame4Player player) : base(owner)
    {
        this.pv = pv;
        this.player = player;
    }

    public override void Enter()
    {
        if (canvas == null)
        {
            canvas = UIMgr.Instance.GetBaseCanvasPrefab<InGame4Canvas>() as InGame4Canvas;  // 인게임 캔버스 프리펩을 불러옵니다.
            canvas = MonoBehaviour.Instantiate<InGame4Canvas>(canvas);                      // 인게임 캔버스를 생성합니다.
        }

        microwave = canvas.GetComponentInChildren<InGame4UI_Microwave>();
        
        mainTimer = canvas.GetComponentInChildren<InGame4UI_Timer>();
        childTimers[(int)InGame4Player.UNCLE] = mainTimer.transform.GetChild(0).gameObject.GetComponent<InGame4UI_ChildTimer>();
        childTimers[(int)InGame4Player.NEPHEW] = mainTimer.transform.GetChild(1).gameObject.GetComponent<InGame4UI_ChildTimer>();
        mainTimer.StartTimer();
        
             
    }
    //uncleTime = canvas.GetComponentInChildren<InGame4UI_UncleTime>();
        //nephewTime = canvas.GetComponentInChildren<InGame4UI_NephewTime>();
        //uncleText = uncleTime.GetComponentInChildren<Text>();
        //nephewText = nephewTime.GetComponentInChildren<Text>();   
    public override void Execute()
    {
        if (!endGame &&(((InGame4.Instance.GetTouch(InGame4Player.UNCLE) == true) && (InGame4.Instance.GetTouch(InGame4Player.NEPHEW) == true)) ||
            (mainTimer.GetTime() >= 12f)))
            StopInGame4();
        #region Debugyong
        /*    
        if (Input.GetMouseButtonDown(0)) //컴퓨터에서 테스팅용으로 함.(클릭 감지)
        {
            if (player == InGame4Player.UNCLE)
            {
                InGame4.Instance.SetTimer(InGame4Player.UNCLE, mainTimer.GetTime());
                uncleText.text = mainTimer.GetTimerText();
                InGame4.isTimerTouched[(int)InGame4Player.UNCLE] = true;
                if (player == InGame4Player.NEPHEW)
                    uncleText.text = mainTimer.GetTimerText();
            }
            else 
            {
                nephewText.text = mainTimer.GetTimerText();
                InGame4.isTimerTouched[(int)InGame4Player.NEPHEW] = true;
                if(player==InGame4Player.UNCLE)
                    nephewText.text = mainTimer.GetTimerText();
            }                
        }
        */
        #endregion
        mainTimer.CopyTimer();          //터치 감지 및 검사, 이후 타이머 복사
        mainTimer.FadeOutTimer();       //메인타이머는 2초 후에 완전히 안보임
        mainTimer.TicTokTimer();
    }

    public override void Exit()
    {

    }

    public void StopInGame4() 
    {
        endGame = true;
        microwave.UnsetUI();                                            //전자레인지 꺼짐
        UIMgr.Instance.StartCoroutine(Pausefor2sec());
    }

    /*public void RestartGame()
    {
        mainTimer.ResetTimer();                                             //타이머 초기화
        
        mainTimer.FadeOutTimer();
        microwave.SetUI();                                                  //전자레인지 다시 키기

        childTimers[(int)InGame4Player.UNCLE].SetPlayerTimer("");       //플레이어 타이머도 초기화
        childTimers[(int)InGame4Player.NEPHEW].SetPlayerTimer("");

        InGame4.isTimerTouched[(int)InGame4Player.UNCLE] = false;           //터치 상황 초기화
        InGame4.isTimerTouched[(int)InGame4Player.NEPHEW] = false;
    }
    */
    public void EndInGame4()
    {
        owner.ChangeStateWithEveryOne(InGame4State.OUTRO);
    }
 
    public IEnumerator Pausefor2sec()
    {
        yield return new WaitForSeconds(2f);
        EndInGame4();
    }
}
