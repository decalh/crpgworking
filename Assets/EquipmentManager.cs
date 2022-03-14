using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public Equipment[] currentEquipped = new Equipment[11];

    public delegate void OnEquipmentChanged();
    public OnEquipmentChanged onEquipmentChanged;

    private void Start()
    {
        
    }

    public void Equip(Equipment item, int slot)
    {
        currentEquipped[slot] = (item);

        //add equipment to the list

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke();
        }
    }

    public void Unequip(Equipment item, int slot)
    {
        currentEquipped[slot] = null;

        //remove equipment from the list

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke();
        }
    }
    
}
