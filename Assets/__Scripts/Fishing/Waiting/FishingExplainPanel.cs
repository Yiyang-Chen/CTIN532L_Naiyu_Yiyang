using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingExplainPanel : BasePanel
{
    public Text _text;
    List<string> informationTexts = new List<string>()
    {
        "In real game, you still need to play a game to catch it.\n\nThis is a picture of the prototype\nthat will happen if you click the mouse successfully.",
        "You fail to catch the fish.\nTry listen to the 'Di' sound and click left mouse.\n\nThis is a picture of the prototype\nthat will happen if you click the mouse successfully."
    };

    public void SetText(int i,string s)
    {
        _text.text = s+informationTexts[i];
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "Exit":
                MusicMgr.GetInstance().PlaySound("_Using/DM-CGS-03", false);
                EventCenter.GetInstance().EventTrigger("EndFishing");
                this.gameObject.SetActive(false);
                UIMgr.GetInstance().HidePanel("Fishing/Waiting/FishingExplainPanel");
                break;
        }
    }
}
