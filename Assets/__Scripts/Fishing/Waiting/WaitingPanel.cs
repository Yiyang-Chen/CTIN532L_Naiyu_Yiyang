using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WaitingPanel : BasePanel
{
    public Text title;
    public Text content;
    public string[] buttonStrings;

    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener("HideWaitingPanel", EndWaiting);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener("HideWaitingPanel", EndWaiting);
    }

    void Start()
    {
        title.text = "WAITING PHASE";
        content.text = "Wait for a fish to be attracted.\nA sound will also appear to remind you.";
        buttonStrings = new string[1] { "ExitUI" };

        for (int i = 0; i < buttonStrings.Length; i++)
        {
            int index = i;
            UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[index])[0], EventTriggerType.PointerEnter, (data) =>
            {
                MouseEnter(index);
            });

            UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[index])[0], EventTriggerType.PointerExit, (data) =>
            {
                MouseExit(index);
            });
        }
    }

    protected override void OnClick(string btnName)
    {
        StartCoroutine(SmallAndLarge(btnName));

        if (btnName == buttonStrings[0])//Exit
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            MouseExit(0);
            EventCenter.GetInstance().EventTrigger("EndFishing");
        }
    }

    private void MouseEnter(int index)
    {
        int j = index;
        string buttonS = buttonStrings[j];

        GetControl<Button>(buttonStrings[j])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit(int index)
    {
        int j = index;
        string buttonS = buttonStrings[j];

        GetControl<Button>(buttonStrings[j])[0].GetComponent<UIButtonTemplete>().MouseExit();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
    }

    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
    //Events

    void EndWaiting()
    {
        UIMgr.GetInstance().HidePanel("Fishing/Waiting/WaitingPanel");
    }
}
