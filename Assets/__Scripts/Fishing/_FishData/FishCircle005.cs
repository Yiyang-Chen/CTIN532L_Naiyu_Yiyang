using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle005 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 3;
        _fishData = _FishDataMgr.GetInstance().GetFishByID(fishID);
        fSpeed = _fishData.fSpeed;
        spaceStormIndex = _fishData.spaceStormIndex;
        restIndex = _fishData.notContainindex;
    }

    public override void BeginMovement(GameObject obj)
    {
        base.BeginMovement(obj);

        coroCnt = 0; 
        velocities = new Vector3[3] { new Vector3(-4, -3, 0), new Vector3(1f, 0, 0), new Vector3(-3, 4, 0) };
        minTimes = new float[3] { 300, 250, 250 };
        maxTimes = new float[3] { 450, 350, 300 };
        currentCoro = new Coroutine[2] { StartCoroutine(Action1()), StartCoroutine(CreateSpaceStorm()) };
    }

    /// <summary>
    /// ��ĵ�һ�ֶ���
    /// </summary>
    /// <returns></returns>
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
    /// ��ĵ�һ�ֶ���
    /// </summary>
    /// <returns></returns>
    IEnumerator Action2()
    {
        velocity = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
        velocity = velocity.normalized*restIndex;

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
        float time = 1.5f;
        MakeSpaceStorm(Vector3.zero, new Vector3(8, 8, 1), new Vector3(1, 1, 0), 3f, time, "_Perfab/Fishing/Hooking/CircleSpaceStorm");

        yield return new WaitForSeconds(time);

        MakeSpaceStorm(Vector3.zero, new Vector3(8, 8, 1), new Vector3(-1, 1, 0), 3f, time, "_Perfab/Fishing/Hooking/CircleSpaceStorm");

        yield return new WaitForSeconds(time);

        MakeSpaceStorm(Vector3.zero, new Vector3(8, 8, 1), new Vector3(-1, -1, 0), 3f, time, "_Perfab/Fishing/Hooking/CircleSpaceStorm");

        yield return new WaitForSeconds(time);

        MakeSpaceStorm(Vector3.zero, new Vector3(8, 8, 1), new Vector3(1, -1, 0), 3f, time, "_Perfab/Fishing/Hooking/CircleSpaceStorm");

        yield return new WaitForSeconds(time);
        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
