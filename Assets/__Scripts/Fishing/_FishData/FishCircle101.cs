using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle101 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 101;
        _fishData = _FishDataMgr.GetInstance().GetFishByID(fishID);
        fSpeed = _fishData.fSpeed;
        spaceStormIndex = _fishData.spaceStormIndex;
        restIndex = _fishData.notContainindex;
    }

    public override void BeginMovement(GameObject obj)
    {
        base.BeginMovement(obj);

        coroCnt = 0;
        velocities = new Vector3[4] { new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 1, 0) };
        minTimes = new float[4] { 150, 150, 250, 250 };
        maxTimes = new float[4] { 300, 300, 400, 400 };
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
        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(2, 0, 0), new Vector3(0.5f, 0.5f, 1),true, new float[1] {2f }, 10f);
        
        yield return new WaitForSeconds(5f);

        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(0, 2, 0), new Vector3(0.5f, 0.5f, 1), true, new float[1] { 2f }, 10f);

        yield return new WaitForSeconds(5f);
        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
