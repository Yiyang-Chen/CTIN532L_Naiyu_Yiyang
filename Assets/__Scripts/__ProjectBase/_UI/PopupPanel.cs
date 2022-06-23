using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupPanel : BasePanel
{
    private Vector2[] _size;

    private string _title;
    public Text titleText;

    private string[] _contents;
    public Text contentText;

    private Sprite[] _sprites;
    public Image contentImage;

    private int _currentPage;
    private int _page;
    public Text pageText;

    private string _eventString;

    private string[] buttonStrings;
    public GameObject confirmStrong;

    private void OnEnable()
    {
        InputMgr.GetInstance().StartOrEndCheck(false);
    }

    private void OnDisable()
    {
        InputMgr.GetInstance().StartOrEndCheck(true);
    }

    private void Start()
    {
        buttonStrings = new string[3] { "LeftPage", "RightPage" , "Confirm" };

        for(int i = 0; i < buttonStrings.Length; i++)
        {
            int index = i;
            UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[index])[0], EventTriggerType.PointerEnter, (data) =>
            {
                MouseEnter(index);
            });

            UIMgr.AddCustomEventListener(GetControl<Button>(buttonStrings[index])[0], EventTriggerType.PointerExit, (data) =>
            {
                MouseExit(index);
            });

            MouseExit(index);
        }
    }

    private void Update()
    {
        ChangePage(_currentPage);
    }

    public void SetContent(string title, string[] contents, float[] width, float[] height, Sprite[] sprites, string eventString)
    {
        _title = title;
        titleText.text = _title;

        _currentPage = 0;
        _contents = contents;
        _page = _contents.Length-1;//index С 1

        Sprite[] s = new Sprite[_contents.Length];
        for(int i = 0; i < s.Length; i++)
        {
            if (sprites != null)
            {
                if (i < sprites.Length) s[i] = sprites[i];
                else s[i] = null;
            }    
            else s[i] = null;
        }
        _sprites = s;

        Vector2[] xy = new Vector2[_contents.Length];
        for (int i = 0; i < xy.Length; i++)
        {
            if (width != null)
            {
                if (i < width.Length) xy[i].x = width[i];
                else xy[i].x = 1920;
            }
            else xy[i].x = 1920;

            if (height != null)
            {
                if (i < height.Length) xy[i].y = height[i];
                else xy[i].y = 1080;
            }
            else xy[i].y = 1080;
        }
        _size = xy;

        _eventString = eventString;
    }

    protected override void OnClick(string btnName)
    {
        if (btnName == buttonStrings[0])//left
        {
            StartCoroutine(SmallAndLarge(btnName));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseLeftRightSound, false);
            if (_currentPage > 0) _currentPage -= 1;
        }
        else if (btnName == buttonStrings[1])//right
        {
            StartCoroutine(SmallAndLarge(btnName));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseLeftRightSound, false);
            if (_currentPage <= _page) _currentPage += 1;
        }
        else if (btnName == buttonStrings[2])//confirm
        {
            StartCoroutine(SmallAndLarge(btnName));

            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().confirmSound, false);
            if (_eventString!=null) EventCenter.GetInstance().EventTrigger(_eventString);
            UIMgr.GetInstance().HidePanel("__ProjectBase/PopupPanel");
        }
    }

    public void ChangePage(int p)
    {
        this.GetComponent<RectTransform>().sizeDelta = _size[p];

        if (_sprites[p] != null)
        {
            contentImage.gameObject.SetActive(true);
            contentImage.sprite = _sprites[p];
            contentText.rectTransform.offsetMin = new Vector2(500, contentText.rectTransform.offsetMin.y);
        }
        else
        {
            contentImage.gameObject.SetActive(false);
            contentText.rectTransform.offsetMin = new Vector2(0, contentText.rectTransform.offsetMin.y);
        }

        contentText.text = _contents[p];
        _currentPage = p;
        pageText.text = (_currentPage+1) + " - " + (_page+1);
        
        if(_currentPage==0) GetControl<Button>(buttonStrings[0])[0].gameObject.SetActive(false);
        else GetControl<Button>(buttonStrings[0])[0].gameObject.SetActive(true);

        if (_currentPage == _page)
        {
            GetControl<Button>(buttonStrings[2])[0].gameObject.SetActive(true);
            GetControl<Button>(buttonStrings[1])[0].gameObject.SetActive(false);
        }
        else 
        {
            GetControl<Button>(buttonStrings[2])[0].gameObject.SetActive(false);
            GetControl<Button>(buttonStrings[1])[0].gameObject.SetActive(true);
        }
    }

    private void MouseEnter(int i)
    {
        if (buttonStrings[i] == "Confirm")
        {
            GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
            confirmStrong.SetActive(true);
        }
        else 
        {
            GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseEnterSound, false);
            ChangeAlpha(GetControl<Button>(buttonStrings[i])[0].GetComponent<Image>(), 1f);
        }
    }

    private void MouseExit(int i)
    {
        if (buttonStrings[i] == "Confirm")
        {
            GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(1f, 1f, 1);
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
            confirmStrong.SetActive(false);
        }
        else
        {
            GetControl<Button>(buttonStrings[i])[0].transform.localScale = new Vector3(1f, 1f, 1);
            MusicMgr.GetInstance().PlaySound(MusicMgr.GetInstance().mouseExitSound, false);
            ChangeAlpha(GetControl<Button>(buttonStrings[i])[0].GetComponent<Image>(), 0.7f);
        }
    }
    IEnumerator SmallAndLarge(string btnName)
    {
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        yield return new WaitForSeconds(0.1f);
        GetControl<Button>(btnName)[0].transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }

    void ChangeAlpha(Image i,float a)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, a);
    }
}
