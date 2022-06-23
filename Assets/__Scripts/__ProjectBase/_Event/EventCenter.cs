using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//The parent class of Event Info.
public interface IEventInfo
{
    
}

//Events which you want to send a parameter.
//If you want to send more than one paramenter, use structure.
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

//Events which you don't want to send a parameter.
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

//�¼�����
//Manage all the events.
public class EventCenter : SingletonManager<EventCenter>
{
    //string�¼���,UnityAction�¼�������һϵ��function
    //string is the name of the event.
    //UnityAction is a series of functions you want to trigger when the event is triggered.
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// ����¼�����
    /// Add event listener with parameter.
    /// </summary>
    /// <param name="name">�¼��� Event Name</param>
    /// <param name="action">
    /// �¼�������һϵ��function
    /// The function you want to call when the event is triggered.
    /// The function should receive only one parameter with type T.
    /// </param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }
    /// <summary>
    /// ��������Ҫ�������¼�
    /// Add event listener without parameter.
    /// </summary>
    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }
    /// <summary>
    /// �Ƴ��¼�����
    /// Remove event listener.
    /// Be sure to do this in case any bugs.
    /// <summary>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
        else
        {
            Debug.Log("no such event");
        }
    }

    /// <summary>
    /// �Ƴ�����Ҫ�������¼�
    /// Remove event listener.
    /// </summary>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
    }
    /// <summary>
    /// �¼�����
    /// Trigger an event.
    /// Send the parameter with expected type.
    /// </summary>
    /// <param name="name">�¼���  Event Name you assigned.</param>
    public void EventTrigger<T>(string name, T info)
    {
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo<T>).actions!=null)
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
        }
    }
    /// <summary>
    /// ��������Ҫ�������¼�
    /// Trigger an event.
    /// </summary>
    public void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();
        }
    }
    /// <summary>
    /// ����¼�����,�糡���л�ʱ
    /// Clear all the events. 
    /// </summary>
    public void clear()
    {
        eventDic.Clear();
    }
}

