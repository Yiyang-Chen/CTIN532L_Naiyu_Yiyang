using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 动态切换场景
/// Load scene manager.
/// </summary>
public class SceneMgr : SingletonManager<SceneMgr>
{
    /// <summary>
    /// 同步切换
    /// Load scene Synchronizely.
    /// </summary>
    public void LoadScene(string name,UnityAction callback)
    {
        SceneManager.LoadScene(name);
        callback();
    }

    /// <summary>
    /// 异步切换
    /// Load scene Asynchronizely.
    /// </summary>
    public void LoadSceneAsyn(string name, UnityAction callback)
    {
        MonoManager.GetInstance().StartCoroutine(ILoadSceneAsyn(name, callback));
    }

    private IEnumerator ILoadSceneAsyn(string name, UnityAction callback)
    {
        AsyncOperation ao=SceneManager.LoadSceneAsync(name);
        //可以用ao.process得到场景加载进度
        //You can use ao.process to get the process of loading a scene.
        while (ao.isDone)
        {
            //这里更新进度条
            //You can update the process bar here.
            EventCenter.GetInstance().EventTrigger("进度条更新",ao.progress);
            yield return ao.progress;
        }
        yield return ao;
        
        callback();
    }
}
