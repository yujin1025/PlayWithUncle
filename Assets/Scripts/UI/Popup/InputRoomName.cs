using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//SetRoomNamePopup에 InputRoomName.cs 적용
//InputField에 내용 입력시 SetRoomName()실행
//OKButton 클릭시 CreateRoom() 실행
public class InputRoomName : MonoBehaviour
{
    public Text showText;
    public InputField inputText;


    public void CreateRoom()
    {
        if(!NetworkMgr.Instance.CreateRoom(inputText.text))
            UIMgr.Instance.TurnOnPopup("NetworkPopup", gameObject);
    }
}
