using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenPanel : BasePanel
{
    public GameObject buttons;
    public GameObject loadGame;
    public GameObject[] Icons;
    public string[] buttonStrings;
    private bool hasLoad;
    private bool[] isShowingImgs;
    private float lerpDuration;
    private float disappearSpeedIndex;
    public float[] timeLerped;
    private float minAlpha;
    private float maxAlpha;
    private void Start()
    {
        buttonStrings = new string[5] { "LoadGame", "NewGame", "Settings" , "Credit" , "Exit" };
        hasLoad = false;
        isShowingImgs = new bool[5] { false, false, false, false, false };
        lerpDuration = 0.45f;
        disappearSpeedIndex = 1.2f;
        timeLerped = new float[5] {0,0,0,0,0};
        minAlpha = 0.1f;
        maxAlpha = 0.3f;

        this.transform.localPosition = new Vector3(-510,-40,0);
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(600,800);

        if (hasLoad)
        {
            buttons.transform.localPosition = new Vector3(buttons.transform.localPosition.x, buttons.transform.localPosition.y-55, buttons.transform.localPosition.z);
            loadGame.SetActive(true);
            UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[0])[0], EventTriggerType.PointerEnter, (data) =>
            {
                MouseEnter(0);
            });

            UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[0])[0], EventTriggerType.PointerExit, (data) =>
            {
                MouseExit(0);
            });
        } 

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[1])[0], EventTriggerType.PointerEnter, (data) =>
        {
            MouseEnter(1);
        });

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[1])[0], EventTriggerType.PointerExit, (data) =>
        {
            MouseExit(1);
        });

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[2])[0], EventTriggerType.PointerEnter, (data) =>
        {
            MouseEnter(2);
        });

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[2])[0], EventTriggerType.PointerExit, (data) =>
        {
            MouseExit(2);
        });

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[3])[0], EventTriggerType.PointerEnter, (data) =>
        {
            MouseEnter(3);
        });

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[3])[0], EventTriggerType.PointerExit, (data) =>
        {
            MouseExit(3);
        });

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[4])[0], EventTriggerType.PointerEnter, (data) =>
        {
            MouseEnter(4);
        });

        UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[4])[0], EventTriggerType.PointerExit, (data) =>
        {
            MouseExit(4);
        });
    }

    private void Update()
    {
        int j = 1;
        if (hasLoad) j = 0; 
        for(int i = j; i < isShowingImgs.Length; i++)
        {
            if (isShowingImgs[i])
            {
                if (timeLerped[i] <= lerpDuration - 0.01)
                {
                    timeLerped[i] += Time.deltaTime;
                }
                else
                {
                    timeLerped[i] = lerpDuration;
                }
                ChangeImageAlpha(i);
            }
            else
            {
                if (timeLerped[i] >= 0.01)
                {
                    timeLerped[i] -= Time.deltaTime* disappearSpeedIndex;
                    ChangeImageAlpha(i);
                }
                else
                {
                    timeLerped[i] = 0;
                    //GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color = new Color(GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.r, GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.g, GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.b, 0);
                    Icons[i].gameObject.GetComponent<Image>().color = new Color(GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.r, GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.g, GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.b, 0);
                }

            }
        }
    }

    private void ChangeImageAlpha(int i)
    {
        //GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color = new Color(GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.r, GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.g, GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.b, Mathf.Lerp(minAlpha, maxAlpha, timeLerped[i] / lerpDuration));
        Icons[i].gameObject.GetComponent<Image>().color = new Color(GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.r, GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.g, GetControl<Button>(buttonStrings[i])[0].gameObject.GetComponent<Image>().color.b, Mathf.Lerp(minAlpha, maxAlpha, timeLerped[i] / lerpDuration));
    }

    private void MouseEnter(int i)
    {
        string buttonS = buttonStrings[i];
        isShowingImgs[i] = true;
        GetControl<Button>(buttonS)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit(int i)
    {
        string buttonS = buttonStrings[i];
        isShowingImgs[i] = false;
        GetControl<Button>(buttonS)[0].transform.localScale = new Vector3(1f, 1f, 1);
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
    }
    protected override void OnClick(string btnName)
    {
        StartCoroutine(SmallAndLarge(btnName));
        if (btnName == buttonStrings[0])//load
        {

        }else if(btnName == buttonStrings[1])//start
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            UIMgr.GetInstance().HidePanel("_Start/StartScenePanel");
            EventCenter.GetInstance().EventTrigger("SwitchToMainScene");
        }
        else if (btnName == buttonStrings[2])//Settings
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
            UIMgr.GetInstance().HidePanel("_Start/StartScenePanel");
            UIMgr.GetInstance().ShowPanel<SystemSettingsPanel>("_Start/PanelGameSettings",(panel)=> 
            { 
                panel.showWhichFirst = SETTINGMEAU.GRAPHIC; panel.isInStartScene = true;
                panel.GetComponent<RectTransform>().sizeDelta = new Vector2(1380, 780);
            });
        }
        else if (btnName == buttonStrings[3])//Credit
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchRoomSound, false);
            UIMgr.GetInstance().HidePanel("_Start/StartScenePanel");
            UIMgr.GetInstance().ShowPanel<CreditPanel>("_Start/CreditPanel");
        }
        else if (btnName == buttonStrings[4])//Exit
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().exitSound, false);
            Application.Quit(0);
        }
    }
    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
}
