using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle204 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 204;
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
        yield return new WaitForSeconds(1f);
        Vector3 v = (transform.position + playerPosition) / 2 + velocity- spriteContainer.transform.position;
        Vector3 s = new Vector3((Mathf.Abs(v.normalized.x) + 1) * 0.3f, (Mathf.Abs(v.normalized.x) + 1) * 0.3f, 1) ;
        v += (Quaternion.AngleAxis(90, Vector3.forward) * v).normalized*0.2f;
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", v, s,0, 3f, 2f);
        yield return new WaitForSeconds(0.7f);

        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
