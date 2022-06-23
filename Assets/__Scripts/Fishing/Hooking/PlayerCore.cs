using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public PlayerCircle parentPlayerCircle;
    public float maxPD;
    private float currentPDIndex;
    private float minPDIndex;
    private bool isInErosion;
    private float erosionSpeed;

    private void OnEnable()
    {
        currentPDIndex = 1.0f;
        minPDIndex = 0.5f;
        isInErosion = false;
        erosionSpeed = 0.2f;
        EventCenter.GetInstance().EventTrigger<GameObject>("UpdateCorePosition",this.gameObject);
    }

    private void FixedUpdate()
    {
        if (isInErosion&& parentPlayerCircle.velocity.magnitude  > 0.3f)
        {
            currentPDIndex -= erosionSpeed * Time.fixedDeltaTime;
            if (currentPDIndex <= minPDIndex) currentPDIndex = minPDIndex;
        }else if (currentPDIndex < 1)
        {
            currentPDIndex += erosionSpeed * 3 * Time.fixedDeltaTime;
            if (currentPDIndex >= 1) currentPDIndex = 1;
        }

        float currentPD = maxPD * currentPDIndex;
        this.transform.localScale = new Vector3(currentPD * 2, currentPD * 2, 1);
        EventCenter.GetInstance().EventTrigger<float>("UpdateCurrentPD",currentPD);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {    
        //¼ì²â×²Ç½·´µ¯
        if (collision.gameObject.tag == "Wall")
        {
            EventCenter.GetInstance().EventTrigger<float>("PlayerHitTheWall",parentPlayerCircle.velocity.magnitude/parentPlayerCircle.maxV.magnitude);
            Vector3 normal = parentPlayerCircle.transform.localPosition - collision.transform.localPosition;// parentPlayerCircle.spriteContainer.transform.position;
            float magnitude = parentPlayerCircle.velocity.magnitude;
            parentPlayerCircle.velocity = Vector3.Reflect(parentPlayerCircle.velocity,normal);
            parentPlayerCircle.velocity = parentPlayerCircle.velocity.normalized * magnitude;
            parentPlayerCircle.velocity = parentPlayerCircle.velocity * parentPlayerCircle.vIndex;
            MusicMgr.GetInstance().PlaySound("_Using/DM-CGS-31", false);
        }
        if (collision.gameObject.tag == "EnergyWall")
        {
            if (!collision.gameObject.GetComponent<EnergyWall>().allowPass)
            {
                EventCenter.GetInstance().EventTrigger<float>("PlayerHitTheWall", parentPlayerCircle.velocity.magnitude / parentPlayerCircle.maxV.magnitude);
                Vector3 normal = parentPlayerCircle.transform.localPosition - collision.transform.localPosition;
                float magnitude = parentPlayerCircle.velocity.magnitude;
                parentPlayerCircle.velocity = Vector3.Reflect(parentPlayerCircle.velocity, normal);
                parentPlayerCircle.velocity = parentPlayerCircle.velocity.normalized * magnitude;
                parentPlayerCircle.velocity = parentPlayerCircle.velocity * (parentPlayerCircle.vIndex+0.5f);
                MusicMgr.GetInstance().PlaySound("_Using/DM-CGS-31", false);
            }         
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SpaceStorm")
        {
            Vector3 _force;
            if (collision.GetComponent<SpaceStorm>().isInside)
            {
                Vector3 normal = -(transform.localPosition - collision.transform.localPosition);
                float magnitude = collision.gameObject.GetComponent<SpaceStorm>().force.magnitude;
                _force = normal.normalized * magnitude;
            }
            else
            {
                _force = collision.gameObject.GetComponent<SpaceStorm>().force;
            }
            parentPlayerCircle.deltaSpeed += _force*0.1f/parentPlayerCircle.m;
        }
        if (collision.gameObject.tag == "ErosionCloud")
        {
            isInErosion = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        isInErosion = false;
    }
}
