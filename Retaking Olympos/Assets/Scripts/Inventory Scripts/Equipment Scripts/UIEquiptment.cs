﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIEquiptment : MonoBehaviour
{
    // Equipment slots
    UIEquipmentSlots helmetSlot;
    UIEquipmentSlots chestSlot;
    UIEquipmentSlots legsSlot;
    UIEquipmentSlots bootsSlot;
    UIEquipmentSlots weaponSlot;

    public int gladiatorIndex = 0;

    public GladiatorEquiptment gladiatorEquiptment;
    public HoldPlayerInformation playerInformation;

    // Container to hold game objects
    public Transform itemContainer;
    // template game object
    public Transform itemTemplate;

    private void Awake()
    {
        
        // Find container and template, slots, and subscribe to onItemDropped Events
        itemContainer = transform.Find("itemContainerEquipment");
        itemTemplate = transform.Find("itemTemplate");

        helmetSlot = transform.Find("Helmet Slot").GetComponent<UIEquipmentSlots>();
        chestSlot = transform.Find("Chest Slot").GetComponent<UIEquipmentSlots>();
        legsSlot = transform.Find("Legs Slot").GetComponent<UIEquipmentSlots>();
        bootsSlot = transform.Find("Boots Slot").GetComponent<UIEquipmentSlots>();
        weaponSlot = transform.Find("Weapon Slot").GetComponent<UIEquipmentSlots>();

        weaponSlot.OnItemDropped += weaponSlot_OnItemDropped;
        helmetSlot.OnItemDropped += helmetSlot_OnItemDropped;
        chestSlot.OnItemDropped += chestSlot_OnItemDropped;
        legsSlot.OnItemDropped += legsSlot_OnItemDropped;
        bootsSlot.OnItemDropped += bootsSlot_OnItemDropped;
    }

    // Events for when item is dripped onto slot, test if item is accepted in the slot
    private void bootsSlot_OnItemDropped(object sender, UIEquipmentSlots.OnItemDroppedEventArgs e)
    {

        gladiatorEquiptment.testEquip(GladiatorEquiptment.SlotName.Boots, e.item, gladiatorIndex);
        UpdateItem();
    }

    private void legsSlot_OnItemDropped(object sender, UIEquipmentSlots.OnItemDroppedEventArgs e)
    {

        gladiatorEquiptment.testEquip(GladiatorEquiptment.SlotName.Pants, e.item, gladiatorIndex);
        UpdateItem();
    }

    private void chestSlot_OnItemDropped(object sender, UIEquipmentSlots.OnItemDroppedEventArgs e)
    {

        gladiatorEquiptment.testEquip(GladiatorEquiptment.SlotName.Chestplate, e.item, gladiatorIndex);
        UpdateItem();
    }

    private void helmetSlot_OnItemDropped(object sender, UIEquipmentSlots.OnItemDroppedEventArgs e)
    {

        gladiatorEquiptment.testEquip(GladiatorEquiptment.SlotName.Helmet, e.item, gladiatorIndex);
        UpdateItem();
    }

    private void weaponSlot_OnItemDropped(object sender, UIEquipmentSlots.OnItemDroppedEventArgs e)
    {

        gladiatorEquiptment.testEquip(GladiatorEquiptment.SlotName.Sword, e.item, gladiatorIndex);
        UpdateItem();
        
    }

    // Set local copy of equipment 
    public void SetEquipment(GladiatorEquiptment gladiatorEquiptment) 
    {
        this.gladiatorEquiptment = gladiatorEquiptment;
        UpdateItem();

        gladiatorEquiptment.onEquipmentChange += CharacterEquipment_OnEquipmentChange;
    }

    // When equipment change event fires, update items
    private void CharacterEquipment_OnEquipmentChange(object sender, EventArgs e)
    {
        
        UpdateItem();
    }

    // Update visuals of equipment
    private void UpdateItem() 
    {
        itemContainer = transform.Find("itemContainerEquipment");
        itemTemplate = transform.Find("itemTemplate");
        // Destroy old game objects
        foreach (Transform child in itemContainer) 
        {
            Destroy(child.gameObject);
        }
        // Check if item is in each slot, if so display it
        Item weapon = gladiatorEquiptment.GetWeapon(gladiatorIndex);
        if (weapon != null)
        {
            weaponSlot.transform.Find("DefaultSprite").gameObject.SetActive(false);
            SetEquipedItem(weapon, weaponSlot);
        }
        else 
        {
            weaponSlot.transform.Find("DefaultSprite").gameObject.SetActive(true);
        }
        Item chestplate = gladiatorEquiptment.GetChestplate(gladiatorIndex);
        if (chestplate != null)
        {
            chestSlot.transform.Find("DefaultSprite").gameObject.SetActive(false);
            SetEquipedItem(chestplate, chestSlot);
        }
        else
        {
            chestSlot.transform.Find("DefaultSprite").gameObject.SetActive(true);
        }
        Item helmet = gladiatorEquiptment.GetHelmet(gladiatorIndex);
        if (helmet != null)
        {
            helmetSlot.transform.Find("DefaultSprite").gameObject.SetActive(false);
            SetEquipedItem(helmet, helmetSlot);
        }
        else
        {
            helmetSlot.transform.Find("DefaultSprite").gameObject.SetActive(true);
        }
        Item legs = gladiatorEquiptment.GetLegs(gladiatorIndex);
        if (legs != null)
        {
            legsSlot.transform.Find("DefaultSprite").gameObject.SetActive(false);
            SetEquipedItem(legs, legsSlot);
        }
        else
        {
            legsSlot.transform.Find("DefaultSprite").gameObject.SetActive(true);
        }
        Item boots = gladiatorEquiptment.GetBoots(gladiatorIndex);
        if (boots != null)
        {
            bootsSlot.transform.Find("DefaultSprite").gameObject.SetActive(false);
            SetEquipedItem(boots, bootsSlot);
        }
        else
        {
            bootsSlot.transform.Find("DefaultSprite").gameObject.SetActive(true);
        }
    }

    // Takes an item and a slot and puts that item in that slot
    private void SetEquipedItem(Item item, UIEquipmentSlots slot)
    {
        Transform itemTransform = Instantiate(itemTemplate, itemContainer);
        itemTransform.gameObject.SetActive(true);

        Image spriteImage = itemTransform.Find("Image").GetComponent<Image>();
        spriteImage.sprite = item.GetSprite();

        TextMeshProUGUI amountText = itemTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
        amountText.SetText("");

        itemTransform.GetComponent<RectTransform>().anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
        UIDragAndDropItem uiItem = itemTransform.GetComponent<UIDragAndDropItem>();

        uiItem.SetItem(item);
        itemTransform.gameObject.GetComponent<ItemClickable>().item = item;
    }

    // Update index for showing equipment on a gladiator
    public void IncrementEquipmentIndex(HoldPlayerInformation playerInformation)
    {
        gladiatorIndex = (gladiatorIndex + 1) % playerInformation.gladiatorList.Count;
        UpdateItem();
    }

    public void DecrementEquipmentIndex(HoldPlayerInformation playerInformation)
    {
        gladiatorIndex = (gladiatorIndex - 1) % playerInformation.gladiatorList.Count;
        if (gladiatorIndex < 0)
        {
            gladiatorIndex += playerInformation.gladiatorList.Count;
        }
        UpdateItem();
    }
}
