using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{
    INGAME1 = 0,
    INGAME2,
    INGAME3,
    INGAME4,
    INGAME5,
}

public class TutorialCanvas : BaseCanvas
{
    [SerializeField] TutorialBtn[] tutorialBtns;
    TutorialBtn tutorialBtn;

    public TutorialBtn GetTutorialBtn(TutorialType type)
    {
        tutorialBtn = Instantiate(tutorialBtns[(int)type], transform);

        return tutorialBtn;
    }
}
