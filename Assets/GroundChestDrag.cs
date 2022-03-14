using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GroundChestDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool wasMoved = false;
    public Item item;
    public Image icon;
    private Transform originalParent;
    private Vector3 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        wasMoved = false;
        item = transform.parent.GetComponent<MyGroundChestSlotManager>().item;
        transform.parent.GetComponent<MyGroundChestSlotManager>().ClearSlot();


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
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;


        if (!wasMoved)
        {
            //should probably make this throw it on the ground
            transform.parent.GetComponent<MyGroundChestSlotManager>().AddItem(item);
        }


        item = null;
        GameObject.FindObjectOfType<InventoryPanelScript>().UpdateInventory();
        Debug.Log("OnEndDrag Bag Slot Icon");
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
