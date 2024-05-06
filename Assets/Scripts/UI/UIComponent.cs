using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main 0 ~ 20까지 사용할 것
// Prologue 21 ~ 40까지 사용할 것
// Ending 41 ~ 60까지 사용할 것
// UncleRoom 61 ~ 90까지 사용할 것
// NephewRoom 91 ~ 120까지 사용할 것
// InGame1 121 ~ 150까지 사용할 것
// InGame2 151 ~ 170까지 사용할 것
// InGame3 171 ~ 190까지 사용할 것
// InGame4 191 ~ 210까지 사용할 것
// InGame5 211 ~ 230까지 사용할 것
// Ending 231 ~ 250까지 사용할 것

// InGame1UI_PlayingPopup = 121 처럼 Enum 사용 시작처에 적절한 수를 집어넣습니다.
// 이 처리를 하지 않으면 프리팹에 넣어놓은 Enum이 모두 밀림 *** 주의 ***

public enum UIComponentType // 만드는 enum과 UIComponent를 상속받은 클래스의 이름이 정확히 일치해야함!!
{
    MainJoinBtn = 0,
    MainStartBtn,
    CreditBtn,
    SoundBtn,

    PrologueBtn = 21,
    SkipBtn,

    UncleRoomNumUI = 61,
    UncleStartUI,
    UncleWaitUI,

    NephewBackBtn = 91,
    EnterUI,

    InGame1UI_Uncle = 121,
    InGame1UI_Nephew,
    InGame1UI_PlayingPopup,
    InGame1UI_ResultPopup,
    TutorialBtn,

    InGame2UI_Background = 151,
    InGame2UI_UIbundle,
    InGame2UI_HandPoint,
    InGame2UI_Cookies,

    InGame3UI_NephewRun = 171,
    InGame3UI_UncleRun,
    InGame3UI_TimerRun,
    InGame3UI_ButtonRun,
    InGame3UI_ArrowRun,

    InGame4UI_Microwave = 191,    
    InGame4UI_ResultPopup,
    InGame4UI_Timer,

    InGame5UI_Arrow = 211,
    InGame5UI_Nephew,
    InGame5UI_Uncle,
    Timer,

    EndingUI_UncleWin = 231,
    EndingUI_NephewWin,
}

public class UIComponent : MonoBehaviour
{
    public virtual void SetUI(UIParam param = null)
    {
        gameObject.SetActive(true);
    }

    public virtual void UnsetUI()
    {
        gameObject.SetActive(false);
    }

    public virtual Vector2 GetPos()
    {
        return GetComponent<RectTransform>().position;
    }
    
    public virtual void SetPos(Vector2 pos)
    {
        GetComponent<RectTransform>().position = pos;
    }
}

public class ResultUIParam : UIParam
{
    public string winner;
    public Sprite sprite;

    public ResultUIParam(string str, Sprite sprite)
    {
        winner = str;
        this.sprite = sprite;
    }
}

public interface IPopup
{
    void OnClickOk();
}

public interface ISelection
{
    void OnClickYes();
    void OnClickNo();
}

public interface IPanel
{

}

public interface SceneLoadBtn
{
    void LoadScene(SceneMgr.Scene scene);
}