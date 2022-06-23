using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//自动挂载，手动不要挂载，同一个单例脚本不能挂载多次
//In case anyone perfer this type of Singleton.
//Autolly attached to a game object.
//Don't attach manually or attach multiple times.

//!!! IMPORTANT:
//              As you can realize, this script is not very safe to use.
//              From my experience, you can always use SingletonManager with MonoManager instead.
//!!! IMPORTANT
public class SingletonManagerMono<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //mono不能new
        if(instance == null)
        {
            GameObject obj= GameObject.Find("SingletonMono");
            if (obj == null)
            {
                obj = new GameObject();
                obj.name = "SingletonMono";
            }
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<T>();
        }
        return instance;
    }

}

//public class NewMono : SingletonManagerMono<NewMono>
//{
//    public void Test()
//    {
//        Debug.Log("Test");
//    }
//}
