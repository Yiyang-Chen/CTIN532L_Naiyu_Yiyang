using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCircle : MonoBehaviour
{
    //Container
    public GameObject spriteContainer;
    //要清零的变量
    public Rigidbody2D parentRB;
    public bool isHookingStart;
    public Vector3 velocity;
    public float angle;
    public int coroCnt;
    public Coroutine[] currentCoro;
    public float fSpeed;
    public Vector3 deltaSpeed;
    public float spaceStormIndex;
    public float restIndex;
    public float vIndex;
    public int fishID;
    protected _FishData _fishData;
    //playerposition
    protected Vector3 playerPosition;
   
    private void Start()
    {
    }

    private void OnEnable()
    {
        InitialStatus();
        EventCenter.GetInstance().AddEventListener<GameObject>("HookingStart", BeginMovement);
        EventCenter.GetInstance().AddEventListener<Vector2>("UpdatePlayerPosition", UpdatePlayerPosition);
        EventCenter.GetInstance().AddEventListener<int>("SuccessHookingFish", SuccessHookingFish);
        EventCenter.GetInstance().AddEventListener("EndFishing", EndFishing);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<GameObject>("HookingStart", BeginMovement);
        EventCenter.GetInstance().RemoveEventListener<Vector2>("UpdatePlayerPosition", UpdatePlayerPosition);
        EventCenter.GetInstance().RemoveEventListener<int>("SuccessHookingFish", SuccessHookingFish);
        EventCenter.GetInstance().RemoveEventListener("EndFishing", EndFishing);
    }

    private void FixedUpdate()
    {
        if (isHookingStart)
        {
            angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            parentRB.rotation = angle;
            Vector3 deltaS = (velocity * fSpeed + deltaSpeed*spaceStormIndex) * Time.fixedDeltaTime;
            parentRB.velocity = new Vector2(deltaS.x, deltaS.y);

            deltaSpeed = Vector3.zero;

            if (Vector2.Distance(parentRB.position, new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y)) >= (4.00 - parentRB.transform.localScale.x / 2)* spriteContainer.transform.localScale.x)
            {
                parentRB.MovePosition(new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y)+(parentRB.position - new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y)).normalized * (3.96f - parentRB.transform.localScale.x / 2) * spriteContainer.transform.localScale.x);
            }

            UpdateFishPosition();
        }
        else
        {
            InitialStatus();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //检测撞墙反弹
        if (collision.gameObject.tag == "Wall")
        {
            EventCenter.GetInstance().EventTrigger("FishHitTheWall");
            Vector3 normal = parentRB.transform.localPosition - collision.transform.localPosition;
            float magnitude = velocity.magnitude;
            velocity = Vector3.Reflect(velocity, normal);
            velocity = velocity.normalized * magnitude;
            velocity = velocity * vIndex;

            angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            parentRB.rotation = angle+180;
            
            MusicMgr.GetInstance().PlaySound("_Using/DM-CGS-31", false);
        }
        if (collision.gameObject.tag == "EnergyWall")
        {
            if (!collision.gameObject.GetComponent<EnergyWall>().allowPass)
            {
                EventCenter.GetInstance().EventTrigger("FishHitTheWall");
                Vector3 normal = parentRB.transform.localPosition - collision.transform.localPosition;
                float magnitude = velocity.magnitude;
                velocity = Vector3.Reflect(velocity, normal);
                velocity = velocity.normalized * magnitude;
                velocity = velocity * (vIndex+0.5f);

                angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                parentRB.rotation = angle + 180;
                MusicMgr.GetInstance().PlaySound("_Using/DM-CGS-31", false);
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "SpaceStorm")
        {
            Vector3 _force;
            if (collision.GetComponent<SpaceStorm>().isInside)
            {
                Vector3 normal =  -(parentRB.transform.localPosition - collision.transform.localPosition);
                float magnitude = collision.gameObject.GetComponent<SpaceStorm>().force.magnitude;
                _force = normal.normalized*magnitude;
            }
            else
            {
                _force = collision.gameObject.GetComponent<SpaceStorm>().force;
            }
            deltaSpeed += _force;
        }
    }

    public virtual void InitialStatus()
    {
        isHookingStart = false;
        //初始化速度
        velocity = new Vector3(0f, 0f, 0);
        angle = 0;
        //初始化速度系数
        fSpeed = 50f;
        deltaSpeed = Vector3.zero;
        vIndex = 0.4f;
        //初始化技能计数
        coroCnt = 0;
        //清空现存动作
        currentCoro = null;

        UpdateFishPosition();
    }

    public virtual void BeginMovement(GameObject obj)
    {
        isHookingStart = true;
        spriteContainer = obj;
    }

    protected void Escape()
    {
        EventCenter.GetInstance().EventTrigger("HookingTimeExceed");
    }

    private void SuccessHookingFish(int i)
    {
        StopAction();
        InitialStatus();
        PoolMgr.GetInstance().PushObj("_Perfab/Fishing/FishDataBase/HookingFish" + fishID.ToString("D3"), parentRB.gameObject);
    }

    protected void EndFishing()
    {
        if(currentCoro!=null) StopAction();
        InitialStatus();
        PoolMgr.GetInstance().PushObj("_Perfab/Fishing/FishDataBase/HookingFish" + fishID.ToString("D3"), parentRB.gameObject);
    }

    protected void StopAction()
    {
        for (int i = 0; i < currentCoro.Length; i++)
        {
            if (currentCoro[i] != null)
            {
                StopCoroutine(currentCoro[i]);
            }
        }     
    }

    void UpdateFishPosition()
    {
        EventCenter.GetInstance().EventTrigger<Vector2>("UpdateFishPosition", parentRB.position);
    }

    private void UpdatePlayerPosition(Vector2 O)
    {
        playerPosition = new Vector3( O.x,O.y,0);
    }

    //Skills
    protected GameObject MakeSpaceStorm(Vector3 position,Vector3 scale,Vector3 forceDirection, float forceMag,float lastTime,string path)
    {
        GameObject _obj = null;
        PoolMgr.GetInstance().GetObj(path, (obj) =>
        {
            obj.transform.localPosition = position+spriteContainer.transform.localPosition;
            obj.transform.localScale = scale*spriteContainer.transform.localScale.x;
            obj.transform.SetParent(spriteContainer.transform);
            obj.GetComponent<SpaceStorm>().SetForce(forceDirection,forceMag);
            float _angle = Mathf.Atan2(obj.GetComponent<SpaceStorm>().force.y, obj.GetComponent<SpaceStorm>().force.x) * Mathf.Rad2Deg;
            obj.transform.localRotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            obj.GetComponent<SpaceStorm>().lastTime = lastTime;
            obj.GetComponent<SpaceStorm>()._path = path;
            _obj = obj;
            obj.GetComponent<SpaceStorm>().hasLoaded = true; 
        });
        return _obj;
    }

    protected GameObject MakeEnergyWall(string path, Vector3 position, Vector3 scale,bool allowPass, float[] changeTime, float lastTime)
    {
        GameObject _obj = null;
        PoolMgr.GetInstance().GetObj(path, (obj) =>
        {
            obj.transform.localPosition = position + spriteContainer.transform.localPosition;
            obj.transform.localScale = scale * spriteContainer.transform.localScale.x;
            obj.transform.SetParent(spriteContainer.transform);
            obj.GetComponent<EnergyWall>().SetEnergyWall(path,lastTime,changeTime,allowPass);
            _obj = obj;
            obj.GetComponent<EnergyWall>().hasLoaded = true;
        });
        return _obj;
    }

    protected GameObject MakeDefensiveWeapon(string path, Vector3 position, Vector3 scale,float rotation, float damage, float lastTime)
    {
        GameObject _obj = null;
        PoolMgr.GetInstance().GetObj(path, (obj) =>
        {
            obj.transform.localPosition = position + spriteContainer.transform.localPosition;
            obj.transform.localScale = scale * spriteContainer.transform.localScale.x;
            obj.transform.localEulerAngles =new Vector3(0,0, rotation);
            obj.transform.SetParent(spriteContainer.transform);
            obj.GetComponent<DefensiveWeapon>().SetDefensiveWeapon(path, damage, lastTime);
            _obj = obj;
            obj.GetComponent<DefensiveWeapon>().hasLoaded = true;
        });
        return _obj;
    }

    protected GameObject MakeErosionCloud(string path, Vector3 position, Vector3 scale, float rotation, float lastTime)
    {
        GameObject _obj = null;
        PoolMgr.GetInstance().GetObj(path, (obj) =>
        {
            obj.transform.localPosition = position + spriteContainer.transform.localPosition;
            obj.transform.localScale = scale * spriteContainer.transform.localScale.x;
            obj.transform.localEulerAngles = new Vector3(0, 0, rotation);
            obj.transform.SetParent(spriteContainer.transform);
            obj.GetComponent<ErosionCloud>().SetErosionCloud(path, lastTime);
            _obj = obj;
            obj.GetComponent<ErosionCloud>().hasLoaded = true;
        });
        return _obj;
    }
}
