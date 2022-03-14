using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotManager : MonoBehaviour, IDropHandler
{
    public int slotNumber;

    public void OnDrop(PointerEventData eventData)
    {
        

        MyBagSlotDrag draggedItem = eventData.pointerDrag.GetComponent<MyBagSlotDrag>();
        if (draggedItem != null)
        {
            draggedItem.icon.sprite = null;
            draggedItem.icon.enabled = false;
            if (equipment == null)
            {
                Debug.Log("OnDrop to " + gameObject.name);
                Equipment tmpE = (Equipment)draggedItem.item;

                if (tmpE.equipSlot.ToString() == gameObject.name)
                {
                    AddItem((Equipment)draggedItem.item);
                    draggedItem.wasMoved = true;
                }
                
            }
        }


    }


    #region ItemDisplay

    public Image icon;
    public Equipment equipment;

    public void AddItem(Equipment newEquipment)
    {
        equipment = newEquipment;

        icon.sprite = equipment.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        equipment = null;

        icon.sprite = null;
        icon.enabled = false;
    }


    #endregion
}
