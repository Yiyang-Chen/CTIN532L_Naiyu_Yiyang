using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCam;

    private float shakeAmount;
    // Start is called before the first frame update

    private void OnEnable()
    {
        shakeAmount = 0;
        mainCam = Camera.main;
        EventCenter.GetInstance().AddEventListener<float>("PlayerHitTheWall", PlayerHitTheWall);
        EventCenter.GetInstance().AddEventListener("FishHitTheWall", FishHitTheWall);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<float>("PlayerHitTheWall", PlayerHitTheWall);
        EventCenter.GetInstance().RemoveEventListener("FishHitTheWall", FishHitTheWall);
    }

    private void CamShake(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating("BeginShake",0,0.01f);
        Invoke("StopShake", length);
    }

    private void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;
            
        }
    }

    private void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = Vector3.zero;
    }

    //Events
    private void PlayerHitTheWall(float amt)
    {
        CamShake(0.2f*amt, 0.1f);
    }

    private void FishHitTheWall()
    {
        CamShake(0.1f, 0.1f);
    }
}
