using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr
{
    public static NetworkResource resource = Resources.Load<NetworkResource>("NetworkResource");
    public static PlayerEnum[] players = new PlayerEnum[2] { PlayerEnum.UNCLE, PlayerEnum.NEPHEW };

    public static string GetCurrentRoomName()
    {
        return resource.roomName;
    }
}
