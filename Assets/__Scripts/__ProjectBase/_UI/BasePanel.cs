using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//面板基类
//找到所有自己面板下的子控件
//提供显示隐藏的函数

//The most basic panel, shold be attached to UI panels.
//Easily get access to all its controls.
//Can do something when the panel is showed or hidden.

public class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        //要用到什么控件就要在这里读取这些控件
        //What kind of controls you are going to use.
        //You can call more other controls in children classes.
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<InputField>();
    }

    /// <summary>
    /// 显示自己
    /// Do something when the panel is showed.
    /// </summary>
    public virtual void ShowPanel()
    {

    }

    /// <summary>
    /// 隐藏自己
    /// Do something when the panel is hidden.
    /// </summary>
    public virtual void HidePanel()
    {

    }

    /// <summary>
    /// Get the list of type T with exact name.
    /// </summary>

    protected List<T> GetControl<T>(string controlName)where T:UIBehaviour
    {
        List<T> returnArray = new List<T>();
        if (controlDic.ContainsKey(controlName))
        {
            for(int i = 0; i < controlDic[controlName].Count; ++i)
            {
                if (controlDic[controlName][i] is T)
                    returnArray.Add(controlDic[controlName][i] as T);
            }
        }
        return returnArray;
    }

    //这里是事件监听写在basepanel里的例子,在FindChildrenControl里面最后有主要部分
    //Example of how to write events in basepanel.
    //In FindChildrenControl function, you can find where the OnClick is called.
    /// <summary>
    /// 一个虚函数来确认哪个按钮被点击了
    /// A virtual function to do things when a button is clicked.
    /// </summary>
    /// <param name="buttonName"></param>
    protected virtual void OnClick(string buttonName)
    {

    }

    protected virtual void onValueChanged(string toggleName,bool value)
    {

    }

    /// <summary>
    /// Find children in Dic.
    /// </summary>
    private void FindChildrenControl<T>() where T:UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();

        for(int i = 0; i < controls.Length; ++i)
        {
            string objName = controls[i].gameObject.name;
            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(controls[i]);
            else
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });

            //这里是事件监听写在basepanel里的例子
            //Example of how to write events in basepanel.
            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(()=>
                {
                    OnClick(objName);
                });
            }
            else if(controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    onValueChanged(objName,value);
                });
            }
        }
    }

}
