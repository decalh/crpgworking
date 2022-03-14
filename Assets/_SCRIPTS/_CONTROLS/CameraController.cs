using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 10f;
    public float scrollSensitivity = 10f;
    public float borderThickness = 2f;
    public Vector2 xzLimit;
    public float minY;
    public float maxY;

    public bool useMouseCameraMovement = false;


    private float scroll;
    private Vector3 pos;
    private float newSen;

    private void Awake()
    {
        TimeSystem.Create();
    }

    //called on first frame
    void Start()
    {
        //this won't live here, it's being held here.
        TimeSystem.OnTick += delegate (object sender, TimeSystem.OnTickEventArgs e)
        {
            //Debug.Log("tick: " + TimeSystem.GetTick());
        };
        TimeSystem.OnTick_6 += delegate (object sender, TimeSystem.OnTickEventArgs e)
        {
            //Debug.Log("tick_6: " + e.tick);
        };
    }

    void Update()
    {
        
    }

    // LateUpdate is called once per frame - ideal for camera controls
    private void LateUpdate()
    {
        pos = transform.position;
        newSen = sensitivity + pos.y;

        if (useMouseCameraMovement)
        {
            if (MousePos().y >= Screen.height - borderThickness)
            {
                pos.z += newSen * Time.unscaledDeltaTime;
            }
            if (MousePos().y <= borderThickness)
            {
                pos.z -= newSen * Time.unscaledDeltaTime;
            }
            if (MousePos().x >= Screen.width - borderThickness)
            {
                pos.x += newSen * Time.unscaledDeltaTime;
            }
            if (MousePos().x <= borderThickness)
            {
                pos.x -= newSen * Time.unscaledDeltaTime;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            pos.z += newSen * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos.z -= newSen * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += newSen * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= newSen * Time.unscaledDeltaTime;
        }
        
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            scroll = Input.GetAxis("Mouse ScrollWheel");
        }
        pos.y -= scroll * scrollSensitivity * 200f * Time.unscaledDeltaTime;

        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, -xzLimit.x, xzLimit.x);
        pos.z = Mathf.Clamp(pos.z, -xzLimit.y, xzLimit.y);
        transform.position = pos;


        //needs target following.
        //some smoothing would be good.
        //auto adjust based on height of things in view.
    }

    Vector3 MousePos()
    {
        return Input.mousePosition;
    }
}
