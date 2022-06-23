using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryItem : BasePanel
{
    public Image picture;
    public Image strengthBK;
    protected _FishData _info;
    private void Start()
    {
        UIMgr.AddCustomEventListener(GetControl<Button>("ClickableCover")[0], EventTriggerType.PointerEnter, (data) =>
        {
            MouseEnter("ClickableCover");
        });

        UIMgr.AddCustomEventListener(GetControl<Button>("ClickableCover")[0], EventTriggerType.PointerExit, (data) =>
        {
            MouseExit("ClickableCover");
        });
    }
    /// <summary>
    /// 初始化格子信息
    /// </summary>
    public virtual void InitItemInfo(_FishData info)
    {
        _info = info;
        //读取道具表
        //更新图标
        picture.sprite = ResourceMgr.GetInstance().Load<Sprite>("_Sprites/Fishes/_FishPictures/HookingFish" + info.fishID.ToString("D3"));
        //更新稀有度
        switch (info.strength)
        {
            case 1:
                strengthBK.color = new Color32(100,100,100,255);
                break;
            case 2:
                strengthBK.color = new Color32(20, 190, 240, 255);
                break;
            case 3:
                strengthBK.color = new Color32(220, 180, 100, 255);
                break;
            default:
                strengthBK.color = new Color32(100, 100, 100, 255);
                break;
        }
        //更新名字
        //更新道具数量

        GetControl<Text>("TextNum")[0].text = info.num.ToString();
        if (info.fishID == 0) GetControl<Text>("TextNum")[0].text = "\u221E";
    }

    protected virtual void MouseEnter(string buttonS)
    {
    }
    protected virtual void MouseExit(string buttonS)
    {
    }

    protected override void OnClick(string buttonName)
    {
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().applySound, false);
    }

    protected void ChangePictureAlpha(float a)
    {
        picture.color = new Color(picture.color.r,picture.color.g,picture.color.b,a);
    }
}
