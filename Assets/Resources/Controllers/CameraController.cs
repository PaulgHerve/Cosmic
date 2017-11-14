using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private static CameraController cControl;

    Camera main;
    Player player;
    Input_Controller inputControl;
    Vector3 mousePosition;
    Vector3 mouseMove;
    Vector3 clickPosition;
    static float minDepth = -2;
    static float  maxDepth = -500;
    static bool gameActive = true;
    bool isMoving = false;
    static int pausePanelCount = 0;

    private float scrollSensitivity;
    private float panSensitivity;

    private bool isZooming = false;

    private Vector3 dest;

    private static float depth;
    public static GameObject target;

    void Awake()
    {
        main = Camera.main;
        cControl = this;
        inputControl = FindObjectOfType<Input_Controller>();
        scrollSensitivity = inputControl.zoomSpeed;
        panSensitivity = inputControl.panSpeed;
        clickPosition = new Vector2(0, 0);
        depth = -Vector3.Magnitude(main.transform.position);
    }

    //Controls camera movement using click functions
    void MoveControls()
    {
        if (Input_Controller.GetTouch() && gameActive)
        {
            if (Input_Controller.GetTouchDown())
            {
                clickPosition = mousePosition;
            }

            mouseMove.x = (2 * Screen.width * (clickPosition.x - mousePosition.x)) * depth * -.01f;
            mouseMove.y = (2 * Screen.height * (clickPosition.y - mousePosition.y)) * depth * -.01f; 

            mouseMove *= panSensitivity;          
                        
            main.transform.Translate(mouseMove);

            RotateCamera();

            Vector3 targetPos = new Vector3(0, 0, 0);

            if (target)
            {
                targetPos = target.transform.position;
            }

            float d = Mathf.Abs(depth);

            Vector3 newPos = Camera.main.transform.position;

            newPos = targetPos + Vector3.ClampMagnitude(newPos - targetPos, d);
            main.transform.position = newPos;

            clickPosition = mousePosition;
        }
    }

    //Rotates Camera Around a centerpoint
    private void RotateCamera()
    {
        Vector3 targetPos;

        if (target)
        {
            targetPos = target.transform.position;
        }
        else
        {
            targetPos = new Vector3(0, 0, 0);
        }

        Vector3 relative = targetPos - Camera.main.transform.position;
        Quaternion current = Camera.main.transform.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        Camera.main.transform.localRotation = Quaternion.Slerp(current, rotation, 1);
    }

    //Rotates Camera Around a centerpoint
    private static IEnumerator RotateCamera(Vector3 targetPos, float speed)
    {
        float inc = 1.000f / speed;

        Vector3 relative = targetPos - Camera.main.transform.position;
        Quaternion current = Camera.main.transform.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        for (float i = 0.000f; i <= 240; i += inc)
        {
            relative = targetPos - Camera.main.transform.position;
            current = Camera.main.transform.localRotation;
            rotation = Quaternion.LookRotation(relative);

            Camera.main.transform.localRotation = Quaternion.Slerp(current, rotation, speed);

            yield return new WaitForEndOfFrame();
        }
    }

    //controls all mouse/click functions
    public void MouseControls()
    {
        mousePosition.x = ((Input_Controller.Get0TouchPosition().x / Screen.width) - .5f);
        mousePosition.y = ((Input_Controller.Get0TouchPosition().y / Screen.height) - .5f);

        if (gameActive)
        {
            if (Input.mousePresent)
            {
                ZoomControls();
                MoveControls();
            }

            else if (Input.touchSupported)
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

    public static void StepBack()
    {
        if(target)
        {
            target = null;
        }
    }

    public static void SetTarget(GameObject item)
    {
        target = item;
        Vector3 pos = new Vector3(0, 0, 0);

        if (target) { pos = target.transform.position; }

        depth = -Mathf.Abs(Vector3.Magnitude(Camera.main.transform.position - pos));

        cControl.Rotate_To_Target(item);
    }

    public void Rotate_To_Target(GameObject target)
    {
        Vector3 pos = new Vector3(0, 0, 0);

        if (target)
        {
            pos = target.transform.position;
        }

        IEnumerator thing = RotateCamera(pos, .2f);

        StartCoroutine(thing);
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
            float zoom = inputControl.Zoom();

            if (zoom > 0)
            {
                StartCoroutine("ZoomIn");
            }
            else if (zoom < 0)
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
            newDelta = Input_Controller.CenterMultiTouch();
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
        float speed = inputControl.Zoom() * scrollSensitivity;

        isZooming = true;

        if (Input.touchCount == 2)
        {
            //main.fieldOfView -= speed * panSensitivity;
            Vector3 current = main.transform.position;
            float change = speed * .05f;

            if (depth + change <= minDepth)
            {
                depth += change;
                main.transform.Translate(0, 0, change);
                yield return new WaitForSeconds(.001f);
            }
        }
        else
        {
            float depthRatio;
            float change;

            for (int i = 0; i < 6; i++)
            {
                Vector3 current = main.transform.position;
                depthRatio = .5f + 3 * (depth / maxDepth);
                change = (speed * depthRatio) / 20;

                if (depth + change <= minDepth)
                {                  
                    depth += change;
                    main.transform.Translate(0, 0, change);
                    yield return new WaitForEndOfFrame();
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
        float speed = inputControl.Zoom() * scrollSensitivity;

        isZooming = true;

        if (Input.touchCount == 2)
        {
            //main.fieldOfView -= speed * panSensitivity;
            Vector3 current = main.transform.position;
            float change = speed * .05f;

            if (depth + change >= maxDepth)
            {
                depth += change;
                main.transform.Translate(0, 0, change);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            float depthRatio;
            float change;

            for (int i = 0; i < 6; i++)
            {
                Vector3 current = main.transform.position;
                depthRatio = .5f + 3 * (depth / maxDepth);
                change = (speed * depthRatio) / 20;

                if (depth + change >= maxDepth)
                {
                    depth += change;
                    main.transform.Translate(0, 0, change);
                    yield return new WaitForSeconds(.001f);
                }
                else
                {
                    break;
                }
            }
        }

        isZooming = false;
    }

    public static float GetCameraScaleRatio()
    {
        float val = depth / (maxDepth - minDepth);

        return val;
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