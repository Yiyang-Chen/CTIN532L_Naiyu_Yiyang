using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// ��̬�л�����
/// Load scene manager.
/// </summary>
public class SceneMgr : SingletonManager<SceneMgr>
{
    /// <summary>
    /// ͬ���л�
    /// Load scene Synchronizely.
    /// </summary>
    public void LoadScene(string name,UnityAction callback)
    {
        SceneManager.LoadScene(name);
        callback();
    }

    /// <summary>
    /// �첽�л�
    /// Load scene Asynchronizely.
    /// </summary>
    public void LoadSceneAsyn(string name, UnityAction callback)
    {
        MonoManager.GetInstance().StartCoroutine(ILoadSceneAsyn(name, callback));
    }

    private IEnumerator ILoadSceneAsyn(string name, UnityAction callback)
    {
        AsyncOperation ao=SceneManager.LoadSceneAsync(name);
        //������ao.process�õ��������ؽ���
        //You can use ao.process to get the process of loading a scene.
        while (ao.isDone)
        {
            //������½�����
            //You can update the process bar here.
            EventCenter.GetInstance().EventTrigger("����������",ao.progress);
            yield return ao.progress;
        }
        yield return ao;
        
        callback();
    }
}
