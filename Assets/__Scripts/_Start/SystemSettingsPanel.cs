using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SETTINGMEAU { GRAPHIC, VOLUME, SYSTEM};

public class SystemSettingsPanel : BasePanel
{
    public Text title;
    public Text content;

    public SETTINGMEAU showWhichFirst;
    public bool isInStartScene;

    public GameObject graphicMeau;
    public GameObject systemMeau;
    public GameObject volumeMeau;

    public GameObject[] ButtonStrongs;
    public string[] buttonStrings;

    public AudioMixer audioMixer;
    public Dropdown resoluDropdown;
    Resolution[] resolutions;
    List<ResItem> resItems;

    public Toggle fullScreenToggle;
    public Toggle vsyncToggle;
    public Dropdown qualityDropdown;
    
    public Slider masterVolumSlider;
    public Slider BKMusicSlider;
    public Slider SFXVloumeSlider;

    public Toggle availabilityToggle;

    private bool changeFullScreen;

    public Text debugText;

    private void Start()
    {
        title.text = "CONTROL ROOM";
        content.text = "Welcome, fisher #0027.\nYou can change the settings of the fishing ship here.";

        buttonStrings = new string[13] { "Graphics", "System", "Back","Save","Load","Exit","Apply","Volumes", "GraphicUI", "VoulmeUI", "SystemUI", "ExitUI", "ExitGameUI" };
        
        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        resItems = new List<ResItem>();
        ResItem lastRes = new ResItem();
        lastRes.height = 0;
        lastRes.width = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if(lastRes.height != resolutions[i].height || lastRes.width != resolutions[i].width)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                ResItem tempRes = new ResItem();
                tempRes.width = resolutions[i].width;
                tempRes.height = resolutions[i].height;
                resItems.Add(tempRes);

                lastRes.width = resolutions[i].width;
                lastRes.height = resolutions[i].height;
            }
        }

        int currentReso = 0;
        for (int i = 0; i < resItems.Count; i++)
        {
            if (resItems[i].width == Screen.currentResolution.width && resItems[i].height == Screen.currentResolution.height)
            {
                currentReso = i;
            }
        }

        resoluDropdown.ClearOptions();
        resoluDropdown.AddOptions(options);
        resoluDropdown.value = currentReso;
        resoluDropdown.RefreshShownValue();

        float currVolum = 0f;
        audioMixer.GetFloat("MasterVolume", out currVolum);
        masterVolumSlider.value = currVolum;
        audioMixer.GetFloat("BKMusicVloume", out currVolum);
        BKMusicSlider.value = currVolum;
        audioMixer.GetFloat("SFXVolume", out currVolum);
        SFXVloumeSlider.value = currVolum;

        qualityDropdown.value = QualitySettingsMgr.GetInstance().GetQuality();

        fullScreenToggle.isOn = Screen.fullScreen;
        changeFullScreen = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

        availabilityToggle.isOn = _FishDataMgr.GetInstance().casualMode;

        for (int i = 0; i < buttonStrings.Length; i++)
        {
            int index = i;
            UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[index])[0], EventTriggerType.PointerEnter, (data) =>
            {
                MouseEnter(index);
            });

            UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[index])[0], EventTriggerType.PointerExit, (data) =>
            {
                MouseExit(index);
            });
            if(i<8) ButtonStrongs[i].SetActive(false);
        }

        if (showWhichFirst == SETTINGMEAU.SYSTEM) SwitchToSystem();
        else if (showWhichFirst == SETTINGMEAU.GRAPHIC) SwitchToGraphics();
        else if (showWhichFirst == SETTINGMEAU.VOLUME) SwitchToVolumes();
    }

    private void Update()
    {
        Screen.fullScreen = changeFullScreen;
    }

    private void MouseEnter(int i)
    {
        
        if (i == 0||i==8)
        {
            GetControl<Button>(buttonStrings[0])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            GetControl<Button>(buttonStrings[8])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        }
        if (i == 1||i==10)
        {
            GetControl<Button>(buttonStrings[1])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            GetControl<Button>(buttonStrings[10])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        }
        if (i == 7||i==9)
        {
            GetControl<Button>(buttonStrings[7])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            GetControl<Button>(buttonStrings[9])[0].GetComponent<UIButtonTemplete>().MouseEnter();
        }
        if (i == 2||i==11)
        {
            GetControl<Button>(buttonStrings[2])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            GetControl<Button>(buttonStrings[11])[0].GetComponent<UIButtonTemplete>().MouseEnter();
            ButtonStrongs[2].SetActive(true);
        }
        if (i == 5||i==12)
        {
            GetControl<Button>(buttonStrings[5])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            GetControl<Button>(buttonStrings[12])[0].GetComponent<UIButtonTemplete>().MouseEnter();
            ButtonStrongs[5].SetActive(true);
        }
        if(i == 3||i == 4)
        {
            GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            ButtonStrongs[i].SetActive(true);
        }
        if (i == 6)
        {
            content.text = "Apply the Graphic Settings.";
            GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            ButtonStrongs[i].SetActive(true);
        }
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
    }
    private void MouseExit(int i)
    {
        if (i == 0||i==8)
        {
            GetControl<Button>(buttonStrings[0])[0].transform.localScale = new Vector3(1f, 1f, 1);
            GetControl<Button>(buttonStrings[8])[0].GetComponent<UIButtonTemplete>().MouseExit();
        }
        if (i == 1||i==10)
        {
            GetControl<Button>(buttonStrings[1])[0].transform.localScale = new Vector3(1f, 1f, 1);
            GetControl<Button>(buttonStrings[10])[0].GetComponent<UIButtonTemplete>().MouseExit();
        }
        if (i == 7||i==9)
        {
            GetControl<Button>(buttonStrings[7])[0].transform.localScale = new Vector3(1f, 1f, 1);
            GetControl<Button>(buttonStrings[9])[0].GetComponent<UIButtonTemplete>().MouseExit();
        }
        if (i == 2||i==11)
        {
            GetControl<Button>(buttonStrings[2])[0].transform.localScale = new Vector3(1f, 1f, 1);
            GetControl<Button>(buttonStrings[11])[0].GetComponent<UIButtonTemplete>().MouseExit();
            ButtonStrongs[2].SetActive(false);
        }
        if (i == 5||i==12)
        {
            GetControl<Button>(buttonStrings[5])[0].transform.localScale = new Vector3(1f, 1f, 1);
            GetControl<Button>(buttonStrings[12])[0].GetComponent<UIButtonTemplete>().MouseExit();
            ButtonStrongs[5].SetActive(false);
        }
        if (i == 3 || i == 4 || i == 6)
        {
            GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(1f, 1f, 1);
            ButtonStrongs[i].SetActive(false);
        }  
        MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
    }

    protected override void OnClick(string btnName)
    {
        StartCoroutine(SmallAndLarge(btnName));
        if (btnName == buttonStrings[0]|| btnName == buttonStrings[8])//Graphics
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().tagSound, false);
            SwitchToGraphics();
        }
        else if (btnName == buttonStrings[1]||btnName == buttonStrings[10])//System
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().tagSound, false);
            SwitchToSystem();
        }
        else if (btnName == buttonStrings[7]|| btnName == buttonStrings[9])//Volumes
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().tagSound, false);
            SwitchToVolumes();
        }
        else if (btnName == buttonStrings[2] || btnName == buttonStrings[11])//Back
        {
            if(isInStartScene) UIMgr.GetInstance().ShowPanel<StartScreenPanel>("_Start/StartScenePanel");
            else UIMgr.GetInstance().ShowPanel<SystemRPanel>("Ship/Room_System/SystemRPanel");
            UIMgr.GetInstance().HidePanel("_Start/PanelGameSettings");
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
        }
        else if (btnName == buttonStrings[3])//Save
        {
        }
        else if (btnName == buttonStrings[4])//Load
        {
        }
        else if (btnName == buttonStrings[5] || btnName == buttonStrings[12])//Exit
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().exitSound, false);
            Application.Quit(0);
        }else if(btnName == buttonStrings[6])//Apply
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().applySound, false);
            changeFullScreen = fullScreenToggle.isOn;

            QualitySettingsMgr.GetInstance().SetQuality(qualityDropdown.value);
            if (vsyncToggle.isOn)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }

            Screen.SetResolution(resItems[resoluDropdown.value].width, resItems[resoluDropdown.value].height, Screen.fullScreen);
        }
    }

    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }

    private void SwitchToGraphics()
    {
        ButtonStrongs[0].SetActive(true);
        ButtonStrongs[1].SetActive(false);
        ButtonStrongs[7].SetActive(false);
        graphicMeau.SetActive(true);
        systemMeau.SetActive(false);
        volumeMeau.SetActive(false);
    }

    private void SwitchToSystem()
    {
        ButtonStrongs[0].SetActive(false);
        ButtonStrongs[1].SetActive(true);
        ButtonStrongs[7].SetActive(false);
        graphicMeau.SetActive(false);
        systemMeau.SetActive(true);
        volumeMeau.SetActive(false);

        if (!PopUpMgr.GetInstance().LoadAndSave)
        {
            UIMgr.GetInstance().PopUp("Save and Load",
                 new string[1] { "The SAVE and LOAD option is not available.\n\nBut you can still use EXIT to quit the game." });
            PopUpMgr.GetInstance().LoadAndSave = true;
        }
    }

    private void SwitchToVolumes()
    {
        ButtonStrongs[0].SetActive(false);
        ButtonStrongs[1].SetActive(false);
        ButtonStrongs[7].SetActive(true);
        graphicMeau.SetActive(false);
        systemMeau.SetActive(false);
        volumeMeau.SetActive(true);
    }

    //VolumeSettings
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);       
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetBKVolume(float volume)
    {
        audioMixer.SetFloat("BKMusicVloume", volume);
    }

    //Availablility
    public void SetAvailablilityToggle(bool isON)
    {
        _FishDataMgr.GetInstance().casualMode = isON;
    }

}

public class ResItem
{
    public int width, height;
}

