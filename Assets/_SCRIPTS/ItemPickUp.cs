using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;

    public override void Interact()
    {
        //runs the interact from Interactable first
        base.Interact();

        PickUp();
    }

    public void PickUp()
    {
        Debug.Log("picking up " + item.name);

        //add to inventory
        bool wasPickedUp = player.GetComponent<Inventory>().Add(item);

        if (wasPickedUp)
        {
            Destroy(this.gameObject);
        }
    }
}
