using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMain : MonoBehaviour
{
    public GameObject mapBK;
    public Sprite[] backgrounds;

    private bool isZoomIn;
    private Vector3 targetV;
    private float timeLerped;
    private float lerpDuration;

    public GameObject shipSprite;
    public GameObject rodSprite;
    public GameObject _light;
    public GameObject bigRoom;

    public Sprite[] shipSPrites;
    public Sprite[] roomsNormal;
    public Sprite[] roomsOnclick;

    public GameObject controlR;
    public GameObject fishR;
    public GameObject powerR;
    public GameObject collectionR;

    private bool isFailWaitingFish;
    private bool isIncorrectMapOrBait;
    private bool isFailHookingFish;
    private int successFishID;

    // Start is called before the first frame update
    void Start()
    {
        isZoomIn = false;
        targetV = new Vector3(1, 1, 1);
        timeLerped = 0f;
        lerpDuration = 1f;

        _FishDataMgr.GetInstance().InitItemsInfo();
        LoadShipMain();
    }

    private void Update()
    {
        if (!PopUpMgr.GetInstance().Awake)
        {
            Sprite catImage = ResourceMgr.GetInstance().Load<Sprite>("_Sprites/_Start/Them");
            List<string> s = new List<string>();
            s.Add("Ah, my new assistant. I see that you're finally here. First day reporting to the fishing ship, aren't you? I am your new boss, a scientist focusing on biology.");
            s.Add("Let's get you familiarized with the fishing system on board. Click 'Fish' to go to the fishing room. And the click start.");

            List<Sprite> sp = new List<Sprite>() { catImage, catImage };
            UIMgr.GetInstance().PopUp("The Cat", s.ToArray(),
                new Vector2(0, -300), new float[2] { 1920,1920 }, new float[2] { 550,550 },
                sp.ToArray());
            PopUpMgr.GetInstance().Awake = true;
        }
        if (isZoomIn)
        {
            BackgroundZoomInEffect();
        }
    }

    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener("StartFishing", StartFishing);
        EventCenter.GetInstance().AddEventListener("EndFishing", EndFishing);
        EventCenter.GetInstance().AddEventListener("LoadShipMain", LoadShipMain);
        EventCenter.GetInstance().AddEventListener("FailWaitingFish", FailWaitingFish);
        EventCenter.GetInstance().AddEventListener("IncorrectMapOrBait", IncorrectMapOrBait);
        EventCenter.GetInstance().AddEventListener("FailHookingFish", FailHookingFish);
        EventCenter.GetInstance().AddEventListener<int>("SuccessHookingFish", SuccessHookingFish); 
        EventCenter.GetInstance().AddEventListener<PerfabWaitingFish>("StartHooking", StartHooking);
        EventCenter.GetInstance().AddEventListener<string>("ScreenMouseEnterButton", ScreenMouseEnterButton);
        EventCenter.GetInstance().AddEventListener<string>("ScreenMouseExitButton", ScreenMouseExitButton);
        EventCenter.GetInstance().AddEventListener<string>("ClickScreenRoom", ClickScreenRoom);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener("StartFishing", StartFishing);
        EventCenter.GetInstance().RemoveEventListener("EndFishing", EndFishing);
        EventCenter.GetInstance().RemoveEventListener("LoadShipMain", LoadShipMain);
        EventCenter.GetInstance().RemoveEventListener("FailWaitingFish", FailWaitingFish);
        EventCenter.GetInstance().RemoveEventListener("FailHookingFish", FailHookingFish);
        EventCenter.GetInstance().RemoveEventListener<int>("SuccessHookingFish", SuccessHookingFish);
        EventCenter.GetInstance().RemoveEventListener<PerfabWaitingFish>("StartHooking", StartHooking);
        EventCenter.GetInstance().RemoveEventListener<string>("ScreenMouseEnterButton", ScreenMouseEnterButton);
        EventCenter.GetInstance().RemoveEventListener<string>("ScreenMouseExitButton", ScreenMouseExitButton);
        EventCenter.GetInstance().RemoveEventListener<string>("ClickScreenRoom", ClickScreenRoom);
    }

    public void ChangeObjAlpha(GameObject obj, float alpha)
    {
        Color c = obj.GetComponent<SpriteRenderer>().color;
        obj.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, alpha);
    }

    public void ChangeObjSprite(GameObject obj, Sprite s)
    {
        obj.GetComponent<SpriteRenderer>().sprite = s;
    }

    public void BoolShip(bool ac)
    {
        shipSprite.SetActive(ac);
        BoolShipRooms(ac);
    }

    public void BoolShipRooms(bool ac)
    {
        controlR.SetActive(ac);
        fishR.SetActive(ac);
        powerR.SetActive(ac);
        collectionR.SetActive(ac);
    }

    public void BoolBigRoom(bool ac, string name)
    {
        if (!ac) bigRoom.GetComponent<BigRoom>().DeletePerfab();
        else bigRoom.GetComponent<BigRoom>().LoadPerfab(name);  
    }

    public void SwitchBigRoom(int index)
    {
        if (index < 0 || index >= 4)
        {
            index = -1;
        }

        switch (index)
        {
            case 0:
                BoolBigRoom(true, "_Perfab/Ship/Rooms/ControlRPerfab");
                break;
            case 1:
                BoolBigRoom(true, "_Perfab/Ship/Rooms/FishingRPerfab");
                break;
            case 2:
                BoolBigRoom(true, "_Perfab/Ship/Rooms/MapRPerfab");
                break;
            case 3:
                BoolBigRoom(true, "_Perfab/Ship/Rooms/CollectionRPerfab");
                break;
            default:
                BoolBigRoom(true, null);
                break;
        }
    }

    public void SwitchBackground(bool isBigRoom,bool shouldZoom)
    {
        if (!shouldZoom)
        {
            isZoomIn = false;
            targetV = new Vector3(1, 1, 1);
            timeLerped = 0f;
            SetBackgroundScale(targetV);
        }
        else
        {
            isZoomIn = true;
            targetV = new Vector3(1f, 1.1f, 1);
            timeLerped = 0f;
            lerpDuration = 2f;
            SetBackgroundScale(new Vector3(0.8f, 0.8f, 1));
        }     

        if(isBigRoom) mapBK.GetComponent<SpriteRenderer>().sprite = backgrounds[0];
        else mapBK.GetComponent<SpriteRenderer>().sprite = backgrounds[MapMgr.GetInstance().GetMapByInt()+1];
    }

    private void BackgroundZoomInEffect()
    {
        Vector3 currentScale = mapBK.transform.localScale;

        SetBackgroundScale(Vector3.Lerp(currentScale,targetV,timeLerped/lerpDuration));
        timeLerped += Time.deltaTime;

        if (mapBK.transform.localScale.x >= 1.04|| timeLerped >= lerpDuration)
        {
            SetBackgroundScale(targetV);
            timeLerped = 0f;
            isZoomIn = false;
        }
    }

    private void SetBackgroundScale(Vector3 scale)
    {
        mapBK.transform.localScale = scale;
    }

    private void ScreenMouseEnterButton(string rooms)
    {
        //GameObject room = null;
        switch (rooms)
        {
            case "SystemRoom":
                ChangeObjSprite(controlR, roomsOnclick[0]);
                break;
            case "FishingRoom":
                ChangeObjSprite(fishR, roomsOnclick[1]);      
                break;
            case "PowerRoom":
                ChangeObjSprite(powerR, roomsOnclick[2]);   
                break;
            case "CollectionRoom":
                ChangeObjSprite(collectionR, roomsOnclick[3]);
                break;
            default:
                return;
        }
        
        //ChangeObjAlpha(room, 1f);
    }

    private void ScreenMouseExitButton(string rooms)
    {
        //GameObject room = null;
        switch (rooms)
        {
            case "SystemRoom":
                ChangeObjSprite(controlR, roomsNormal[0]);
                break;
            case "FishingRoom":
                ChangeObjSprite(fishR, roomsNormal[1]);
                break;
            case "PowerRoom":
                ChangeObjSprite(powerR, roomsNormal[2]);
                break;
            case "CollectionRoom":
                ChangeObjSprite(collectionR, roomsNormal[3]);
                break;
            default:
                return;
        }
        //ChangeObjAlpha(room, 0.5f);
    }

    private void ClickScreenRoom(string rooms)
    {
        BoolShip(false);
        int index;
        switch (rooms)
        {
            case "SystemRoom":
                index = 0;
                break;
            case "FishingRoom":
                index = 1;
                break;
            case "PowerRoom":
                index = 2;
                break;
            case "CollectionRoom":
                index = 3;
                break;
            default:
                return;
        }
        SwitchBigRoom(index);
        SwitchBackground(true, true);
    }

    //Events
    private void LoadShipMain()
    {
        UIMgr.GetInstance().ShowPanel<ShipPanel>("Ship/_Ship/ShipPanel");
        ChangeObjSprite(controlR, roomsNormal[0]);
        ChangeObjSprite(fishR, roomsNormal[1]);
        ChangeObjSprite(powerR, roomsNormal[2]);
        ChangeObjSprite(collectionR, roomsNormal[3]);
        BoolShip(true);
        BoolBigRoom(false, null);
        SwitchBackground(false, false);
    }

    private void StartFishing()
    {
        isFailWaitingFish = false;
        isIncorrectMapOrBait = false;
        isFailHookingFish = false;
        BoolShip(true);
        BoolShipRooms(false);
        BoolBigRoom(false, null);
        SwitchBackground(false, false);
        this.transform.localPosition = new Vector3(6.5f,-2.5f,0);
        this.transform.localScale = new Vector3(1, 1, 1);
        ChangeObjSprite(shipSprite, shipSPrites[1]);
    }

    private void EndFishing()
    {
        MusicMgr.GetInstance().CrossFading("mx_Menu");
        BoolShip(false);
        SwitchBigRoom(1);
        SwitchBackground(true, true);
        this.transform.localPosition = new Vector3(0, -1.2f, 0);
        this.transform.localScale = new Vector3(1.7f, 1.7f, 1);
        ChangeObjAlpha(_light, 1f);
        ChangeObjAlpha(shipSprite, 1f);
        ChangeObjAlpha(rodSprite, 1f);
        ChangeObjSprite(shipSprite, shipSPrites[0]);
        UIMgr.GetInstance().ShowPanel<FishingRPanel>("Ship/Room_Fishing/FishingRPanel", (obj) =>
        {
            obj.extraText = true;
            obj.isFailWaitingFish = isFailWaitingFish;
            obj.isIncorrectMapOrBait = isIncorrectMapOrBait;
            obj.isFailHookingFish = isFailHookingFish;
            if (!isFailHookingFish) obj.successFishID = successFishID;
            EventCenter.GetInstance().EventTrigger("HideWaitingPanel");
            EventCenter.GetInstance().EventTrigger("HideHookingPanel");
        });
    }

    private void StartHooking(PerfabWaitingFish fishID)
    {
        ChangeObjAlpha(_light, 0f);
        ChangeObjAlpha(shipSprite, 0.1f);
        ChangeObjAlpha(rodSprite, 0.8f);
    }
    private void FailHookingFish()
    {
        isFailHookingFish = true;
    }

    private void FailWaitingFish()
    {
        isFailWaitingFish = true;
    }

    private void IncorrectMapOrBait()
    {
        isIncorrectMapOrBait = true;
    }

    private void SuccessHookingFish(int fishID)
    {
        successFishID = fishID;
    }
}
