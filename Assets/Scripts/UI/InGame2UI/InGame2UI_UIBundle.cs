using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGame2UI_UIBundle : UIComponent, IPointerDownHandler, IDragHandler, IPointerUpHandler
{   
    InGame2 owner;
    StartPoint bag;
    EndPoint endPoint;

    Vector2 mousePos;
    bool cookieDown = false;

    private void Start()
    {
        owner = FindObjectOfType<InGame2>();
        bag = GetComponentInChildren<StartPoint>();
        endPoint = GetComponentInChildren<EndPoint>();
    }

    // 배경 클릭 시 손 위치를 이동
    public void OnPointerDown(PointerEventData eventData)
    {
        owner.event_PointerDown();

        // 조카: 가방 터치시 쿠키 생성
        if (!NetworkMgr.Instance.IsMasterClient() && eventData.pointerEnter.gameObject == bag.gameObject)  
        {
            cookieDown = true;
            owner.event_CookieDown(null);
        }
    }

    // 배경 드래그 시 손 위치 업데이트
    public void OnDrag(PointerEventData eventData)
    {
        owner.event_PointerDrag();

        // 조카: 쿠키 위치도 업데이트
        if (cookieDown)                             
            owner.event_CookieDrag(null);
    }

    // 배경 클릭 끝나면 손 위치 원래 자리로 돌리기
    public void OnPointerUp(PointerEventData eventData)
    {
        owner.event_PointerUp();

        // 조카: 쿠키 해제
        if (cookieDown)
        {
            owner.event_CookieUp(null);
            cookieDown = false;
        }
    }
}
