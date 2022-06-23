using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveWeapon : MonoBehaviour
{
    public bool hasLoaded;

    public string _path;
    public float lastTime;
    public Coroutine currentCoro;
    private bool isPlayerInside;
    private float damage;

    private void OnEnable()
    {
        InitialStatus();
        EventCenter.GetInstance().AddEventListener("EndFishing", EndFishing);
        EventCenter.GetInstance().AddEventListener<int>("SuccessHookingFish", SuccessHookingFish);

    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener("EndFishing", EndFishing);
        EventCenter.GetInstance().RemoveEventListener<int>("SuccessHookingFish", SuccessHookingFish);

    }

    private void Update()
    {
        if (hasLoaded)
        {
            hasLoaded = false;
            currentCoro = StartCoroutine(DestorySelf());
        }
    }

    public void SetDefensiveWeapon(string path,float d,float lT)
    {
        _path = path;
        if (d <= 0) d = 2;
        damage = d;
        lastTime = lT;
    }

    IEnumerator DestorySelf()
    {
        if (lastTime == 0) lastTime = 2f;
        yield return new WaitForSeconds(lastTime);
        if(isPlayerInside)
        {
            EventCenter.GetInstance().EventTrigger<float>("PlayerInsideDamageZone", damage);
        }
        PoolMgr.GetInstance().PushObj(_path, this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerInside = false;
    }

    private void InitialStatus()
    {
        isPlayerInside = false;
        damage = 0;
        currentCoro = null;
        hasLoaded = false;
    }
    //Events
    private void SuccessHookingFish(int i)
    {
        //Í£Ö¹coroutine
        StopAction();
        InitialStatus();
        //É¾³ý×Ô¼º
        PoolMgr.GetInstance().PushObj(_path, this.gameObject);
    }
    protected void EndFishing()
    {
        //Í£Ö¹coroutine
        StopAction();
        InitialStatus();
        //É¾³ý×Ô¼º
        PoolMgr.GetInstance().PushObj(_path, this.gameObject);
    }

    protected void StopAction()
    {
        if (currentCoro != null)
        {
            StopCoroutine(currentCoro);
        }
    }
}
