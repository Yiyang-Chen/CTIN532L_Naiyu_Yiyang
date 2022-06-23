using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The button in UI designed only for this game.
public class UIButtonTemplete : MonoBehaviour
{
    public Text contentText;

    public GameObject upButton;
    public GameObject downButton;
    public GameObject focusImage;

    public int yOffset;

    public Image informationWindow;
    public Text informationText;
    [TextArea]
    public string informationString;
    public int informationHeight;
    
    void Start()
    {
        informationWindow.rectTransform.sizeDelta = new Vector2(informationWindow.rectTransform.rect.width , informationHeight);
        informationText.text = informationString;
        focusImage.SetActive(false);
        informationWindow.gameObject.SetActive(false);

        if (upButton != null)
        {
            Vector3 upPosition = upButton.GetComponent<RectTransform>().position;
            this.GetComponent<RectTransform>().position = new Vector3(upPosition.x, upPosition.y+yOffset,0);
        }
    }

    public void MouseEnter()
    {
        this.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        focusImage.SetActive(true);
        contentText.text = informationString;
        //informationWindow.gameObject.SetActive(true);
        //if (downButton != null) downButton.GetComponent<UIButtonTemplete>().MoveY(informationHeight*-1);
    }

    public void MouseExit()
    {
        this.transform.localScale = new Vector3(1f, 1f, 1);
        focusImage.SetActive(false);
        //informationWindow.gameObject.SetActive(false);
        //if (downButton != null) downButton.GetComponent<UIButtonTemplete>().MoveY(informationHeight);
    }

    public void MoveY(int dY)
    {
        Vector3 oldPosition = this.transform.position;
        this.transform.position = new Vector3(oldPosition.x, oldPosition.y + dY, 0);
        if (downButton != null) downButton.GetComponent<UIButtonTemplete>().MoveY(dY);
    }
}
