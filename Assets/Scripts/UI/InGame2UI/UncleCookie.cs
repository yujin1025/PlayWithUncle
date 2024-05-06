using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 클릭이 가능한 삼촌쿠키
public class UncleCookie : Cookie, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    InGame2 owner;   

    void Start()
    {
        owner = FindObjectOfType<InGame2>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        owner.event_CookieDown(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        owner.event_CookieDrag(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        owner.event_CookieUp(this);
    }

}
