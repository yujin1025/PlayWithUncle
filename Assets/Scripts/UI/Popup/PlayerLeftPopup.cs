using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftPopup : UI_Popup
{
    public override void ClosePopup()
    {
        FindObjectOfType<Room>().ChangeState(RoomState.UNCLEWAIT);
        base.ClosePopup();
    }
}
