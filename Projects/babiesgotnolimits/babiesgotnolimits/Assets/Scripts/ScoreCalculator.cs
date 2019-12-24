using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
    public TextMeshProUGUI Money;
    public int scoreAdd = 1;
    public float MoneyAmount;
    public float maxScore;

    private void Start()
    {
        maxScore = Mathf.Round(BabyController.scoreValue / 3);
    }
    void Update()
    {
        Money.text = "$ " + MoneyAmount.ToString();

        if (MoneyAmount < maxScore)
        {
            MoneyAmount += scoreAdd;
        } else
        {
            scoreAdd = 0;
        }
    }
}
