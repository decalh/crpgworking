using UnityEngine.UI;
using UnityEngine;

public class SelectPanelScript : MonoBehaviour
{
    [HideInInspector]
    public PartyManager pM;

    private void Start()
    {
        pM = GameObject.FindObjectOfType<PartyManager>();
    }

    public void SelectAllButtonPress()
    {
        pM.SelectEntireParty();
    }

    public void DeselectAllButtonPress()
    {
        pM.DeselectEntireParty();
    }
}
