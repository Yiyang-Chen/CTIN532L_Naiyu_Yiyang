using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle202 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 202;
        _fishData = _FishDataMgr.GetInstance().GetFishByID(fishID);
        fSpeed = _fishData.fSpeed;
        spaceStormIndex = _fishData.spaceStormIndex;
        restIndex = _fishData.notContainindex;
    }

    public override void BeginMovement(GameObject obj)
    {
        base.BeginMovement(obj);

        coroCnt = 0;
        velocities = new Vector3[3] { new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(-3, 4, 0) };
        minTimes = new float[3] { 160, 20, 250 };
        maxTimes = new float[3] { 170, 50, 300 };
        currentCoro = new Coroutine[2] { StartCoroutine(Action1()), StartCoroutine(CreateSpaceStorm()) };
    }

    /// <summary>
    /// 鱼的第一种动作
    /// </summary>
    /// <returns></returns>
    IEnumerator Action1()
    {
        velocity = velocities[coroCnt];
        velocity = velocity.normalized;

        yield return new WaitForSeconds(Random.Range(minTimes[coroCnt], maxTimes[coroCnt]) / 100);

        coroCnt++;
        if (coroCnt >= 4)
        {
            coroCnt = 0;
        }

        currentCoro[0] = StartCoroutine(Action1());
    }

    IEnumerator CreateSpaceStorm()
    {
        yield return new WaitForSeconds(1f);
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", (transform.position+playerPosition)/2 - spriteContainer.transform.position + velocity*Time.fixedDeltaTime*5, new Vector3(1.8f, 1.8f, 1),0, 2f, 2f);
        yield return new WaitForSeconds(0.5f);

        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
