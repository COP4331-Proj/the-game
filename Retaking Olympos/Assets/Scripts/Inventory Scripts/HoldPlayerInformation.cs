﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object that holds
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats")]
public class HoldPlayerInformation : ScriptableObject
{
    [SerializeField] private int baseGold = 0;
    public int gold;
    public PlayerInventory shopInventory = new PlayerInventory();
    public PlayerInventory playerInventory = new PlayerInventory();
    public List<Gladiator> gladiatorList = new List<Gladiator>();
    public List<IndividualGladiatorEquipment> individualGladiatorEquipment = new List<IndividualGladiatorEquipment>();
    private void OnEnable()
    {
        gold = baseGold;
    }
}
