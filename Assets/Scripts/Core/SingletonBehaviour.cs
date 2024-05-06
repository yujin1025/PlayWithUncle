using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : ManagerBehaviour where T : SingletonBehaviour<T>
{
    public static T Instance { get; protected set; } // 전역적으로 접근할 수 있도록 Instance를 제작합니다.

    public override ManagerBehaviour GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != null && Instance != this) // 반드시 하나만 존재하도록 합니다.
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = (T)this;
        }

        DontDestroyOnLoad(gameObject); // 씬을 넘나들어도 반드시 하나는 존재하도록 합니다.
    }

    public static bool IsSingletonExist() => Instance != null;

}