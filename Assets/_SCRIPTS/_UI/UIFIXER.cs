using UnityEngine;
using UnityEngine.UI;

public class UIFIXER : MonoBehaviour
{
    public Canvas UICANVAS;
    public Canvas MainUI;

    private void Awake()
    {
        UICANVAS.gameObject.SetActive(true);
        MainUI.gameObject.SetActive(true);
    }
}
