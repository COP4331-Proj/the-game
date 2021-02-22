﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayGladiatorStats : MonoBehaviour
{
    List<Gladiator> gladiatorList;
    [SerializeField] int gladiatorIndex = 0;

    // Text fields for all 6 gladiator attributes 
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI defenseText;

    // Start is called before the first frame update
    void Start()
    {
        gladiatorList = GetComponent<ViewGladiator>().GetGladiatorList();
        RefreshStats();
    }

    // Refreshes text fields with new gladiators stats
    public void RefreshStats() 
    {
        nameText.text = gladiatorList[gladiatorIndex].GetName().ToString();
        levelText.text = gladiatorList[gladiatorIndex].GetLevel().ToString();
        healthText.text = gladiatorList[gladiatorIndex].GetHealth().ToString();
        staminaText.text = gladiatorList[gladiatorIndex].GetStamina().ToString();
        powerText.text = gladiatorList[gladiatorIndex].GetPower().ToString();
        defenseText.text = gladiatorList[gladiatorIndex].GetDefense().ToString();

    }

    public void IncrementGladiatorIndex() 
    {
        gladiatorIndex = (gladiatorIndex + 1) % gladiatorList.Count;
        RefreshStats();
    }

    public void DecrementGladiatorIndex()
    {
        gladiatorIndex = (gladiatorIndex - 1) % gladiatorList.Count;
        if (gladiatorIndex < 0) 
        {
            gladiatorIndex += gladiatorList.Count;
        }
        RefreshStats();
    }
}