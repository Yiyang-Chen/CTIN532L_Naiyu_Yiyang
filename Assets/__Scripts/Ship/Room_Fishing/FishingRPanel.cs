using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishingRPanel : BasePanel
{
    public Text title;
    public Text screenText;
    public bool extraText;
    public bool isFailWaitingFish;
    public bool isIncorrectMapOrBait;
    public bool isFailHookingFish;
    public int successFishID;

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
        title.text = "FISHING ROOM";
        screenText.gameObject.SetActive(false);

        buttonStrings = new string[10] { "Bait", "Map", "Start", "Illustration", "Exit", "BaitUI", "MapUI", "StartUI", "IllustrationUI", "ExitUI" };

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

        StartCoroutine(ShowTextLater());
    }

    IEnumerator ShowTextLater()
    {
        yield return new WaitForSeconds(0.17f);
        screenText.gameObject.SetActive(true);
        screenText.text = "Current Status:\nLocation: " + MapMgr.GetInstance().GetMapByString() + "  Bait: #" + _FishDataMgr.GetInstance().currentBait.ToString("D3") + " " + _FishDataMgr.GetInstance().fishDatas[_FishDataMgr.GetInstance().currentBait].fishName;
        if (extraText&&isFailWaitingFish)
        {
            screenText.text += "\nYou fail to catch the fish.";
        }
        else if (extraText && isIncorrectMapOrBait)
        {
            screenText.text += "\nThe bait seems cannot attract the fishes in this map.";
        }
        else if(extraText&&isFailHookingFish)
        {
            screenText.text += "\nYou fail to catch the fish.";
        }
    }

    protected override void OnClick(string btnName)
    {
        if (btnName == buttonStrings[0] || btnName == buttonStrings[5])//"Bait", 
        {
            if (btnName == buttonStrings[5]) StartCoroutine(SmallAndLarge(buttonStrings[5]));

            EventCenter.GetInstance().EventTrigger<string>("FishingRoomMouseExitButton", buttonStrings[0]);
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().tagSound, false);
            UIMgr.GetInstance().ShowPanel<BaitPanel>("Ship/Room_Fishing/BaitPanel",(obj)=> { 
                UIMgr.GetInstance().HidePanel("Ship/Room_Fishing/FishingRPanel"); 
            });
        }
        else if (btnName == buttonStrings[1] || btnName == buttonStrings[6])//"Map", 
        {
            if (btnName == buttonStrings[6]) StartCoroutine(SmallAndLarge(buttonStrings[6]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            EventCenter.GetInstance().EventTrigger<string>("ClickScreenRoom", "PowerRoom");
            
            UIMgr.GetInstance().ShowPanel<SwitchMapPanel>("Ship/Room_Map/SwitchMapPanel",(obj)=>
            {
                obj.isBackToFish = true;
                UIMgr.GetInstance().HidePanel("Ship/Room_Fishing/FishingRPanel");     
            });
        }
        else if (btnName == buttonStrings[2] || btnName == buttonStrings[7])//"Start", 
        {
            if (btnName == buttonStrings[7]) StartCoroutine(SmallAndLarge(buttonStrings[7]));

            //Check bait
            _FishData bait = _FishDataMgr.GetInstance().GetFishByID((_FishDataMgr.GetInstance().currentBait));
            if (bait.num > 0)
            {
                MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseStartFishingSound, false);
                MusicMgr.GetInstance().CrossFading("MX_WaitingPhase");
                MouseExit(2);
                EventCenter.GetInstance().EventTrigger("StartFishing");
                UIMgr.GetInstance().ShowPanel<WaitingPanel>("Fishing/Waiting/WaitingPanel",(obj)=> {
                    UIMgr.GetInstance().HidePanel("Ship/Room_Fishing/FishingRPanel");
                });

                if (!PopUpMgr.GetInstance().StartWaiting)
                {
                    UIMgr.GetInstance().PopUp("Waiting Fish",
                         new string[3] { "Fishing contains two parts: Waiting and Hooking.","In waiting phase, you need to wait until a fish rushes to the middle.\n\nYou can also hear a hint sound when you can hook a fish.","Use SPACE or LEFT CLICK to hook the fish.\n\nYou can review this information later in COLLECTION -> TUTORIAL." },
                         new Vector2(600,-200), new float[3] { 1000,1000,1000 }, new float[3] { 500,700,700 }, null
                         ,"StartWaiting");
                    PopUpMgr.GetInstance().StartWaiting = true;
                }
                else
                {
                    EventCenter.GetInstance().EventTrigger("StartWaiting");
                }
                if (bait.fishID > 0) bait.num -= 1;
            }
            else
            {

                MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().failToStartSound, false);
                screenText.text = "Current Status:\nLocation: " + MapMgr.GetInstance().GetMapByString() + "  Bait: #"+ _FishDataMgr.GetInstance().currentBait.ToString("D3")+" " + _FishDataMgr.GetInstance().fishDatas[_FishDataMgr.GetInstance().currentBait].fishName;
                screenText.text += "\n\nThe bait has used up.\nTry another bait.\n\n";
            }
        }
        else if (btnName == buttonStrings[3] || btnName == buttonStrings[8])//"Illustration", 
        {
            if (btnName == buttonStrings[8]) StartCoroutine(SmallAndLarge(buttonStrings[8]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            EventCenter.GetInstance().EventTrigger<string>("ClickScreenRoom", "CollectionRoom");
            UIMgr.GetInstance().ShowPanel<CollectionInfoPanel>("Ship/Room_Collection/CollectionInfoPanel", (panel) => 
            { 
                panel.showWhichFirst = COLLECTIONMEAU.INVENTORY;
                panel.isBackToFish = true;
                UIMgr.GetInstance().HidePanel("Ship/Room_Fishing/FishingRPanel");
            });
        }
        else if (btnName == buttonStrings[4] || btnName == buttonStrings[9])//"Exit",
        {
            if (btnName == buttonStrings[9]) StartCoroutine(SmallAndLarge(buttonStrings[9]));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            MouseExit(4);
            EventCenter.GetInstance().EventTrigger("LoadShipMain");
            UIMgr.GetInstance().HidePanel("Ship/Room_Fishing/FishingRPanel");
        }
    }

    private void MouseEnter(int index)
    {
        int j = index;
        if (j >= buttonStrings.Length / 2) j -= buttonStrings.Length / 2;
        string buttonS = buttonStrings[j];

        EventCenter.GetInstance().EventTrigger<string>("FishingRoomMouseEnterButton", buttonS);
        GetControl<Button>(buttonStrings[j + buttonStrings.Length / 2])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit(int index)
    {
        int j = index;
        if (j >= buttonStrings.Length / 2) j -= buttonStrings.Length / 2;
        string buttonS = buttonStrings[j];

        EventCenter.GetInstance().EventTrigger<string>("FishingRoomMouseExitButton", buttonS);
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
            MetricManagerScript.instance.LogString("fRoomClicked", Input.mousePosition.ToString());
        }
    }

}
