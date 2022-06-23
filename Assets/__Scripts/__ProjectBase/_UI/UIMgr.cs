using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//UI 层级
//UI Layers. Adjust if necessary. Adjust both E_UI_Layer and Canvas perfab.
public enum E_UI_Layer
{
    BOT,
    MID,
    TOP,
    POPUP,
    SYSTEM,
}

/// <summary>
/// 管理UI
/// 1.加载或移除一个面板
/// 2.管理所有显示的面板
/// 3.提供外部操作面板的方法
/// 
/// Manage UI
/// 1.Show or hide a panel
/// 2.Manage all the shown panels
/// 3.Provide easy access to any panel
/// 
/// !!! IMPORTANT 
///                 The Canvas and the EventSystem should not be put into the scene.
/// !!! IMPORTANT                 
/// </summary>
public class UIMgr : SingletonManager<UIMgr>
{
    public Dictionary<string,BasePanel> panelDic=new Dictionary<string,BasePanel>();

    //根据需求自己设置
    //Set by necessary
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform popUp;
    private Transform system;

    public RectTransform canvas;

    public UIMgr()
    {
        GameObject obj = ResourceMgr.GetInstance().Load<GameObject>("_UI/__ProjectBase/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);
        obj = ResourceMgr.GetInstance().Load<GameObject>("_UI/__ProjectBase/EventSystem");
        GameObject.DontDestroyOnLoad(obj);

        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        popUp = canvas.Find("PopUp");
        system = canvas.Find("System");
    }

    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.BOT:
                return this.bot;
            case E_UI_Layer.MID:
                return this.mid;
            case E_UI_Layer.TOP:
                return this.top;
            case E_UI_Layer.POPUP:
                return this.popUp;
            case E_UI_Layer.SYSTEM:
                return this.system;
        }
        return null;
    }

    /// <summary>
    /// 加载panel，并放在要求的层数
    /// Load a panel to desired layer
    /// </summary>
    /// <typeparam name="T">面板脚本类型  The class of your panel</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="callBack">创建完成后想做的事</param>
    /// <param name="layer">显示在哪一层，默认MID  By default, MID.</param>
    public void ShowPanel<T>(string panelName,UnityAction<T> callBack=null,E_UI_Layer layer = E_UI_Layer.MID) where T:BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowPanel();
            if (callBack != null)
                callBack(panelDic[panelName] as T);
            return;
        }
        else
        {
            ResourceMgr.GetInstance().LoadAsyn<GameObject>("_UI/" + panelName, (o) =>
            {
                Transform father = bot;
                switch (layer)
                {
                    case E_UI_Layer.MID:
                        father = mid;
                        break;
                    case E_UI_Layer.TOP:
                        father = top;
                        break;
                    case E_UI_Layer.POPUP:
                        father = popUp;
                        break;
                    case E_UI_Layer.SYSTEM:
                        father = system;
                        break;
                }

                o.transform.SetParent(father);

                o.transform.localPosition = Vector3.zero;
                o.transform.localScale = Vector3.one;

                (o.transform as RectTransform).offsetMax = Vector2.zero;
                (o.transform as RectTransform).offsetMin = Vector2.zero;

                //得到预设体上的面板脚本
                //Get the script of your panel class.
                T panel = o.GetComponent<T>();
                //处理面板创建完成后的逻辑
                //After loaded call functions.
                panel.ShowPanel();
                if (callBack != null)
                    callBack(panel);
                //保存面板
                panelDic.Add(panelName, panel);
            });
        } 
    }
    
    /// <summary>
    /// 删除面板预设体
    /// Delete a panel
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HidePanel();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// 得到某一个已经存在的面板
    /// Get a exist panel.
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }


    public void PopUp(string title, string[] contents, Vector2 pos, float[] width = null, float[] height=null, Sprite[] sprites=null, string eventString = null)
    {
        if (pos == null)
        {
            pos = Vector2.zero;
        }
        UIMgr.GetInstance().ShowPanel<PopupPanel>("__ProjectBase/PopupPanel", (p) =>
        {
            p.GetComponent<RectTransform>().anchoredPosition = pos; 
            p.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
            p.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            p.SetContent(title,contents,width,height, sprites,eventString);
        }, E_UI_Layer.POPUP);
    }

    public void PopUp(string title, string[] contents, float[] width = null, float[] height = null, Sprite[] sprites = null, string eventString = null)
    {
        PopUp(title, contents, Vector2.zero, height, width, sprites, eventString);
    }

    /// <summary>
    /// 封装event trigger自定义事件监听
    /// Allow you customize event trigger.
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件响应函数</param>
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);
        //这里可以检测有没有已经有这个事件了，一般不会出现
        trigger.triggers.Add(entry);
    }
}
