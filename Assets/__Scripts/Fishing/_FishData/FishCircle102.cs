using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle102 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 102;
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
        minTimes = new float[3] { 160,  20, 250 };
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
        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(0, 3f, 0), new Vector3(3.7f, 3.7f, 1), true, new float[3] {1f, 5f,2f }, 8f);

        yield return new WaitForSeconds(8f);
        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
