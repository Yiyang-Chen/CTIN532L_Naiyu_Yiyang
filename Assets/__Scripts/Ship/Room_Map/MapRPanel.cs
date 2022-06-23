using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MapRPanel : BasePanel
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
        title.text = "NAVIGATION ROOM";
        content.text = "Welcome, fisher #0027.\nCurrent location: "+MapMgr.GetInstance().GetMapByString()+"\nWhere do you want to navigate?";
        buttonStrings = new string[6] { "Map", "Exit","Fish", "MapUI", "ExitUI", "FishUI" };

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
        if (btnName == buttonStrings[0]|| btnName == buttonStrings[3])//Map
        {
            if (btnName == buttonStrings[3]) StartCoroutine(SmallAndLarge(buttonStrings[3]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
            MouseExit(0);
            UIMgr.GetInstance().ShowPanel<SwitchMapPanel>("Ship/Room_Map/SwitchMapPanel",(obj)=> 
            {
                obj.isBackToFish = false;
                UIMgr.GetInstance().HidePanel("Ship/Room_Map/MapRPanel");
            });
            
        }
        else if (btnName == buttonStrings[1] || btnName == buttonStrings[4])//Exit
        {
            if (btnName == buttonStrings[4]) StartCoroutine(SmallAndLarge(buttonStrings[4]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            MouseExit(1);
            EventCenter.GetInstance().EventTrigger("LoadShipMain");
            UIMgr.GetInstance().HidePanel("Ship/Room_Map/MapRPanel");
        }
        else if (btnName == buttonStrings[2] || btnName == buttonStrings[5])//Fish
        {
            if (btnName == buttonStrings[5]) StartCoroutine(SmallAndLarge(buttonStrings[5]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            MouseExit(2);
            EventCenter.GetInstance().EventTrigger<string>("ClickScreenRoom", "FishingRoom");
            UIMgr.GetInstance().HidePanel("Ship/Room_Map/MapRPanel");
            UIMgr.GetInstance().ShowPanel<FishingRPanel>("Ship/Room_Fishing/FishingRPanel", (obj) => {
                obj.extraText = false;
            });
        }
    }

    private void MouseEnter(int index)
    {
        int j = index;
        if (j >= buttonStrings.Length / 2) j -= buttonStrings.Length / 2;
        string buttonS = buttonStrings[j];

        EventCenter.GetInstance().EventTrigger<string>("MapRoomMouseEnterButton", buttonS);
        GetControl<Button>(buttonStrings[j + buttonStrings.Length / 2])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit(int index)
    {
        int j = index;
        if (j >= buttonStrings.Length / 2) j -= buttonStrings.Length / 2;
        string buttonS = buttonStrings[j];

        EventCenter.GetInstance().EventTrigger<string>("MapRoomMouseExitButton", buttonS);
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
            MetricManagerScript.instance.LogString("mapRoomClicked", Input.mousePosition.ToString());
        }
    }
}
