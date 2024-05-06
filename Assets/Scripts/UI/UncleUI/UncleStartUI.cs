using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UncleStartUI : UIComponent
{
    public void UnsetStartBtn()
    {
        GameObject.Find("StartButton").GetComponent<Button>().interactable = false;
    }

    public void StartGame()
    {
        FindObjectOfType<Room>().ChangeState(RoomState.UNCLESTART);
    }
}
