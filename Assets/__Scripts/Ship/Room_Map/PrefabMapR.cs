using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMapR : MonoBehaviour
{
    public GameObject exit;
    public GameObject map;
    public GameObject fish;

    public Sprite[] exits;
    public Sprite[] maps;
    public Sprite[] fishes;

    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener<string>("MapRoomMouseEnterButton", MapRoomMouseEnter);
        EventCenter.GetInstance().AddEventListener<string>("MapRoomMouseExitButton", MapRoomMouseExit);
        SwitchPicture("Map",false);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<string>("MapRoomMouseEnterButton", MapRoomMouseEnter);
        EventCenter.GetInstance().RemoveEventListener<string>("MapRoomMouseExitButton", MapRoomMouseExit);
    }

    private void MapRoomMouseEnter(string buttonS)
    {
        SwitchPicture(buttonS, true);
    }

    private void MapRoomMouseExit(string buttonS)
    {
        SwitchPicture(buttonS, false);
    }

    public void SwitchPicture(string buttonS, bool isEnter)
    {
        int index = 0;
        if (isEnter) index = 1;

        switch (buttonS)
        {
            case "Map":
                map.GetComponent<SpriteRenderer>().sprite = maps[index];
                break;
            case "Fish":
                fish.GetComponent<SpriteRenderer>().sprite = fishes[index];
                break;
            case "Exit":
                exit.GetComponent<SpriteRenderer>().sprite = exits[index];
                break;
        }
    }
}
