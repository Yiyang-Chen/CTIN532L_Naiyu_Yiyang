using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum COLLECTIONMEAU { TOTURIAL, INFORMATION, INVENTORY };
public class CollectionInfoPanel : BasePanel
{
    public bool isBackToFish;

    public Text title;
    public Text content;
    public COLLECTIONMEAU showWhichFirst;

    public GameObject toturialMeau;
    public Text toturialText;
    public Image toturialImage;
    public Text toturialPageText;
    private int toturialPage;

    public GameObject infoMeau;
    public Text infoText;

    public GameObject inventoryMeau;
    public CollectionPanel inventory;
    public Text inventoryText;
    public Text pageText;
    private int currentFishID;
    private int page;

    public GameObject[] ButtonStrongs;
    public string[] buttonStrings;

    

    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener<int>("ChangeInventoryText", ChangeInventoryText);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<int>("ChangeInventoryText", ChangeInventoryText);
    }

    private void Start()
    {
        title.text = "COLLECTION ROOM";
        content.text = "Please observe the confidentiality agreement.";

        toturialPage = 1;
        toturialPageText.text = toturialPage + " - " + PopUpMgr.GetInstance().toturialContent.Length;
        toturialImage.sprite = PopUpMgr.GetInstance().toturialSprite[toturialPage - 1];
        toturialText.text = PopUpMgr.GetInstance().toturialContent[toturialPage - 1];

        inventory.ShowPanel();
        inventory.checkShowOrHide();
        currentFishID = 0;
        page = 1;
        pageText.text = page + " - 2";
        buttonStrings = new string[12] { "Toturial", "Information", "Back", "Inventory", "ToturialUI", "InfoUI", "InventoryUI", "ExitUI", "LeftPage", "RightPage", "ToturialLeftPage", "ToturialRightPage" };
        ChangeInventoryText();

        for (int i = 0; i < buttonStrings.Length; i++)
        {
            if (i == 1 || i == 5) continue;
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

        if (showWhichFirst == COLLECTIONMEAU.TOTURIAL) SwitchToToturial();
        //else if (showWhichFirst == COLLECTIONMEAU.INFORMATION) SwitchToInfomation();
        else if (showWhichFirst == COLLECTIONMEAU.INVENTORY) SwitchToInventory();  
    }

    private void MouseEnter(int i)
    {
        if (i == 0 || i==4)
        {
            GetControl<Button>(buttonStrings[4])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        }
        if (i == 1 || i == 5)
        {
            GetControl<Button>(buttonStrings[5])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        }
        if (i == 3 || i == 6)
        {
            GetControl<Button>(buttonStrings[6])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        }
        if (i == 2 || i == 7)
        {
            GetControl<Button>(buttonStrings[7])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        }

        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit(int i)
    {
        if (i == 0 || i == 4)
        {
            GetControl<Button>(buttonStrings[4])[0].GetComponent<UIButtonTemplete>().MouseExit();
        }
        if (i == 1 || i == 5)
        {
            GetControl<Button>(buttonStrings[5])[0].GetComponent<UIButtonTemplete>().MouseExit();
        }
        if (i == 3 || i == 6)
        {
            GetControl<Button>(buttonStrings[6])[0].GetComponent<UIButtonTemplete>().MouseExit();
        }
        if (i == 2 || i == 7)
        {
            GetControl<Button>(buttonStrings[7])[0].GetComponent<UIButtonTemplete>().MouseExit();
        }
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
    }

    protected override void OnClick(string btnName)
    {
        StartCoroutine(SmallAndLarge(btnName));
        if (btnName == buttonStrings[0]|| btnName == buttonStrings[4])//Toturial
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().tagSound, false);
            SwitchToToturial();
        }
        else if (btnName == buttonStrings[1]||btnName == buttonStrings[5])//Information
        {
            /*MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().tagSound, false);
            SwitchToInfomation();*/
        }
        else if (btnName == buttonStrings[3] || btnName == buttonStrings[6])//Inventory
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().tagSound, false);
            SwitchToInventory();
        }
        else if (btnName == buttonStrings[2] || btnName == buttonStrings[7])//Back
        {
            if (!isBackToFish)
            {
                UIMgr.GetInstance().ShowPanel<CollectionRPanel>("Ship/Room_Collection/CollectionRPanel",(obj)=>
                {
                    UIMgr.GetInstance().HidePanel("Ship/Room_Collection/CollectionInfoPanel");
                });
            }
            else
            {
                EventCenter.GetInstance().EventTrigger<string>("ClickScreenRoom", "FishingRoom");
                UIMgr.GetInstance().ShowPanel<FishingRPanel>("Ship/Room_Fishing/FishingRPanel", (obj) =>
                {
                    UIMgr.GetInstance().HidePanel("Ship/Room_Collection/CollectionInfoPanel");
                });
            }
     
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
        }
        else if (btnName == buttonStrings[8])//LeftPage
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseLeftRightSound, false);
            page = 1;
            pageText.text = page + " - 2";
            ChangeInventoryText();
        }
        else if (btnName == buttonStrings[9])//RightPage
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseLeftRightSound, false);
            page = 2;
            pageText.text = page + " - 2";
            ChangeInventoryText();
        }
        else if (btnName == buttonStrings[10])//ToturialLeftPage
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseLeftRightSound, false);
            toturialPage -= 1;
            if (toturialPage == 0) toturialPage = 1;
            toturialPageText.text = toturialPage + " - "+ PopUpMgr.GetInstance().toturialContent.Length;
            ChangeToturialContent();
        }
        else if (btnName == buttonStrings[11])//ToturialRightPage
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseLeftRightSound, false);
            toturialPage += 1;
            if (toturialPage > PopUpMgr.GetInstance().toturialContent.Length) toturialPage = PopUpMgr.GetInstance().toturialContent.Length;
            toturialPageText.text = toturialPage + " - " + PopUpMgr.GetInstance().toturialContent.Length;
            ChangeToturialContent();
        }
    }
    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
    private void SwitchToToturial()
    {
        showWhichFirst = COLLECTIONMEAU.TOTURIAL;
        ButtonStrongs[0].SetActive(true);
        ButtonStrongs[1].SetActive(false);
        ButtonStrongs[3].SetActive(false);
        toturialMeau.SetActive(true);
        infoMeau.SetActive(false);
        inventoryMeau.SetActive(false);
    }

    private void SwitchToInfomation()
    {
        showWhichFirst = COLLECTIONMEAU.INFORMATION;
        ButtonStrongs[0].SetActive(false);
        ButtonStrongs[1].SetActive(true);
        ButtonStrongs[3].SetActive(false);
        toturialMeau.SetActive(false);
        infoMeau.SetActive(true);
        inventoryMeau.SetActive(false);
    }

    private void SwitchToInventory()
    {
        showWhichFirst = COLLECTIONMEAU.INVENTORY;
        ButtonStrongs[0].SetActive(false);
        ButtonStrongs[1].SetActive(false);
        ButtonStrongs[3].SetActive(true);
        toturialMeau.SetActive(false);
        infoMeau.SetActive(false);
        inventoryMeau.SetActive(true);

        ChangeInventoryText(0);
    }

    private void ChangeToturialContent()
    {
        if (PopUpMgr.GetInstance().toturialSprite[toturialPage-1] != null)
        {
            toturialImage.sprite = PopUpMgr.GetInstance().toturialSprite[toturialPage-1];
        }
        toturialText.text = PopUpMgr.GetInstance().toturialContent[toturialPage - 1];
    }

    //Event
    private void ChangeInventoryText(int fishID)
    {
        page = 1;
        currentFishID = fishID;
        pageText.text = page + " - 2";
        ChangeInventoryText();
    }
    private void ChangeInventoryText()
    {
        if (showWhichFirst == COLLECTIONMEAU.INVENTORY)
        {
            _FishData thisFish = _FishDataMgr.GetInstance().GetFishByID(currentFishID);
            if (thisFish.fishID!=0&&thisFish.totalNum == 0)
            {
                inventoryText.text = "Fish Name: ???"+
                    "\nFish ID: " + thisFish.fishID.ToString("D3")+
                    "\n\nCatch the fish to get more information.";
            }
            else if(page==1)
            {
                string mapString = "{ ";
                string baitString = "{ ";
                if (thisFish.fishID >= 1)
                {
                    for (int i = 0; i < thisFish.mapIDs.Length; i++)
                    {
                        if (i != 0) mapString += ", ";
                        mapString += thisFish.mapIDs[i].ToString("D3");
                    }

                    for (int i = 0; i < thisFish.baitIDs.Length; i++)
                    {
                        if (i != 0) baitString += ", ";
                        baitString += thisFish.baitIDs[i].ToString("D3");
                    }
                }

                mapString += " },";
                baitString += " }.";

                inventoryText.text = "Fish Name: " + thisFish.fishName +
                    "\nFish ID: " + thisFish.fishID.ToString("D3") +
                    "\n\nWhich Map to catch in ID: " + mapString +
                    "\nUse which Baits in ID: " + baitString;
                if (thisFish.fishID != 0) inventoryText.text += "\nTotal caught: " + thisFish.totalNum;
                else inventoryText.text += "\nTotal caught: \u221E";
                if (thisFish.fishID!=0) inventoryText.text+="\nCurrent Bait Num: " + thisFish.num;
                else inventoryText.text += "\nCurrent Bait Num: \u221E";
            }
            else if (page == 2)
            {
                inventoryText.text = "Fish Description:\n\n" + thisFish.fishDiscription;
            }
        }
            
    }
}
