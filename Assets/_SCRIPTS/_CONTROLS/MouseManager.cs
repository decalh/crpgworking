using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Threading;

public class MouseManager : MonoBehaviour
{
    //gives me access to the party
    private PartyManager pm;

    public LayerMask clickMask; // the layermasks that i can click on basically
    public RectTransform selectionRect;


    private Vector3 mousePos1;
    private Vector3 endPoint;

    private bool validClick = false;
    private float timeClicked; //keeps track of how long im clicking, for single click vs drag
    private Vector3 initialPoint; //the point i first click on
    private Vector3 currentPoint; //the point my mouse is currently on while dragging
    private bool drawSelectionArea = false; //tells the game to draw the rectangles

    public GameObject hoverUIObject;
    private Collider hoverCollider;
    private float hoverTime;
    private bool hoverDisplay;
    private HoverInfoUIController hoverUI;

    //this should be a player setting
    private const float clickVsDragTime = .2f;
    private const float hoverTimer = 1.25f;

    private RaycastHit hit;

    private void Awake()
    {
        selectionRect.gameObject.SetActive(false); //turn off the selection area box at start
    }

    private void Start()
    {
        pm = FindObjectOfType<PartyManager>();


        //dummy collider initialized so that it is never null, it doesn't matter what
        //the collider is because it's nonessential
        hoverCollider = FindObjectOfType<Collider>();

        hoverUI = hoverUIObject.GetComponent<HoverInfoUIController>();
    }

    
    //gathering the input, i.e. the clicks
    private void Update()
    {
        hit = DrawRay();
        
        // if I left mouse click and am not over a menu item.
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //measuring time with unscaled deltatime because pausing
            timeClicked = Time.unscaledDeltaTime;

            //set the initial point I clicked for selection rectangle UI
            initialPoint = hit.point;

            //this locks in the position for the real selection rectangle
            mousePos1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            validClick = true;
            //Debug.Log("mouse click down");
        }

        if (Input.GetMouseButton(0) && validClick)
        {
            //adds up the amount of time the mouse was down, pause independant
            timeClicked += Time.unscaledDeltaTime;

            //since the mouse is being held down, draw the selection area
            if (!drawSelectionArea && timeClicked > clickVsDragTime &&
                !EventSystem.current.IsPointerOverGameObject())
            {
                drawSelectionArea = true;
                selectionRect.gameObject.SetActive(true);
                Debug.Log("drawing selection");
            }
            //Debug.Log("mouse held down");
        }

        if (Input.GetMouseButtonUp(0) && validClick)
        {
            //Debug.Log("mouse button up");
            //either way, we are done drawing the box
            drawSelectionArea = false;
            selectionRect.gameObject.SetActive(false);
            ResetRect(); //remove phantom box
            

            //if it was truly a click, less than .25 of a second
            if (timeClicked <= clickVsDragTime)
            {
                LMBClick();
            }
            else //otherwise you clicked and dragged
            {
                BoxSelectParty();
            }

            //and we are reseting if a validClick has occured because the last valid
            //click is about to reach a valid resolution
            validClick = false;
        }

        //if you right click, deselect everything
        if (Input.GetMouseButtonDown(1))
        {
            RMBClick();
        }


        //if the drawSelectionArea is true, fucking draw the selection area
        if (drawSelectionArea) { DrawSelectionArea(); }

        //hit is being populated by DrawRay() every frame on update
        //if what I'm hovering over isn't marked as hoverCollider
        if (hoverCollider != hit.collider)
        {
            //mark what im over as being hovered
            hoverCollider = hit.collider;
            //reset hover time
            hoverTime -= hoverTime;
            //reset hoverDisplay
            hoverDisplay = false;
        }
        else if (hoverCollider.tag == "PartyMemberCollider")
        {
            //if party member
            hoverTime += Time.unscaledDeltaTime;

            //if i have hovered over this party member for more than the hoverTimer
            if (hoverTime >= hoverTimer)
            {
                hoverDisplay = true;
            }
        }
        

