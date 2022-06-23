using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodReform : MonoBehaviour
{
    public GameObject ship;
    public GameObject shipLight;
    private GameObject followObj;

    //用Bezier跟随
    public RodForce forceObj;
    LineRenderer lRend;

    public int pointsize;
    public GameObject[] bezierPointList;
    private Vector2[] initLocalPositions;
    public float[] Fc;

    public int sampleSize;
    private Vector2[] samplePositions;

    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener<GameObject>("UpdateCorePosition", UpdateCorePosition);
        EventCenter.GetInstance().AddEventListener<PerfabWaitingFish>("StartHooking", StartHooking);
        EventCenter.GetInstance().AddEventListener("EndFishing", EndFishing);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<GameObject>("UpdateCorePosition", UpdateCorePosition);
        EventCenter.GetInstance().RemoveEventListener<PerfabWaitingFish>("StartHooking", StartHooking);
        EventCenter.GetInstance().RemoveEventListener("EndFishing", EndFishing);
    }

    void Start()
    {
        followObj = shipLight;
        forceObj.transform.position = followObj.transform.position;

        //用Bezier跟随
        pointsize = bezierPointList.Length;
        initLocalPositions = new Vector2[pointsize];

        for (int i = 0; i < pointsize; i++)
        {
            initLocalPositions[i] = bezierPointList[0].transform.InverseTransformPoint(transform.TransformPoint(bezierPointList[i].transform.position));
        }

        samplePositions = new Vector2[sampleSize];
        lRend = this.GetComponent<LineRenderer>();
        lRend.positionCount = sampleSize+1;

        SetLineGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        forceObj.transform.position = followObj.transform.position;

        //用Bezier跟随
        UpdateControlPoint();

        SetLineGenerator();
    }

    private void UpdateControlPoint()
    {
        float F = forceObj.force;

        //根据受力计算各个控制点旋转角度
        for (int i = 1; i < pointsize - 1; i++)//第一个和最后一个点不计算弯曲
        {
            //计算最大弯曲方向
            Vector3 forcePos = forceObj.transform.TransformPoint(new Vector3(0, 0, 0));//bezierPointList[pointsize - 1].transform.position;
            forcePos = bezierPointList[i - 1].transform.InverseTransformPoint(forcePos);
            Vector3 toVector = forcePos - bezierPointList[i].transform.localPosition;
            Quaternion maxRotation = Quaternion.FromToRotation(Vector3.up, toVector);
            //计算弯曲比例
            float rotateRate = Mathf.Clamp(F / Fc[i], 0f, 1.0f);
            //设置旋转角度
            bezierPointList[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), maxRotation, rotateRate);
        }
    }

    private void SetLineGenerator()
    {
            //take sample
        for (int i = 0; i < sampleSize; i++)
        {
            samplePositions[i] = CalculateBezier((float)i / (float)sampleSize);
        }

            //draw line
        for (int i = 0; i < sampleSize; i++)
        {
            lRend.SetPosition(i, V2ToV3(samplePositions[i], 0));
        }

        lRend.SetPosition(sampleSize, forceObj.transform.position);
    }

    private Vector3 V2ToV3(Vector2 v,float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    //贝塞尔曲线公式
    private Vector2 CalculateBezier(float t)
    {
        Vector2 ret = new Vector2(0, 0);
        int n = pointsize;
        GameObject[] allPoints = new GameObject[n+1];

        for(int i = 0; i < n; i++)
        {
            allPoints[i] = bezierPointList[i];
        }
        allPoints[n] = forceObj.gameObject;

        for (int i = 0; i <= n; i++)
        {
            ret = ret + Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) * Cn_m(n, i) * new Vector2(allPoints[i].transform.position.x, allPoints[i].transform.position.y);
        }
        return ret;
    }
    //组合数方程
    private int Cn_m(int n, int m)
    {
        int ret = 1;
        for (int i = 0; i < m; i++)
        {
            ret = ret * (n - i) / (i + 1);
        }
        return ret;
    }


    //Events

    private void UpdateCorePosition(GameObject core)
    {
        followObj = core;
    }

    private void StartHooking(PerfabWaitingFish i)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(lRend.colorGradient.colorKeys
            , new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        lRend.colorGradient = gradient;
    }

    private void EndFishing()
    {
        followObj = shipLight;
        Gradient gradient = new Gradient();
        gradient.SetKeys(lRend.colorGradient.colorKeys
            , new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        lRend.colorGradient = gradient;
    }
}
