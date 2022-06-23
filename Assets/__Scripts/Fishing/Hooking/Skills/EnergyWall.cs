using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWall : MonoBehaviour
{
    public bool hasLoaded;

    public SpriteRenderer _animation;

    public string _path;
    public float[] changeTime;
    public bool allowPass;
    public float lastTime;
    public int switchCount;

    public Coroutine[] currentCoro;
    
    private float playerBlurTime;
    private float fishBlurTime;
    private float wallBlurSpeed;
    private Color32[] colors;
    void Start()
    {

    }

    private void OnEnable()
    {
        InitialStatus();
        EventCenter.GetInstance().AddEventListener<float>("PlayerHitTheWall", PlayerHitTheWall);
        EventCenter.GetInstance().AddEventListener("FishHitTheWall", FishHitTheWall);
        EventCenter.GetInstance().AddEventListener("EndFishing", EndFishing);
        EventCenter.GetInstance().AddEventListener<int>("SuccessHookingFish", SuccessHookingFish);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<float>("PlayerHitTheWall", PlayerHitTheWall);
        EventCenter.GetInstance().RemoveEventListener("FishHitTheWall", FishHitTheWall);
        EventCenter.GetInstance().RemoveEventListener("EndFishing", EndFishing);
        EventCenter.GetInstance().RemoveEventListener<int>("SuccessHookingFish", SuccessHookingFish);
    }

    private void Update()
    {
        if (hasLoaded)
        {
            hasLoaded = false;
            currentCoro = new Coroutine[2] { StartCoroutine(DestorySelf()), StartCoroutine(SwitchPass()) };
        }
    }

    public void SetEnergyWall(string path,float lT, float[] t,bool b)
    {
        _path = path;
        lastTime = lT;
        changeTime = t;
        allowPass = b;
        if(!allowPass) _animation.color = colors[1];
    }

    IEnumerator DestorySelf()
    {
        if (lastTime == 0) lastTime = 5f;
        yield return new WaitForSeconds(lastTime);
        PoolMgr.GetInstance().PushObj(_path, this.gameObject);
    }

    IEnumerator SwitchPass()
    {
        if (changeTime[switchCount] == 0) changeTime[switchCount] = 1;
        yield return new WaitForSeconds(changeTime[switchCount]);
        switchCount++;
        if (switchCount >= changeTime.Length) switchCount = 0;
        allowPass = !allowPass;
        if (allowPass)
        {
            _animation.color = colors[0];
        }
        else
        {
            _animation.color = colors[1];
        }
        currentCoro[1] = StartCoroutine(SwitchPass());
    }

    private void InitialStatus()
    {
        hasLoaded = false;
        currentCoro = null;
        allowPass = true;
        lastTime = 0;
        switchCount = 0;
        changeTime = null;
        playerBlurTime = 0.1f;
        fishBlurTime = 0.15f;
        wallBlurSpeed = 20f;
        colors = new Color32[2] { new Color32(0xBF, 0xE9, 0xFF, 83), new Color32(0xFF, 0x26, 0x29, 83) };
    }

    //Events
    protected void EndFishing()
    {
        //Í£Ö¹coroutine
        StopAction();
        InitialStatus();
        //É¾³ý×Ô¼º
        PoolMgr.GetInstance().PushObj(_path, this.gameObject);
    }

    private void SuccessHookingFish(int i)
    {
        //Í£Ö¹coroutine
        StopAction();
        InitialStatus();
        //É¾³ý×Ô¼º
        PoolMgr.GetInstance().PushObj(_path, this.gameObject);
    }

    protected void StopAction()
    {
        for(int i = 0; i < currentCoro.Length; i++)
        {
            if (currentCoro[i] != null)
            {
                StopCoroutine(currentCoro[i]);
            }
        }    
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
