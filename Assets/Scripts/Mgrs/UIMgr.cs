 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RotaryHeart.Lib.SerializableDictionary;

public enum CanvasType
{
    MainCanvas = 0,
    PrologueCanvas,
    UncleRoomCanvas,
    NephewRoomCanvas,
    TutorialCanvas,
    SceneLoadCanvas,
    InGame1Canvas,
    InGame2UncleCanvas,
    InGame2NephewCanvas,
    InGame3Canvas,
    InGame4Canvas,
    InGame5Canvas,
    EndingCanvas
}

public class UIMgr : SingletonBehaviour<UIMgr>
{
    StateMachine<UIMgr> stateMachine;
    [System.Serializable] class CanvasDictionary : SerializableDictionaryBase<CanvasType, BaseCanvas> { }
    [SerializeField] CanvasDictionary canvasPrefab_Dictionary = new CanvasDictionary();

    SceneLoadCanvas sceneLoadView = null; // 씬 로드 뷰는 UIMgr이 파괴되어도 살아남아야 하므로 특별 취급함
    UI_Popup popup = null;

    private new void Awake()
    {
        base.Awake();
        popup = GetComponent<UI_Popup>();
        ChangeState(SceneMgr.Instance._currScene);
    }

    void Update()
    {
        stateMachine.Run();
    }

    public void ChangeState(SceneMgr.Scene scene) // 씬에 따라 다른 캔버스 및 사용 스타일 적용
    {
        switch (scene)
        {
            case SceneMgr.Scene.MainPage: // 메인 페이지에는 메인 캔버스만이 존재함
                stateMachine = new StateMachine<UIMgr>(new UIState_Normal(this, canvasPrefab_Dictionary[CanvasType.MainCanvas]));
                break;

            case SceneMgr.Scene.Room: // Room에는 삼촌 캔버스와 조카 캔버스가 존재함, 각 씬에서 독립적으로 제어하도록 양도
                stateMachine = new StateMachine<UIMgr>(new UIState_Wait(this,
                    new List<BaseCanvas>() { canvasPrefab_Dictionary[CanvasType.UncleRoomCanvas], canvasPrefab_Dictionary[CanvasType.NephewRoomCanvas] }));
                break;

            case SceneMgr.Scene.Prologue: // 프롤로그에는 프롤로그 캔버스만이 존재함
                stateMachine = new StateMachine<UIMgr>(new UIState_Normal(this, canvasPrefab_Dictionary[CanvasType.PrologueCanvas]));
                break;

            case SceneMgr.Scene.InGame1: // Ingame1에는 튜토리얼 캔버스와 인게임 캔버스가 존재함, 각 씬에서 독립적으로 제어하도록 양도
                stateMachine = new StateMachine<UIMgr>(new UIState_Wait(this,
                    new List<BaseCanvas>() { canvasPrefab_Dictionary[CanvasType.TutorialCanvas], canvasPrefab_Dictionary[CanvasType.InGame1Canvas] }));
                break;

            case SceneMgr.Scene.InGame2: // ingame2에는 삼촌 캔버스와 조카 캔버스 존재.
                stateMachine = new StateMachine<UIMgr>(new UIState_Wait(this,
                    new List<BaseCanvas>() { canvasPrefab_Dictionary[CanvasType.TutorialCanvas], canvasPrefab_Dictionary[CanvasType.InGame2UncleCanvas], canvasPrefab_Dictionary[CanvasType.InGame2NephewCanvas] }));
                break;
            case SceneMgr.Scene.InGame3: // Ingame3에는 튜토리얼 캔버스와 인게임 캔버스가 존재함, 각 씬에서 독립적으로 제어하도록 양도
                stateMachine = new StateMachine<UIMgr>(new UIState_Wait(this,
                    new List<BaseCanvas>() { canvasPrefab_Dictionary[CanvasType.TutorialCanvas], canvasPrefab_Dictionary[CanvasType.InGame3Canvas] }));
                break;
            case SceneMgr.Scene.InGame4:// Ingame4에는 튜토리얼 캔버스와 인게임 캔버스가 존재함, 각 씬에서 독립적으로 제어하도록 양도
                stateMachine = new StateMachine<UIMgr>(new UIState_Wait(this,
                    new List<BaseCanvas>() { canvasPrefab_Dictionary[CanvasType.TutorialCanvas], canvasPrefab_Dictionary[CanvasType.InGame4Canvas] }));
                break;
            case SceneMgr.Scene.InGame5: // Ingame5에는 튜토리얼 캔버스와 인게임 캔버스가 존재함, 각 씬에서 독립적으로 제어하도록 양도
                stateMachine = new StateMachine<UIMgr>(new UIState_Wait(this,
                    new List<BaseCanvas>() { canvasPrefab_Dictionary[CanvasType.TutorialCanvas], canvasPrefab_Dictionary[CanvasType.InGame5Canvas] }));
                break;
            case SceneMgr.Scene.Ending: // Ending에는 Ending 캔버스만이 존재함
                stateMachine = new StateMachine<UIMgr>(new UIState_Normal(this, canvasPrefab_Dictionary[CanvasType.EndingCanvas]));
                break;
        }
    }

    public GameObject UI_Instantiate(string path, Transform parent = null)
    {
        return popup.UI_Instantiate(path, parent);
    }

    public void TurnOnPopup(string path, GameObject origin = null)
    {
        if (origin == null) // 켜져있는 캔버스 하나 찾아서 박아넣음, 버그 발생 여지 있음
        {
            origin = FindObjectOfType<Canvas>().gameObject;
        }

        popup.ShowPopup(path, origin);
    }

    public BaseCanvas GetCurrentCanvas<T>() where T : State<UIMgr> // 현재 켜져 있는 캔버스를 가져 옵니다.
    {
        if (typeof(T) == typeof(UIState_Normal))
        {
            UIState_Normal state = stateMachine.stateStack.Peek() as UIState_Normal;
            return state._canvas;
        }

        else if (typeof(T) == typeof(UIState_Wait))
        {
            return FindObjectOfType<BaseCanvas>();
        }

        return null;
    }

    public BaseCanvas GetBaseCanvasPrefab<T>() where T : BaseCanvas // 외부에서 캔버스를 생성할 수 있도록 프리팹을 제공합니다.
    {
        CanvasType ct = TypeToUITypeConverter<CanvasType>(typeof(T));
        return (T)canvasPrefab_Dictionary[ct];
    }

    public static T TypeToUITypeConverter<T>(Type t) // Type과 Enum의 이름이 반드시 같아야 합니다.
        => (T)Enum.Parse(typeof(T), t.ToString());

    public void TurnOnSceneLoadUI()
    {
        sceneLoadView = (SceneLoadCanvas)canvasPrefab_Dictionary[CanvasType.SceneLoadCanvas];
        sceneLoadView = Instantiate<SceneLoadCanvas>(sceneLoadView);
        DontDestroyOnLoad(sceneLoadView);
        sceneLoadView.TurnOnCanvas();
    }

    public void TurnOffSceneLoadUI()
    {
        sceneLoadView = FindObjectOfType<SceneLoadCanvas>();

        if (sceneLoadView != null)
        {
            sceneLoadView.TurnOffCanvas();
        }
    }

}
