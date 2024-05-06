using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBtn : UIComponent
{
    public void MuteButton()
    {
        SoundMgr.Instance.MuteOrPlayAllSound();
    }
}
