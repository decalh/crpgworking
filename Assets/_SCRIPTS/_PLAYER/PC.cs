using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PC : MonoBehaviour
{
    public Sprite portrait;
    private CharacterPanelScript cps;

    public readonly Color selectedColor = new Color(.2117647f, 1f, 0f, 1f);
    public readonly Color notSelectedColor = new Color(0.1939818f, .6132076f, .08388218f, .33f);

    private PartyManager pm;
    private MeshRenderer myRend;
    private NavMeshAgent navAgent;
    private ParticleSystem navRing;

    public bool selected = false;
    public bool isMoving = false;

    private GameObject oldMarker;
    private Vector3 targetPos = Vector3.down; //dummy load

    private Interactable target;
    private float originalStoppingDistance;

    private void Awake()
    {
        //i need these party members in this list f'in ASAP
        pm = FindObjectOfType<PartyManager>();
        pm.partyMembers.Add(this.gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {

        cps = FindObjectOfType<CharacterPanelScript>();

        //sets up control of MeshRenderer
        myRend = GetComponentInChildren<MeshRenderer>();

        //enables navigation in the environment
        navAgent = GetComponent<NavMeshAgent>();
        originalStoppingDistance = navAgent.stoppingDistance;

        //enables control of navigationring in PC
        navRing = GetComponentInChildren<ParticleSystem>();
        if (navRing == null) { Debug.LogError("No Nav Ring Agent on " + gameObject.name);  }
        

        //can preform operations that aren't critical here, less taxing on PC
        TimeSystem.OnTick += delegate (object sender, TimeSystem.OnTickEventArgs e)
        {
            //once you are decently close to the marker, erase it
            //Debug.Log(transform.position + targetPos);
            if (Vector3.Distance(transform.position, targetPos) <= 0.76f)
            {
                if (oldMarker != null)
                {
                    Destroy(oldMarker);
                    isMoving = false;
                }
            }

            if (target != null)
            {
                FollowTarget();
            }
            else
            {
                RemoveTarget();
            }
        };


        //invoke runs select .1 seconds after game is loaded
        //literally to make sure PartyManager is populated before
        //it runs and gives a null error
        Invoke("Select", .1f);

    }
    

    private void Update()
    {

    }

    private void LateUpdate()
    {
        //FollowTarget is in line with the Ticks, for performance and timing handling.
        //Facetarget needs to be here for smooth turning
        if (target != null)
        {
            FaceTarget();
        }
    }


    public void Move(GameObject marker)
    {
        if (oldMarker != null) { Destroy(oldMarker); }
        if (marker == null) { return; }
        oldMarker = marker;

        targetPos = marker.transform.position;
        navAgent.SetDestination(targetPos);
        RemoveTarget();
        isMoving = true;
    }

    private GameObject followMarker;

    //this is being called from tick timer
    private void FollowTarget()
    {

        float distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
        float stopRadius = target.radius * .8f;

        //if there is an old movement marker from... moving
        if (oldMarker != null) { Destroy(oldMarker); }

        //Things to do based on distance from target
        //Like should I be using a walk animation?
        //Should I throw down a movement marker?
        if (distanceFromTarget > stopRadius)
        {
            isMoving = true;

            //if i am pretty far away I'd like to set a movementMarker to my projected location
            //but, only if i haven't already
            if (followMarker == null)
            {
                Vector3 markerPos = target.transform.position;
                Vector3 dir = transform.position - target.transform.position;
                dir.y = 0;

                followMarker = Instantiate(pm.moveFormationSolo, markerPos, new Quaternion());
                followMarker.transform.Translate(dir.normalized * stopRadius * .5f);
            }

        }
        else
        {
            isMoving = false;
            if (followMarker != null) { Destroy(followMarker); }
        }
        

        navAgent.stoppingDistance = stopRadius;
        navAgent.updateRotation = false;

        navAgent.SetDestination(target.transform.position);
    }

    //this should be called from LateUpdate for smooth rotation
    private void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
     


    //handles setting target
    public void SetTarget(Interactable interactable)
    {

        if (interactable.gameObject != target)
        {
            if (target != null)
            {
                target.OnDefocused();
            }
            
            target = interactable;
            FollowTarget();

            if (followMarker != null) { Destroy(followMarker); }
        }
        
        target.OnFocused(this.gameObject);
    }

    //removes target and normalizes values
    public void RemoveTarget()
    {
        if (target != null)
        {
            target.OnDefocused();
        }

        if (followMarker != null) { Destroy(followMarker); isMoving = false; }
        
        navAgent.stoppingDistance = originalStoppingDistance;
        navAgent.updateRotation = true;
        target = null;
    }

    private GameObject ChildSearchByTag(string s)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == s)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public delegate void MemberSelectionChange();
    public MemberSelectionChange memberSelectionChangeCallback;

    public void Select()
    {
        //Debug.Log(gameObject.name + " Selected");
        var main = navRing.main;
        main.startColor = selectedColor;
        selected = true;

        if (memberSelectionChangeCallback != null)
        {
            memberSelectionChangeCallback.Invoke();
        }
    }

    public void Deselect()
    {
        //Debug.Log(gameObject.name + " Deselected");
        var main = navRing.main;
        main.startColor = notSelectedColor;
        selected = false;

        if (memberSelectionChangeCallback != null)
        {
            memberSelectionChangeCallback.Invoke();
        }
    }

    //Toggles Select
    public void SelectToggle()
    {
        //Debug.Log(gameObject.name + " toggle selection");
        if (!selected)
        {
            Select();
        }
        else if (selected)
        {
            Deselect();
        }
    }
}