using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SystemRPanel : BasePanel
{
    public Text title;
    public Text content;
    public string[] buttonStrings;
    private void OnEnable()
    {
        InputMgr.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener<int>("MouseDown", CheckMouseDown);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<int>("MouseDown", CheckMouseDown);
    }

    void Start()
    {
        title.text = "CONTROL ROOM";
        content.text = "Welcome, fisher #0027.\nYou can change the settings of the fishing ship here.";
        buttonStrings = new string[8] { "System", "Graphic", "Volume", "Exit", "SystemUI", "GraphicUI", "VolumeUI", "ExitUI" };
        
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
        if (btnName == buttonStrings[0]|| btnName == buttonStrings[4])//System
        {
            if (btnName == buttonStrings[4]) StartCoroutine(SmallAndLarge(buttonStrings[4]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
            MouseExit(0);
            UIMgr.GetInstance().ShowPanel<SystemSettingsPanel>("_Start/PanelGameSettings", (panel) => 
            { 
                panel.showWhichFirst = SETTINGMEAU.SYSTEM; panel.isInStartScene = false;
                panel.GetComponent<RectTransform>().sizeDelta = new Vector2(1380, 780);
            });
            UIMgr.GetInstance().HidePanel("Ship/Room_System/SystemRPanel");
        }
        else if(btnName == buttonStrings[1] || btnName == buttonStrings[5])//Graphic
        {
            if (btnName == buttonStrings[5]) StartCoroutine(SmallAndLarge(buttonStrings[5]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
            MouseExit(1);
            UIMgr.GetInstance().ShowPanel<SystemSettingsPanel>("_Start/PanelGameSettings", (panel) => 
            { 
                panel.showWhichFirst = SETTINGMEAU.GRAPHIC; panel.isInStartScene = false;
                panel.GetComponent<RectTransform>().sizeDelta = new Vector2(1380, 780);
            });
            UIMgr.GetInstance().HidePanel("Ship/Room_System/SystemRPanel");
        }
        else if (btnName == buttonStrings[2] || btnName == buttonStrings[6])//Volume
        {
            if (btnName == buttonStrings[6]) StartCoroutine(SmallAndLarge(buttonStrings[6]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
            MouseExit(2);
            UIMgr.GetInstance().ShowPanel<SystemSettingsPanel>("_Start/PanelGameSettings", (panel) => 
            { 
                panel.showWhichFirst = SETTINGMEAU.VOLUME; panel.isInStartScene = false;
                panel.GetComponent<RectTransform>().sizeDelta = new Vector2(1380, 780);
            });
            UIMgr.GetInstance().HidePanel("Ship/Room_System/SystemRPanel");
        }
        else if (btnName == buttonStrings[3] || btnName == buttonStrings[7])//Exit
        {
            if (btnName == buttonStrings[7]) StartCoroutine(SmallAndLarge(buttonStrings[7]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            MouseExit(3);
            EventCenter.GetInstance().EventTrigger("LoadShipMain");
            UIMgr.GetInstance().HidePanel("Ship/Room_System/SystemRPanel");
        }
    }

    private void MouseEnter(int index)
    {
        int j = index;
        if (j >= buttonStrings.Length / 2) j -= buttonStrings.Length / 2;
        string buttonS = buttonStrings[j];

        EventCenter.GetInstance().EventTrigger<string>("SystemRoomMouseEnterButton", buttonS);
        GetControl<Button>(buttonStrings[j + buttonStrings.Length / 2])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit(int index)
    {
        int j = index;
        if (j >= buttonStrings.Length / 2) j -= buttonStrings.Length / 2;
        string buttonS = buttonStrings[j];

        EventCenter.GetInstance().EventTrigger<string>("SystemRoomMouseExitButton", buttonS);
        GetControl<Button>(buttonStrings[j + buttonStrings.Length / 2])[0].GetComponent<UIButtonTemplete>().MouseExit();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
    }
    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
    //Events

    private void CheckMouseDown(int mouse)
    {
        if (MetricManagerScript.instance != null)
        {
            MetricManagerScript.instance.LogString("systemRoomClicked", Input.mousePosition.ToString());
        }
    }
}
