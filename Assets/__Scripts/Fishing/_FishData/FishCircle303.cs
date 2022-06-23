using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle303 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 303;
        _fishData = _FishDataMgr.GetInstance().GetFishByID(fishID);
        fSpeed = _fishData.fSpeed;
        spaceStormIndex = _fishData.spaceStormIndex;
        restIndex = _fishData.notContainindex;
    }

    public override void BeginMovement(GameObject obj)
    {
        base.BeginMovement(obj);

        coroCnt = 0;
        velocities = new Vector3[4] { new Vector3(0.05f, -1, 0), new Vector3(-1, -1, 0), new Vector3(3, 4, 0), new Vector3(-3, 2, 0) };
        minTimes = new float[4] { 700, 20, 250, 50 };
        maxTimes = new float[4] { 600, 50, 300, 100 };
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
        MakeErosionCloud("_Perfab/Fishing/Hooking/CircleErosionCloud", new Vector3(0, 0, 0), new Vector3(3f, 3f, 1), 0, 2f);
        yield return new WaitForSeconds(4f);

        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
