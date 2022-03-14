using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    //public List<Item> items = new List<Item>();
    public const int SPACE = 24;

    public Item[] itemArray = new Item[24];

    public bool Add(Item item)
    {
        int openSlotIndex = -1;
        int itemCount = 0;

        //rewrote for item array (so i can have fixed positions in inventory)
        if (!item.isDefaultItem)
        { 
            for (int i = 0; i < SPACE; i++)
            {
                //Debug.Log("array length" + i);
                if (itemArray[i] != null)
                {
                    itemCount++;
                }
                else if (openSlotIndex == -1) //gets the first open slot
                {

                    openSlotIndex = i;
                }
            }

            if (itemCount < SPACE)
            {
                itemArray[openSlotIndex] = item;
                if (onItemChangedCallback != null)
                {
                    onItemChangedCallback.Invoke();
                }
            }
            else
            {
                Debug.Log("No Space In Inventory");
                return false;
            }
        }
        return true;
    }

    public void Remove(Item item)
    {
        //Remove Items
    }
}
