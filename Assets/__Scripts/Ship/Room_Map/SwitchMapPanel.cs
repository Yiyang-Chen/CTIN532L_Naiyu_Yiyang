using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SwitchMapPanel : BasePanel
{
    public bool isBackToFish;

    public GameObject[] masks; 

    public Text title;
    public Text content;
    public Text pageText;

    public Text mapTitle;
    public Text instruction;
    
    public Image map;
    public Sprite[] maps;

    public Image confirmButton;
    public GameObject confirmStrong;
    
    public string[] buttonStrings;
    private string[] mapInstructions;

    private int page;

    private int oldMap;
    private int currentMap;
    private void Start()
    {
        oldMap = MapMgr.GetInstance().GetMapByInt();
        currentMap = MapMgr.GetInstance().GetMapByInt();

        for(int i = 0; i < masks.Length; i++)
        {
            masks[i].SetActive(MapMgr.GetInstance().isLock[i]);
        }

        title.text = "NAVIGATION ROOM";
        content.text = "After clicking confirm, the ship will navigate\nfrom   " + MapMgr.GetInstance().GetMapByString(oldMap)+"   to   "+ MapMgr.GetInstance().GetMapByString(currentMap);

        buttonStrings = new string[7] { "Map1", "Map2", "Map3", "Map4", "Confirm" , "LeftPage", "RightPage" };

        mapInstructions = new string[8] {
            "[Message from The Cat]\n\n...Our Gods had passed by the sector long, long ago.\n\nIn order to spread the glory and knowledge, we consider to switch to a more acceptable naming convention used by near by newly-borned human empire.",
            "[My shiplog - Entry 450]\n\nThe war between the Empire and the artificial intelligences has been going on for more than a hundred solar years.\n\nThe Spaceship Graveyard has long become inhabitable by humans, but the Resistance Force remained the Human's last stand.",
            "[Message from The Cat]\n\nThe intergalactic empire that was built by humans.\nThe Pope rules the Empire, both as the political leader and as the spiritual leader. The Holy See is the capital star, on which the polititians, the church and the Pope Himself live. Around the capital star there are also some scientific research facilities and a few universities. \nCreature similar to the Colony, as well as Humans can be found here.",
            "[My shiplog - Entry 990]\n\nA habitat for a hive mind of insects. Most of the celestial bodies in this system are gas giants, making it hard for other life forms to live in. The insects, on the contrary, found it their best home.\n\nThe insects called themselves ¡°The Brotherhood¡± - under the Mother, all of them were brothers that were connected to and related to each other.",
            "Special:\nSpace storms happen frequently in this area.\nSome fishes are even able to create space storm themselves.\n\nSpace Storm: \ncontinously push to one direction in space storm area.",
            "Special:\nThe ruins of war have caused the walls here to be very unstable.\nSome fishes are even able to make use of these walls.\n\nEnergy Wall: \nRed will bump back extremely quick, Blue will be able to go through.",
            "Special:\nThe high tech of the empire spaceships have strong weapons.\n\nDefensive Weapon: \nCreate an red area. After two seconds, if the player is in the area, the process bar will drop.",
            "Special:\nThe creatures here can secrete erosion cloud.\n\nErosion Cloud: \nIf the player move in the could, the circle will become small. The circle will recover if stay still or outside the could."};

        page = 1;
        pageText.text = page+" - 2";
        mapTitle.text = MapMgr.GetInstance().GetMapByString(currentMap);
        instruction.text = mapInstructions[currentMap+4*(page-1)];

        confirmStrong.SetActive(false);

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

            MouseExit(index);
        }
    }

    private void MouseEnter(int i)
    {
        string buttonS = buttonStrings[i];
        if (i < 4)
        {
            if(!MapMgr.GetInstance().isLock[i]) map.sprite = maps[i];
        }
        if(i==4)
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
            GetControl<Button>(buttonStrings[4])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            confirmStrong.SetActive(true);
        }
        if (i == 5 || i == 6)
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
            GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(0.55f, 0.55f, 1);
        }
    }
    private void MouseExit(int i)
    {
        string buttonS = buttonStrings[i];
        if (i < 4)
        {
            if (!MapMgr.GetInstance().isLock[i]) map.sprite = maps[currentMap];
        }
        if(i==4)
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
            GetControl<Button>(buttonStrings[4])[0].transform.localScale = new Vector3(1f, 1f, 1);
            confirmStrong.SetActive(false);
        }
        if (i == 5 || i == 6)
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
            GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
    }
    protected override void OnClick(string btnName)
    {
        if (btnName == buttonStrings[0])//Map1
        {
            SwitchMap(0);
        }
        if (btnName == buttonStrings[1])//Map2
        {
            SwitchMap(1);
        }
        if (btnName == buttonStrings[2])//Map3
        {
            SwitchMap(2);
        }
        if (btnName == buttonStrings[3])//Map4
        {
            SwitchMap(3);
        }
        if (btnName == buttonStrings[4])//Confirm
        {
            StartCoroutine(SmallAndLarge(btnName));

            if (!isBackToFish)
            {
                UIMgr.GetInstance().ShowPanel<MapRPanel>("Ship/Room_Map/MapRPanel", (obj) =>
                {
                    UIMgr.GetInstance().HidePanel("Ship/Room_Map/SwitchMapPanel");
                });
            }
            else
            {
                EventCenter.GetInstance().EventTrigger<string>("ClickScreenRoom", "FishingRoom");

                UIMgr.GetInstance().ShowPanel<FishingRPanel>("Ship/Room_Fishing/FishingRPanel", (obj) =>
                {
                    UIMgr.GetInstance().HidePanel("Ship/Room_Map/SwitchMapPanel");
                });
            }
            
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
        }
        if (btnName == buttonStrings[5])//LeftPage
        {
            StartCoroutine(SmallAndLarge2(btnName));
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseLeftRightSound, false);
            page = 1;
            pageText.text = page + " - 2";
            instruction.text = mapInstructions[currentMap + 4 * (page - 1)];
        }
        if (btnName == buttonStrings[6])//RightPage
        {
            StartCoroutine(SmallAndLarge2(btnName));
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseLeftRightSound, false);
            page = 2;
            pageText.text = page + " - 2";
            instruction.text = mapInstructions[currentMap + 4 * (page - 1)];
        }
    }
    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
    IEnumerator SmallAndLarge2(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.45f, 0.45f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.55f, 0.55f, 1);
    }

    private void SwitchMap(int newMap)
    {
        if (!MapMgr.GetInstance().isLock[newMap])
        {
            page = 1;
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchMapSuccessSound, false);
            MapMgr.GetInstance().ChangeMapByInt(newMap);
            map.sprite = maps[newMap];
            currentMap = newMap;
            mapTitle.text = MapMgr.GetInstance().GetMapByString();
            instruction.text = mapInstructions[currentMap + 4 * (page - 1)];
            pageText.text = page + " - 2";
            content.text = "After clicking confirm, the ship will navigate\nfrom   " + MapMgr.GetInstance().GetMapByString(oldMap) + "   to   " + MapMgr.GetInstance().GetMapByString(currentMap);
        }
        else
        {
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().switchMapFailSound, false);
            content.text = "You cannot choose the maps that are currently locked.";
        }
    }
}
