using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRoom : MonoBehaviour
{
    public string _perfabName; 
    public GameObject _perfab;
    public void LoadPerfab(string _name)
    {
        if(_name != null)
        {
            if (_perfab != null)
            {
                DeletePerfab();
            }
            _perfabName = _name;
            PoolMgr.GetInstance().GetObj(_perfabName, (obj) =>
            {
                obj.transform.SetParent(this.transform);
                if(_perfabName == "_Perfab/Ship/Rooms/FishingRPerfab")
                {
                    obj.transform.localPosition = new Vector3(0.12f,-0.1f,0);
                    obj.transform.localScale = new Vector3(0.63f,0.53f,1);
                }else if (_perfabName == "_Perfab/Ship/Rooms/ControlRPerfab")
                {
                    obj.transform.localPosition = new Vector3(0.15f, -0.15f, 0);
                    obj.transform.localScale = new Vector3(0.63f, 0.53f, 1);
                }else if (_perfabName == "_Perfab/Ship/Rooms/MapRPerfab")
                {
                    obj.transform.localPosition = new Vector3(0.1f, -0.1f, 0);
                    obj.transform.localScale = new Vector3(0.63f, 0.53f, 1);
                }
                else if (_perfabName == "_Perfab/Ship/Rooms/CollectionRPerfab")
                {
                    obj.transform.localPosition = new Vector3(0.1f, -0.1f, 0);
                    obj.transform.localScale = new Vector3(0.63f, 0.53f, 1);
                }

                _perfab = obj;
            });
        }
        else
        {
            _perfabName = null;
        }     
    }

    public void DeletePerfab()
    {
        if (_perfabName != null&&_perfab !=null)
        {
            PoolMgr.GetInstance().PushObj(_perfabName, _perfab);
            _perfab = null;
            _perfabName = "";
        }
        
    }
}
