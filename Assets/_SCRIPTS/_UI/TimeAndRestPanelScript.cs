using UnityEngine;

public class TimeAndRestPanelScript : MonoBehaviour
{
    private KeyBoardInputManager kbIM;
    // Start is called before the first frame update
    void Start()
    {
        kbIM = FindObjectOfType<KeyBoardInputManager>();
        if (kbIM == null)
        {
            Debug.Log("note to self, bad call");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PauseButtonClick()
    {
        kbIM.TogglePause();
    }
}
