using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MainCanvas에서 JoinButton에 적용되는 스크립트
public class MainJoinBtn : UIComponent, SceneLoadBtn
{
    public void TestJoinBtn()
    {
        LoadScene(SceneMgr.Scene.Room);
    }

    public void LoadScene(SceneMgr.Scene scene)
    {
        SceneMgr.Instance.LoadScene(scene);
    }
}
