using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : UIComponent
{
    Cookie newCookie;
    GameObject cookie;

    // 가방 터치시 랜덤 쿠키 생성
    public Cookie NewCookie(bool isUncleCookie)
    {
        if (Random.Range(0, 2) == 0)
            cookie = isUncleCookie ? UIMgr.Instance.UI_Instantiate($"InGame2/UncleCookie1", transform) : 
                UIMgr.Instance.UI_Instantiate($"InGame2/Cookie1", transform);
        else
            cookie = isUncleCookie ? UIMgr.Instance.UI_Instantiate($"InGame2/UncleCookie2", transform) :
                UIMgr.Instance.UI_Instantiate($"InGame2/Cookie2", transform);

        newCookie = cookie.GetComponent<Cookie>();
        return newCookie;
    }

    
}
