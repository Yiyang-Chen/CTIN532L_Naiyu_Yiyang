using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 背包面板，主要用来更新背包逻辑
/// </summary>
public class BaitPanel : InventoryPanel 
{
    public Text baitTitle;
    public Text baitContent;
    public Image strengthImage;
    public Image fishImage;
    public Text bkTitle;
    public Text bkContent;

    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener("ChangeBait", ChangeBait);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener("ChangeBait", ChangeBait);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void Start()
    {
        SetContent();
        UIMgr.AddCustomEventListener(GetControl<Button>("Exit")[0], EventTriggerType.PointerEnter, (data) =>
        {
            MouseEnter();
        });

        UIMgr.AddCustomEventListener(GetControl<Button>("Exit")[0], EventTriggerType.PointerExit, (data) =>
        {
            MouseExit();
        });
    }
    public override void ShowPanel()
    {
        base.ShowPanel();   
    }
    protected override void UpdateItemString()
    {
        base.UpdateItemString();
        itemString = "_UI/Ship/Room_Fishing/BaitItem";
    }
    protected override void UpdateShowData()
    {
        base.UpdateShowData();
        foreach (_FishData fishData in _FishDataMgr.GetInstance().fishDatas)
        {
            if (fishData.fishID == -1) continue;
            
            showDatas.Add(fishData);
        }
    }
    private void MouseEnter()
    {
        GetControl<Button>("Exit")[0].GetComponent<UIButtonTemplete>().MouseEnter();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit()
    {
        GetControl<Button>("Exit")[0].GetComponent<UIButtonTemplete>().MouseExit();
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
    }
    protected override void OnClick(string btnName)
    {
        StartCoroutine(SmallAndLarge(btnName));
        switch (btnName)
        {
            case "Exit":
                MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
                UIMgr.GetInstance().HidePanel("Ship/Room_Fishing/BaitPanel");
                UIMgr.GetInstance().ShowPanel<FishingRPanel>("Ship/Room_Fishing/FishingRPanel", (obj) => {
                    obj.extraText = false;
                });
                break;
        }
    }

    private void SetContent()
    {
        _FishData chosenBait = _FishDataMgr.GetInstance().fishDatas[_FishDataMgr.GetInstance().currentBait];

        bkTitle.text = "FISHING ROOM - Bait";
        bkContent.text = "Chosen Bait: #" + chosenBait.fishID.ToString("D3") + " " + chosenBait.fishName;

        baitTitle.text = chosenBait.fishName;
        baitContent.text = "Bait ID: #" + chosenBait.fishID.ToString("D3") + ",\nStrength level: " + chosenBait.strength + ",\nPossible attract fishes ID:\n" + _FishDataMgr.GetInstance().GetBaitAttractionString(chosenBait.fishID);

        switch (chosenBait.strength)
        {
            case 1:
                strengthImage.color = new Color32(100, 100, 100, 255);
                break;
            case 2:
                strengthImage.color = new Color32(20, 190, 240, 255);
                break;
            case 3:
                strengthImage.color = new Color32(220, 180, 100, 255);
                break;
            default:
                strengthImage.color = new Color32(100, 100, 100, 255);
                break;
        }
        fishImage.sprite = ResourceMgr.GetInstance().Load<Sprite>("_Sprites/Fishes/_FishPictures/HookingFish" + chosenBait.fishID.ToString("D3"));
    }
    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
    //Events
    private void ChangeBait()
    {
        SetContent();    
    }
}
