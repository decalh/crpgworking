﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;

    public int armorModifier;
    public int damageModifier;
}

public enum EquipmentSlot { Amulet, Belt, Boots, Cloak, Chest, Gloves, Helmet, Offhand, Ring, Weapon }