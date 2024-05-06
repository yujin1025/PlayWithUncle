using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using UnityEngine.UI;

public class Prologue : MonoBehaviourPunCallbacks
{
    public static Prologue Instance;

    PhotonView pv;
    public static int PrologueProgress;
    bool isLoading;
    Text waitingText;

    void Awake()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
        PrologueProgress = 1;
        isLoading = false;
        NetworkMgr.Instance.UnReady();
    }

    public void PrologueEnd()
    {
        NetworkMgr.Instance.Ready();

        if (!isLoading && NetworkMgr.Instance.IsAllReady())
        {
            isLoading = true;
            pv.RPC("GameStart", RpcTarget.All);
        }              
      
        waitingText = FindObjectOfType<Text>();
        if (waitingText) waitingText.text = "다른 플레이어를 기다리는 중...";
    }

    [PunRPC]
    public void GameStart()
    {
        NetworkMgr.Instance.UnReady();
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.InGame1);    // 이 부분 수정해서 테스트
    }
}
