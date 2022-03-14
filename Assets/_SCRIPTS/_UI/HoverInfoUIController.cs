using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoverInfoUIController : MonoBehaviour
{
    public Image backgroundImage;
    public Image frameImage;
    public TextMeshProUGUI displayText;

    public bool isEnabled = false;

    private Color green = new Color(.2117647f, 1f, 0f, 1f);
    private Color yellow = new Color(.9427508f, 1f, 0f, 1f);
    private Color red = new Color(1f, .1204694f, 0f, 1f);

    private void Start()
    {
        DisableHoverDisplay();
    }
    
    public void SetHoverText(string textToDisplay)
    {
        displayText.GetComponent<TextMeshProUGUI>().text = textToDisplay;

    }

    public void DisableHoverDisplay()
    {
        isEnabled = false;
        backgroundImage.gameObject.SetActive(false);
        frameImage.gameObject.SetActive(false);
        displayText.gameObject.SetActive(false);
    }

    public void EnableHoverDisplay()
    {
        isEnabled = true;
        backgroundImage.gameObject.SetActive(true);
        frameImage.gameObject.SetActive(true);
        displayText.gameObject.SetActive(true);
    }

    public void AdjustPosition(Vector3 pos)
    {
        /*Vector3 newPosition = Vector3.zero;
        newPosition.x = pos.x;
        newPosition.y = pos.y + 18f;
        newPosition.z = pos.z;*/

        pos.y += 18f;

        transform.position = pos;
    }

    public void ColorGreen()
    {
        ChangeColor(green);
    }

    public void ColorYellow()
    {
        ChangeColor(yellow);
    }

    public void ColorRed()
    {
        ChangeColor(red);
    }

    private void ChangeColor(Color color)
    {
        frameImage.GetComponent<Image>().color = color;
        displayText.GetComponent<TextMeshProUGUI>().color = color;
    }
}
