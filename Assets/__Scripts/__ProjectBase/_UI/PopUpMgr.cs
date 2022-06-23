using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpMgr : SingletonManager<PopUpMgr>
{
    //还没有popup的都是false
    public bool Awake = false;
    public bool Bait = false;
    public bool AlphaVersion = false;
    public bool LoadAndSave = false;
    public bool[] RoomInstruction = new bool[4] {false,false,false,false};
    public bool StartWaiting = false;
    public bool HookingConfirm = false;
    public bool[] MapSkills = new bool[4] { false, false, false, false };
    public string[] mapSkillContent = new string[4]
    {
        "Fishes in a same map have a similar skill.\n\nThe skill in this map is called 'Space Storm'.\nFishes and Players will be pushed to the arrow direction in storm area.\nSome fish can create storm and some can reduce the influence of space storm.",
        "Fishes in a same map have a similar skill.\n\nThe skill in this map is called 'Energy Wall'.\nThe walls will turn from red to blue periodly.\nPlayer and fishes can go through blue wall, but will bounce back from red wall.",
        "Fishes in a same map have a similar skill.\n\nThe skill in this map is called 'Defensive Weapon'.\nThe process bar will decrease in some amount, if the player's circle is in the yellow area after the area appears 2 seconds.",
        "Fishes in a same map have a similar skill.\n\nThe skill in this map is called 'Erosion Cloud'.\nPlayer's circle will become small if player move in the cloud.\nThe circle will recover if player is outside the cloud or not moving.\n\nOnly one sample fish is created in this map."
    };
    public string[] toturialContent = new string[6]
    {
        "Go to the fishing room and then click start to begin fishing.",
        "Fishing contains two phases, Waiting and Hooking.",
        "In WAITING phase, you simply wait for a fish to bite.\n\nYou can also hear a hint sound when you can hook a fish.\n\nUse SPACE or LEFT CLICK.",
        "During HOOKING phase, the fish struggles, and you need to keep the fish in the circle.\n\nUse WASD to move your circlr.",
        "You can use hooked fish as bait to hook other fishes.\n\nFor now, fish can only attract the other fish in the same map.",
        "After unlock the maps (by hooking specific fish once), you can switch to the other map in navigation room."
    };

    public Sprite[] toturialSprite = new Sprite[6]
    {
        ResourceMgr.GetInstance().Load<Sprite>("_Sprites/_Start/1"),
        ResourceMgr.GetInstance().Load<Sprite>("_Sprites/_Start/23"),
        ResourceMgr.GetInstance().Load<Sprite>("_Sprites/_Start/23"),
        ResourceMgr.GetInstance().Load<Sprite>("_Sprites/_Start/4"),
        ResourceMgr.GetInstance().Load<Sprite>("_Sprites/_Start/5"),
        ResourceMgr.GetInstance().Load<Sprite>("_Sprites/_Start/6")
    };
}
