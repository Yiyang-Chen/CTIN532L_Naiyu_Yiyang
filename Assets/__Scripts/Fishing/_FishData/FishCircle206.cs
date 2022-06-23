using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle206 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 206;
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
        yield return new WaitForSeconds(1f);
        Vector3 v = new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f),0);
        Vector3 p = v.normalized * 2.7f ;
        for(int i = 0; i < 24; i++)
        {
            Vector3 normal = Quaternion.AngleAxis(90, Vector3.forward) * p;
            float angle = Vector3.Angle(Vector3.right, normal);
            MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", p, new Vector3(16f, 0.1f, 1),angle, 0.1f, 2f);
            p = Quaternion.AngleAxis(15, Vector3.forward) * p;
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(2f);
        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
