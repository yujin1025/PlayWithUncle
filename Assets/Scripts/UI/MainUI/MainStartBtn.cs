using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MainCanvas에서 StartButton에 적용되는 스크립트
public class MainStartBtn : UIComponent
{
    //방 생성
    public void StartBtnClick()
    {
        if (!NetworkMgr.Instance.IsConnectedAndReady() && !NetworkMgr.Instance.IsMasterServer())
        {
            UIMgr.Instance.TurnOnPopup("OfflinePopup", gameObject);
        }
        else
        {
            //SceneMgr.Instance.LoadScene(SceneMgr.Scene.InGame5); //<--테스트중
            UIMgr.Instance.TurnOnPopup("SetRoomNamePopup", gameObject);
        }
    }
}
