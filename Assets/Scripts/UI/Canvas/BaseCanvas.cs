using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RotaryHeart.Lib.SerializableDictionary;

public class BaseCanvas : MonoBehaviour
{
    // [SerializeField] public List<UIComponent> uiComponentPrefabs = new List<UIComponent>(); // 이제 사용하지 않음 딕셔너리로 옮길 것
    // List<UIComponent> uiComponents = new List<UIComponent>(); // 이제 사용하지 않음 딕셔너리로 옮길 것
    [System.Serializable] class UIComponentDictionary : SerializableDictionaryBase<UIComponentType, UIComponent> { }
    [SerializeField] UIComponentDictionary uiPrefabDictionary = new UIComponentDictionary();

    UIComponentDictionary uiDictionary = new UIComponentDictionary();

    Canvas canvas;
    CanvasScaler scaler;

    protected void Awake()
    {
        canvas = GetComponent<Canvas>();
        scaler = GetComponent<CanvasScaler>();

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        //foreach (var ui in uiComponentPrefabs) // UI를 Awake 시기에 생성합니다.
        //{
        //    uiComponents.Add(Instantiate(ui, transform)); // 캔버스의 자식 컴포넌트로 부착합니다.
        //}

        foreach (var ui in uiPrefabDictionary)
        {
            uiDictionary.Add(ui.Key, Instantiate(ui.Value, transform));
        }
    }

    public virtual void TurnOnCanvas()
    {
        gameObject.SetActive(true);
    }

    public virtual void TurnOffCanvas()
    {
        gameObject.SetActive(false);
    }


    public void SetUIPanel<T>(UIParam param = null, bool isActive = true) where T : UIComponent
    {
        UIComponentType uitype = UIMgr.TypeToUITypeConverter<UIComponentType>(typeof(T));

        if (isActive)
        {
            uiDictionary[uitype].SetUI(param);
        }
        else
        {
            uiDictionary[uitype].UnsetUI();
        }

    }

    public UIComponent GetUIPanel<T>() where T : UIComponent
    {
        UIComponentType uitype = UIMgr.TypeToUITypeConverter<UIComponentType>(typeof(T));

        if (uiDictionary.Count > 0)
        {
            return uiDictionary[uitype];
        }

        return null;
    }
}
