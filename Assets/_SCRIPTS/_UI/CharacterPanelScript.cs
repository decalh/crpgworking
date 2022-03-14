using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterPanelScript : MonoBehaviour, IDropHandler
{
    //public CharacterButtonDrag.Slot typeOfItem = CharacterButtonDragSlot.WEAPON

    public List<Button> buttons;
    public List<Image> images;

    private PartyManager pm;

    // Start is called before the first frame update
    private void Start()
    {
        pm = FindObjectOfType<PartyManager>();

        SetAmountOfPortraits();

        UpdatePortraitPartyOrder();

        pm.partySelectionChangedCallback += PortraitBorderColorUpdate;
    }

    public void SetAmountOfPortraits()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }

        for (int i = 0; i < pm.partyMembers.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            images[i].gameObject.SetActive(true);
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        
        Debug.Log(eventData.pointerDrag.name);

        CharacterButtonDrag cbd = eventData.pointerDrag.GetComponent<CharacterButtonDrag>();
        if (cbd != null)
        {
            //if (typeOfItem == d.typeOfItem || typeOfItem == CharacterButtonDrag.Slot.INVENTORY ) { below }
            cbd.parentToReturnTo = transform;
        }

        MyBagSlotDrag mbsd = eventData.pointerDrag.GetComponent<MyBagSlotDrag>();
        if (mbsd != null)
        {
            
            int i = 0;

            foreach(Item item in pm.partyMembers[mbsd.numberInParty].GetComponent<Inventory>().itemArray)
            {
                if (item != null)
                {
                    i++;
                }
            }

            if (i < 24)
            {
                pm.partyMembers[mbsd.numberInParty].GetComponent<Inventory>().Add(
                    mbsd.item);
                mbsd.wasMoved = true;
            }
            
        }
    }
    

    public void UpdatePortraitPartyOrder()
    {
        int i = 0;
        foreach (Image image in images)
        {
            if (image.IsActive())
            {
                image.sprite = pm.partyMembers[i].GetComponent<PC>().portrait;
                i++;
            }
        }
    }

    public void PortraitBorderColorUpdate()
    {
        int i = 0;

        /*foreach (GameObject member in pm.partyMembers)
        {
            if (member.GetComponent<PC>().selected)
            {
                buttons[i].GetComponent<Image>().color = member.GetComponent<PC>().selectedColor;
            }
            else
            {
                buttons[i].GetComponent<Image>().color = Color.white;
            }
            i++;
        }*/
        
        foreach (GameObject member in pm.partyMembers)
        {
            if (member.GetComponent<PC>().selected)
            {
                foreach (Button button in buttons)
                {
                    if (button.transform.GetSiblingIndex() == i)
                    {
                        button.GetComponent<Image>().color = member.GetComponent<PC>().selectedColor;
                    }
                }
            }
            else
            {
                foreach (Button button in buttons)
                {
                    if (button.transform.GetSiblingIndex() == i)
                    {
                        button.GetComponent<Image>().color = Color.white;
                    }
                }
            }
            i++;
        }
    }

    public void ClickSelect()
    {
        
        pm.SelectPartyMember(EventSystem.current.currentSelectedGameObject.
            transform.GetSiblingIndex());
    }


    /*
    #region ButtonClickToSelect

    public void Button0()
    {

        pm.SelectPartyMember(0);
    }

    public void Button1()
    {
        pm.SelectPartyMember(1);
    }

    public void Button2()
    {
        pm.SelectPartyMember(2);
    }

    public void Button3()
    {
        pm.SelectPartyMember(3);
    }

    public void Button4()
    {
        pm.SelectPartyMember(4);
    }

    public void Button5()
    {
        pm.SelectPartyMember(5);
    }

    

    #endregion*/
}
