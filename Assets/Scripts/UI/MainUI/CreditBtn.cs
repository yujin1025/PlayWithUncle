using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreditBtn : UIComponent
{
    Text t;

    void Start()
    {
        t = GetComponentInChildren<Text>();
        t.gameObject.SetActive(false);
    }

    public void OnClickCredit()
    {
        if (!t.isActiveAndEnabled)
            t.gameObject.SetActive(true);
        else
            t.gameObject.SetActive(false);
    }

}
