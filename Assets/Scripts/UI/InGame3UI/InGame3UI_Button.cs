using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame3UI_Button : UIComponent
{
    InGame3Player player;
    int score = 0;

    // 플레이어를 설정
    public void SetPlayer(InGame3Player player)
    {
        this.player = player;
    }

    // 버튼을 클릭하면 실행되는 함수: 스코어를 증가시키고 설정합니다
    public void OnClicked()
    {
        score++;
        InGame3.Instance.SetScore(player, score);

        
    }
}
