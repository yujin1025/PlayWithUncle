using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame5UI_Pills : MonoBehaviour
{
    [SerializeField] public InGame5UI_Pill[] pills;

    void Start()
    {
        pills = GetComponentsInChildren<InGame5UI_Pill>();

    }

    public void FindPill(InGame5UI_Pill destroyPill)
    {
        for(int i = 0; i < pills.Length; i++)
        {
            if (pills[i] == null) continue;
            if(destroyPill == pills[i])
            {
                // tag와 index번호를 모든 클라이언트에게 알림
                InGame5.Instance.DestroyPillWithEveryOne(InGame5.Instance._player, i);
            }
        }
    }

    public void DestroyPill(int index)
    {
        Destroy(pills[index].gameObject);
    }

}
