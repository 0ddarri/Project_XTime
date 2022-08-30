using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingUIController : MonoBehaviour
{
    [SerializeField] Text SubText;
    [SerializeField] Text Time;
    [SerializeField] Button MainButton;

    private void Start()
    {
        MainButton.onClick.AddListener(Restart);
    }

    public void Initialize()
    {
        Time.text = "생존 시간 : " + SceneManager.Ins.Scene.SaveTime.ToString("F1");
    }


    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
