using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public float showDuration;
    public float lerpDuration;
    public SpriteRenderer CreditContainer;
    public Sprite[] creditPictures;
    private bool isShowing;
    private int currentCredit;
    private float timeShowed;

    public GameObject ship;
    private bool isZoomIn;
    private float shipLerpDuration;
    private float shipTimeLerped;
    public Vector3 targetS;
    public Vector3 targetP;

    public GameObject Backgrounds;
    public Sprite[] backgroundSprites;
    public SpriteRenderer BK;

    private AsyncOperation asyO;
    void Start()
    {
        isShowing = false;
        isZoomIn = false;
        asyO = null;
        ShowPictures();

    }
    private void Update()
    {
        if (isShowing)
        {
            PictureShowOffEffect();
        }

        if (isZoomIn)
        {
            BackgroundZoomInEffect();
        }
    }

    private void OnEnable()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
        EventCenter.GetInstance().AddEventListener("SwitchToMainScene", SwitchToMainScene);
        EventCenter.GetInstance().AddEventListener<int>("SwitchCreditBackground", SwitchCreditBackground);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener("SwitchToMainScene", SwitchToMainScene);
        EventCenter.GetInstance().AddEventListener<int>("SwitchCreditBackground", SwitchCreditBackground);
    }

    private void ShowPictures()
    {
        CreditContainer.gameObject.SetActive(true);
        Backgrounds.SetActive(false);
        InputMgr.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener("AnyKeyPushDown", AnyKeyPushDown);
        isShowing = true;
        currentCredit = 0;
        timeShowed = 0;
        CreditContainer.color = new Color(CreditContainer.color.r, CreditContainer.color.g, CreditContainer.color.b, 0);
    }

    private void PictureShowOffEffect()
    {
        CreditContainer.sprite = creditPictures[currentCredit];

        //逐渐显示
        if (CreditContainer.color.a>=0.99 || timeShowed >= lerpDuration)//可以点击跳过
        {
            CreditContainer.color = new Color(CreditContainer.color.r, CreditContainer.color.g, CreditContainer.color.b, 1);
        }
        else
        {
            CreditContainer.color = new Color(CreditContainer.color.r, CreditContainer.color.g, CreditContainer.color.b, Mathf.Lerp(0, 1, timeShowed / lerpDuration));
        }

        //停留一会
        timeShowed += Time.deltaTime;
        
        //下一张
        if(timeShowed >= showDuration)//可以点击跳过
        {
            currentCredit++;
            timeShowed = 0f;
            CreditContainer.color = new Color(CreditContainer.color.r, CreditContainer.color.g, CreditContainer.color.b, 0);
        }

        //显示完结束
        if (currentCredit == creditPictures.Length)
        {
            isShowing = false;
            CreditContainer.gameObject.SetActive(false);
            timeShowed = 0f;

            InputMgr.GetInstance().StartOrEndCheck(false);
            EventCenter.GetInstance().RemoveEventListener("AnyKeyPushDown", AnyKeyPushDown);
            
            Backgrounds.SetActive(true);
            UIMgr.GetInstance().ShowPanel<StartScreenPanel>("_Start/StartScenePanel", (obj) => { }, E_UI_Layer.MID);

            MusicMgr.GetInstance().PlayBkMusic("mx_Menu");

            //用这种形式popup
            if (!PopUpMgr.GetInstance().AlphaVersion) UIMgr.GetInstance().PopUp("Beta Version",
                new string[2] { "The game is in FINAL RELEASED version.\n\nThe suggested resolution is the Multipliers of 1920 x 1080",
                "The game has not been fully checked on a mac computer.\n\nStrange bugs may appear on a mac system."});
        }
    }

    //Event
    private void AnyKeyPushDown()
    {
        if (timeShowed <= lerpDuration) timeShowed = lerpDuration;
        else if (timeShowed <= showDuration) timeShowed = showDuration;
    }

    private void BackgroundZoomInEffect()
    {
        if(shipTimeLerped >= 0.2f&&asyO == null) StartCoroutine(LoadAsync());

        Vector3 currentScale = ship.transform.localScale;
        Vector3 currentPosition = ship.transform.localPosition;

        SetShip(Vector3.Lerp(currentScale, targetS, shipTimeLerped / shipLerpDuration), Vector3.Lerp(currentPosition, targetP, shipTimeLerped / shipLerpDuration));
        shipTimeLerped += Time.deltaTime;   

        if (shipTimeLerped >= shipLerpDuration*0.9f)
        {
            SetShip(targetS,targetP);
            shipTimeLerped = 0f;
            isZoomIn = false;
        }
    }
    private void SetShip(Vector3 scale,Vector3 position)
    {
        ship.transform.localPosition = position;
        ship.transform.localScale = scale;
    }

    IEnumerator LoadAsync()
    {
        asyO = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);

        while (!asyO.isDone)
        {
            yield return null;
        }

    }

    //Events
    private void SwitchToMainScene()
    {
        isZoomIn = true;
        targetS = new Vector3(1.7f, 1.7f, 1f);
        targetP = new Vector3(0f, -1.2f, 0f);
        shipTimeLerped = 0f;
        shipLerpDuration = 2f;
        //StartCoroutine(LoadAsync());
    }

    private void SwitchCreditBackground(int index)
    {
        BK.sprite = backgroundSprites[index];
    }
}
