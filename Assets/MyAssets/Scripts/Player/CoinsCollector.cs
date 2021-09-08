using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinsCollector : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static CoinsCollector coinsCollector;
    private int coinsAmount;

    public void Start()
    {
        if(coinsCollector == null)
        {
            coinsCollector = this;
        }
    }
    public void ChangeScore(int coinValue)
    {
        coinsAmount += coinValue;
        text.text = "X " + coinsAmount.ToString();
    }
}
