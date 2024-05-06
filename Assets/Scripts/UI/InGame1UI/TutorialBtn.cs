using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBtn : UIComponent
{
    FadePanel[] tutorialPanels;

    void Awake()
    {
        tutorialPanels = GetComponentsInChildren<FadePanel>();
    }

    public bool TutorialProgress(ref int count, ref int progress)
    {
        if (progress >= count) return false;

        if (progress > 0) tutorialPanels[progress - 1].UnsetUI();
        if (count > progress) tutorialPanels[progress].SetUI();

        progress++;

        return true;
    }
}
