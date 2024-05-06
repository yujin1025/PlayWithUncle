using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

public class Prologue : MonoBehaviourPunCallbacks
{
    public static Prologue Instance;

    PhotonView pv;
    public static int PrologueProgress = 1;
    bool isLoading = false;

    void Awake()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
    }

    public void PrologueEnd()
    {
        if (NetworkMgr.Instance.IsMasterClient())
        {
            if (!isLoading && NetworkMgr.Instance.IsAllReady())
            {
                isLoading = true;
                pv.RPC("GameStart", RpcTarget.All);
            }
        }

        NetworkMgr.Instance.Ready();
    }

    [PunRPC]
    public void GameStart()
    {
        NetworkMgr.Instance.UnReady();
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.InGame3);    // 이 부분 수정해서 테스트
    }
}
