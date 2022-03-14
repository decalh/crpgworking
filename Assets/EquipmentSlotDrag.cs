using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool wasMoved = false;
    public Equipment equipment;

    public Image icon;

    private Transform originalParent;
    private Vector3 originalPosition;


    public void OnBeginDrag(PointerEventData eventData)
    {
        icon = GetComponent<Image>();
        wasMoved = false;
        equipment = transform.parent.GetComponent<EquipmentSlotManager>().equipment;
        transform.parent.GetComponent<EquipmentSlotManager>().ClearSlot();

        FindObjectOfType<InventoryPanelScript>().UpdateInventory();

        icon.sprite = equipment.icon;
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
            transform.parent.GetComponent<EquipmentSlotManager>().equipment = equipment;
        }


        equipment = null;
        GameObject.FindObjectOfType<InventoryPanelScript>().UpdateInventory();
        Debug.Log("OnEndDrag EquipmentSlot Icon");
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
