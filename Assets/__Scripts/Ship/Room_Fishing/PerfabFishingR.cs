using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfabFishingR : MonoBehaviour
{
    public GameObject exit;
    public GameObject bait;
    public GameObject start;
    public GameObject illustration;
    public GameObject map;

    public Sprite[] exits;
    public Sprite[] baits;
    public Sprite[] illustrations;
    public Sprite[] maps;
    public Sprite[] starts;

    private void OnEnable()
    {
        SwitchPicture("Start", false);
        EventCenter.GetInstance().AddEventListener<string>("FishingRoomMouseEnterButton", FishingRoomMouseEnter);
        EventCenter.GetInstance().AddEventListener<string>("FishingRoomMouseExitButton", FishingRoomMouseExit);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<string>("FishingRoomMouseEnterButton", FishingRoomMouseEnter);
        EventCenter.GetInstance().RemoveEventListener<string>("FishingRoomMouseExitButton", FishingRoomMouseExit);
    }

    private void FishingRoomMouseEnter(string buttonS)
    {
        SwitchPicture(buttonS, true);
    }

    private void FishingRoomMouseExit(string buttonS)
    {
        SwitchPicture(buttonS, false);
    }

    public void SwitchPicture(string buttonS,bool isEnter)
    {
        int index = 0;
        if (isEnter) index = 1;

        switch (buttonS)
        {
            case "Bait":
                bait.GetComponent<SpriteRenderer>().sprite = baits[index];
                break;
            case "Start":
                _FishData b = _FishDataMgr.GetInstance().GetFishByID((_FishDataMgr.GetInstance().currentBait));
                if (b.num > 0)
                {
                    if (index == 1) start.GetComponent<SpriteRenderer>().color = new Color32(200, 10, 10, 255);
                    else start.GetComponent<SpriteRenderer>().color = new Color32(1, 7, 65, 255);
                }
                else
                {
                    start.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
                }
                start.GetComponent<SpriteRenderer>().sprite = starts[index];
                break;
            case "Map":
                map.GetComponent<SpriteRenderer>().sprite = maps[index];
                break;
            case "Illustration":
                illustration.GetComponent<SpriteRenderer>().sprite = illustrations[index];
                break;
            case "Exit":
                exit.GetComponent<SpriteRenderer>().sprite = exits[index];
                break;
        }
    }
}
