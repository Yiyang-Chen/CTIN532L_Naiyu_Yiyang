using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CollectionRPanel : BasePanel
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
        title.text = "COLLECTION ROOM";
        content.text = "Welcome, fisher #0027.\nWhat information do you want to check this time?";
        buttonStrings = new string[10] { "Inventory", "Information", "Toturial", "Exit","Fish", "InventoryUI", "InformationUI", "ToturialUI", "ExitUI", "FishUI" };

        for(int i = 0; i < buttonStrings.Length; i++)
        {
            if (i == 1 || i == 6) continue;
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
        if (btnName == buttonStrings[0] || btnName == buttonStrings[5])//Inventory
        {
            if (btnName == buttonStrings[5]) StartCoroutine(SmallAndLarge(buttonStrings[5]));
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
            MouseExit(0);
            UIMgr.GetInstance().ShowPanel<CollectionInfoPanel>("Ship/Room_Collection/CollectionInfoPanel", (panel) => 
            { 
                panel.showWhichFirst = COLLECTIONMEAU.INVENTORY;
                panel.isBackToFish = false;
                UIMgr.GetInstance().HidePanel("Ship/Room_Collection/CollectionRPanel");
            });           
        }
        else if (btnName == buttonStrings[1] || btnName == buttonStrings[6])//Information
        {
            /*if (btnName == buttonStrings[6]) StartCoroutine(SmallAndLarge(buttonStrings[6]));
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
            MouseExit(1);
            UIMgr.GetInstance().ShowPanel<CollectionInfoPanel>("Ship/Room_Collection/CollectionInfoPanel", (panel) => 
            { 
                panel.showWhichFirst = COLLECTIONMEAU.INFORMATION;
                panel.isBackToFish = false;
                UIMgr.GetInstance().HidePanel("Ship/Room_Collection/CollectionRPanel");
            });*/
        }
        else if (btnName == buttonStrings[2] || btnName == buttonStrings[7])//Toturial
        {
            if (btnName == buttonStrings[7]) StartCoroutine(SmallAndLarge(buttonStrings[7]));
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
            MouseExit(2);
            UIMgr.GetInstance().ShowPanel<CollectionInfoPanel>("Ship/Room_Collection/CollectionInfoPanel", (panel) => 
            { 
                panel.showWhichFirst = COLLECTIONMEAU.TOTURIAL;
                panel.isBackToFish = false;
                UIMgr.GetInstance().HidePanel("Ship/Room_Collection/CollectionRPanel");
            });
        }
        else if (btnName == buttonStrings[3] || btnName == buttonStrings[8])//Exit
        {
            if (btnName == buttonStrings[8]) StartCoroutine(SmallAndLarge(buttonStrings[8]));
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            MouseExit(3);
            EventCenter.GetInstance().EventTrigger("LoadShipMain");
            UIMgr.GetInstance().HidePanel("Ship/Room_Collection/CollectionRPanel");
        }
        else if (btnName == buttonStrings[4] || btnName == buttonStrings[9])//Fish
        {
            if (btnName == buttonStrings[9]) StartCoroutine(SmallAndLarge(buttonStrings[9]));
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            MouseExit(4);
            EventCenter.GetInstance().EventTrigger<string>("ClickScreenRoom", "FishingRoom");
            UIMgr.GetInstance().HidePanel("Ship/Room_Collection/CollectionRPanel");
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

        EventCenter.GetInstance().EventTrigger<string>("CollectionRoomMouseEnterButton", buttonS);
        GetControl<Button>(buttonStrings[j + buttonStrings.Length / 2])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit(int index)
    {
        int j = index;
        if (j >= buttonStrings.Length / 2) j -= buttonStrings.Length / 2;
        string buttonS = buttonStrings[j];

        EventCenter.GetInstance().EventTrigger<string>("CollectionRoomMouseExitButton", buttonS);
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
            MetricManagerScript.instance.LogString("collectionRoomClicked", Input.mousePosition.ToString());
        }
    }
}
