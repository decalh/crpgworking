using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelScript : MonoBehaviour
{

    public GameObject inventoryPanel;

    public void InventoryButton()
    {
        inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);
    }
}
