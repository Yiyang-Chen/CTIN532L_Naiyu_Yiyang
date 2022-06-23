using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditPanel : BasePanel
{
    public float length;
    public Text content;
    public float speed;
    
    public string[] buttonStrings; 

    private void Start()
    {
        length = 0;
        buttonStrings = new string[1] { "Back" };

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[0])[0], EventTriggerType.PointerEnter, (data) =>
        {
            MouseEnter(0);
        });

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[0])[0], EventTriggerType.PointerExit, (data) =>
        {
            MouseExit(0);
        });
    }

    private void Update()
    {
        length += speed * Time.deltaTime;

        EventCenter.GetInstance().EventTrigger<int>("SwitchCreditBackground", (int)(length / 300) % 4);

        if (content.rectTransform.anchoredPosition.y<=3050) content.rectTransform.anchoredPosition = content.rectTransform.anchoredPosition + new Vector2(0,speed*Time.deltaTime);
    }

    private void MouseEnter(int i)
    {
        GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);

        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit(int i)
    {
        GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(1f, 1f, 1);

        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
    }

    protected override void OnClick(string btnName)
    {
        if (btnName == buttonStrings[0])//Back
        {
            StartCoroutine(SmallAndLarge(btnName));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            EventCenter.GetInstance().EventTrigger<int>("SwitchCreditBackground", 0);
            UIMgr.GetInstance().HidePanel("_Start/CreditPanel");
            UIMgr.GetInstance().ShowPanel<StartScreenPanel>("_Start/StartScenePanel");
        }
    }

    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
}
