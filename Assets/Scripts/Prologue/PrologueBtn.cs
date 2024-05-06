using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PrologueBtn : UIComponent
{
    FadePanel[] prologuePanels;

    void Start()
    {
        prologuePanels = GetComponentsInChildren<FadePanel>();
        prologuePanels[0].GetComponent<CanvasGroup>().alpha = 1;
    }

    public void PrologueProgress()
    {
        if (Prologue.PrologueProgress >= prologuePanels.Length)
        {
            FindObjectOfType<Prologue>().PrologueEnd();
            return;
        }

        if (Prologue.PrologueProgress > 0) prologuePanels[Prologue.PrologueProgress - 1].UnsetUI();
        if (prologuePanels.Length > Prologue.PrologueProgress) prologuePanels[Prologue.PrologueProgress].SetUI();

        Prologue.PrologueProgress++;
    }
}