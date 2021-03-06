using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle103 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 103;
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
        minTimes = new float[4] { 700, 20, 250,50 };
        maxTimes = new float[4] { 600, 50, 300,100 };
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
        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(0, 1, 0), new Vector3(2f, 2f, 1), true, new float[4] { 1f, 2.5f, 1f,3.5f }, 8f);
        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(0, -3.5f, 0), new Vector3(2f, 2f, 1), false, new float[4] { 1.5f, 0.5f, 4f,2f }, 8f);
        yield return new WaitForSeconds(8f);
        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
