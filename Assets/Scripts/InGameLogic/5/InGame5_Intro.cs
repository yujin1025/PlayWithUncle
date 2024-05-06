using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InGame5_Intro : State<InGame5>
{
    PhotonView pv;
    TutorialCanvas tutorialCanvas;
    TutorialBtn tutorialBtn;

    int tutorialCount = 2, tutorialProgress = 0;
    float btnDelay = 1.5f, currDelay = 0.0f;

    public InGame5_Intro(InGame5 owner, PhotonView pv, InGame5State state) : base(owner)
    {
        this.pv = pv;
    }

    public override void Enter()
    {
        tutorialCanvas = UIMgr.Instance.GetBaseCanvasPrefab<TutorialCanvas>() as TutorialCanvas;    // 튜토리얼 캔버스 프리펩을 불러옵니다.
        tutorialCanvas = MonoBehaviour.Instantiate<TutorialCanvas>(tutorialCanvas); // 튜토리얼 캔버스를 생성합니다.
        tutorialBtn = tutorialCanvas.GetTutorialBtn(TutorialType.INGAME5);
        tutorialBtn.TutorialProgress(ref tutorialCount, ref tutorialProgress);
    }

    public override void Execute()
    {
        currDelay += Time.deltaTime;

        if (currDelay > btnDelay) // 딜레이 후
        {
            currDelay = 0.0f;
            NextTutorial(); // 튜토리얼을 진행시킵니다.
        }
    }

    public override void Exit()
    {
        MonoBehaviour.Destroy(tutorialCanvas.gameObject);
    }


    public void NextTutorial()
    {
        if (!tutorialBtn.TutorialProgress(ref tutorialCount, ref tutorialProgress)) // 튜토리얼을 넘깁니다.
        {

            if (NetworkMgr.Instance.IsAllReady())
            {
                NetworkMgr.Instance.UnReady();
                StartInGame5();
            }
        }

        if (tutorialCount <= tutorialProgress) // 모두 진행이 되었다면
        {
            NetworkMgr.Instance.Ready();
        }

    }

    public void StartInGame5()  // 모든 플레이어 게임 스타트
    {
        owner.ChangeStateWithEveryOne(InGame5State.GAME); 
        //owner.ChangeState(InGame5State.GAME);   // 테스트 중
    }
}
