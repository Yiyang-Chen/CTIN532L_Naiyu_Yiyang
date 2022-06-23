using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理玩家输入
/// </summary>
public class InputMgr : SingletonManager<InputMgr>
{
    private bool isStart = false;
    public InputMgr()
    {
        MonoManager.GetInstance().AddUpdateListener(Update);
    }
    /// <summary>
    /// 开启关闭输入检测
    /// </summary>
    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }

    private void CheckKeyCode(KeyCode key)
    {
        if (!isStart) return;
        
        if(Input.GetKeyDown(key))
        {
            EventCenter.GetInstance().EventTrigger("KeyDown", key);
        }
        if (Input.GetKeyUp(key))
        {
            EventCenter.GetInstance().EventTrigger("KeyUp", key);
        }
    }

    private void CheckMouseClick(int click)
    {
        if (!isStart) return;

        if (Input.GetMouseButtonDown(click))
        {
            EventCenter.GetInstance().EventTrigger("MouseDown", click);
        }
        if (Input.GetMouseButtonUp(click))
        {
            EventCenter.GetInstance().EventTrigger("MouseUp", click);
        }
    }

    private void CheckAnyKey()
    {
        if (!isStart) return;

        if (Input.anyKey)
        {
            EventCenter.GetInstance().EventTrigger("AnyKeyHeldDown");
        }
        if (Input.anyKeyDown)
        {
            EventCenter.GetInstance().EventTrigger("AnyKeyPushDown");
        }
    }

    private void Update()
    {
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.D);
        CheckKeyCode(KeyCode.R);
        CheckKeyCode(KeyCode.Escape);
        CheckKeyCode(KeyCode.Space);
        CheckMouseClick(0);
        CheckMouseClick(1);
        CheckAnyKey();
    }
}
