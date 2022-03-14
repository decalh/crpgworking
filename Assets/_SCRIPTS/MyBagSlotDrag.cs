using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyBagSlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    public Transform droppedOnParent;
    private Vector3 originalPosition;

    public bool wasMoved = false;
    public Item item = null;
    public Image icon;

    public int numberInParty;

    public bool changedCharacter = false;

    public Equipment equipSlot;

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        wasMoved = false;
        item = transform.parent.GetComponent<MyBagSlotManager>().item;
        transform.parent.GetComponent<MyBagSlotManager>().ClearSlot();
        FindObjectOfType<InventoryPanelScript>().UpdateInventory();
        icon.sprite = item.icon;
        icon.enabled = true;
        originalParent = transform.parent;
        originalPosition = transform.position;
      
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        
        transform.SetParent(transform.parent.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            GameObject mouseOverObject = eventData.pointerCurrentRaycast.gameObject;
            if (mouseOverObject.name == "Image")
            {
                Button characterSelectButton = mouseOverObject.GetComponentInParent<Button>();
                GameObject characterPortrait = characterSelectButton.gameObject;
                numberInParty = characterPortrait.transform.GetSiblingIndex();
                
            }
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        

        if (!wasMoved)
        {
            //should probably make this throw it on the ground
            transform.parent.GetComponent<MyBagSlotManager>().AddItem(item);
        }
        

        item = null;
        GameObject.FindObjectOfType<InventoryPanelScript>().UpdateInventory();
        //Debug.Log("OnEndDrag Bag Slot Icon");
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }



}
