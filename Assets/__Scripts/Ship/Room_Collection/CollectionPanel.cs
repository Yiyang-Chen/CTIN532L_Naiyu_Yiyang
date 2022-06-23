using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPanel : InventoryPanel
{
    protected override void Update()
    {
        base.Update();
    }
    public override void ShowPanel()
    {
        base.ShowPanel();
    }
    protected override void UpdateItemString()
    {
        base.UpdateItemString();
        itemString = "_UI/Ship/Room_Collection/CollectionItem";
    }
    protected override void UpdateShowData()
    {
        base.UpdateShowData();
        foreach (_FishData fishData in _FishDataMgr.GetInstance().fishDatas)
        {
            if (fishData.fishID != -1)
            {
                showDatas.Add(fishData);
            }
        }
    }
}
