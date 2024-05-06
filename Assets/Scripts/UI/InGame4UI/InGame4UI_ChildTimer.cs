using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame4UI_ChildTimer : MonoBehaviour
{
    Text t;
    void Start()
    {
        t = GetComponent<Text>();
    }

    public void SetPlayerTimer(string setTime = "")
    {       
        t.text = setTime;
    }
}
