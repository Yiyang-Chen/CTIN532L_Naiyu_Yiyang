using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 放在背包里的一个个格子容器
/// </summary>
public class BaitItem : InventoryItem
{
    public Image buttonImage;
    public Sprite[] backgrounds;
    private bool _isStrong;
    public GameObject greyCover;
    private void OnDisable()
    {
        if(_isStrong) EventCenter.GetInstance().RemoveEventListener("ChangeBait", ChangeBait);
    }
    public override void InitItemInfo(_FishData info)
    {
        base.InitItemInfo(info);
        if (info.fishID != _FishDataMgr.GetInstance().currentBait)
        {
            isStrong(false);
        }
        else
        {
            isStrong(true);
            EventCenter.GetInstance().AddEventListener("ChangeBait", ChangeBait);
        }

        greyCover.SetActive(true);
        bool isCorrectMap = false;
        for (int i = 0; i < info.mapIDs.Length; i++)
        {
            if (info.mapIDs[i] == MapMgr.GetInstance().GetMapByInt() || info.mapIDs[i] == -2) isCorrectMap = true;
        }
        if (isCorrectMap&& info.num != 0&&info.strength!=3) greyCover.SetActive(false);
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
                    _FishDataMgr.GetInstance().currentBait = _info.fishID;
                    EventCenter.GetInstance().EventTrigger("ChangeBait");
                    EventCenter.GetInstance().AddEventListener("ChangeBait", ChangeBait);
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
    private void ChangeBait()
    {
        isStrong(false);
        EventCenter.GetInstance().RemoveEventListener("ChangeBait", ChangeBait);
    }
}
