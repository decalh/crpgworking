using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 2f;

    public bool hostile = false;

    private bool isFocus = false;
    public GameObject player;

    public bool hasInteracted = false;

    public virtual void Interact()
    {

        Debug.Log("INTERACTED WITH: " + player.name);
    }

    private void Update()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused(GameObject playerObject)
    {
        isFocus = true;
        player = playerObject;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }
}
