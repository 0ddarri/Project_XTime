using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageString : MonoBehaviour
{
    [Header("�ڵ����� ���� ��")]
    [SerializeField] bool Auto_Settings;
    [SerializeField] TextMeshProUGUI  TextMeshProUGUI;

    public string str;


    private void Update()
    {
        if(Auto_Settings)
            TextMeshProUGUI.text = str;
    }
}
