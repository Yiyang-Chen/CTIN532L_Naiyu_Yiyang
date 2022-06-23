using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarPanel : BasePanel
{
    public Image bar;

    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener("EndFishing", EndFishing);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener("EndFishing", EndFishing);
    }

    public void SetBarValue(float v)
    {
        bar.fillAmount = v;
    }

    private void EndFishing()
    {
        UIMgr.GetInstance().HidePanel("Fishing/Hooking/ProgressBarPanel");
    }
}
