using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle105 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 105;
        _fishData = _FishDataMgr.GetInstance().GetFishByID(fishID);
        fSpeed = _fishData.fSpeed;
        spaceStormIndex = _fishData.spaceStormIndex;
        restIndex = _fishData.notContainindex;
    }

    public override void BeginMovement(GameObject obj)
    {
        base.BeginMovement(obj);

        coroCnt = 0;
        velocities = new Vector3[4] { new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(1, 0, 0), new Vector3(-1, -1, 0) };
        minTimes = new float[4] { 300, 50, 200, 350 };
        maxTimes = new float[4] { 450, 150, 300, 450 };
        currentCoro = new Coroutine[2] { StartCoroutine(Action1()), StartCoroutine(CreateSpaceStorm()) };
    }

    IEnumerator Action1()
    {
        velocity = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
        velocity = velocity.normalized;

        yield return new WaitForSeconds(Random.Range(minTimes[coroCnt], maxTimes[coroCnt]) / 100);

        coroCnt++;
        if (coroCnt >= velocities.Length)
        {
            coroCnt = 0;
        }

        currentCoro[0] = StartCoroutine(Action2());
    }

    /// <summary>
    /// 鱼的第一种动作
    /// </summary>
    /// <returns></returns>
    IEnumerator Action2()
    {
        velocity = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
        velocity = velocity.normalized * restIndex;

        yield return new WaitForSeconds(Random.Range(minTimes[coroCnt], maxTimes[coroCnt]) / 100);

        coroCnt++;
        if (coroCnt >= velocities.Length)
        {
            coroCnt = 0;
        }

        currentCoro[0] = StartCoroutine(Action1());
        MusicMgr.GetInstance().PlaySound("_Using/DM-CGS-47", false);

    }

    IEnumerator CreateSpaceStorm()
    {
        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(0, 0, 0), new Vector3(1.5f, 1.5f, 1), true, new float[2] { 4f, 4f }, 8f);
        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(2, 2, 0), new Vector3(1f, 1f, 1), false, new float[1] { 8f }, 8f);
        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(-2, 2, 0), new Vector3(1f, 1f, 1), false, new float[1] { 8f }, 8f);
        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(2, -2, 0), new Vector3(1f, 1f, 1), false, new float[1] { 8f }, 8f);
        MakeEnergyWall("_Perfab/Fishing/Hooking/CircleEnergyWall", new Vector3(-2, -2, 0), new Vector3(1f, 1f, 1), false, new float[1] { 8f }, 8f);
        yield return new WaitForSeconds(8f);

        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
