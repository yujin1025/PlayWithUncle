using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public abstract class ManagerBehaviour : MonoBehaviourPunCallbacks
{
    public static ManagerBehaviour ManagerInstance { get; protected set; }

    [SerializeField] public GameInstance.ManagerType Type;

    static public GameInstance.ManagerType CachedType = GameInstance.ManagerType.NONE;

    public List<SceneMgr.Scene> LifeCycles { get => _lifeCycles; }

    [SerializeField] public List<SceneMgr.Scene> _lifeCycles;

    public abstract ManagerBehaviour GetInstance();

    protected virtual void Awake()
    {
        CachedType = Type;
    }

}