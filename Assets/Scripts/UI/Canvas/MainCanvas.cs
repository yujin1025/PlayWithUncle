using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : BaseCanvas
{
    private void Start()
    {
        if (!NetworkMgr.Instance.IsConnectedAndReady()) NetworkMgr.Instance.Connect();
    }
}
