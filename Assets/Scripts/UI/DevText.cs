using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevText : UIComponent
{
    public void SetDevText(string s)
    {
        Text text = GetComponent<Text>();
        text.text = s;
    }
}
