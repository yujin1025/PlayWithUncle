using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame2UI_HandPoint : UIComponent
{
    [SerializeField] Sprite bigHand;
    [SerializeField] Sprite smallHand;

    InGame2 owner;
    Vector2 firstHandPos;
    Vector2 mousePos;
    Image image;

    void Start()
    {
        owner = FindObjectOfType<InGame2>();
        owner.event_PointerDown += UpdatePos;
        owner.event_PointerDrag += UpdatePos;
        owner.event_PointerUp += ResetPos;
        
        image = GetComponentInChildren<Image>();

        firstHandPos = GetPos();
    }

    public void ResetPos()
    {
        image.sprite = bigHand;
        image.SetNativeSize();
        SetPos(firstHandPos);
    }

    // 손 이미지 교체
    // 오브젝트 위치를 마우스 위치로 업데이트
    public void UpdatePos()
    {
        image.sprite = smallHand;
        image.SetNativeSize();

        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        SetPos(mousePos);
    }
}
