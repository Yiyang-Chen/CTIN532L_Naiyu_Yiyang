using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle306 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 306;
        _fishData = _FishDataMgr.GetInstance().GetFishByID(fishID);
        fSpeed = _fishData.fSpeed;
        spaceStormIndex = _fishData.spaceStormIndex;
        restIndex = _fishData.notContainindex;
    }

    public override void BeginMovement(GameObject obj)
    {
        base.BeginMovement(obj);

        coroCnt = 0;
        velocities = new Vector3[6] { new Vector3(0.2f, 1, 0), new Vector3(1, -0.2f, 0), new Vector3(0.15f, -1, 0), new Vector3(-1, 0.2f, 0), new Vector3(0.1f, 1, 0), new Vector3(1f, -0.3f, 0) };
        minTimes = new float[6] { 250, 250, 250, 300, 300, 250 };
        maxTimes = new float[6] { 400, 400, 400, 500, 500, 400 };
        currentCoro = new Coroutine[2] { StartCoroutine(Action1()), StartCoroutine(CreateSpaceStorm()) };
    }

    /// <summary>
    /// 鱼的第一种动作
    /// </summary>
    /// <returns></returns>
    IEnumerator Action1()
    {
        velocity = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
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
        MakeErosionCloud("_Perfab/Fishing/Hooking/CircleErosionCloud", new Vector3(3, 0, 0), new Vector3(3f, 3f, 1), 0, 100f);
        MakeErosionCloud("_Perfab/Fishing/Hooking/CircleErosionCloud", new Vector3(-3, 0, 0), new Vector3(3f, 3f, 1), 0, 100f);
        MakeErosionCloud("_Perfab/Fishing/Hooking/CircleErosionCloud", new Vector3(0, 3, 0), new Vector3(3f, 3f, 1), 0, 100f);
        MakeErosionCloud("_Perfab/Fishing/Hooking/CircleErosionCloud", new Vector3(0, -3, 0), new Vector3(3f, 3f, 1), 0, 100f);
        MakeErosionCloud("_Perfab/Fishing/Hooking/CircleErosionCloud", new Vector3(0, 0, 0), new Vector3(3f, 3f, 1), 0, 100f);
        yield return new WaitForSeconds(100f);

        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
