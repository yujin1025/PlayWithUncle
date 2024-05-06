using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//팝업 Prefab의 생성과 삭제 (UI Manager을 판다면 prefab관리는 Manager에서 하도록 수정 - 싱글톤 패턴으로)
public class UI_Popup : MonoBehaviour
{
    //-- prefab 관리 --
    //prefab을 load하고 instatiate하는 함수
    public GameObject UI_Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>($"{path}");
        if (prefab == null)
        {
            Debug.Log($"{path} prefab 로드 실패");
            return null;
        }
        return Instantiate(prefab, parent);
    }

    //prefab을 destroy하는 함수
    public void UI_Destroy(GameObject go)
    {
        if (go == null)
            return;
        Destroy(go);
    }


    //-- Popup 관리 --
    GameObject _popup;

    //팝업 띄우기
    public void ShowPopup(string path, GameObject origin = null)
    {
        Transform parent = null;
        if (origin != null)
            parent = origin.GetComponentInParent<Canvas>().transform;

        _popup = UI_Instantiate($"Popup/{path}", parent);
    }

    //팝업 내리기 (gameObject를 Destroy할지->OnClick에 이벤트로 추가, 멤버변수 _popup을 Destroy할지->스택으로 팝업 관리)
    public virtual void ClosePopup()
    {
        UI_Destroy(gameObject);
    }
}
