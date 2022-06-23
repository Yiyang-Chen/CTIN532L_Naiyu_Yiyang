using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfabWaitingFish : MonoBehaviour
{
    public GameObject spriteContainer;

    private float timePassed;
    private int rushIndex;
    private Vector2 targetV;
    private float speed;
    private float distance;
    private Rigidbody2D rb;

    private bool isMoving;
    private bool tooClose;
    private bool isEnterClose;
    private bool tooFar;

    private Vector2 newTarget;
    private Vector2 lNew;
    private Vector2 lO;

    public _FishData _data;
    public bool isTarget;
    public float waitTime;

    public float RUSH_TIME;
    // Start is called before the first frame update
    void Start()
    {
        RUSH_TIME = 2.0f;
    }

    private void OnEnable()
    {
        isMoving = false;
        
        EventCenter.GetInstance().AddEventListener("LoadedAllFishingperfab", StartMoving);
    }

    private void OnDisable()
    {
        isMoving = false;

        EventCenter.GetInstance().RemoveEventListener("LoadedAllFishingperfab", StartMoving);
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Vector2 direction = targetV - rb.position;
            direction = direction.normalized;

            rb.velocity = direction * speed;

            distance = Vector3.Distance(rb.position, spriteContainer.transform.position);
            //都不许太远离
            if (distance <= 3.0f * spriteContainer.transform.localScale.x && !isEnterClose) isEnterClose = true;
            if (distance >= 3.0f * spriteContainer.transform.localScale.x && isEnterClose) tooFar = true;

            //不是target不许靠近
            if (distance <= 2f * spriteContainer.transform.localScale.x && isEnterClose)
            {
                tooClose = true;
            }

            if ((tooClose||tooFar) && !((waitTime - timePassed) <= RUSH_TIME && isTarget))
            {
                rb.velocity = rb.velocity * -1;
            }

            timePassed += Time.deltaTime;
        }
    }

    private void StartMoving()
    {
        isMoving = true;
        targetV = new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y);
        rushIndex = 0;
        timePassed = 0;
        speed = 0.5f;

        tooClose = false;
        isEnterClose = false;
        tooFar = false;
        StartCoroutine(fakeAction());
    }

    IEnumerator fakeAction()
    {
        if ((waitTime - timePassed) <= RUSH_TIME && isTarget)
        {
            if (distance <= 0.1f)
            {
                speed = 0.1f;
            }
            else
            {
                speed = (new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y) - rb.position).magnitude / (waitTime - timePassed);
            }
            
            tooClose = false;
            tooFar = false;
            targetV = new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y);
        }
        else
        {
            if (_data.rushMid[rushIndex])
            {
                speed = 1.0f;
                tooClose = false;
                tooFar = false;
                targetV = new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y);
            }
            else
            {
                speed = 0.5f;
                tooClose = false;
                tooFar = false;
                SetTarget();
            }
        }
        yield return new WaitForSeconds(Random.Range(1f,1.5f));
        rushIndex++;
        if (rushIndex >= 10) rushIndex = 0;
        StartCoroutine(fakeAction());
    }

    private void SetTarget()
    {
        newTarget = new Vector2(Random.Range(-7, 0.5f), Random.Range(-3.5f, 3.5f));
        newTarget *= spriteContainer.transform.localScale.x;
        lNew = newTarget - rb.position;
        lO = new Vector2(spriteContainer.transform.position.x, spriteContainer.transform.position.y) - rb.position;
        if (Vector2.Angle(lNew,lO) <= 30f)
        {
            SetTarget();
        }
        else
        {
            targetV = newTarget;
        }
    }

    public void SetPosition()
    {
        Vector3 position = RandomPosition();
        Vector3 scale = RandomScale();
        this.transform.position = position;
        this.transform.localScale = scale;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.position = new Vector2(this.transform.position.x, this.transform.position.y);
        distance = Vector3.Distance(rb.position, spriteContainer.transform.position);
    }

    private Vector3 RandomPosition()
    {
        Vector3 v = new Vector3(Random.Range(-7, 0.5f), Random.Range(-3.5f, 3.5f), 0);
        v *= spriteContainer.transform.localScale.x;
        if (Vector3.Distance(v, spriteContainer.transform.position) <= 2f* spriteContainer.transform.localScale.x || Vector3.Distance(v, this.transform.parent.position) >= 4.5f* this.transform.parent.localScale.x)
        {
            return RandomPosition();
        }
        return v;
    }

    private Vector3 RandomScale()
    {
        float s = 0;
        if (_data.scale != 0)
        {
            s = Random.Range(_data.scale - 0.05f, _data.scale + 0.05f);
        }
        else
        {
            s= Random.Range(0.05f, 0.25f);
        }
        
        Vector3 v = new Vector3(s, s, 0);
        return v;
    }
}
