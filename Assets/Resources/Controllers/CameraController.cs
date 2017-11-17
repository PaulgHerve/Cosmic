using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public enum focus_Level { NONE, STAR, SYSTEM, PLANET, STRUCTURE };

    private static CameraController cControl;

    Camera main;
    Input_Controller inputControl;
    Vector3 mousePosition;
    Vector3 mouseMove;
    Vector3 clickPosition;
    static float minDepth = -150;
    static float  maxDepth = -2400;
    static bool gameActive = true;
    static int pausePanelCount = 0;

    private float scrollSensitivity;
    private float panSensitivity;

    static bool isMoving = false;
    static bool isZooming = false;
    static IEnumerator currentRotation = null;

    private static float depth;
    private static focus_Level focus;
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

        for (float i = 0.000f; i <= 64; i += inc)
        {
            relative = targetPos - Camera.main.transform.position;
            current = Camera.main.transform.localRotation;
            rotation = Quaternion.LookRotation(relative);

            Camera.main.transform.localRotation = Quaternion.Slerp(current, rotation, speed);

            yield return new WaitForSeconds(.02f);
        }

        relative = targetPos - Camera.main.transform.position;
        current = Camera.main.transform.localRotation;
        rotation = Quaternion.LookRotation(relative);

        Camera.main.transform.localRotation = Quaternion.Slerp(current, rotation, 1);
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

    public static void Zoom_To_Location(Vector3 destination, float endDepth, float speed)
    {
        IEnumerator item = cControl.ZoomZoom(destination, endDepth, speed);

        cControl.StartCoroutine(item);
    }

    public static void Zoom_To_Selection_Object(Selection_Object destination_Object, float endDepth, float speed)
    {
        Vector3 destination = destination_Object.transform.position;

        IEnumerator item = cControl.ZoomZoom(destination, endDepth, speed);

        cControl.StartCoroutine(item);
    }

    public static void Zoom_Then_Deselect(Selection_Object destination_Object, float endDepth, float speed)
    {
        Vector3 destination = destination_Object.transform.position;

        IEnumerator item = cControl.ZoomThenDeselect(destination, endDepth, speed);

        cControl.StartCoroutine(item);
    }

    private IEnumerator ZoomThenDeselect(Vector3 destination, float endDepth, float speed)
    {
        if (isMoving == false)
        {
            Vector3 current = transform.position;
            Vector3 distance = destination - current;
            float sizeChange = endDepth - depth;
            int ticks = Mathf.Abs((int)(sizeChange / speed));

            if (sizeChange < 0) { speed *= -1; }

            isMoving = true;

            for (int i = 0; i < ticks; i++)
            {
                if (depth + speed != minDepth)
                {
                    depth += speed;

                    main.transform.Translate(0, 0, speed);

                    yield return new WaitForSeconds(.01f);
                }
            }

            sizeChange = endDepth - depth;
            depth += sizeChange;

            main.transform.Translate(0, 0, sizeChange);

            isMoving = false;

            yield return new WaitForSeconds(.01f);
        }
        
        SetTarget(null);
    }

    private IEnumerator ZoomZoom(Vector3 destination, float endDepth, float speed)
    {
        if (isMoving == false)
        {
            Vector3 current = transform.position;
            Vector3 distance = destination - current;
            float sizeChange = endDepth - depth;
            int ticks = Mathf.Abs((int)(sizeChange / speed));

            if (sizeChange < 0) { speed *= -1; }

            isMoving = true;

            for (int i = 0; i < ticks; i++)
            {
                if (depth + speed != minDepth)
                {
                    depth += speed;

                    main.transform.Translate(0, 0, speed);

                    yield return new WaitForSeconds(.02f);
                }             
            }

            sizeChange = endDepth - depth;
            depth += sizeChange;

            main.transform.Translate(0, 0, sizeChange);

            isMoving = false;
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

        if (currentRotation != null)
        {
            cControl.StopCoroutine(currentRotation);
            currentRotation = null;
        }

        IEnumerator thing = RotateCamera(pos, .25f);

        currentRotation = thing;

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

    public static float Get_Depth()
    {
        return depth;
    }

    public Quaternion GetCameraRotation()
    {
        return GetComponent<Transform>().rotation;
    }

    public static void Set_Focus_Level(focus_Level newLevel)
    {
        focus = newLevel;
    }

    public static focus_Level Get_Focus_Level()
    {
        return focus;
    }

    public static void Reduce_Focus_Level()
    {
        if (focus > 0)
        {
            focus--;
        }
    }
}