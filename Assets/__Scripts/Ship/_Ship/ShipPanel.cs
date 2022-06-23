using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipPanel : BasePanel
{
    public Text title;
    public Text content;
    public List<Sprite> sprites;
    public string[] buttonStrings;

    List<string> informationTexts = new List<string>()
    {
        "Welcome NO.810975...\n\nShip is constructing...\n\nMove on different rooms to see what will come",
        "This is the Control Room.\n\nYou can manage all the \nsettings of the spaceship.",
        "This is the Fishing Room.\n\nThis is probably the most important room to you.\n\nYou can manage all the fishing properties here.\n\nClick START to start fishing.",
        "This is the Power Room.\n\nYou can navigate to any space zone available on the map.",
        "This is the Collection Room.\n\nYou can find all kinds of information here."
    };

    // Start is called before the first frame update
    void Start()
    {
        title.text = "MAIN";
        content.text = "Welcome, fisher #0027.\nCurrent Location: "+MapMgr.GetInstance().GetMapByString();

        buttonStrings = new string[8] { "SystemRoom", "FishingRoom", "PowerRoom", "CollectionRoom", "SettingUI", "FishingUI", "NavigationUI", "CollectionUI" };

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

    private void MouseEnter(int index)
    {
        int j = index;
        if (j >= buttonStrings.Length / 2) j -= buttonStrings.Length / 2;
        string roomS = buttonStrings[j];

        EventCenter.GetInstance().EventTrigger<string>("ScreenMouseEnterButton", roomS);
        GetControl<Button>(buttonStrings[j + buttonStrings.Length / 2])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);

    }

    private void MouseExit(int index)
    {
        int j = index;
        if (j >= buttonStrings.Length / 2) j -= 4;
        string roomS = buttonStrings[j];

        EventCenter.GetInstance().EventTrigger<string>("ScreenMouseExitButton", roomS);
        GetControl<Button>(buttonStrings[j + buttonStrings.Length / 2])[0].GetComponent<UIButtonTemplete>().MouseExit();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);

    }

    protected override void OnClick(string btnName)
    {
        if (btnName == buttonStrings[0]|| btnName == buttonStrings[4])//"SystemRoom"
        {
            if(btnName == buttonStrings[4]) StartCoroutine(SmallAndLarge(buttonStrings[4]));
            ClickRoom(0);
        }
        else if (btnName == buttonStrings[1] || btnName == buttonStrings[5])//"FishingRoom"
        {
            if (btnName == buttonStrings[5]) StartCoroutine(SmallAndLarge(buttonStrings[5]));
            ClickRoom(1);
        }
        else if (btnName == buttonStrings[2] || btnName == buttonStrings[6])//"PowerRoom"
        {
            if (btnName == buttonStrings[6]) StartCoroutine(SmallAndLarge(buttonStrings[6]));
            ClickRoom(2);
        }
        else if (btnName == buttonStrings[3] || btnName == buttonStrings[7])//"CollectionRoom"
        {
            if (btnName == buttonStrings[7]) StartCoroutine(SmallAndLarge(buttonStrings[7]));
            ClickRoom(3);
        }
    }

    private void ClickRoom(int index)
    {
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
        
        string clickedRoom;

        switch (index)
        {
            case 0:
                clickedRoom = "SystemRoom";
                UIMgr.GetInstance().ShowPanel<SystemRPanel>("Ship/Room_System/SystemRPanel",(obj)=>
                {
                    UIMgr.GetInstance().HidePanel("Ship/_Ship/ShipPanel");
                });
                break;
            case 1:
                clickedRoom = "FishingRoom";
                UIMgr.GetInstance().ShowPanel<FishingRPanel>("Ship/Room_Fishing/FishingRPanel",(obj)=> {
                    obj.extraText = false;
                    UIMgr.GetInstance().HidePanel("Ship/_Ship/ShipPanel");
                });
                break;
            case 2:
                clickedRoom = "PowerRoom";
                UIMgr.GetInstance().ShowPanel<MapRPanel>("Ship/Room_Map/MapRPanel", (obj) =>
                {
                    UIMgr.GetInstance().HidePanel("Ship/_Ship/ShipPanel");
                });
                break;
            case 3:
                clickedRoom = "CollectionRoom";
                UIMgr.GetInstance().ShowPanel<CollectionRPanel>("Ship/Room_Collection/CollectionRPanel", (obj) =>
                {
                    UIMgr.GetInstance().HidePanel("Ship/_Ship/ShipPanel");
                });
                break;
            default:
                return;
        }

        EventCenter.GetInstance().EventTrigger<string>("ClickScreenRoom", clickedRoom);
    }

    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
}
