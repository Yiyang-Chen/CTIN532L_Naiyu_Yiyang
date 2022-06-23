using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public float health;
    public float[] position;
    public string playerName;

    public PlayerData(Player player)
    {
        level = player.level;
        health = player.health;
        playerName = player.myName;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
    public PlayerData()
    {
        level = -1;
        health = 102;
        playerName = "null";

        position = new float[3];
        position[0] = 0;
        position[1] = 0;
        position[2] = 0;
    }
}
