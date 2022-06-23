using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookingBackground : MonoBehaviour
{
    public Sprite[] mapBackgrounds;

    private float playerBlurTime = 0.1f;
    private float fishBlurTime = 0.15f;
    private float wallBlurSpeed = 20f;
    private void OnEnable()
    {
        CheckBackground();
        EventCenter.GetInstance().AddEventListener<float>("PlayerHitTheWall", PlayerHitTheWall);
        EventCenter.GetInstance().AddEventListener("FishHitTheWall", FishHitTheWall);
        EventCenter.GetInstance().AddEventListener("EndFishing", EndFishing);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<float>("PlayerHitTheWall", PlayerHitTheWall);
        EventCenter.GetInstance().RemoveEventListener("FishHitTheWall", FishHitTheWall);
        EventCenter.GetInstance().RemoveEventListener("EndFishing", EndFishing);
    }

    private void CheckBackground()
    {
        switch (MapMgr.GetInstance().currentMap)
        {
            case SpaceMap.NORMAL:
                this.GetComponent<SpriteRenderer>().sprite = mapBackgrounds[0];
                break;
            case SpaceMap.CYBER:
                this.GetComponent<SpriteRenderer>().sprite = mapBackgrounds[1];
                break;
            case SpaceMap.CIVILIZATION:
                this.GetComponent<SpriteRenderer>().sprite = mapBackgrounds[2];
                break;
            case SpaceMap.INSECT:
                this.GetComponent<SpriteRenderer>().sprite = mapBackgrounds[3];
                break;
        }
    }

    //Events
    private void EndFishing()
    {
        PoolMgr.GetInstance().PushObj("_Perfab/Fishing/Hooking/PerfabHookingBK", this.gameObject);
    }

    private void PlayerHitTheWall(float x)
    {
        this.GetComponent<Blur>().ActiveBlur(playerBlurTime, wallBlurSpeed);
    }

    private void FishHitTheWall()
    {
        this.GetComponent<Blur>().ActiveBlur(fishBlurTime, wallBlurSpeed);
    }
}
