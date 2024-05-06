using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterUI : UIComponent, IPopup
{
    [SerializeField] InputField RoomNameInput;

    public void OnclickEnterBtn()
    {
        if (RoomNameInput.text != "")
        {
            if(!NetworkMgr.Instance.JoinRoom(RoomNameInput.text)) // 조인 룸 실패
                UIMgr.Instance.TurnOnPopup("CantFindtheRoomPopup", this.gameObject);
        }
    }

    public void OnClickOk() // NetworkMgr이 JoinRoom에 성공할 시 뜨는 팝업이 사용하는 함수
    {
        FindObjectOfType<Room>().ChangeState(RoomState.NEPHEWREADY);
    }
}
