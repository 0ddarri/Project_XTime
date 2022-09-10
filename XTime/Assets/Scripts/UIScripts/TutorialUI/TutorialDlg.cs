using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDlg : MonoBehaviour
{
    [SerializeField] GameObject[] pageArray = { };
    [SerializeField] Transform transShowPos = null;
    [SerializeField] Button btnTutorial = null;
    [SerializeField] Button btnNext = null;
    [SerializeField] Button btnBefore = null;
    [SerializeField] Button btnExit = null;

    int curIndex = 0;
    bool isShow = false;
    Vector3 originPos = Vector3.zero;

    void ShowDlg()
    {
        if (isShow)
            transform.position = Vector3.Lerp(transform.position, transShowPos.position, Time.deltaTime * 6.0f);
        else
        {
            transform.position = Vector3.Lerp(transform.position, originPos, Time.deltaTime * 6.0f);
            if (transform.position.y + 1.0f >= originPos.y)
            {
                transform.position = originPos;
                gameObject.SetActive(false);
            }
        }
    }

    void IndexCheck()
    {
        btnNext.gameObject.SetActive(true);
        btnBefore.gameObject.SetActive(true);

        if (curIndex >= pageArray.Length - 1)
        {
            curIndex = pageArray.Length - 1;
            btnNext.gameObject.SetActive(false);
        }
        else if (curIndex <= 0)
        {
            curIndex = 0;
            btnBefore.gameObject.SetActive(false);
        }
    }

    void ShowPage()
    {
        IndexCheck();

        for (int i = 0; i < pageArray.Length; i++)
            pageArray[i].SetActive(false);

        pageArray[curIndex].SetActive(true);
    }

    void OnClick_Tutorial()
    {
        gameObject.SetActive(true);
    }

    void OnClick_Next()
    {
        curIndex++;
        ShowPage();
    }

    void OnClick_Before()
    {
        curIndex--;
        ShowPage();
    }

    void OnClick_Exit()
    {
        isShow = false;
    }

    void Start()
    {
        originPos = transform.position;
        btnTutorial.onClick.AddListener(OnClick_Tutorial);
        btnNext.onClick.AddListener(OnClick_Next);
        btnBefore.onClick.AddListener(OnClick_Before);
        btnExit.onClick.AddListener(OnClick_Exit);
        gameObject.SetActive(false);
    }

    void Update()
    {
        ShowDlg();

        if (Input.GetKeyDown(KeyCode.RightArrow))
            OnClick_Next();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            OnClick_Before();

        if (Input.GetKeyDown(KeyCode.Escape))
            OnClick_Exit();
    }

    void OnEnable()
    {
        isShow = true;
        curIndex = 0;
        ShowPage();
    }
}