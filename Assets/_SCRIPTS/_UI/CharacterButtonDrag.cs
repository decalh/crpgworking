using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButtonDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public enum Slot { WEAPON, HEAD, CHEST, LEGS, FEET, INVENTORY };
    //public Slot typeOfItem = Slot.WEAPON;

    [HideInInspector]
    public Transform parentToReturnTo = null;

    GameObject placeholder = null;

    private int originalIndex;
    



    public void OnBeginDrag(PointerEventData eventData)
    {
        //captures the position of the character
        originalIndex = transform.GetSiblingIndex();

        //Debug.Log("On Begin Drag");

        //make a placeholder
        placeholder = new GameObject();
        placeholder.transform.SetParent(transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        le.flexibleHeight = 0f;
        le.flexibleWidth = 0f;

        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        //save out old parent
        parentToReturnTo = transform.parent;

        //removes it from it's parent so the layout will update
        transform.SetParent(transform.root);

        //turn off raycast blocking so my raycasts can shoot down and detect whats below
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("On Drag");

        transform.position = eventData.position;

        int newSiblingIndex = placeholder.transform.GetSiblingIndex();

        for (int i = 0; i < parentToReturnTo.childCount; i++)
        {
            if (transform.position.y < parentToReturnTo.GetChild(i).position.y)
            {
                placeholder.transform.SetSiblingIndex(i);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        Debug.Log("On End Drag: ");

        //return to our old parent with our fancy layout
        transform.SetParent(parentToReturnTo);

        //put the button down where the placeholder has moved to
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        
        //turn back on raycast blocking so raycasts can hit this button again
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //if placeholder != null then we have successfully swapped positions 
        if (placeholder != null)
        {
            if (originalIndex != transform.GetSiblingIndex())
            {
                PartyManager pm = FindObjectOfType<PartyManager>();

                Debug.Log("OriginalIndex: " + originalIndex + "CurrentIndex: " + transform.GetSiblingIndex());

                Debug.Log("Order before insertion.");
                int y = 0;
                foreach (GameObject member in pm.partyMembers)
                {
                    Debug.Log(y + ": " + member.name);
                    y++;
                }

                if (originalIndex > transform.GetSiblingIndex())
                {
                    //if original index is (5) higher than current (2)
                    //insert partyMember 5 at index 2, then erase the overflow partyMember
                    //which now resides at index 6 (5+1), which is a 7th party member, we don't want
                    pm.partyMembers.Insert(transform.GetSiblingIndex(), pm.partyMembers[originalIndex]);
                    pm.partyMembers.RemoveAt(originalIndex + 1);
                }
                else
                {
                    //this part is easy compared to above
                    pm.partyMembers.Insert(transform.GetSiblingIndex() + 1, pm.partyMembers[originalIndex]);
                    pm.partyMembers.RemoveAt(originalIndex);
                }

                Debug.Log("Order after deletion.");
                int x = 0;
                foreach (GameObject member in pm.partyMembers)
                {
                    Debug.Log(x + ": " + member.name);
                        x++;
                }
                //update party portrait order
                //FindObjectOfType<CharacterPanelScript>().UpdatePortraitPartyOrder();

            }

            //Get rid of our uneeded placeholder
            Destroy(placeholder);
        }
    }
}
