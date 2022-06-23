using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle203 : FishCircle
{
    Vector3[] velocities;
    float[] minTimes;
    float[] maxTimes;
    public override void InitialStatus()
    {
        base.InitialStatus();
        fishID = 203;
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
        minTimes = new float[4] { 700, 20, 250, 50 };
        maxTimes = new float[4] { 600, 50, 300, 100 };
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
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", new Vector3(3, 0, 0), new Vector3(2.2f, 2.2f, 1),0, 1f, 2f);
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", new Vector3(-3, 0, 0), new Vector3(2.2f, 2.2f, 1),0, 1f, 2f);
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", new Vector3(0, 3, 0), new Vector3(2.2f, 2.2f, 1),0, 1f, 2f);
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", new Vector3(0, -3, 0), new Vector3(2.2f, 2.2f, 1),0, 1f, 2f);
        yield return new WaitForSeconds(1f);
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", new Vector3(1.5f, 1.5f, 0), new Vector3(1f, 1f, 1),0, 2f, 2f);
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", new Vector3(1.5f, -1.5f, 0), new Vector3(1f, 1f, 1),0, 2f, 2f);
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", new Vector3(-1.5f, 1.5f, 0), new Vector3(1f, 1f, 1),0, 2f, 2f);
        MakeDefensiveWeapon("_Perfab/Fishing/Hooking/CircleDefensiveWeapon", new Vector3(-1.5f, -1.5f, 0), new Vector3(1f, 1f, 1),0, 2f, 2f);
        yield return new WaitForSeconds(1f);
        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
