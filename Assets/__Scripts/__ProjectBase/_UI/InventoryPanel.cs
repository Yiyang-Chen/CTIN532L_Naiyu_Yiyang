using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : BasePanel
{
    //��Ҫ��ʾ��data
    protected List<_FishData> showDatas;
    //ʹ�õĸ��ӵ�·��
    protected string itemString;
    //content ��Ҫλ�ã�����Ҫ�Ѹ�����Ϊ�Ӷ���
    public RectTransform content;
    //���ӷ�Χ
    public int viewPortH;
    //���ӵĸ�
    public int slotH;
    //һ�и�����
    public int slotNum;
    //���ӵĸ߿���
    public int slotsX = 5;
    public int slotsY = 10;
    //��ǰ��ʾ�ĸ���
    protected Dictionary<int, GameObject> nowShowItems = new Dictionary<int, GameObject>();
    //��һ֡��������Χ
    protected int oldMinIndex = -1;
    protected int oldMaxIndex = -1;

    public override void ShowPanel()
    {
        base.ShowPanel();
        //��ʾ���ʱ����
        //��ʼ����Ҫ��ʾ��data
        showDatas = new List<_FishData>();
        UpdateShowData();
        //��ʼ��ʹ�õĸ���
        UpdateItemString();
        //��ʼ��content����
        content.sizeDelta = new Vector2(0, Mathf.CeilToInt(showDatas.Count / (float)slotNum) * (slotH + slotsY));
        //����
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

    //�����Щ��Ʒ�ñ���ʾ
    public void checkShowOrHide()
    {

        if (content.anchoredPosition.y < 0)
            return;

        int minIndex;
        int maxIndex;

        //Boundary Check
        //С��0
        minIndex = (int)(content.anchoredPosition.y / (slotH + slotsY)) * slotNum;
        maxIndex = (int)((content.anchoredPosition.y + viewPortH) / (slotH + slotsY) + 1) * slotNum - 1;
        //���ڱ����������
        if (maxIndex >= showDatas.Count)
            maxIndex = showDatas.Count - 1;

        //ɾ������
        //�����������
        for (int i = oldMinIndex; i < minIndex; ++i)
        {
            if (nowShowItems.ContainsKey(i))
            {
                if (nowShowItems[i] != null)
                    PoolMgr.GetInstance().PushObj(itemString, nowShowItems[i]);

                nowShowItems.Remove(i);
            }
        }
        //β���������
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
        //��Ӹ���
        for (int i = minIndex; i <= maxIndex; ++i)
        {
            if (nowShowItems.ContainsKey(i))
                continue;
            else
            {
                //�˱��ʽ��Ҫ���첽����
                int index = i;

                nowShowItems.Add(index, null);
                //��������
                PoolMgr.GetInstance().GetObj(itemString, (obj) =>
                {
                    //���ø�����
                    obj.transform.SetParent(content);
                    //���Ŵ�С
                    obj.transform.localScale = Vector3.one;
                    //����λ��
                    obj.transform.localPosition = new Vector3((index % slotNum) * (slotH + slotsX), -index / slotNum * (slotH + slotsY), 0);
                    //���¸�����Ϣ
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
