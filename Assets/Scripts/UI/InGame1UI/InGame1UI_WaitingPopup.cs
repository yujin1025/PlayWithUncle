using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame1UI_WaitingPopup : UIComponent
{
    /*
    Turn currTurn;
    public GameObject img;

    private void Update()
    {
        if(isPlayer())
        {
            img.SetActive(false);
        }
        else
        {
            img.SetActive(true);
        }
    }
    bool isPlayer()  // 삼촌 턴의 삼촌이거나, 조카 턴의 조카인 경우 player임
    {
        return (NetworkMgr.Instance.IsMasterClient() && currTurn == Turn.UNCLEPOWER)
        || (!NetworkMgr.Instance.IsMasterClient() && currTurn == Turn.NEPHEWPOWER);
    }*/
    /*
    public void SetDescription(string s)
    {
        Text description = transform.Find("Description").Find("Text").GetComponent<Text>();
        description.text = s;
    }

    public void SetTime(int t)
    {
        Text timer = transform.Find("Timer").GetComponent<Text>();
        timer.text = t.ToString();
    }

    public PopupAngle GetPopupAngle()
    {
        return FindObjectOfType<PopupAngle>();
    }

    public PopupPower GetPopupPower()
    {
        return FindObjectOfType<PopupPower>();
    }
    */
}
