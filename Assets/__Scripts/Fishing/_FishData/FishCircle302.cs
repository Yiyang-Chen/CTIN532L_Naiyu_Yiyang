using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle302 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 302;
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
    /// ��ĵ�һ�ֶ���
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
        MakeErosionCloud("_Perfab/Fishing/Hooking/CircleErosionCloud", new Vector3(0,0,0), new Vector3(1f, 1f, 1), 0, 100f);
        yield return new WaitForSeconds(100f);

        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
