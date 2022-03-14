using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPile : Interactable
{
    public Item[] items = new Item[40];
    public InventoryPanelScript ips;

    public bool isActiveAndOpen = false;

    public override void Interact()
    {
        //runs the interact from Interactable first
        base.Interact();

        CheckIfEmpty();

        ips.OpenInventoryPanel();

        ips.inventoryUpdatedCallback += CheckIfEmpty;

        FindObjectOfType<PartyManager>().newInteractableSelectedCallback += SetInactive;

        FindObjectOfType<PartyManager>().newPartyMovingCallback += SetInactive;

        PickUp();

        isActiveAndOpen = true;
    }

    public void NewPileFromInventory()
    {
        ips = FindObjectOfType<InventoryPanelScript>();

        ips.inventoryUpdatedCallback += CheckIfEmpty;

        FindObjectOfType<PartyManager>().newInteractableSelectedCallback += SetInactive;

        FindObjectOfType<PartyManager>().newPartyMovingCallback += SetInactive;

        isActiveAndOpen = true;

        Debug.Log("NEWPILEFROMINVENTORY");
    }

    private void SetInactive()
    {
        isActiveAndOpen = false;

        if (ips != null)
        {
            ips.GroundChestWalkAway();
            ips.gameObject.SetActive(false);
        }
    }

    //need to build in a functionality if the player walks away, that it closes item pile

    public void PickUp()
    {
        for (int i=0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                //ips.myGroundChestSlots[i].AddItem(items[i]);
                ips.GroundChestDisplay(i, items[i]);
            }
        }
    }

    private void CheckIfEmpty()
    {
        int x = 0;
        for (int i=0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                x++;
                //ips.myGroundOrChestSlots[i].ClearSlot();
            }
        }

        if (x == 40)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public bool AddToPile(int index, Item newItem)
    {
        //add an item to the pile, create pile if empty
        if (items[index] == null)
        {
            items[index] = newItem;
            return true;
        }

        return false;
    }

    public void RemoveFromPile(int index)
    {
        //remove item from pile, check if empty
        items[index] = null;

        CheckIfEmpty();
    }
}
