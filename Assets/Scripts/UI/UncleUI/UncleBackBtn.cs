using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncleBackBtn : UIComponent
{
    public void LeaveRoom()
    {
        NetworkMgr.Instance.LeaveRoom(); // 방을 떠납니다.
    }
}
