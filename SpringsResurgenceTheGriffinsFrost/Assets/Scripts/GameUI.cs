using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI itemText;

    // instance
    public static GameUI instance;

    void Awake()
    {
        instance = this;
    }

    public void UpdateItemText(int item)
    {
        itemText.text = "<b>Item:</b> " + item;
    }

}