using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject moveFormationSolo;
    public GameObject moveFormationOne;

    [HideInInspector]
    public List<GameObject> partyMembers;

    private void Start()
    {
        foreach (GameObject member in partyMembers)
        {
            member.GetComponent<PC>().memberSelectionChangeCallback += MemberChanged;
        }
    }

    public delegate void PartySelectionChanged();
    public PartySelectionChanged partySelectionChangedCallback;

    public void MemberChanged()
    {
        /*int i = 0;
        foreach (GameObject member in partyMembers)
        {
            Debug.Log("Position " + i + " is: " + member.name);
            i++;
        }*/
        if (partySelectionChangedCallback != null)
        {
            partySelectionChangedCallback.Invoke();
        }
    }


    public delegate void NewInteractableSelected();
    public NewInteractableSelected newInteractableSelectedCallback;


    public void Interact(Interactable _interactable)
    {
        if (newInteractableSelectedCallback != null)
        {
            newInteractableSelectedCallback.Invoke();
        }


        if (_interactable.hostile)
        {
            //attack or something
        }
        else //or NOT hostile
        {
            //sends the first selected partymember over to the peaceful object
            foreach(GameObject member in partyMembers)
            {
                if (member.GetComponent<PC>().selected)
                {
                    member.GetComponent<PC>().SetTarget(_interactable);
                    break;
                }
            }

        }
    }

    public void SelectPartyMember(int partyMemberNumber)
    {
        int i = 0;
        foreach (GameObject member in partyMembers)
        {
            if (partyMemberNumber == i)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    partyMembers[partyMemberNumber].GetComponent<PC>().SelectToggle();
                }
                else
                {
                    DeselectEntireParty();
                    partyMembers[partyMemberNumber].GetComponent<PC>().SelectToggle();
                }
            }

            i++;
        }

        
    }

    public void SelectPartyMember(Collider partyMemberCollider)
    {
        //need to work on this and take it out of mousemanager
    }

    public delegate void PartyMoving();
    public PartyMoving newPartyMovingCallback;


    public void MoveSelectedPartyMembers(Vector3 _destination)
    {

        if (newPartyMovingCallback != null)
        {
            newPartyMovingCallback.Invoke();
        }

        int selectedCount = 0;
        Vector3 leaderPos = partyMembers[0].transform.position;
        Quaternion rotation;

        foreach (GameObject member in partyMembers)
        {
            if (member.GetComponent<PC>().selected)
            {
                if (selectedCount == 0)
                {
                    leaderPos = member.transform.position;
                }
                selectedCount++;
            }
        }
        
        //sets the angle with some serious wizardry
        float angle = (Mathf.Atan2(leaderPos.x - _destination.x,
            leaderPos.z - _destination.z) * Mathf.Rad2Deg) + 180f;
        rotation = Quaternion.Euler(0f, angle, 0f);

        //if there is only one player moving, we always use MovementFormationSolo
        if (selectedCount == 1)
        {
            GameObject formation = Instantiate(moveFormationSolo, _destination, rotation);
            foreach (GameObject member in partyMembers)
            {
                if (member.GetComponent<PC>().selected)
                {
                    GameObject marker = formation.transform.GetChild(0).gameObject;

                    member.GetComponent<PC>().Move(marker);
                }
            }
            formation.transform.DetachChildren();
            Destroy(formation);
        }
        else
        {
            /*
             * I'll want to build a check in here to determine what formation
             * is selected eventually. For now we will simply declare the only group
             * formation again to make changing it later easier
             */
            GameObject cF = moveFormationOne;

            
            GameObject formation = Instantiate(cF, _destination, rotation);

            int childCount = 0;

            foreach (GameObject member in partyMembers)
            {
                if (member.GetComponent<PC>().selected)
                {
                    GameObject marker = formation.transform.GetChild(childCount).gameObject;

                    member.GetComponent<PC>().Move(marker);

                    childCount++;
                }
            }
            while (childCount < 6)
            {
                Destroy(formation.transform.GetChild(childCount).gameObject);
                childCount++;
            }
            formation.transform.DetachChildren();
            Destroy(formation);

        }
    }

    public void DeselectEntireParty()
    {
        foreach(GameObject member in partyMembers)
        {
            member.GetComponent<PC>().Deselect();
        }
    }

    public void SelectEntireParty()
    {
        foreach(GameObject member in partyMembers)
        {
            member.GetComponent<PC>().Select();
        }
    }

    public int FirstSelectedMember()
    {
        int i = 0;

        foreach (GameObject member in partyMembers)
        {
            if (member.GetComponent<PC>().selected)
            {
                return i;
            }
            i++;
        }

        return 0;
    }
}
