using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_U : MonoBehaviour
{
    public Text valueText;

    void Update()
    {
        valueText.text = InGame1.GetTotalScore2(Turn.UNCLEWAIT).ToString() + "cm";

    }

}
