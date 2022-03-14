using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelFix : MonoBehaviour
{

    public GameObject InventoryPanel;

    // Start is called before the first frame update
    void Start()
    {
        //do to the limitations of the unity engine, inventory panel must be open to initialize
        //arrays on awake, then i have this script close it on start so that it's ready for
        //gameplay
        InventoryPanel.SetActive(true);
        InventoryPanel.SetActive(false);
    }
    
}
