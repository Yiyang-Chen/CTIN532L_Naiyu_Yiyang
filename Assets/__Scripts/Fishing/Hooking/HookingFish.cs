using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookingFish : MonoBehaviour
{
    public GameObject spriteContainer;

    private bool isBKLoaded;
    private bool isPanelLoaded;
    private bool isPlayerLoaded;
    private bool isFishLoaded;
    private bool isHookingStart;
    private bool isPlayerConfirmed;

    private _FishData _data;
    private float pD;
    private float fD;
    private Vector2 pO;
    private Vector2 fO;
    private float notContainindex;

    private GameObject SpriteProgressBar;
    private float successValue;
    private float currentValue;
    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener("HookingTimeExceed", FailHooking);
        EventCenter.GetInstance().AddEventListener("PlayerHookingComfirmed", PlayerHookingComfirmed);
        EventCenter.GetInstance().AddEventListener<PerfabWaitingFish>("StartHooking", StartHooking);
        EventCenter.GetInstance().AddEventListener<Vector2>("UpdatePlayerPosition", UpdatePlayerPosition);
        EventCenter.GetInstance().AddEventListener<Vector2>("UpdateFishPosition", UpdateFishPosition);
        EventCenter.GetInstance().AddEventListener<float>("PlayerInsideDamageZone", PlayerInsideDamageZone);
        EventCenter.GetInstance().AddEventListener<float>("UpdateCurrentPD", UpdateCurrentPD);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener("HookingTimeExceed", FailHooking);
        EventCenter.GetInstance().RemoveEventListener("PlayerHookingComfirmed", PlayerHookingComfirmed);
        EventCenter.GetInstance().RemoveEventListener<PerfabWaitingFish>("StartHooking", StartHooking);
        EventCenter.GetInstance().RemoveEventListener<Vector2>("UpdatePlayerPosition", UpdatePlayerPosition);
        EventCenter.GetInstance().RemoveEventListener<Vector2>("UpdateFishPosition", UpdateFishPosition);
        EventCenter.GetInstance().RemoveEventListener<float>("PlayerInsideDamageZone", PlayerInsideDamageZone);
        EventCenter.GetInstance().RemoveEventListener<float>("UpdateCurrentPD", UpdateCurrentPD);
    }

    private void Start()
    {
        MetricsEvents.OnDataCollect += this.CollectData;
    }
    private void Update()
    {
        if (isPlayerLoaded && isFishLoaded && isBKLoaded && isPlayerConfirmed && isPanelLoaded && !isHookingStart)
        {
            isHookingStart = true;
            EventCenter.GetInstance().EventTrigger<GameObject>("HookingStart", spriteContainer);
        }

        if (isHookingStart)
        {
            if (ContainFish())
            {
                currentValue += Time.deltaTime;
            }
            else
            {
                currentValue -= Time.deltaTime * notContainindex;
            }
            
            SpriteProgressBar.GetComponent<SpriteProgressBar>().SetBarValue(currentValue / successValue);

            if (currentValue >= successValue)
            {
                Initialize();
                if(_FishDataMgr.GetInstance().fishDatas[_data.fishID].strength!=3) _FishDataMgr.GetInstance().fishDatas[_data.fishID].num+=4;
                _FishDataMgr.GetInstance().fishDatas[_data.fishID].totalNum++;
                EventCenter.GetInstance().EventTrigger<int>("SuccessHookingFish", _data.fishID);
                MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().successHookingSound, false);
                Sprite fishImage = ResourceMgr.GetInstance().Load<Sprite>("_Sprites/Fishes/_FishPictures/HookingFish" + _data.fishID.ToString("D3"));
                List<string> s= new List<string>(){ "\nYou caught the fish " + _FishDataMgr.GetInstance().GetFishByID(_data.fishID).fishName + "!!\n\nYou can check out the fish in your collection room.\n\nYou can also choose it as bait in the fishing room.", "Fish Information:\n\n" + _FishDataMgr.GetInstance().GetFishByID(_data.fishID).fishDiscription };
                List<Sprite> sp = new List<Sprite>() { fishImage, fishImage };
                Sprite catImage = ResourceMgr.GetInstance().Load<Sprite>("_Sprites/_Start/Them");

                if (!PopUpMgr.GetInstance().Bait)
                {
                    s.Add("Congratulations, my assistant. Now that you have one fish in your collection, you can use it as a bait to catch higher level fish. Click 'Bait' to choose your bait in fishing room.");
                    sp.Add(catImage);
                    s.Add("A fish in this area seems to come from another space zone. Try to catch it so that we can get the coordination of a new space zone.");
                    sp.Add(catImage);
                    PopUpMgr.GetInstance().Bait = true;
                }
                else if (_data.fishID == 5 && MapMgr.GetInstance().isLock[1])
                {
                    MapMgr.GetInstance().isLock[1] = false;
                    s.Add("You get the interstellar coordinates from this fish. Now Map "+MapMgr.GetInstance().GetMapByString(1)+" is unlocked. Go to naviagtion room to change the map.");
                    sp.Add(fishImage);
                }else if(_data.fishID == 104 && MapMgr.GetInstance().isLock[2])
                {
                    MapMgr.GetInstance().isLock[2] = false;
                    s.Add("You get the interstellar coordinates from this fish. Now Map " + MapMgr.GetInstance().GetMapByString(2) + " is unlocked. Go to naviagtion room to change the map.");
                    sp.Add(fishImage);
                }
                else if (_data.fishID == 105 && MapMgr.GetInstance().isLock[3])
                {
                    MapMgr.GetInstance().isLock[3] = false;
                    s.Add("You get the interstellar coordinates from this fish. Now Map " + MapMgr.GetInstance().GetMapByString(3) + " is unlocked. Go to naviagtion room to change the map.");
                    sp.Add(fishImage);
                }

                UIMgr.GetInstance().PopUp("Congratulations!!",s.ToArray(),
                new Vector2(0, 0), new float[1] { 1920 }, new float[1] { 1080 }, 
                sp.ToArray()
                , "EndFishing");
            }
            else if (currentValue <= 0)
            {
                if (_FishDataMgr.GetInstance().casualMode) currentValue = 0;
                else FailHooking();
            }
        }
    }

    private void Initialize()
    {
        isHookingStart = false;
        isBKLoaded = false;
        isFishLoaded = false;
        isPlayerLoaded = false;
        isPlayerConfirmed = false;
        isPanelLoaded = false;
        pO = Vector2.zero;
        fO = Vector2.zero;
        pD = 0;
        fD = 0;
    }

    /// <summary>
    /// 通过两圆圆心距和半径得到包含关系
    /// </summary>
    /// <returns></returns>
    private bool ContainFish()
    {
        //两圆圆心位置
        float distance = Vector2.Distance(pO, fO);

        if (distance + fD / 2 <= pD)
        {
            return true;
        }

        return false;
    }

    //Events
    private void StartHooking(PerfabWaitingFish fish)
    {
        Initialize();

        int fishID = fish._data.fishID;

        _data = _FishDataMgr.GetInstance().GetFishByID(fish._data.fishID);
        successValue = _data.successHookTime;
        currentValue = _data.beginHookTime;
        notContainindex = _data.notContainindex;
        pD = _data.pD;
        fD = _data.fD;

        PoolMgr.GetInstance().GetObj("_Perfab/Fishing/Hooking/PerfabHookingBK", (obj) =>
        {
            SpriteProgressBar = obj;
            obj.transform.SetParent(spriteContainer.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<SpriteProgressBar>().SetBarValue(currentValue / successValue);
            isBKLoaded = true;
        });

        UIMgr.GetInstance().ShowPanel<HookingPanel>("Fishing/Hooking/HookingPanel", (obj)=>
        {
            obj.hookingMgr = this;
            isPanelLoaded = true;
            EventCenter.GetInstance().EventTrigger("HideWaitingPanel");
        });

        PoolMgr.GetInstance().GetObj("_Perfab/Fishing/Hooking/HookingPlayer", (obj) =>
        {
            obj.transform.SetParent(spriteContainer.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(_data.pD * 2, _data.pD * 2, 1);
            obj.GetComponent<PlayerCircle>().core.GetComponent<PlayerCore>().maxPD = _data.pD;
            isPlayerLoaded = true;
        });

        PoolMgr.GetInstance().GetObj("_Perfab/Fishing/FishDataBase/HookingFish" + fishID.ToString("D3"), (obj) =>
        {
             obj.transform.SetParent(spriteContainer.transform);
             obj.transform.position = fish.gameObject.transform.position;
             obj.transform.localScale = new Vector3(_data.fD * 2, _data.fD * 2, 1);
             isFishLoaded = true;
        });

        InputMgr.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyDown", CheckInputDown);
    }

    private void CheckInputDown(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                BeginHooking();
                break;
            case KeyCode.A:
                BeginHooking();
                break;
            case KeyCode.S:
                BeginHooking();
                break;
            case KeyCode.D:
                BeginHooking();
                break;
            case KeyCode.Escape:
                FailHooking();
                break;
        }
    }

    void BeginHooking()
    {
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("KeyDown", CheckInputDown);
        EventCenter.GetInstance().EventTrigger("PlayerHookingComfirmed");
    }

    //Events

    private void PlayerHookingComfirmed()
    {
        isPlayerConfirmed = true;
    }

    private void UpdatePlayerPosition(Vector2 O)
    {
        pO = O;
    }

    private void UpdateFishPosition(Vector2 O)
    {
        fO = O;
    }

    private void UpdateCurrentPD(float p)
    {
        pD = p;
    }

    public void FailHooking()
    {
        Initialize();
        EventCenter.GetInstance().EventTrigger("FailHookingFish");
        EventCenter.GetInstance().EventTrigger("EndFishing");
    }

    private void PlayerInsideDamageZone(float damage)
    {
        currentValue -= damage;
        if (currentValue < 0) currentValue = 0;
    }

    public void CollectData()
    {
        if (isHookingStart)
        {
            if (MetricManagerScript.instance != null)
            {
                MetricManagerScript.instance.LogString("ProcessBarValue", currentValue.ToString());
            }
        }

    }
}

