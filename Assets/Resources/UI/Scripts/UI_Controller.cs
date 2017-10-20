using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public Camera uiCamera;

    static GameObject buttonHit;
    static Selection_Object selected_Object_Hit;
    static Selection_Object prev_Object;
    static Vector3 mousePos;
    static Vector3 uiMousePos;
    static Vector3 clickPos;

    private static RaycastHit hit;    

    void Update()
    {
        GeneralControls();
    }

    private void GeneralControls()
    {
        if (InputController.GetTouch())
        {
            SetMousePos();
        }

        //Stores click location to prevent selecting a hex if the camera is being panned
        if (InputController.GetTouchDown())
        {
            clickPos = mousePos;
        }

        if (InputController.GetTouchUp())
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

            //ViewSelectedHex();
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
                uiMousePos = uiCamera.ScreenToWorldPoint(InputController.Get0TouchPosition());                
            }
        }
        else if (Input.mousePresent)
        {
            mousePos = Input.mousePosition;
            uiMousePos = uiCamera.ScreenToWorldPoint(InputController.Get0TouchPosition());
        }

        Select_UI_Object();
    }

    public static void Select_UI_Object()
    {
        RaycastHit[] uiArray = Physics.RaycastAll(uiMousePos, Vector3.zero);

        for (int i = 0; i < uiArray.Length; i++)
        {
            RaycastHit item = uiArray[i];

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
