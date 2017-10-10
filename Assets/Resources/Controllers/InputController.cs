using UnityEngine;

public class InputController : MonoBehaviour {

    public float zoomSpeed;
    public float panSpeed;

    void Start()
    {
        ClampVariables();
    }

    public static bool GetTouch()
    {
        bool touch;

        //touch = Input.GetTouch(0) == true;
        touch = Input.GetMouseButton(0);

        return touch;
    }

    public static bool GetTouchDown()
    {
        bool touch;

        touch = Input.GetMouseButtonDown(0);

        return touch;
    }

    public static bool GetTouchUp()
    {
        bool touch;

        touch = Input.GetMouseButtonUp(0);

        return touch;
    }

    public static Vector2 Get0TouchPosition()
    {
        Vector2 touchPos;

        touchPos = Input.mousePosition;

        if (Input.touchCount == 1)
        {
            touchPos = Input.GetTouch(0).position;
        }

        return touchPos;
    }

    //Determines Zoom control type and controls ortho camera based on input
    public float Zoom()
    {
        float deltaMagnitudeDiff;

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touchOnePrevPos = touch1.position - touch1.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            // Find the difference in the distances between each frame. 
            deltaMagnitudeDiff = -(prevTouchDeltaMag - touchDeltaMag) * Time.deltaTime * 2;
        }
        else
        {
            deltaMagnitudeDiff = Input.GetAxis("MouseScrollWheel") * 1000;
        }
        return deltaMagnitudeDiff;
    }

    public static Vector3 CenterMultiTouch()
    {
        Vector3 meanDelta;

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        meanDelta = (touch0.deltaPosition + touch1.deltaPosition) / 2;


        return meanDelta;
    }

    private void ClampVariables()
    {
        if (zoomSpeed < 0)
        {
            zoomSpeed *= -1;
        }
        else if (zoomSpeed > 10)
        {
            zoomSpeed = 10;
        }
        if (panSpeed < 0)
        {
            panSpeed *= -1;
        }
        else if (panSpeed > 10)
        {
            panSpeed = 10;
        }
    }
}
