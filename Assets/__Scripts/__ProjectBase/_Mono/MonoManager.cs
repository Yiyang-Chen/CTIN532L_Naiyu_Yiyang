using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 由于u3d update性能问题，可以把大部分update加入到这里统一update
/// </summary>
public class MonoManager : SingletonManager<MonoManager>
{
    private MonoController controller;

    public MonoManager()
    {
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }
    //这个只能运行在controller里有的函数
    public Coroutine StartCoroutine(string methodName)
    {
        return controller.StartCoroutine(methodName);
    }
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return controller.StartCoroutine(methodName, value);
    }

    public void StopAllCoroutines()
    {
        controller.StopAllCoroutines();
    }

    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(string methodName)
    {
        controller.StopCoroutine(methodName);
    }

    public void DontDestoryOnLoad(GameObject obj)
    {
        controller._DontDestoryOnLoad(obj);
    }

    public void Destory(Object obj)
    {
        controller._Destory(obj);
    }
}
