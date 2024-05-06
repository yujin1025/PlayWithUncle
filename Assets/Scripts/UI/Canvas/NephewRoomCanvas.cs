using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NephewRoomCanvas : BaseCanvas
{
    void Start()
    {
        if (NetworkMgr.Instance.IsConnectedAndReady()) NetworkMgr.Instance.JoinLobby();
    }
}
