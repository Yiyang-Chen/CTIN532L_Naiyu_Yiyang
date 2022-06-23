using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health;
    public int level;
    public string myName;

    // Start is called before the first frame update
    void Start()
    {
        health = 100.0f;
        level = 0;
        myName = "new";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ClickedButton()
    {
        health -= 1.0f;
        level += 1;
        myName += "old";
    }

    public void Save()
    {
        DataMgr.GetInstance().Save<PlayerData>(new PlayerData(this),DataPath.PERSISTENT,"/test.cyy");
    }

    public void Delete()
    {
        DataMgr.GetInstance().Delete(DataPath.PERSISTENT, "/test.cyy");
    }

    public void Load()
    {
        PlayerData data = DataMgr.GetInstance().Load<PlayerData>(DataPath.PERSISTENT, "/test.cyy");
        if (data == null)
        {
            data = new PlayerData();
        }

        level = data.level;
        health = data.health;
        myName = data.playerName;

        Vector3 _position;
        _position.x = data.position[0];
        _position.y = data.position[1];
        _position.z = data.position[2];
        transform.position = _position;

    }
}

