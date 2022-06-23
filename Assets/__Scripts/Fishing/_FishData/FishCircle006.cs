using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle006 : FishCircle
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
    /// 鱼的第一种动作
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
        float time = 1.5f;
        int action = Random.Range(0, 2);

        if(action == 0)
        {
            time = 2f;
            MakeSpaceStorm(Vector3.zero, new Vector3(8, 8, 1), new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), 0), 2f, time, "_Perfab/Fishing/Hooking/CircleSpaceStorm");

            yield return new WaitForSeconds(time);
        }
        else
        {
            time = 4f;
            MakeSpaceStorm(Vector3.zero, new Vector3(8, 8, 1), new Vector3(1, 0, 0), 4f, time, "_Perfab/Fishing/Hooking/CircleSpaceStormInside");

            yield return new WaitForSeconds(time);

        }
        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
