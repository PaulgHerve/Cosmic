using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    Camera main;
    Player player;
    InputController inputControl;
    Vector3 mousePosition;
    Vector3 mouseMove;
    Vector3 clickPosition;
    float minDepth = -40;
    float maxDepth = -400;
    public float depth;
    static bool gameActive = true;
    bool isMoving = false;
    static int pausePanelCount = 0;

    private float scrollSensitivity;
    private float panSensitivity;

    public bool isZooming = false;

    private Vector3 dest;

    public GameObject target;

    void Awake()
    {
        main = Camera.main;
        inputControl = FindObjectOfType<InputController>();
        scrollSensitivity = inputControl.zoomSpeed;
        panSensitivity = inputControl.panSpeed;
        clickPosition = new Vector2(0, 0);
        depth = main.transform.position.z;
    }

    void Update()
    {
        if (player)
        {
            if (gameActive)
            {
                MouseControls();
                RotateCamera(null);
            }
        }
    }

    public void DefineConstraints(float xmin, float xmax, float ymin, float ymax)
    {
        float height = Screen.height / 5;
        float width = Screen.width / 5;
    }

    //Controls camera movement using click functions
    void MoveControls()
    {
        if (InputController.GetTouch() && gameActive)
        {
            Vector3 newPos = main.transform.position;

            if (InputController.GetTouchDown())
            {
                clickPosition = mousePosition;
            }

            mouseMove.x = (2 * Screen.width * (clickPosition.x - mousePosition.x)) * depth * -.01f;
            mouseMove.y = (2 * Screen.height * (clickPosition.y - mousePosition.y)) * depth * -.01f; 

            mouseMove *= panSensitivity;            

            float d = Mathf.Abs(depth);

            main.transform.Translate(mouseMove);

            main.transform.position = Vector3.ClampMagnitude(main.transform.position, d);

            clickPosition = mousePosition;
        }
    }

    //Rotates Camera Around a centerpoint
    private void RotateCamera(GameObject target)
    {
        Vector3 targetPos = new Vector3(0, 0, 0);

        if (target)
        {
            targetPos = target.transform.position;
        }

        Vector3 relative = targetPos - main.transform.position;
        Quaternion current = main.transform.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        main.transform.localRotation = Quaternion.Slerp(current, rotation, 1);
    }

    //controls all mouse/click functions
    void MouseControls()
    {
        mousePosition.x = ((InputController.Get0TouchPosition().x / Screen.width) - .5f);
        mousePosition.y = ((InputController.Get0TouchPosition().y / Screen.height) - .5f);

        if (Input.touchSupported)
        {
            if (Input.touchCount == 2)
            {
                ZoomControls();
            }
            else if (Input.touchCount == 1)
            {
                MoveControls();
            }
        }
        else
        {
            ZoomControls();
            MoveControls();
        }                
    }

    public void ZoomToLocation(Vector3 destination)
    {
        dest = destination;
        StartCoroutine("ZoomZoom");
    }

    private IEnumerator ZoomZoom()
    {
        if (isMoving == false)
        {
            int rate = 1 + (int)(20 / scrollSensitivity);

            Vector3 distance = dest - gameObject.transform.position;
            float sizechange = 150 - main.orthographicSize;

            isMoving = true;

            for (int i = 0; i < rate; i++)
            {
                gameObject.transform.position += distance / rate;
                main.orthographicSize += sizechange / rate;

                yield return new WaitForSeconds(.001f);
            }

            isMoving = false;
        }
    }

    public static void ToggleGameActive()
    {
        gameActive = !gameActive;
    }

    public static bool GetGameActive()
    {
        return gameActive;
    }

    public static void SetGameActive(bool newState)
    {
        if (newState == true)
        {
            pausePanelCount--;

            if (pausePanelCount <= 0)
            {
                pausePanelCount = 0;
                gameActive = newState;
            }

        }
        else if (newState == false)
        {
            pausePanelCount++;
            gameActive = newState;
        }
    }

    public float GetCameraSize()
    {
        return main.orthographicSize;
    }

    //Controlls zoom functions and creates zoom animation
    void ZoomControls()
    {
        if (!isZooming)
        {
            if (inputControl.Zoom() > 0)
            {
                StartCoroutine("ZoomIn");
            }
            else if (inputControl.Zoom() < 0)
            {
                StartCoroutine("ZoomOut");
            }
        }
    }

    //Centers Screen during zooming if 2 fingers are present
    Vector3 ZoomCenter()
    {
        Vector3 newDelta;

        if (Input.touchCount == 2)
        {
            newDelta = InputController.CenterMultiTouch();
        }
        else
        {
            //This wont work right now
            float size = main.orthographicSize / 800;
            float speed = inputControl.zoomSpeed * size;

            mousePosition.x = ((Input.mousePosition.x / Screen.width) - .5f);
            mousePosition.y = ((Input.mousePosition.y / Screen.height) - .5f);

            newDelta = new Vector3(mousePosition.x * Screen.width / 40 * speed, mousePosition.y * Screen.height / 40 * speed);
        }

        return newDelta;
    }

    IEnumerator ZoomIn()
    {
        float speed = .05f * inputControl.Zoom() * scrollSensitivity;

        isZooming = true;

        if (Input.touchCount == 2)
        {
            //main.fieldOfView -= speed * panSensitivity;
            Vector3 current = main.transform.position;
            float change = speed * panSensitivity * .05f;

            if (depth + change <= minDepth)
            {
                depth += change;
                main.transform.Translate(0, 0, change);
                yield return new WaitForSeconds(.01f);
            }
        }
        else
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 current = main.transform.position;
                float change = speed * panSensitivity;

                if (depth + change <= minDepth)
                {                  
                    depth += change;
                    main.transform.Translate(0, 0, change);
                    yield return new WaitForSeconds(.01f);
                }
                else
                {
                    break;
                }
            }
        }

        isZooming = false;
    }

    IEnumerator ZoomOut()
    {        
        float speed = .05f * inputControl.Zoom() * scrollSensitivity;

        isZooming = true;

        if (Input.touchCount == 2)
        {
            //main.fieldOfView -= speed * panSensitivity;
            Vector3 current = main.transform.position;
            float change = speed * panSensitivity * .05f;

            if (depth + change >= maxDepth)
            {
                depth += change;
                main.transform.Translate(0, 0, change);
                yield return new WaitForSeconds(.01f);
            }
        }
        else
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 current = main.transform.position;
                float change = speed * panSensitivity;

                if (depth + change >= maxDepth)
                {
                    depth += change;
                    main.transform.Translate(0, 0, change);
                    yield return new WaitForSeconds(.01f);
                }
                else
                {
                    break;
                }
            }
        }

        isZooming = false;
    }

    public Quaternion GetCameraRotation()
    {
        return GetComponent<Transform>().rotation;
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }
}