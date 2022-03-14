using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyBagSlotManager : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        MyBagSlotDrag draggedItem = eventData.pointerDrag.GetComponent<MyBagSlotDrag>();
        if (draggedItem != null)
        {
            draggedItem.icon.sprite = null;
            draggedItem.icon.enabled = false;
            if (item == null)
            {
                Debug.Log("OnDrop to " + gameObject.name);
                AddItem(draggedItem.item);
                draggedItem.wasMoved = true;
            }
        }

        
        EquipmentSlotDrag draggedEquipment = eventData.pointerDrag.GetComponent<EquipmentSlotDrag>();
        if (draggedEquipment != null)
        {
            Debug.Log("draggedequipment isn't null");
            draggedEquipment.icon.sprite = null;
            draggedEquipment.icon.enabled = false;
            if (item == null)
            {
                Debug.Log("OnDrop to: " + gameObject.name);
                AddItem(draggedEquipment.equipment);
                draggedEquipment.wasMoved = true;
            }
        }

        GroundChestDrag draggedGround = eventData.pointerDrag.GetComponent<GroundChestDrag>();
        if (draggedGround != null)
        {
            Debug.Log("draggedequipment isn't null");
            draggedGround.icon.sprite = null;
            draggedGround.icon.enabled = false;
            if (item == null)
            {
                Debug.Log("OnDrop to: " + gameObject.name);
                AddItem(draggedGround.item);
                draggedGround.wasMoved = true;
            }
        }

    }


    #region ItemDisplay

    public Image icon;
    public Item item;
    
    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }


    #endregion
}
