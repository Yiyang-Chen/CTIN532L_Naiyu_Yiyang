using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The very basic singleton structure
//All other managers in project base inherites this class
//public class GameManager : SingletonManager<GameManager>
//{

//}


//!!! IMPORTANT
//               This SingletonManager is NOT a monobehavior.
//               You cannot attach it to a game object or use Update() or other Unity Methods.
//               Check MonoManager (not SingletonManagerMono) for reference on how to call Update() in SingletomManager.
//               You can find an example in ResourceMgr.
//!!! IMPORTANT
public class SingletonManager<T> where T:new()
{
    private static T instance;

    public static T GetInstance()
    {
        if (instance == null) instance = new T();
        return instance;
    }
}

