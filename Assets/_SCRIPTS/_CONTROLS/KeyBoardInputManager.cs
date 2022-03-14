using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyBoardInputManager : MonoBehaviour
{
    public TextMeshProUGUI log;
    

    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        log = GameObject.FindGameObjectWithTag("LOG").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }



        
    }

    public void TogglePause()
    {
        if (isPaused) { Unpause(); }
        else { Pause(); }
    }

    private void Pause()
    {
        log.text += "<b><color=red>\n\nPAUSED</color></b>";
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void Unpause()
    {
        log.text += "<color=red>\n\nUNPAUSED</color>";
        Time.timeScale = 1f;
        isPaused = false;
    }
}
