using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfabCollectionR : MonoBehaviour
{
    public GameObject exit;
    public GameObject toturial;
    public GameObject information;
    public GameObject inventory;
    public GameObject fish;

    public Sprite[] exits;
    public Sprite[] toturials;
    public Sprite[] informations;
    public Sprite[] inventorys;
    public Sprite[] fishes;

    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener<string>("CollectionRoomMouseEnterButton", CollectionRoomMouseEnter);
        EventCenter.GetInstance().AddEventListener<string>("CollectionRoomMouseExitButton", CollectionRoomMouseExit);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<string>("CollectionRoomMouseEnterButton", CollectionRoomMouseEnter);
        EventCenter.GetInstance().RemoveEventListener<string>("CollectionRoomMouseExitButton", CollectionRoomMouseExit);
    }

    private void CollectionRoomMouseEnter(string buttonS)
    {
        SwitchPicture(buttonS, true);
    }

    private void CollectionRoomMouseExit(string buttonS)
    {
        SwitchPicture(buttonS, false);
    }

    public void SwitchPicture(string buttonS, bool isEnter)
    {
        int index = 0;
        if (isEnter) index = 1;

        switch (buttonS)
        {
            case "Toturial":
                toturial.GetComponent<SpriteRenderer>().sprite = toturials[index];
                break;
            case "Information":
                information.GetComponent<SpriteRenderer>().sprite = informations[index];
                break;
            case "Inventory":
                inventory.GetComponent<SpriteRenderer>().sprite = inventorys[index];
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
