using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStorm : MonoBehaviour
{
    public bool hasLoaded;

    public string _path;
    public Vector3 force;
    public float lastTime;
    public Coroutine currentCoro;

    public bool isInside;
    void Start()
    {
        
    }

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

    public void SetForce(Vector3 forceDirection, float forceMag)
    {
        force = forceDirection.normalized * forceMag;
    }

    IEnumerator DestorySelf()
    {
        if (lastTime == 0) lastTime = 5f;
        yield return new WaitForSeconds(lastTime);
        PoolMgr.GetInstance().PushObj(_path, this.gameObject);
    }

    private void InitialStatus()
    {
        hasLoaded = false;
        currentCoro = null;
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
        if (currentCoro != null)
        {
            StopCoroutine(currentCoro);
        }
    }
}
