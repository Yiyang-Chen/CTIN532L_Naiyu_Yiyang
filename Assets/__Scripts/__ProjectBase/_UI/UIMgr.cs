using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//UI �㼶
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
/// ����UI
/// 1.���ػ��Ƴ�һ�����
/// 2.����������ʾ�����
/// 3.�ṩ�ⲿ�������ķ���
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

    //���������Լ�����
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
    /// ����panel��������Ҫ��Ĳ���
    /// Load a panel to desired layer
    /// </summary>
    /// <typeparam name="T">���ű�����  The class of your panel</typeparam>
    /// <param name="panelName">�����</param>
    /// <param name="callBack">������ɺ���������</param>
    /// <param name="layer">��ʾ����һ�㣬Ĭ��MID  By default, MID.</param>
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

                //�õ�Ԥ�����ϵ����ű�
                //Get the script of your panel class.
                T panel = o.GetComponent<T>();
                //������崴����ɺ���߼�
                //After loaded call functions.
                panel.ShowPanel();
                if (callBack != null)
                    callBack(panel);
                //�������
                panelDic.Add(panelName, panel);
            });
        } 
    }
    
    /// <summary>
    /// ɾ�����Ԥ����
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
    /// �õ�ĳһ���Ѿ����ڵ����
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
    /// ��װevent trigger�Զ����¼�����
    /// Allow you customize event trigger.
    /// </summary>
    /// <param name="control">�ؼ�����</param>
    /// <param name="type">�¼�����</param>
    /// <param name="callBack">�¼���Ӧ����</param>
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);
        //������Լ����û���Ѿ�������¼��ˣ�һ�㲻�����
        trigger.triggers.Add(entry);
    }
}
