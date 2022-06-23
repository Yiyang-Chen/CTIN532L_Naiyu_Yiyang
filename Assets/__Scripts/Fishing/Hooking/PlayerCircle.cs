using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCircle : MonoBehaviour
{
    public GameObject core;
    //需要清零的变量
    //游戏开始
    public bool isHookingStart;
    private Rigidbody2D rb;
    //物理模拟的力
    private Vector3 forceDirection;
    public Vector3 velocity;
    //WASD模式变量
    public bool isW;
    public bool isA;
    public bool isS;
    public bool isD;
    //阻力系数
    private float fIndex;
    //撞墙反弹的速度系数
    public float vIndex;
    //物体重量
    public float m;
    //最大速度
    public Vector3 maxV;
    //现在的coro
    private Coroutine currentCoro;
    //Container
    public GameObject spriteContainer;
    //物体停下的error
    private float error;
    //SpaceStrom
    public Vector3 deltaSpeed;

    // Start is called before the first frame update
    void Start()
    {
        InitialStatus();

        error = 0.001f;

        this.gameObject.tag = "Player";
    }

    private void OnEnable()
    {
        InitialStatus();
        
        InputMgr.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyDown", CheckInputDown);
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyUp", CheckInputUp);
        EventCenter.GetInstance().AddEventListener<int>("SuccessHookingFish", SuccessHookingFish);
        EventCenter.GetInstance().AddEventListener<GameObject>("HookingStart", HookingStart);
        EventCenter.GetInstance().AddEventListener("EndFishing", EndFishing);
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("KeyDown", CheckInputDown);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("KeyUp", CheckInputUp);
        EventCenter.GetInstance().RemoveEventListener<int>("SuccessHookingFish", SuccessHookingFish);
        EventCenter.GetInstance().RemoveEventListener<GameObject>("HookingStart", HookingStart);
        EventCenter.GetInstance().RemoveEventListener("EndFishing", EndFishing);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isHookingStart)
        {
            InputMgr.GetInstance().StartOrEndCheck(true);
            PlayerMove();

            UpdatePlayerPosition();
        }
        else
        {
            InitialStatus();
        }
    }

    public void InitialStatus()
    {
        //初始化游戏状态
        isHookingStart = false;
        rb = this.GetComponent<Rigidbody2D>();
        rb.MovePosition(this.transform.position);
        rb.velocity = Vector2.zero;
        //初始化速度和力
        forceDirection = new Vector3(0, 0, 0);
        velocity = new Vector3(0, 0, 0);
        
        isW = false;
        isA = false;
        isS = false;
        isD = false;

        fIndex = 0.2f;
        vIndex = 0.8f;
        m = 0.2f;
        maxV = new Vector3(1.5f, 1.5f, 0);

        currentCoro=null;

        deltaSpeed = Vector3.zero;
    }

    private void HookingStart(GameObject obj)
    {
        isHookingStart = true;
        spriteContainer = obj;
    }

    private void CheckInputDown(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                isW = true;
                break;
            case KeyCode.A:
                isA = true;
                break;
            case KeyCode.S:
                isS = true;
                break;
            case KeyCode.D:
                isD = true;
                break;
        }

    }

    private void CheckInputUp(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Escape:
                break;
            case KeyCode.R:
                break;
            case KeyCode.W:
                isW = false;
                break;
            case KeyCode.A:
                isA = false;
                break;
            case KeyCode.S:
                isS = false;
                break;
            case KeyCode.D:
                isD = false;
                break;
        }

    }

    void PlayerMove()
    {
        
        forceDirection = new Vector3(0, 0, 0);
        if (isW || isA || isS || isD)
        {
            if (isW)
            {
                forceDirection += new Vector3(0, 1, 0);
            }
            if (isS)
            {
                forceDirection += new Vector3(0, -1, 0);
            }
            if (isD)
            {
                forceDirection += new Vector3(1, 0, 0);
            }
            if (isA)
            {
                forceDirection += new Vector3(-1, 0, 0);
            }
        }

        forceDirection.z = 0;

        forceDirection = forceDirection.normalized;

        //计算增加的速度
        Vector3 deltaV = new Vector3(0, 0, 0);
        deltaV.x = forceDirection.x / m * Time.fixedDeltaTime - fIndex * velocity.x / m * Time.fixedDeltaTime;
        deltaV.y = forceDirection.y / m * Time.fixedDeltaTime - fIndex * velocity.y / m * Time.fixedDeltaTime;
        deltaV.z = forceDirection.z / m * Time.fixedDeltaTime - fIndex * velocity.z / m * Time.fixedDeltaTime;
                
        //SpaceStorm
        deltaV += deltaSpeed * Time.fixedDeltaTime; 

        deltaSpeed = Vector3.zero;
        //算出现在的速度
        velocity.x += deltaV.x;
        velocity.y += deltaV.y;
        velocity.z += deltaV.z;

        if (velocity.x >= maxV.x)
            velocity.x = maxV.x;
        else if (velocity.x <= -maxV.x)
            velocity.x = -maxV.x;
        else if (velocity.x >= -error && velocity.x <= error)
            velocity.x = 0;

        if (velocity.y >= maxV.y)
            velocity.y = maxV.y;
        else if (velocity.y <= -maxV.y)
            velocity.y = -maxV.y;
        else if (velocity.y >= -error && velocity.y <= error)
            velocity.y = 0;

        if (velocity.z >= maxV.z)
            velocity.z = maxV.z;
        else if (velocity.z <= -maxV.z)
            velocity.z = -maxV.z;
        else if (velocity.z >= -error && velocity.z <= error)
            velocity.z = 0;
       

        //移动
            
        rb.velocity = new Vector2(velocity.x, velocity.y);

        if (Vector2.Distance(rb.position, new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y)) >= (4.02-core.transform.localScale.x/2)*spriteContainer.transform.localScale.x)
        {
            rb.MovePosition(new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y)+(rb.position - new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y)).normalized * (3.98f- core.transform.localScale.x / 2)* spriteContainer.transform.localScale.x);
        }
    }

    void UpdatePlayerPosition()
    {
        EventCenter.GetInstance().EventTrigger<Vector2>("UpdatePlayerPosition",rb.position);
    }

    private void SuccessHookingFish(int i)
    {
        StopAction();
        InitialStatus();
        PoolMgr.GetInstance().PushObj("_Perfab/Fishing/Hooking/HookingPlayer", this.gameObject);
    }

    private void EndFishing()
    {
        StopAction();
        InitialStatus();
        PoolMgr.GetInstance().PushObj("_Perfab/Fishing/Hooking/HookingPlayer", this.gameObject);
    }

    protected void StopAction()
    {
        if (currentCoro != null)
        {
            StopCoroutine(currentCoro);
        }
    }
}
