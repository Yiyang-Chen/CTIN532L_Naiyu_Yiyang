using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteProgressBar : MonoBehaviour
{
    public float FillValue;
    public float DecreaseFillValue;

    public Blur topCircle;
    public Blur bottomCircle;

    public Transform TopDecreaseHideBar;
    public Transform BottomDecreaseHideBar;
    public Transform TopHideBar;
    public Transform BottomHideBar;

    private float playerBlurTime = 0.18f;
    private float fishBlurTime = 0.22f;
    private float barBlurSpeed = 10f;
    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener<float>("PlayerHitTheWall", PlayerHitTheWall);
        EventCenter.GetInstance().AddEventListener("FishHitTheWall", FishHitTheWall);
}

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<float>("PlayerHitTheWall", PlayerHitTheWall);
        EventCenter.GetInstance().RemoveEventListener("FishHitTheWall", FishHitTheWall);
    }

private void FixedUpdate()
    {
        TopDecreaseHideBar.localRotation = Quaternion.Euler(0, 0, -(DecreaseFillValue * 360 > 180 ? 180 : DecreaseFillValue * 360));
        BottomDecreaseHideBar.localRotation = Quaternion.Euler(0, 0, -((DecreaseFillValue * 360 < 180 ? 180 : DecreaseFillValue * 360) + 180));

        TopHideBar.localRotation = Quaternion.Euler(0, 0, -(FillValue * 360 > 180 ? 180 : FillValue * 360));
        BottomHideBar.localRotation = Quaternion.Euler(0, 0, -((FillValue * 360 < 180 ? 180 : FillValue * 360) + 180));

        if (FillValue < 0)
            FillValue = 0;
        if (FillValue > 1)
            FillValue = 1;
    }

    public void SetBarValue(float v)
    {
        if (v >= FillValue)
        {
            FillValue = v;
            DecreaseFillValue = v;
        }
        else
        {
            FillValue = v;
        }
        
    }
    //Events
    private void PlayerHitTheWall(float x)
    {
        topCircle.ActiveBlur(playerBlurTime, barBlurSpeed);
        bottomCircle.ActiveBlur(playerBlurTime, barBlurSpeed);
    }

    private void FishHitTheWall()
    {
        topCircle.ActiveBlur(fishBlurTime, barBlurSpeed);
        bottomCircle.ActiveBlur(fishBlurTime, barBlurSpeed);
    }
}
