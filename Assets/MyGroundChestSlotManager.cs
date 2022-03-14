using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyGroundChestSlotManager : MonoBehaviour, IDropHandler
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
                //Debug.Log("OnDrop to " + gameObject.name);
                AddItem(draggedItem.item);
                draggedItem.wasMoved = true;
            }
        }
    }


    #region ItemDisplay

    public Image icon;
    public Item item;
    private InventoryPanelScript ips;
    public GameObject itemPile; // assigned in Inspector

    public void OnEnable()
    {
        ips = FindObjectOfType<InventoryPanelScript>();

    }

    public void DisplayItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;

        ips.GroundChestPileAdd(transform.GetSiblingIndex(), newItem);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;

        bool itemPileBool = ips.GroundChestPileAdd(transform.GetSiblingIndex(), newItem);
        Debug.Log("itemPile bool: " + itemPileBool);
        if (itemPileBool == false)
        {
            PartyManager pm = FindObjectOfType<PartyManager>();
            Vector3 posToSpawn = pm.partyMembers[pm.FirstSelectedMember()].transform.position;
            Quaternion emptyRotation = new Quaternion();
            GameObject iP = Instantiate(itemPile, posToSpawn, emptyRotation);
            iP.GetComponent<ItemPile>().NewPileFromInventory();
            ips.GroundChestPileAdd(transform.GetSiblingIndex(), newItem);
        }
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        
        ips.GroundChestPileRemove(transform.GetSiblingIndex());
    }

    


    #endregion
}
