using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NephewBackBtn : UIComponent, SceneLoadBtn
{
    [SerializeField] SceneMgr.Scene nextScene;

    void Awake()
    {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(() => LoadScene(nextScene));
    }


    public void LoadScene(SceneMgr.Scene scene)
    {
        NetworkMgr.Instance.LeaveRoom(); // 방을 떠납니다.
    }

}
