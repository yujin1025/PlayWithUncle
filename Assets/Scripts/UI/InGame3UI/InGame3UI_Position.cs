using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame3UI_Position : MonoBehaviour
{
    static InGame3Data data = new InGame3Data();
    RectTransform UnclePos;
    RectTransform NephewPos;
    public GameObject Uncle;
    public GameObject Nephew;

    public void SetUI()
    {
        UnclePos = Uncle.GetComponent<RectTransform>();
        if (data.scores[(int)InGame3Player.UNCLE] > data.scores[(int)InGame3Player.NEPHEW]) //삼촌 > 조카
        {
            UnclePos.anchoredPosition = new Vector2(-129, 0);
            NephewPos.anchoredPosition = new Vector2(129, 0);
        }
        else if (data.scores[(int)InGame3Player.UNCLE] < data.scores[(int)InGame3Player.NEPHEW]) //삼촌 < 조카
        {
            UnclePos.anchoredPosition = new Vector2(-149, 0);
            NephewPos.anchoredPosition = new Vector2(149, 0);
        }
        else //삼촌 == 조카
        {
            UnclePos.anchoredPosition = new Vector2(139, 0);
            NephewPos.anchoredPosition = new Vector2(139, 0);
        }

    }
    /*
    public static InGame3UI_Step[] playerScore = new InGame3UI_Step[2];

    InGame3Player player;

    int NephewPos;
    int UnclePose;

    static InGame3Data data = new InGame3Data();


    // Start is called before the first frame update
    void Start()
    {
        data.scores[(int)player] = score;

    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
