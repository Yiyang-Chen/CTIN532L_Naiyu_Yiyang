using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : InventoryItem
{
    public Image buttonImage;
    public Sprite[] backgrounds;
    private bool _isStrong;
    private void OnDisable()
    {
        if (_isStrong) EventCenter.GetInstance().RemoveEventListener<int>("ChangeInventoryText", ChangeInventoryText);
    }
    public override void InitItemInfo(_FishData info)
    {
        base.InitItemInfo(info);
        if(info.totalNum==0) picture.sprite = ResourceMgr.GetInstance().Load<Sprite>("_Sprites/Fishes/_FishPictures/Locked_Fish");
        if (info.fishID != 0)
        {
            isStrong(false);
        }
        else
        {
            isStrong(true);
            EventCenter.GetInstance().AddEventListener<int>("ChangeInventoryText", ChangeInventoryText);
        }
        GetControl<Text>("TextNum")[0].text = info.totalNum.ToString();
        if (info.fishID == 0) GetControl<Text>("TextNum")[0].text = "\u221E";
    }
    protected override void MouseEnter(string buttonS)
    {
        base.MouseEnter(buttonS);
        if (!_isStrong)
        {
            switch (buttonS)
            {
                case "ClickableCover":
                    ChangePictureAlpha(1.0f);
                    break;
            }
        }

    }

    protected override void MouseExit(string buttonS)
    {
        base.MouseExit(buttonS);
        if (!_isStrong)
        {
            switch (buttonS)
            {
                case "ClickableCover":
                    ChangePictureAlpha(0.8f);
                    break;
            }
        }
    }

    protected override void OnClick(string buttonName)
    {
        base.OnClick(buttonName);
        switch (buttonName)
        {
            case "ClickableCover":
                if (!_isStrong)
                {
                    isStrong(true);
                    EventCenter.GetInstance().EventTrigger<int>("ChangeInventoryText",_info.fishID);
                    EventCenter.GetInstance().AddEventListener<int>("ChangeInventoryText", ChangeInventoryText);
                }
                break;
        }
    }

    private void isStrong(bool bo)
    {
        _isStrong = bo;
        if (bo)
        {
            ChangePictureAlpha(1.0f);
            buttonImage.sprite = backgrounds[1];
        }
        else
        {
            ChangePictureAlpha(0.8f);
            buttonImage.sprite = backgrounds[0];
        }
    }
    //Event
    private void ChangeInventoryText(int i)
    {
        isStrong(false);
        EventCenter.GetInstance().RemoveEventListener<int>("ChangeInventoryText", ChangeInventoryText);
    }
}
