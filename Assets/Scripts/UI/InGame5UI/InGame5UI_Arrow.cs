using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame5UI_Arrow : UIComponent
{

    public void SetPos(Vector2 pos)
    {
        GetComponent<RectTransform>().position = pos;
    }
}
