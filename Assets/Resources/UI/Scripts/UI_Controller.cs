using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public Camera uiCamera;
    private CameraController cameraControl;

    static GameObject buttonHit;
    static Selection_Object selected_Object_Hit;
    static Selection_Object prev_Object;
    static Vector3 mousePos;
    static Vector3 uiMousePos;
    static Vector3 clickPos;
   
    private static RaycastHit hit;    

    private void Awake()
    {
        cameraControl = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        GeneralControls();
    }

    private void GeneralControls()
    {
        if (Input_Controller.GetTouch())
        {
            SetMousePos();

            if (!buttonHit)
            {
                cameraControl.MouseControls();
            }
        }

        //Stores click location to prevent selecting a hex if the camera is being panned
        if (Input_Controller.GetTouchDown())
        {
            clickPos = mousePos;

            Select_UI_Object();
        }

        if (Input_Controller.GetTouchUp())
        {
            if (!buttonHit)
            {
                if (clickPos == mousePos)
                {
                    Select_Object();
                }
            }
            else
            {
                buttonHit = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            CameraController.StepBack();
            SelectNewObject(null);
        }
    }

    private void SetMousePos()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount == 1)
            {
                mousePos = Input.mousePosition;
                uiMousePos = Camera.main.ScreenToViewportPoint(Input_Controller.Get0TouchPosition());
                uiMousePos.x *= Screen.width;
                uiMousePos.y *= Screen.height;
                uiMousePos.z = -1;
            }
        }
        else if (Input.mousePresent)
        {
            mousePos = Input.mousePosition;
            uiMousePos = Camera.main.ScreenToViewportPoint(Input_Controller.Get0TouchPosition());
            uiMousePos.x *= Screen.width;
            uiMousePos.y *= Screen.height;
            uiMousePos.z = -1;
        }
    }

    public static void Select_UI_Object()
    {
        Vector3 dir = new Vector3(0, 0, 20);
        Ray r = new Ray(uiMousePos, dir);
        bool h = Physics2D.Raycast(r.origin, dir, 20);
        RaycastHit2D hit;        

        if (h)
        {
            RaycastHit2D[] hitArray = Physics2D.RaycastAll(r.origin, dir, 20);
            hit = hitArray[0];

            if (hitArray.Length > 0)
            {
                for (int i = 0; i < hitArray.Length; i++)
                {
                    RaycastHit2D item = hitArray[i];

                    if (item.transform.gameObject.CompareTag("UI"))
                    {
                        buttonHit = item.transform.gameObject;

                        break;
                    }
                    else
                    {
                        buttonHit = null;
                    }
                }
            }
        }
    }

    //Selects an Object when clicked if panels aren't open and a button isn't also clicked.
    public static void Select_Object()
    {
        Ray r = Camera.main.ScreenPointToRay(mousePos);
        r.origin = Camera.main.transform.position;
        bool h = Physics.Raycast(r, 1000);
        RaycastHit hit;

        if (h)
        {
            RaycastHit[] hitArray = Physics.RaycastAll(Camera.main.transform.position, r.direction, 1000);
            hit = hitArray[0];

            GameObject objectHit = null;

            if (hitArray.Length > 0)
            {               
                objectHit = hit.transform.gameObject;

                if (objectHit.CompareTag("Selectable_Object"))
                {
                    if (!buttonHit)
                    {
                        Selection_Object sHit = objectHit.GetComponentInParent<Selection_Object>();

                        SelectNewObject(sHit);
                    }
                }
            }
        }
    }

    private static void SelectNewObject(Selection_Object sHit)
    {
        ClearPrevSelectedObject();
        prev_Object = selected_Object_Hit;

        if (prev_Object)
        {
            prev_Object.Deselect_This_Object();
        }

        selected_Object_Hit = sHit;

        if (selected_Object_Hit)
        {
            selected_Object_Hit.Select_This_Object();

            Star star = selected_Object_Hit.GetComponent<Star>();

            if (star)
            {
                CameraController.SetTarget(star.gameObject);
            }
        }
        else
        {
            CameraController.SetTarget(null);
        }
    }

    private static void ClearPrevSelectedObject()
    {
        prev_Object = null;
    }

    public static Vector2 GetMouseLocation()
    {
        return mousePos;
    }

    public static Vector2 GetUIMouseLocation()
    {
        return uiMousePos;
    }

    //public Hex ViewSelectedHex()
    //{
    //    if (MapController.hexSelected)
    //    {
    //        if (prevHex != hexSelected)
    //        {
    //            prevHex = hexSelected;
    //        }

    //        hexSelected = MapController.hexSelected.GetComponent<Hex>();
    //    }
    //    else
    //    {
    //        hexSelected = null;
    //    }

    //    hexInfo.UpdateHudCounters(hexSelected);

    //    return hexSelected;
    //}
}
