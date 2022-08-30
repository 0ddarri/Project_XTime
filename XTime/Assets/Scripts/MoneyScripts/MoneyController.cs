using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{
    public int Money;
    [SerializeField] TextMeshProUGUI MoneyText;

    public void Initialize()
    {
        Money = 0;
    }

    private void Update()
    {
        MoneyText.text = Money.ToString();
    }
}
