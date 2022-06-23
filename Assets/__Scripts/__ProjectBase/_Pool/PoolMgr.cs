using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//The class can save the objects under a father object named as the path of the object in the resource folder.
public class PoolData
{
    public GameObject fatherObj;
    public List<GameObject> poolList;

    public PoolData(GameObject obj,GameObject father)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.SetParent(father.transform);

        poolList = new List<GameObject>();
        PushObj(obj);
    }

    public void PushObj(GameObject obj)
    {
        poolList.Add(obj);

        obj.transform.SetParent(fatherObj.transform);
        obj.SetActive(false);
    }

    public GameObject GetObj()
    {
        GameObject obj0 = null;

        obj0 = poolList[0];
        poolList.RemoveAt(0);

        obj0.transform.SetParent(null);
        obj0.SetActive(true);

        return obj0;
    }
}

//缓存池模块
//在玩家察觉不到的时候再手动GC
//Object pool
//Don't save everything in the pool. If the game is very big, it may cause memory problem.
//You can GC manually as you wish.
public class PoolMgr : SingletonManager<PoolMgr>
{
    //容器
    //Container of all the pools
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    private GameObject pool;

    /// <summary>
    /// Get the object you want.
    /// !!!IMPORTANT: This function loads asynchronizely if no enough objects in the pool. Be sure to deal with asyn issues.
    /// </summary>
    /// <param name="name">
    /// Name (path) of the object.
    /// Everything you load should be under the Resource folder.
    /// </param>
    /// <param name="callback">What do you want the object to do after SetActive.</param>
    public void GetObj(string name, UnityAction<GameObject> callback)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        { 
            callback(poolDic[name].GetObj());
        }
        else
        {
            ResourceMgr.GetInstance().LoadAsyn<GameObject>(name, (obj) =>
            {
                obj.name = name;
                callback(obj);
            });  
        }
    }
    /// <summary>
    /// Push the object into pool synchronizely.
    /// </summary>
    /// <param name="name">
    /// Name (path) of the object.
    /// Everything you load should be under the Resource folder.
    /// </param>
    /// <param name="obj">That obj you want to push.</param>
    public void PushObj(string name, GameObject obj)
    {
        if (pool == null)
        {
            pool = new GameObject("Pool");
        }

        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        else
        {
            poolDic.Add(name, new PoolData(obj,pool));
        }
    }

    //手动GC
    //GC manually.
    //You can add more GC functions as you wish.
    public void Clear()
    {
        poolDic.Clear();
        pool = null;
    }
}
