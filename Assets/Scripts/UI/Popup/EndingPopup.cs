using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Winner
{   
    UNCLE,  // 삼촌 승
    NEPHEW,  // 조카 승
    NONE,   // 무승부
}

// 게임 전체의 승패를 가르기 위한 클래스
public class WinnerScore
{
    public static int[] scores = new int[3]; // 삼촌 승, 조카 승, 무승부의 전체 횟수를 저장
    public static void SetWinner(Winner winner) { scores[(int)winner]++; }   // 승리자 스코어 1점 증가
    public static Winner GetWinner() { Winner winner; return winner = scores[(int)Winner.UNCLE] > scores[(int)Winner.NEPHEW] ? Winner.UNCLE : Winner.NEPHEW; }
    // 전체 게임의 승자를 반환. (무승부가 여러번이라도 반드시 삼촌 혹은 조카가 승리하게 되어있음)

}

public class EndingPopup : MonoBehaviour
{
    [SerializeField] GameObject uncleImage;
    [SerializeField] GameObject nephewImage;
    [SerializeField] GameObject GameEndImage;
    [SerializeField] public Text winText;   // 테스트 중

    public void SetWinner(Winner winner)
    {
        if (winner == Winner.NONE)       // 무승부
        {
            return;
        }
        else if(winner == Winner.UNCLE) // 삼촌 승
        {
            if (NetworkMgr.Instance.IsMasterClient())   // 삼촌이 이겼고 삼촌인 경우 위너 설정
                NetworkMgr.Instance.IamTheWinner();

            Invoke("UncleDelay", 2f);
        }
        else                            // 조카 승
        {
            if (!NetworkMgr.Instance.IsMasterClient())  // 조카가 이겼고 조카인 경우 위너 설정
                NetworkMgr.Instance.IamTheWinner();

            Invoke("NephewDelay", 2f);
        }
    }

    void UncleDelay()
    {
        GameEndImage.SetActive(false);
        uncleImage.SetActive(true);
    }

    void NephewDelay()
    {
        GameEndImage.SetActive(false);
        nephewImage.SetActive(true);
    }
}
