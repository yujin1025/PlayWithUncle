using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class SceneChanged : EventArgs
{
    public SceneMgr.Scene _scene;

    public SceneChanged(SceneMgr.Scene scene)
    {
        _scene = scene;
    }
}

public class UIParam : EventArgs
{

}

[CreateAssetMenu(fileName = "E_OnXXX", menuName = "New Event", order = 1)]

public class ObserverEvent : ScriptableObject
{
    List<EventListener> eventListeners = new List<EventListener>();
    [SerializeField] public int listenerCount;

    public void Register(EventListener listener, UnityAction<EventArgs> action) // 리스너 객체와 콜백을 등록합니다.
    {
        listener.OnNotify.AddListener(action);
        eventListeners.Add(listener);

        listenerCount = eventListeners.Count;
    }

    public void UnRegister(EventListener listener) // 리스너 객체가 듣고 있는 콜백을 해제합니다.
    {
        eventListeners.Remove(listener);
        listenerCount = eventListeners.Count;
    }

    public void Invoke(EventArgs param = null) // 이벤트를 발생시켜, 구독 중인 리스너들의 콟백을 수행합니다.
    {
        foreach (var listener in eventListeners)
        {
            listener.OnNotify.Invoke(param);
        }

        listenerCount = eventListeners.Count;
    }
}
