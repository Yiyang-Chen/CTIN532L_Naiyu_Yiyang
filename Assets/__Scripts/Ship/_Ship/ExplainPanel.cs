using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainPanel : BasePanel
{
    public Image explainContent;

    public void SetContent(Sprite i)
    {
        explainContent.sprite = i;
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "Exit":
                MusicMgr.GetInstance().PlaySound("_Using/DM-CGS-03", false);
                EventCenter.GetInstance().EventTrigger("LoadShipMain");
                this.gameObject.SetActive(false);
                break;
        }
    }
}
