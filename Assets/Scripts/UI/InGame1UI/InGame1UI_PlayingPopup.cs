using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame1UI_PlayingPopup : UIComponent
{
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

    public void ChangeUIColor(float greyAmount)
    {
        Image backPopup = GetComponent<Image>();
        backPopup.material.SetFloat("_GrayscaleAmount", greyAmount);

        foreach (Image i in GetComponentsInChildren<Image>())
        {
            i.material.SetFloat("_GrayscaleAmount", greyAmount);
        }
    }
    
    public void SetNephewHand()
    {
        GameObject uncleHand = transform.Find("UncleHand").gameObject;
        uncleHand.SetActive(false);     
    }

    public PopupAngle GetPopupAngle()
    {
        return FindObjectOfType<PopupAngle>();
    }

    public PopupPower GetPopupPower()
    {
        return FindObjectOfType<PopupPower>();
    }

}
