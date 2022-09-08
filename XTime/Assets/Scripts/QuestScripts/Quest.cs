using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    [SerializeField] int QuestTimeRandomMin; // 퀘스트 시간
    [SerializeField] int QuestTimeRandomMax; // 퀘스트 시간
    [SerializeField] int QuestTime; // 퀘스트 시간
    [SerializeField] int Penalty; // 거절 패널티
    [SerializeField] int AcceptMoney; // 수락 보상
    [SerializeField] int RewardMoney; // 성공 보상
    [SerializeField] int ResignPanalty; // 중도포기 패널티

    [Space(5.0f)]
    [SerializeField] IsoButton YesButton;
    [SerializeField] IsoButton NoButton;
    [SerializeField] IsoSlider Slider;
    [Space(5.0f)]
    [SerializeField] Transform StartTransform;
    [SerializeField] Transform EndTransform;
    [Space(5.0f)]
    [SerializeField] TextMeshProUGUI QuestText;

    private void Start()
    {
        transform.position = StartTransform.position;
        UnableButton();
    }

    public void QuestInit()
    {
        QuestTime = Random.Range(QuestTimeRandomMin, QuestTimeRandomMax);
        QuestText.text = "쓰레기 투기행위 " + QuestTime + " 초간 무시";

        Slider.Initialize();
        Slider.gameObject.SetActive(false);
        StartCoroutine(StartAnim(3.0f));
        EnableButton();
    }

    public void Initialize(bool isAccept)
    {
        if(isAccept)
        {
            Slider.gameObject.SetActive(true);
            StartCoroutine(StartQuest());
            SceneManager.Ins.Scene.MoneyController.Money += AcceptMoney;
        }
        else
        {
            SceneManager.Ins.Scene.buildingManager.Emotion += Penalty;
            Debug.Log("패널티로 인한 악감정 증가");
            StartCoroutine(EndAnim(3.0f));
            SceneManager.Ins.Scene.buildingManager.QuestController.QuestAvail = false;
        }
    }

    IEnumerator StartAnim(float t)
    {
        float time = 0.0f;
        while(time < t)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, EndTransform.position, Time.deltaTime * 5);
            yield return null;
        }
    }

    IEnumerator EndAnim(float t)
    {
        float time = 0.0f;
        while (time < t)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, StartTransform.position, Time.deltaTime * 5);
            yield return null;
        }
    }

    IEnumerator StartQuest()
    {
        float questTime = QuestTime;
        while(questTime > 0.0f)
        {
            questTime -= Time.deltaTime;
            if(CameraManager.Ins.IsCameraCompleteFilmed(CAMERA_TYPE.POL))
            {
                SceneManager.Ins.Scene.buildingManager.Emotion += ResignPanalty;
                Debug.Log("퀘스트 도중실패로 인한 악감정 증가");
                goto END;
            }
            yield return null;
        }
        Debug.Log("성공");
        SceneManager.Ins.Scene.MoneyController.Money += RewardMoney;

    END:
        StartCoroutine(EndAnim(3.0f));
        UnableButton();
        Slider.Initialize();
        Debug.Log("퀘스트 종료");
    }

    void UnableButton()
    {
        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
    }

    void EnableButton()
    {
        YesButton.gameObject.SetActive(true);
        NoButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (YesButton.IsClicked)
        {
            UnableButton();
            Initialize(true);
            YesButton.IsClicked = false;
            YesButton.IsEntered = false;
            StartCoroutine(QuestCountDown());
        }
        if(NoButton.IsClicked)
        {
            NoButton.IsClicked = false;
            NoButton.IsEntered = false;
            UnableButton();
            Initialize(false);
        }
    }

    IEnumerator QuestCountDown()
    {
        float time = 0.0f;
        while(time < QuestTime)
        {
            time += Time.deltaTime;
            Slider.SetFillImage(0, QuestTime, time);
            yield return null;
        }
    }
}
