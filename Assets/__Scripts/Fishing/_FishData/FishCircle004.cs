using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle004 : FishCircle
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
        velocities = new Vector3[3] {  new Vector3(-4, -3, 0), new Vector3(1f, 0, 0), new Vector3(-3, 4, 0) };
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
        velocity = velocities[coroCnt];
        velocity = velocity.normalized;

        yield return new WaitForSeconds(Random.Range(minTimes[coroCnt], maxTimes[coroCnt]) / 100);

        coroCnt++;
        if (coroCnt >= velocities.Length)
        {
            coroCnt = 0;
        }

        currentCoro[0] = StartCoroutine(Action1());
    }

    IEnumerator CreateSpaceStorm()
    {
        MakeSpaceStorm(new Vector3(parentRB.position.x + 0.75f, parentRB.position.y + 0.75f, 0) - spriteContainer.transform.position, new Vector3(1.5f, 1.5f, 1), new Vector3(-1, 0, 0), 2.5f, 6.5f, "_Perfab/Fishing/Hooking/CircleSpaceStorm");
        MakeSpaceStorm(new Vector3(parentRB.position.x + 0.75f, parentRB.position.y - 0.75f, 0) - spriteContainer.transform.position, new Vector3(1.5f, 1.5f, 1), new Vector3(0, 1, 0), 2.5f, 6.5f, "_Perfab/Fishing/Hooking/CircleSpaceStorm");
        MakeSpaceStorm(new Vector3(parentRB.position.x - 0.75f, parentRB.position.y + 0.75f, 0) - spriteContainer.transform.position, new Vector3(1.5f, 1.5f, 1), new Vector3(0, -1, 0), 2.5f, 6.5f, "_Perfab/Fishing/Hooking/CircleSpaceStorm");
        MakeSpaceStorm(new Vector3(parentRB.position.x - 0.75f, parentRB.position.y - 0.75f, 0) - spriteContainer.transform.position, new Vector3(1.5f, 1.5f, 1), new Vector3(1, 0, 0), 2.5f, 6.5f, "_Perfab/Fishing/Hooking/CircleSpaceStorm");
        yield return new WaitForSeconds(2f);

        currentCoro[1] = StartCoroutine(CreateSpaceStorm());
    }
}
