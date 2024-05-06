using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkMgr : SingletonBehaviour<NetworkMgr>
{
    void SetStatus(string s = "")
    {
        Debug.Log("----------------------------------------");
        print(s);
        Info();
        Debug.Log("----------------------------------------");
    }

    #region PhotonAPI

    public bool IsMasterClient() => PhotonNetwork.IsMasterClient;
    public bool IsConnectedAndReady() => PhotonNetwork.IsConnectedAndReady;
    public bool IsFullRoom() => PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public void Disconnect() => PhotonNetwork.Disconnect();
    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public bool IsMasterServer() => PhotonNetwork.Server == ServerConnection.MasterServer;
    public bool IsInLobby() => PhotonNetwork.InLobby;
    public bool IsInRoom() => PhotonNetwork.InRoom;
    public bool IsAllReady() // 모든 클라이언트가 준비가 되었음을 체크합니다.
    {
        var players = PhotonNetwork.PlayerList;
        return players.All(p => p.CustomProperties.ContainsKey("Ready") && (bool)p.CustomProperties["Ready"]);
    }
    public void Ready() // 방장에게 준비가 되었음을 알립니다.
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash["Ready"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void UnReady() // 준비를 해제합니다.
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash["Ready"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void IamTheWinner() // 승자의 Properties에 InGame 이름을 저장합니다.
    {
        string nameOfWinGame;
        SceneMgr.scenemapByEnum.TryGetValue(SceneMgr.Instance._currScene, out nameOfWinGame);
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash[nameOfWinGame] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        Debug.Log(nameOfWinGame + PhotonNetwork.LocalPlayer.IsMasterClient);
    }

    public Winner WhoIsWinner() // 최종 승자를 반환합니다.
    {
        var players = PhotonNetwork.PlayerList;

        Debug.Log(players.Length);
        // players가 항상 2명이라는 것이 보장되어 있어야 함.
        if (players[0].CustomProperties.Count > players[1].CustomProperties.Count)  // 누가 인게임에서 더 많이 승리했는지 체크
            return Winner.UNCLE;    // 승자 반환
        else
            return Winner.NEPHEW;

    }

    public override void OnConnectedToMaster()
    {
        SetStatus("서버 접속 완료");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetStatus("서버 접속 끊김");
    }

    public override void OnJoinedLobby()
    {
        SetStatus("로비 접속 완료");
    }

    public bool CreateRoom(string roomName)
    {
        ResourceMgr.resource.SetRoom(roomName);
        return PhotonNetwork.CreateRoom(ResourceMgr.GetCurrentRoomName(), new RoomOptions { MaxPlayers = 2 });
    }

    public bool JoinRoom(string roomName)
    {
        ResourceMgr.resource.JoinRoom(roomName);
        return PhotonNetwork.JoinRoom(roomName);
    }
    public bool LeaveRoom()
    {
        if (!PhotonNetwork.InRoom) // Room이 아닌 상태에서는 씬 로드만 호출합니다.
        {
            SceneMgr.Instance.LoadScene(SceneMgr.Scene.MainPage);
            return false;
        }

        return PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.MainPage); // 메인 페이지로
        ResourceMgr.resource.ResetRoom(); // 방 정보 초기화
    }

    public override void OnCreatedRoom() // Create Room 함수의 콜백 1
    {
        SetStatus("방 만들기 성공");
    }

    public override void OnJoinedRoom() // Create Room 함수의 콜백 2
    {
        SetStatus("방 참여하기 완료");

        if (IsMasterClient())
        {
            SceneMgr.Instance.LoadScene(SceneMgr.Scene.Room);
        }
        else
        {
            UIMgr.Instance.TurnOnPopup("RoomFoundPopup"); // 방을 찾았습니다.
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message) // Create Room 함수 실패 시 콜백
    {
        SetStatus("방 만들기 실패: " + returnCode + ". " + message);

        switch (returnCode)
        {
            case 32766:
                UIMgr.Instance.TurnOnPopup("RepeatedRoomNamePopup");    // 방 이름이 중복됨
                break;
            default:
                UIMgr.Instance.TurnOnPopup("CantJoinPopup");    // 방 생성 실패
                break;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message) // JoinRoom 실패 시 콜백
    {
        SetStatus("방 참여하기 실패: " + returnCode + ". " + message);

        // 필요한 추가 예외는 returnCode를 추가합니다.
        switch (returnCode)
        {
            case 32758:
                UIMgr.Instance.TurnOnPopup("CantFindtheRoomPopup"); // 적절한 방을 찾지 못함
                break;
            case 32765:
                UIMgr.Instance.TurnOnPopup("CantJoinPopup"); // 무언가의 이유로 방에 참여할 수 없음
                break;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetStatus("조카가 방에 입장했습니다.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetStatus("상대방이 방을 나갔습니다.");

        if (SceneMgr.Instance.IsInOnlineScene()) // 룸을 제외한 온라인 씬에서 플레이어가 나간다면 네트워크 팝업을 띄웁니다.
        {
            LeaveRoom();    // 메인 페이지로 복귀하고
            StartCoroutine(OtherNetworkMainPage()); // 팝업을 띄웁니다
        }
        else if (SceneMgr.Instance.IsRoomScene() && ResourceMgr.resource.currPlayer == PlayerEnum.UNCLE) // 삼촌: 룸에서 조카가 나간다면 팝업을 띄웁니다.
        {
            UIMgr.Instance.TurnOnPopup("PlayerLeftPopup");
        }
        else if (SceneMgr.Instance.IsRoomScene() && ResourceMgr.resource.currPlayer == PlayerEnum.NEPHEW)   // 조카: 룸에서 삼촌이 나가면 팝업을 띄웁니다
        {
            LeaveRoom();    // 메인 페이지로 복귀하고
            StartCoroutine(OtherNetworkMainPage()); // 팝업을 띄웁니다
        }
    }

    private IEnumerator OtherNetworkMainPage()
    {
        yield return new WaitUntil(() => {
            Canvas canvas = FindObjectOfType<Canvas>();
            Debug.Log(canvas.name);
            BaseCanvas baseCanvas = canvas.GetComponent<MainCanvas>();
            return baseCanvas != null;
        }); // 메인 캔버스가 켜질때 까지 딜레이 후
        UIMgr.Instance.TurnOnPopup("OtherNetworkPopup");    // 네트워크 팝업을 띄웁니다       
    }

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원 수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대 인원 수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += ResourceMgr.players[i].ToString() + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는가? : " + PhotonNetwork.InLobby);
            print("연결이 됐는가? : " + PhotonNetwork.IsConnected);
        }
    }
    #endregion
}