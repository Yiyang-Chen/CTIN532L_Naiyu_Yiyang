using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blur : MonoBehaviour
{
    [SerializeField]
    private Material material;
    private float blurAmt;
    private bool blurActive;
    private float blurSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Initalize();
    }

    private void OnEnable()
    {
        Initalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (blurActive) blurAmt += blurSpeed * Time.deltaTime;
        else blurAmt -= blurSpeed * Time.deltaTime;

        blurAmt = Mathf.Clamp(blurAmt,0f,10f);
        material.SetFloat("_BlurAmount", blurAmt);
    }

    private void Initalize()
    {
        blurAmt = 0;
        blurActive = false;
        blurSpeed = 20f;
    }

    public void ActiveBlur(float t,float s)
    {
        blurSpeed = s;
        StartCoroutine(SetBlur(t));
    }

    IEnumerator SetBlur(float t)
    {
        blurActive = true;
        yield return new WaitForSeconds(t);
        blurActive = false;
    }
}
