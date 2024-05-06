using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PillBottleType
{
    None,
    RedPill50,
    RedPill100,
    YellowPill50,
    YellowPill100
}

public class PillBottle : MonoBehaviour
{
    public static PillBottle Instance
    {
        get;
        private set;
    }

    public void Start()
    {
        Instance = this;
    }
    
    public void SetPillBottle(InGame5Player player)
    {
        if (player == InGame5Player.UNCLE)
            UnclePillBottle();
        else
            NephewPillBottle();
    }

    public void UnclePillBottle(GameObject pill = null)
    {
        PillBottleType pillBottleType = PillBottleType.RedPill50;    // 삼촌의 점수가 10점이라면
        if (InGame5.GetScore(InGame5Player.UNCLE) == 20)   // 삼촌의 점수가 20점이라면
            pillBottleType = PillBottleType.RedPill100;
            
        InstantiatePillBottle(pillBottleType);  // 삼촌 약통 UI 업데이트      
    }

    public void NephewPillBottle(GameObject pill = null)
    {
        PillBottleType pillBottleType = PillBottleType.YellowPill50;    //조카의 점수가 10점이라면
        if (InGame5.GetScore(InGame5Player.NEPHEW) == 20)  // 조카의 점수가 20점이라면
            pillBottleType = PillBottleType.YellowPill100;

        InstantiatePillBottle(pillBottleType);       
    }

    void InstantiatePillBottle(PillBottleType pillBottleType)
    {
        UIMgr.Instance.UI_Instantiate($"InGame5/{pillBottleType.ToString()}", transform);
    }
}
