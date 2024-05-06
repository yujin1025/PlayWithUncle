using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public class SceneMgr : SingletonBehaviour<SceneMgr>
{
    
    public enum Scene // 씬의 이름이 담깁니다.
    {
        None = 0,
        MainPage,
        Room,
        Prologue,
        InGame1,
        InGame2,
        InGame3,
        InGame4,
        InGame5,
        Ending
    }

    public Scene _currScene = Scene.None;

    private new void Awake() // SceneMgr이 가장 먼저 생성됨
    {
        base.Awake();
        _currScene = scenemapByString[SceneManager.GetActiveScene().name];
    }

    public static Dictionary<string, Scene> scenemapByString = new Dictionary<string, Scene>()
    {
        {"MainPage", Scene.MainPage},
        {"Room", Scene.Room},
        {"Prologue", Scene.Prologue},
        {"InGame1", Scene.InGame1},
        {"InGame2", Scene.InGame2},
        {"InGame3", Scene.InGame3},
        {"InGame4", Scene.InGame4},
        {"InGame5", Scene.InGame5},
        {"Ending", Scene.Ending}
    };

    public static Dictionary<Scene, string> scenemapByEnum = new Dictionary<Scene, string>()
    {
        {Scene.MainPage, "MainPage"},
        {Scene.Room,  "Room"},
        {Scene.Prologue,  "Prologue"},
        {Scene.InGame1, "InGame1"},
        {Scene.InGame2, "InGame2"},
        {Scene.InGame3, "InGame3"},
        {Scene.InGame4, "InGame4"},
        {Scene.InGame5, "InGame5"},
        {Scene.Ending, "Ending"},
    };

    public void LoadScene(Scene sceneEnum, Action<float> onSceneLoad = null)
    {
        _currScene = sceneEnum;
        SceneManager.sceneLoaded += LoadSceneEnd;
        StartCoroutine(OnLoadSceneCoroutine(sceneEnum, onSceneLoad));
    }

    public bool IsInOnlineScene() => !(_currScene == Scene.MainPage || _currScene == Scene.Room || _currScene == Scene.Ending);
    public bool IsRoomScene() => _currScene == Scene.Room;

    private IEnumerator OnLoadSceneCoroutine(Scene SceneEnum, Action<float> onSceneLoad)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(SceneEnum.ToString(), LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        UIMgr.Instance.TurnOnSceneLoadUI(); // 씬 로딩을 시작하면, 씬 로더 뷰를 생성함

        while (asyncOperation.progress < 0.9f)
        {
            yield return null;

            if (onSceneLoad != null)
                onSceneLoad(asyncOperation.progress);
        }

        asyncOperation.allowSceneActivation = true;
    }

    private void LoadSceneEnd(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name == _currScene.ToString())
        {
            UIMgr.Instance.TurnOffSceneLoadUI();
        }
    }
}