        //after all my checks about what I'm hovering over
        if (hoverDisplay) { DisplayHoverUI(); }
        else { hoverUI.DisableHoverDisplay(); }
    }

    // this is for graphics and shite
    private void FixedUpdate()
    {
        
    }

    private void DisplayHoverUI()
    {
        //if this is the first pass through, with hover ui being enabled, this will ring false
        //and enable the UI elements through the hoverUI control script
        if (!hoverUI.isEnabled) { hoverUI.EnableHoverDisplay(); }

        hoverUI.ColorGreen();

        hoverUI.SetHoverText(hoverCollider.gameObject.GetComponentInParent<PC>().gameObject.name);

        hoverUI.AdjustPosition(Camera.main.WorldToScreenPoint(hit.point));
    }

    private void BoxSelectParty()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //sets the mousePos2 to the place the mouse is when this begins
            Vector3 mousePos2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            //if i am not pressing control, it will not add to selection
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                pm.DeselectEntireParty();
            }

            Rect selectionArea = new Rect(mousePos1.x, mousePos1.y,
                    mousePos2.x - mousePos1.x, mousePos2.y - mousePos1.y);
        
            foreach (GameObject member in pm.partyMembers)
            {
                if (member != null)
                {
                    if (selectionArea.Contains(Camera.main.WorldToViewportPoint(
                        member.transform.position), true))
                    {
                        member.GetComponent<PC>().Select();
                    }
                }
            }
        }
    }
    
    //this is so no phantom box appears in game
    private void ResetRect()
    {
        selectionRect.position = Vector3.zero;
        selectionRect.sizeDelta = Vector2.zero;
    }

    private void DrawSelectionArea()
    {
        //selectionRect;
        //initialPoint;

        //continuously updates the position for the rectangle
        currentPoint = Input.mousePosition;

        Vector3 startPos = Camera.main.WorldToScreenPoint(initialPoint);

        //this .z = 0f might become and issue with inclines
        startPos.z = 0f;
        

        Vector3 center = (currentPoint + startPos) / 2f;
        
        selectionRect.position = center;


        float x = Mathf.Abs(startPos.x - currentPoint.x);
        float y = Mathf.Abs(startPos.y - currentPoint.y);
        

        selectionRect.sizeDelta = new Vector2(x, y);

    }

    //simply shoots a ray and returns the hit so I can draw ray from anywhere
    private RaycastHit DrawRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hitMethod, Mathf.Infinity, clickMask))
        {
            return hitMethod;
        }

        Debug.LogError("We have failed to shoot raybeams.");
        return new RaycastHit();
    }
    
    //this details exactly what should happen on a left mouse click
    //basically it shoots a ray and returns everything in it's path
    //then i sort that ray into pieces so i can make sure i catch what the
    //player intended to click on
    private void LMBClick()
    {         
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Interactable interact = null;
        Vector3 destination = Vector3.zero;
        bool hitPartyMember = false;
        bool hitInteractable = false;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Mathf.Infinity, clickMask);

        foreach (RaycastHit hit in hits)
        {
            //Debug.DrawRay(hit.point, hit.normal, Color.yellow, 20f);
            //Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Walkable")
            {
                destination = hit.point;
            }
            if (hit.collider.tag == "Interactable")
            {
                hitInteractable = true;
                interact = hit.collider.GetComponent<Interactable>();
            }
            if (hit.collider.tag == "PartyMemberCollider")
            {
                hitPartyMember = true;
                //multiselect while presssing left control.
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    hit.collider.GetComponentInParent<PC>().SelectToggle();
                }
                else
                {
                    pm.DeselectEntireParty();
                    hit.collider.GetComponentInParent<PC>().SelectToggle();
                    Debug.Log("Hit: " + hit.collider.name);
                }
                break;  //we do not continue to check for anything once a player has been clicked on
            }
            
        }


        
        if (interact != null && !hitPartyMember)
        {
            pm.Interact(interact);
        }
        //if the raycast did not hit something other than ground &&
        //it did hit the ground and it didn't hit an interactable object
        if (!hitInteractable && !hitPartyMember && destination != Vector3.zero)
        {
            pm.MoveSelectedPartyMembers(destination);
        }
    }

    

    private void RMBClick()
    {
        pm.DeselectEntireParty();
    }
}
