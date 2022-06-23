using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfabSystemR : MonoBehaviour
{
    public GameObject exit;
    public GameObject graphic;
    public GameObject system;
    public GameObject volume;

    public Sprite[] exits;
    public Sprite[] systems;
    public Sprite[] graphics;
    public Sprite[] volumes;

    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener<string>("SystemRoomMouseEnterButton", SystemRoomMouseEnter);
        EventCenter.GetInstance().AddEventListener<string>("SystemRoomMouseExitButton", SystemRoomMouseExit);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<string>("SystemRoomMouseEnterButton", SystemRoomMouseEnter);
        EventCenter.GetInstance().RemoveEventListener<string>("SystemRoomMouseExitButton", SystemRoomMouseExit);
    }

    private void SystemRoomMouseEnter(string buttonS)
    {
        SwitchPicture(buttonS, true);
    }

    private void SystemRoomMouseExit(string buttonS)
    {
        SwitchPicture(buttonS, false);
    }

    public void SwitchPicture(string buttonS, bool isEnter)
    {
        int index = 0;
        if (isEnter) index = 1;

        switch (buttonS)
        {
            case "System":
                system.GetComponent<SpriteRenderer>().sprite = systems[index];
                break;
            case "Graphic":
                graphic.GetComponent<SpriteRenderer>().sprite = graphics[index];
                break;
            case "Volume":
                volume.GetComponent<SpriteRenderer>().sprite = volumes[index];
                break;
            case "Exit":
                exit.GetComponent<SpriteRenderer>().sprite = exits[index];
                break;
        }
    }
}
