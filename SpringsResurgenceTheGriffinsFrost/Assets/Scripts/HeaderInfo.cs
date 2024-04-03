using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeaderInfo : MonoBehaviour
{
    //public TextMeshProUGUI nameText;
    public Image bar;
    private float maxValue;

    public void Initialize(string text, int maxVal)
    {
        //nameText.text = text;
        maxValue = maxVal;
        bar.fillAmount = 1.0f;
    }

    public void UpdateHealthBar(int value)
    {
        bar.fillAmount = (float)value / maxValue;
    }

}