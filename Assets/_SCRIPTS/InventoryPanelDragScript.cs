using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPanelDragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    

    private bool dragWindow = false;
    private Vector2 offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            string pcrString = eventData.pointerCurrentRaycast.gameObject.name;

            Debug.Log(pcrString);
            if (pcrString == "CharacterName(TMP)")
            {
                dragWindow = true;
            }

            offset.x = transform.parent.position.x - eventData.position.x;
            offset.y = transform.parent.position.y - eventData.position.y;

            Debug.Log(offset);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragWindow)
        {
            Vector2 newPos = offset;
            newPos.x += eventData.position.x;
            newPos.y += eventData.position.y;
            transform.parent.position = newPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragWindow = false;
    }
    
}
