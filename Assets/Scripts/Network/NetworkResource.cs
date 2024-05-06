using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[CreateAssetMenu(fileName = "NetworkResource", menuName = "Resources/NetworkResource", order = 0)]

public class NetworkResource : ScriptableObject
{
    public string roomName;
    public PlayerEnum currPlayer;
    public List<PlayerEnum> winnerList = new List<PlayerEnum>();

    public void SetRoom(string name = "")
    {
        ResetRoom();
        roomName = name == "" ? hashFunc() : name;
        currPlayer = PlayerEnum.UNCLE;
    }

    public void JoinRoom(string name)
    {
        ResetRoom();
        roomName = name;
        currPlayer = PlayerEnum.NEPHEW;
    }

    public void ResetRoom()
    {
        roomName = "";
        currPlayer = PlayerEnum.None;
        winnerList.Clear();
    }

    public void SetWinner(PlayerEnum p)
    {
        winnerList.Add(p);
    }

    string hashFunc()
    {
        return "Room" + Random.Range(0, 1000); // 랜덤 방 생성, 단, 같은 해시가 리턴된 경우 처리가 필요하다.
    }

}

public enum PlayerEnum
{
    None,
    UNCLE,
    NEPHEW
}