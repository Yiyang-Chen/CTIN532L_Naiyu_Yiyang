using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingFish : MonoBehaviour
{
    private bool isWaiting;
    private bool isKeyDown;

    public GameObject spriteContainer;

    private GameObject[] fishPerfabs;
    private float waitTime;
    private float DELTATIME;

    private bool[] loaded;
    // Start is called before the first frame update
    private void Start()
    {
        isKeyDown = false;
        isWaiting = false;
        loaded = new bool[5] { false,false,false,false,false};
        fishPerfabs = new GameObject[5];
        waitTime = 0;
        DELTATIME = 0.4f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isWaiting&& isKeyDown)
        {
            StopAllCoroutines();
            if (MapMgr.GetInstance().currentMap == SpaceMap.NORMAL)
            {
                MusicMgr.GetInstance().CrossFading("MX_CatchingPhase_map1");
            }
            else if (MapMgr.GetInstance().currentMap == SpaceMap.CYBER)
            {
                MusicMgr.GetInstance().CrossFading("MX_CatchingPhase_map2");
            }
            else if (MapMgr.GetInstance().currentMap == SpaceMap.CIVILIZATION)
            {
                MusicMgr.GetInstance().CrossFading("MX_CatchingPhase_map3");
            }
            else if (MapMgr.GetInstance().currentMap == SpaceMap.INSECT)
            {
                MusicMgr.GetInstance().CrossFading("MX_CatchingPhase_map4");
            }

            EventCenter.GetInstance().EventTrigger<PerfabWaitingFish>("StartHooking", fishPerfabs[0].GetComponent<PerfabWaitingFish>());

            string[] content;
            if (!PopUpMgr.GetInstance().MapSkills[MapMgr.GetInstance().GetMapByInt()])
            {
                content = new string[3] { "During HOOKING phase, the fish struggles, and you need to keep the fish in the circle.","Use WASD to move your circle.\n\nYou can review this information later in COLLECTION -> TUTORIAL.",
                PopUpMgr.GetInstance().mapSkillContent[MapMgr.GetInstance().GetMapByInt()]};
                PopUpMgr.GetInstance().MapSkills[MapMgr.GetInstance().GetMapByInt()] = true;

                UIMgr.GetInstance().PopUp("Hooking Fish",
                    content,
                    new Vector2(600, -200), new float[3] { 1000, 1000, 1000 }, new float[3] { 500, 700, 1000 }, null
                    );
            }

            EndWaiting();
        }
    }
    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener("StartWaiting", StartWaiting);
        EventCenter.GetInstance().AddEventListener("EndFishing", EndWaiting);
        EventCenter.GetInstance().AddEventListener<int>("LoadedFishingperfab", LoadedFishingPerfab);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener("StartWaiting", StartWaiting);
        EventCenter.GetInstance().RemoveEventListener("EndFishing", EndWaiting);
        EventCenter.GetInstance().RemoveEventListener<int>("LoadedFishingperfab", LoadedFishingPerfab);
    }

    private void CheckMouseUp(int mouse)
    {
        switch (mouse)
        {
            case 0:
                isKeyDown = false;
                break;
            case 1:
                break;
        }
    }

    private void CheckInputUp(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Space:
                isKeyDown = false;
                break;
        }
    }

    private void CheckMouseDown(int mouse)
    {
        switch (mouse)
        {
            case 0:
                isKeyDown = true;
                WaitingClickedMetric();
                break;
            case 1:
                break;
        }
    }

    private void CheckInputDown(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Space:
                isKeyDown = true;
                WaitingClickedMetric();
                break;
            case KeyCode.Escape:
                EventCenter.GetInstance().EventTrigger("EndFishing");
                break;
        }
    }

    private void WaitingClickedMetric()
    {
        if (MetricManagerScript.instance != null)
        {
            for (int i = 0; i < 5; i++)
            {
                MetricManagerScript.instance.LogString("waitingClickedPosition" + i, fishPerfabs[i].transform.position.ToString());
                MetricManagerScript.instance.LogString("waitingClickedVelocity" + i, fishPerfabs[i].GetComponent<Rigidbody2D>().velocity.ToString());
            }
        }
    }

    private void EndWaiting()
    {
        StopAllCoroutines();
        isWaiting = false;
        isKeyDown = false;
        EventCenter.GetInstance().RemoveEventListener<int>("MouseUp", CheckMouseUp);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("KeyUp", CheckInputUp);
        EventCenter.GetInstance().RemoveEventListener<int>("MouseDown", CheckMouseDown);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("KeyDown", CheckInputDown);
        for (int i = 0; i < 5; i++)
        {
            PoolMgr.GetInstance().PushObj("_Perfab/Fishing/Waiting/PerfabWaitingFish", fishPerfabs[i]);
        }
    }

    IEnumerator WaitAndEnd()
    {
        yield return new WaitForSeconds(waitTime);

        if(fishPerfabs[0].GetComponent<PerfabWaitingFish>().isTarget == true)
        {
            isWaiting = true;
            StartCoroutine(PlayAudioManyTimes("_Using/DM-CGS-07", fishPerfabs[0].GetComponent<PerfabWaitingFish>()._data.strength));
        }

        yield return new WaitForSeconds(2.0f);
        if (fishPerfabs[0].GetComponent<PerfabWaitingFish>().isTarget == true)
            EventCenter.GetInstance().EventTrigger("FailWaitingFish");
        else
            EventCenter.GetInstance().EventTrigger("IncorrectMapOrBait");
        EventCenter.GetInstance().EventTrigger("EndFishing");
    }

    IEnumerator PlayAudioManyTimes(string s, int n)
    {
        for (int i = 0; i < n; i++)
        {
            AudioSource aS = null;
            bool loadFinished = false;
            for (int j = 0; j < 30; j++)
            {
                MusicMgr.GetInstance().PlaySound(s, false, (source) =>
                {
                    aS = source;
                    loadFinished = true;
                });
                yield return new WaitForSeconds(0.01f);
                if (loadFinished) break;
            }
            if (i < n - 1)
            {
                yield return new WaitForSeconds(aS.clip.length / 6);

                MusicMgr.GetInstance().StopSound(aS);

                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                yield return new WaitForSeconds(aS.clip.length);
            }
        }
    }

    private void StartWaiting()
    {
        loaded = new bool[5] { false, false, false, false, false };
        isWaiting = false;
        waitTime = 0;

        for (int i = 0; i < 5; i++)
        {
            int index = i;
            _FishData newFishData = _FishDataMgr.GetInstance().NewFish();      
            PoolMgr.GetInstance().GetObj("_Perfab/Fishing/Waiting/PerfabWaitingFish", (obj) =>
            {
                obj.transform.SetParent(spriteContainer.transform);
                obj.GetComponent<PerfabWaitingFish>().spriteContainer = spriteContainer;
                obj.GetComponent<PerfabWaitingFish>()._data = newFishData;
                obj.GetComponent<PerfabWaitingFish>().SetPosition();
                fishPerfabs[index] = obj;
                EventCenter.GetInstance().EventTrigger("LoadedFishingperfab", index);
            });
        }

        InputMgr.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener<int>("MouseUp", CheckMouseUp);
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyUp", CheckInputUp);
        EventCenter.GetInstance().AddEventListener<int>("MouseDown", CheckMouseDown);
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyDown", CheckInputDown);
    }

    private void LoadedFishingPerfab(int index)
    {
        loaded[index] = true;

        if (loaded[0] && loaded[1] && loaded[2] && loaded[3] && loaded[4])
        {
            for (int i = 0; i < 5; i++)
            {
                fishPerfabs[i].GetComponent<PerfabWaitingFish>().isTarget = false;

                if (i == 0)
                {
                    waitTime = Random.Range(fishPerfabs[i].GetComponent<PerfabWaitingFish>()._data.beginWaitTime, fishPerfabs[i].GetComponent<PerfabWaitingFish>()._data.endWaitTime);
                    
                    if(fishPerfabs[i].GetComponent<PerfabWaitingFish>()._data.fishID!=0) 
                        fishPerfabs[i].GetComponent<PerfabWaitingFish>().isTarget = true;

                    if (MetricManagerScript.instance != null)
                    {
                        MetricManagerScript.instance.LogString("waitingFishWaitTime", waitTime.ToString());
                        MetricManagerScript.instance.LogString("TargetFishID", fishPerfabs[0].GetComponent<PerfabWaitingFish>()._data.fishID.ToString("D3"));
                    }
                }

                fishPerfabs[i].GetComponent<PerfabWaitingFish>().waitTime = waitTime+ DELTATIME;
                
            }
            EventCenter.GetInstance().EventTrigger("LoadedAllFishingperfab");
            
            StartCoroutine(WaitAndEnd());
        }
    }
}
