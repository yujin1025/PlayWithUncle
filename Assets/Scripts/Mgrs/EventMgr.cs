using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMgr : SingletonBehaviour<EventMgr> // 이벤트 저장소
{
    public ObserverEvent E_OnUICompleted;
    public ObserverEvent E_OnSceneChanged;
}
