using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cookie : MonoBehaviour
{
    Vector2 mousePos;
    Vector2 firstPos;

    // 쿠키 세팅
    public void SetBlocksRaycast()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void UnSetBlocksRaycast()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public Vector2 GetPos()
    {
        return GetComponent<RectTransform>().position;
    }

    public void SetPos(Vector2 pos)
    {
        GetComponent<RectTransform>().position = pos;
    }

    public void SetParent(UIComponent parent)
    {
        transform.SetParent(parent.gameObject.transform);
    }

    public void SetAlpha(float alpha)
    {
        GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void SetFirstPos()
    {
        firstPos = GetPos();
    }

    public void ResetPos()
    {
        SetPos(firstPos);
    }

    // 쿠키 위치를 마우스 위치로 업데이트.
    public void UpdatePos()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        SetPos(mousePos);
    }
}
