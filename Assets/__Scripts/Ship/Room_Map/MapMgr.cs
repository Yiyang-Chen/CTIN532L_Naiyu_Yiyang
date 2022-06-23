using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpaceMap {NORMAL, CYBER, CIVILIZATION, INSECT };

public class MapMgr : SingletonManager<MapMgr>
{
    public SpaceMap currentMap = SpaceMap.NORMAL;
    public bool[] isLock = new bool[4] { false, true, true, true };
    //public bool[] isLock = new bool[4] { false, false, false, false };
    public int GetMapByInt()
    {
        int i = -1;
        switch (currentMap)
        {
            case SpaceMap.NORMAL:
                i = 0;
                break;
            case SpaceMap.CYBER:
                i = 1;
                break;
            case SpaceMap.CIVILIZATION:
                i = 2;
                break;
            case SpaceMap.INSECT:
                i = 3;
                break;
        }
        return i;
    }

    public int ChangeMapByInt(int i)
    {
        switch (i)
        {
            case 0:
                currentMap = SpaceMap.NORMAL;
                break;
            case 1:
                currentMap = SpaceMap.CYBER;
                break;
            case 2:
                currentMap = SpaceMap.CIVILIZATION;
                break;
            case 3:
                currentMap = SpaceMap.INSECT;
                break;
        }
        return i;
    }

    public string GetMapByString(int i)
    {
        string s = "";
        switch (i)
        {
            case 0:
                s = "#001 The NewLand Colony";
                break;
            case 1:
                s = "#002 Space Graveyard";
                break;
            case 2:
                s = "#003 The Empire of Earthen Sapiens";
                break;
            case 3:
                s = "#004 Oomain of 'The Brotherhood'";
                break;
        }
        return s;
    }

    public string GetMapByString()
    {
        string s = "";
        switch (currentMap)
        {
            case SpaceMap.NORMAL:
                s = "#001 The NewLand Colony";
                break;
            case SpaceMap.CYBER:
                s = "#002 Space Graveyard";
                break;
            case SpaceMap.CIVILIZATION:
                s = "#003 The Empire of Earthen Sapiens";
                break;
            case SpaceMap.INSECT:
                s = "#004 Oomain of 'The Brotherhood'";
                break;
        }
        return s;
    }
}
