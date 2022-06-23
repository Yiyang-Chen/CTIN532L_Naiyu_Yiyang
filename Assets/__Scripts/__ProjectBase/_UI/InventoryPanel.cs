using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : BasePanel
{
    //需要显示的data
    protected List<_FishData> showDatas;
    //使用的格子的路径
    protected string itemString;
    //content 需要位置，还需要把格子设为子对象
    public RectTransform content;
    //可视范围
    public int viewPortH;
    //格子的高
    public int slotH;
    //一行格子数
    public int slotNum;
    //格子的高宽间隔
    public int slotsX = 5;
    public int slotsY = 10;
    //当前显示的格子
    protected Dictionary<int, GameObject> nowShowItems = new Dictionary<int, GameObject>();
    //上一帧的索引范围
    protected int oldMinIndex = -1;
    protected int oldMaxIndex = -1;

    public override void ShowPanel()
    {
        base.ShowPanel();
        //显示面板时更新
        //初始化需要显示的data
        showDatas = new List<_FishData>();
        UpdateShowData();
        //初始化使用的格子
        UpdateItemString();
        //初始化content长度
        content.sizeDelta = new Vector2(0, Mathf.CeilToInt(showDatas.Count / (float)slotNum) * (slotH + slotsY));
        //更新
        checkShowOrHide();
    }

    protected virtual void Update()
    {
        checkShowOrHide();
    }

    protected virtual void UpdateShowData()
    {
    }
    protected virtual void UpdateItemString()
    {
    }

    //检测哪些物品该被显示
    public void checkShowOrHide()
    {

        if (content.anchoredPosition.y < 0)
            return;

        int minIndex;
        int maxIndex;

        //Boundary Check
        //小于0
        minIndex = (int)(content.anchoredPosition.y / (slotH + slotsY)) * slotNum;
        maxIndex = (int)((content.anchoredPosition.y + viewPortH) / (slotH + slotsY) + 1) * slotNum - 1;
        //大于背包最大数量
        if (maxIndex >= showDatas.Count)
            maxIndex = showDatas.Count - 1;

        //删除格子
        //顶部消除溢出
        for (int i = oldMinIndex; i < minIndex; ++i)
        {
            if (nowShowItems.ContainsKey(i))
            {
                if (nowShowItems[i] != null)
                    PoolMgr.GetInstance().PushObj(itemString, nowShowItems[i]);

                nowShowItems.Remove(i);
            }
        }
        //尾部消除溢出
        for (int i = maxIndex + 1; i <= oldMaxIndex; ++i)
        {
            if (nowShowItems.ContainsKey(i))
            {
                if (nowShowItems[i] != null)
                    PoolMgr.GetInstance().PushObj(itemString, nowShowItems[i]);

                nowShowItems.Remove(i);
            }
        }

        oldMinIndex = minIndex;
        oldMaxIndex = maxIndex;

        AddItems(minIndex, maxIndex);
    }

    protected virtual void AddItems(int minIndex, int maxIndex)
    {
        //添加格子
        for (int i = minIndex; i <= maxIndex; ++i)
        {
            if (nowShowItems.ContainsKey(i))
                continue;
            else
            {
                //λ表达式的要求，异步加载
                int index = i;

                nowShowItems.Add(index, null);
                //创建格子
                PoolMgr.GetInstance().GetObj(itemString, (obj) =>
                {
                    //设置父对象
                    obj.transform.SetParent(content);
                    //缩放大小
                    obj.transform.localScale = Vector3.one;
                    //重置位置
                    obj.transform.localPosition = new Vector3((index % slotNum) * (slotH + slotsX), -index / slotNum * (slotH + slotsY), 0);
                    //更新格子信息
                    obj.GetComponent<InventoryItem>().InitItemInfo(showDatas[index]);


                    if (nowShowItems.ContainsKey(index))
                        nowShowItems[index] = obj;
                    else
                        PoolMgr.GetInstance().PushObj(itemString, obj);
                });

            }
        }
    }
}
