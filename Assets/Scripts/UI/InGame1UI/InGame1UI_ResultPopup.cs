using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame1UI_ResultPopup : UIComponent
{
    Image participant;
    Text result;

    public override void SetUI(UIParam param)
    {
        ResultUIParam ruip = param as ResultUIParam;

        participant = transform.Find("Participant").GetComponent<Image>();
        result = transform.Find("Result").GetComponent<Text>();

        if (participant.sprite != null) participant.sprite = ruip.sprite;
        result.text = ruip.winner;

        base.SetUI();
    }
}
