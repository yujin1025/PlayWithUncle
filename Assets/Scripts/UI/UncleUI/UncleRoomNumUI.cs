using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UncleRoomNumUI : UIComponent
{
    public Text titleText;

    public void Awake()
    {
        SetRoomName();
    }   

    public void SetRoomName()
    {
        titleText.text = ResourceMgr.GetCurrentRoomName();
    }
}
