using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame3UI_ArrowN : UIComponent
{
    InGame3Player player;
    public GameObject arrowimg;

    private void Update()
    {
        if(isPlayer()) //삼촌의 경우
        {
            arrowimg.SetActive(false);
        }

        else
        {
            arrowimg.SetActive(true);
        }
    }

    bool isPlayer()  
    {
        return (NetworkMgr.Instance.IsMasterClient() && player == InGame3Player.UNCLE)
        || (!NetworkMgr.Instance.IsMasterClient() && player == InGame3Player.NEPHEW);
    }

}
