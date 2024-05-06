using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame5UI_Pill : MonoBehaviour
{
    InGame5Player player;
    bool isClicked;

    public void Start()
    {
        isClicked = false;
        if (tag == "UNCLE")
            SetPlayer(InGame5Player.UNCLE);
        else if (tag == "NEPHEW")
            SetPlayer(InGame5Player.NEPHEW);
    }

    // 플레이어를 설정하고 버튼에 Listnener를 추가합니다
    public void SetPlayer(InGame5Player player)
    {
        this.player = player;

        if (InGame5.Instance._player == player) // 삼촌은 빨간약에, 조카는 노란약에 OnClick이벤트가 붙는다
            GetComponent<Button>().onClick.AddListener(() => OnClicked());
    }

    // 버튼을 클릭하면 실행되는 함수: 스코어를 증가시키고 설정합니다
    public void OnClicked()
    {
        if (!isClicked)
        {
            isClicked = true;
            int score = InGame5.GetScore(player);
            InGame5.Instance.SetScore(player, ++score);

            if (player == InGame5Player.UNCLE) InGame5.Uncle_ClickPill(gameObject);
            else if (player == InGame5Player.NEPHEW) InGame5.Nephew_ClickPill(gameObject);

            GetComponentInParent<InGame5UI_Pills>().FindPill(this);
        }
        
    }
}
