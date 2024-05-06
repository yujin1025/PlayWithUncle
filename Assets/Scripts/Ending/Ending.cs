using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

/*// 게임 전체의 승패를 가르기 위한 클래스
public class WinnerData
{
    public static int[] scores = new int[3]; // 삼촌 승, 조카 승, 무승부의 전체 횟수를 저장
    public static Winner GetWinner() { Winner winner; return winner = scores[(int)Winner.UNCLE] > scores[(int)Winner.NEPHEW] ? Winner.UNCLE : Winner.NEPHEW; }
    // 전체 게임의 승자를 반환. (무승부가 여러번이라도 반드시 삼촌 혹은 조카가 승리하게 되어있음)
}*/

public class Ending : MonoBehaviour
{
    PhotonView pv;
    bool isLoading;
    public Winner winner;
    public UIComponent winUI;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void SetWinUI()
    {
        winner = NetworkMgr.Instance.WhoIsWinner();

        if (winner == Winner.UNCLE)
            winUI = FindObjectOfType<EndingUI_NephewWin>();
        else if (winner == Winner.NEPHEW)
            winUI = FindObjectOfType<EndingUI_UncleWin>();

        winUI.UnsetUI();

        isLoading = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isLoading)
            {
                isLoading = true;
                PhotonNetwork.LeaveRoom();
            }
        }

    }

    [PunRPC]
    public void GameStart()
    {
        NetworkMgr.Instance.UnReady();
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.MainPage);
    }
}
