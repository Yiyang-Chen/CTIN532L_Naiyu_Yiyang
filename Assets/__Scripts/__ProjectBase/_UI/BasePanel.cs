using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//������
//�ҵ������Լ�����µ��ӿؼ�
//�ṩ��ʾ���صĺ���

//The most basic panel, shold be attached to UI panels.
//Easily get access to all its controls.
//Can do something when the panel is showed or hidden.

public class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        //Ҫ�õ�ʲô�ؼ���Ҫ�������ȡ��Щ�ؼ�
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
    /// ��ʾ�Լ�
    /// Do something when the panel is showed.
    /// </summary>
    public virtual void ShowPanel()
    {

    }

    /// <summary>
    /// �����Լ�
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

    //�������¼�����д��basepanel�������,��FindChildrenControl�����������Ҫ����
    //Example of how to write events in basepanel.
    //In FindChildrenControl function, you can find where the OnClick is called.
    /// <summary>
    /// һ���麯����ȷ���ĸ���ť�������
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

            //�������¼�����д��basepanel�������
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
