using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryPanelScript : MonoBehaviour
{
    public Transform myBagSlotsParent;
    MyBagSlotManager[] myBagSlots;

    public Transform myEquipmentSlotsParent;
    EquipmentSlotManager[] myEquipmentSlots;

    public Transform myGroundOrChestParent;
    MyGroundChestSlotManager[] myGroundChestSlots;

    public TextMeshProUGUI nameTextField; //assigned in Inspector
    private PartyManager pm;
    private GameObject selectedMember;
    private Inventory inventory;
    private EquipmentManager equipment;
    

    private void Awake()
    {
        pm = FindObjectOfType<PartyManager>();
        gameObject.SetActive(false);
        myGroundChestSlots = myGroundOrChestParent.GetComponentsInChildren<MyGroundChestSlotManager>();
        myBagSlots = myBagSlotsParent.GetComponentsInChildren<MyBagSlotManager>();
        myEquipmentSlots = myEquipmentSlotsParent.GetComponentsInChildren<EquipmentSlotManager>();
    }

    private void Start()
    {
        //subscribed to partySelectionChanges
        pm.partySelectionChangedCallback += InventoryCharacter;
    }

    public void OpenInventoryPanel()
    {
        Debug.Log("ACTUALLY OPENED THE FUCKING THING");
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        //myGroundChestSlots = myGroundOrChestParent.GetComponentsInChildren<MyGroundChestSlotManager>();
        //myBagSlots = myBagSlotsParent.GetComponentsInChildren<MyBagSlotManager>();
        //myEquipmentSlots = myEquipmentSlotsParent.GetComponentsInChildren<EquipmentSlotManager>();

        ResetGroundChestSlots();

        InventoryCharacter();
    }

    public void ResetGroundChestSlots()
    {
        for (int i = 0; i < myGroundChestSlots.Length; i++)
        {
            myGroundChestSlots[i].item = null;
        }
    }

    public void InventoryCharacter()
    {
        selectedMember = pm.partyMembers[pm.FirstSelectedMember()];
        if (selectedMember == null) { Debug.LogError("WE ARE GONNA HAVE A PROBLEM HERE."); }
        nameTextField.text = selectedMember.name;
        inventory = selectedMember.GetComponent<Inventory>();
        equipment = selectedMember.GetComponent<EquipmentManager>();
        UpdateUI();

        inventory.onItemChangedCallback += UpdateUI;
    }

    public void GroundChestDisplay(int index, Item item)
    {
        myGroundChestSlots[index].DisplayItem(item);
    }

    public void GroundChestPileRemove(int index)
    {
        //need to access open item pile
        ItemPile[] itemPile = FindObjectsOfType<ItemPile>();

        foreach (ItemPile iP in itemPile)
        {
            if (iP.isActiveAndOpen == true)
            {
                iP.RemoveFromPile(index);
            }
        }
    }

    public bool GroundChestPileAdd(int index, Item newItem)
    {
        //need to access open item pile
        ItemPile[] itemPile = FindObjectsOfType<ItemPile>();

        foreach (ItemPile iP in itemPile)
        {
            if (iP.isActiveAndOpen == true)
            {
                return iP.AddToPile(index, newItem);
            }
        }
        return false;
    }

    public void GroundChestWalkAway()
    {
        for (int i = 0; i < myGroundChestSlots.Length; i++)
        {
            myGroundChestSlots[i].icon.enabled = false;
        }
    }


    public void UpdateUI()
    {
        //Debug.Log("UPDATING UI");

        for (int i = 0; i< myBagSlots.Length; i++)
        {
            if (inventory.itemArray[i] != null)
            {
                myBagSlots[i].AddItem(inventory.itemArray[i]);
            }
            else
            {
                myBagSlots[i].ClearSlot();
            }
        }

        for (int i = 0; i < myEquipmentSlots.Length; i++)
        {
            if (equipment.currentEquipped[i] != null)
            {
                myEquipmentSlots[i].AddItem(equipment.currentEquipped[i]);
            }
            else
            {
                myEquipmentSlots[i].ClearSlot();
            }
        }

        if (inventoryUpdatedCallback != null)
        {
            inventoryUpdatedCallback.Invoke();
        }
    }

    public delegate void InventoryUpdated();
    public InventoryUpdated inventoryUpdatedCallback;

    public void UpdateInventory()
    {
        //Debug.Log("UPDATING CHARACTER INVENTORY");
        for (int i = 0; i< myBagSlots.Length; i++)
        {
            if (myBagSlots[i].GetComponent<MyBagSlotManager>().item != null)
            {
                inventory.itemArray[i] = myBagSlots[i].GetComponent<MyBagSlotManager>().item;
            }
            else
            {
                inventory.itemArray[i] = null;
            }
        }

        for (int i = 0; i < myEquipmentSlots.Length; i++)
        {
            if (myEquipmentSlots[i].GetComponent<EquipmentSlotManager>().equipment != null)
            {
                equipment.currentEquipped[i] = myEquipmentSlots[i].GetComponent<EquipmentSlotManager>().equipment;
            }
            else
            {
                equipment.currentEquipped[i] = null;
            }

            
        }

        UpdateUI();
    }

    
}
