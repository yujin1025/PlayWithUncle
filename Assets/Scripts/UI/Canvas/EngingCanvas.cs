using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngingCanvas : BaseCanvas
{
    Ending owner;
    public void Start()
    {
        owner = FindObjectOfType<Ending>();
        owner.SetWinUI();
    }

}
